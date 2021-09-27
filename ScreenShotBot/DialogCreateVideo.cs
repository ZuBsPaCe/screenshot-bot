using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using ScreenShotBot.Properties;
using VideoSubDirsTypeItem = ScreenShotBot.ComboItem<ScreenShotBot.VideoSubDirsType>;
using VideoScaleTypeItem = ScreenShotBot.ComboItem<ScreenShotBot.VideoScaleType>;

namespace ScreenShotBot
{
    public partial class DialogCreateVideo : Form
    {
        private readonly ILog _log;

        private bool _loading = true;

        private string _startTooltip;

        private Thread _converterThread;
        private State _state = State.Stopped;
        private bool _stoppingReasonIsError;
        private string _stoppingReason;

        private string _currentVideoInputDir;
        private string _currentVideoOutputDir;

        private readonly Regex _regexConverterTime = new Regex(@"time=(?<hours>\d\d)\:(?<minutes>\d\d)\:(?<secs>\d\d)\.(?<msecs>\d\d)", RegexOptions.Compiled);

        private readonly ToolStripDropDownMenu _tsOutputFilenameAddVariable = new();

        public DialogCreateVideo(ILog log)
        {
            _log = log;

            InitializeComponent();
        }

        #region Form Events

        private void DialogCreateVideo_Load(object sender, EventArgs e)
        {
            try
            {
                Tools.AddFormats(_tsOutputFilenameAddVariable.Items);
                _tsOutputFilenameAddVariable.ItemClicked += TsOutputFilenameAddVariableOnItemClicked;

                txtVideoConverterPath.Text = Settings.Default.OptionVideoConverterPath;
                txtVideoFilePattern.Text = Settings.Default.OptionVideoFilePattern;
                
                cbVideoSubDirsType.Items.Add(new VideoSubDirsTypeItem(Resources.info_NoSubDirs, VideoSubDirsType.NoSubDirs));
                cbVideoSubDirsType.Items.Add(new VideoSubDirsTypeItem(Resources.info_IncludeSubDirs, VideoSubDirsType.IncludeSubDirs));
                cbVideoSubDirsType.Items.Add(new VideoSubDirsTypeItem(Resources.info_OnlySubDirs, VideoSubDirsType.OnlySubDirs));
                cbVideoSubDirsType.SelectedIndex = Settings.Default.OptionVideoSubDirsType;

                txtVideoImagesPerSecond.Text = Settings.Default.OptionVideoImagesPerSecond.ToString("0.#####");

                cbVideoScaleType.Items.Add(new VideoScaleTypeItem(Resources.info_ScaleOriginal, VideoScaleType.Original));
                cbVideoScaleType.Items.Add(new VideoScaleTypeItem(Resources.info_ScaleHeight, VideoScaleType.Height));

                if ((VideoScaleType) Settings.Default.OptionVideoScaleType == VideoScaleType.Original)
                {
                    cbVideoScaleType.SelectedIndex = 0;
                    txtVideoScale.Text = string.Empty;
                }
                else
                {
                    cbVideoScaleType.SelectedIndex = 1;
                    txtVideoScale.Text = Settings.Default.OptionVideoScale.ToString();
                }

                txtVideoOutputDir.Text = Settings.Default.OptionVideoOutputDir;
                txtVideoOutputFilename.Text = Settings.Default.OptionVideoOutputFilename;
                
                if (Settings.Default.OptionUseVideoConverterCustomCommand)
                {
                    chkUseVideoConverterCustomCommand.Checked = true;
                    txtVideoConverterCommand.Text = Settings.Default.OptionVideoConverterCustomCommand;
                }
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
            finally
            {
                _loading = false;
                UpdateEnabledState();
            }
        }

        private void DialogCreateVideo_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_startTooltip))
                {
                    toolTipStart.Active = false;
                    return;
                }

                Control control = GetChildAtPoint(e.Location);
                if (control != btnStartStop)
                {
                    toolTipStart.Active = false;
                    return;
                }

                toolTipStart.Active = true;
                toolTipStart.Show(_startTooltip, this, PointToClient(Cursor.Position));
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void DialogCreateVideo_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_state != State.Stopped)
                {
                    if (MessageBox.Show(
                        this,
                        Resources.info_ReallyAbortRunningProcess,
                        Resources.caption_AbortRunningProcess,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    {
                        e.Cancel = true;
                        return;
                    }

                    Stop(false, false, Resources.info_StopReasonAborted);
                }
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        #endregion Form Events

        #region Control Events

        private void lblInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                // https://stackoverflow.com/a/61035650/998987
                Process.Start(
                    new ProcessStartInfo("https://ffmpeg.org/download.html")
                    {
                        UseShellExecute = true
                    });
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void btnSelectVideoConverterPath_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new();

                dlg.CheckFileExists = true;
                dlg.CheckPathExists = true;
                dlg.Filter = "Executables (*.exe)|*.exe|All files (*.*)|*.*";

                string luckyDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ffmpeg");

                if (File.Exists(txtVideoConverterPath.Text))
                {
                    dlg.InitialDirectory = Path.GetDirectoryName(txtVideoConverterPath.Text);
                    dlg.FileName = Path.GetFileName(txtVideoConverterPath.Text);
                }
                else if (Directory.Exists(txtVideoConverterPath.Text))
                {
                    dlg.InitialDirectory = txtVideoConverterPath.Text;
                }
                else if (Directory.Exists(luckyDir))
                {
                    dlg.InitialDirectory = luckyDir;
                    dlg.FileName = "ffmpeg.exe";
                }
                else
                {
                    dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                }

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    if (txtVideoConverterPath.Text != dlg.FileName)
                    {
                        txtVideoConverterPath.Text = dlg.FileName;
                    }
                    else
                    {
                        UpdateEnabledState();
                    }
                }
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void txtVideoConverterPath_TextChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateEnabledState();
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void txtVideoFileFilter_TextChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateEnabledState();
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void cbVideoSubDirsType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateEnabledState();
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void txtVideoFramerate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateEnabledState();
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void txtVideoScale_TextChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateEnabledState();
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void cbVideoScaleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbVideoScaleType.SelectedIndex == 0)
                {
                    if (txtVideoScale.Text != string.Empty)
                    {
                        txtVideoScale.Text = string.Empty;
                    }
                    else
                    {
                        UpdateEnabledState();
                    }
                }
                else
                {
                    if (txtVideoScale.Text != Settings.Default.OptionVideoScale.ToString())
                    {
                        txtVideoScale.Text = Settings.Default.OptionVideoScale.ToString();
                    }
                    else
                    {
                        UpdateEnabledState();
                    }
                }
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void txtVideoInputDir_TextChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateEnabledState();
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void btnSelectVideoInputDir_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog dlg = new();

                if (Directory.Exists(Settings.Default.OptionScreenShotDir))
                {
                    dlg.SelectedPath = Settings.Default.OptionScreenShotDir;
                }
                else
                {
                    string defaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                    dlg.SelectedPath = defaultDirectory;
                }

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    if (txtVideoInputDir.Text != dlg.SelectedPath)
                    {
                        txtVideoInputDir.Text = dlg.SelectedPath;
                    }
                    else
                    {
                        UpdateEnabledState();
                    }
                }
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void btnOpenVideoInputDir_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    string dir = txtVideoInputDir.Text;
                    Process.Start("explorer.exe", dir);
                }
                catch (Exception ex)
                {
                    _log.WriteError(ex);
                }
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void txtVideoOutputDir_TextChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateEnabledState();
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void btnSelectVideoOutputDir_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog dlg = new();

                if (Directory.Exists(_currentVideoOutputDir))
                {
                    dlg.SelectedPath = _currentVideoOutputDir;
                }
                else
                {
                    string defaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                    dlg.SelectedPath = defaultDirectory;
                }

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    if (txtVideoOutputDir.Text != dlg.SelectedPath)
                    {
                        txtVideoOutputDir.Text = dlg.SelectedPath;
                    }
                    else
                    {
                        UpdateEnabledState();
                    }
                }
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void btnOpenVideoOutputDir_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("explorer.exe", _currentVideoOutputDir);
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
            }
        }

        private void txtVideoOutputFilename_TextChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateEnabledState();
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void btnOutputFilenameAddVariable_Click(object sender, EventArgs e)
        {
            try
            {
                _tsOutputFilenameAddVariable.Show(btnOutputFilenameAddVariable, new Point(0, btnOutputFilenameAddVariable.Height), ToolStripDropDownDirection.BelowRight);
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void TsOutputFilenameAddVariableOnItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                int startIndex = e.ClickedItem.Text.IndexOf('{');
                int endIndex = e.ClickedItem.Text.IndexOf('}');
                txtVideoOutputFilename.Paste(e.ClickedItem.Text.Substring(startIndex, endIndex - startIndex + 1));
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void chkVideoConverterCustomCommand_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (_loading)
                {
                    return;
                }

                if (chkUseVideoConverterCustomCommand.Checked)
                {
                    if (!string.IsNullOrWhiteSpace(Settings.Default.OptionVideoConverterCustomCommand))
                    {
                        txtVideoConverterCommand.Text = Settings.Default.OptionVideoConverterCustomCommand;
                    }
                    else
                    {
                        txtVideoConverterCommand.Text = "\"" + txtVideoConverterPath.Text.Trim() + "\"" + " " + GetArguments();
                    }
                }

                UpdateEnabledState();
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void txtVideoConverterCommand_TextChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateEnabledState();
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (_state == State.Stopped)
                {
                    _log.WriteInfo(Resources.info_VideoConverterStarting);

                    //ScreenItem screenItem = (ScreenItem) cbScreen.SelectedItem;
                    //_log.WriteInfo(
                    //    Resources.info_DumpScreen.Swap(cbScreen.SelectedIndex, screenItem.Value.Primary, screenItem.Value.DeviceName, screenItem.Value.Bounds, screenItem.Value.BitsPerPixel));

                    //_log.WriteInfo(Resources.info_DumpDirectory.Swap(txtScreenShotDirectory.Text));
                    //_log.WriteInfo(Resources.info_DumpInterval.Swap(txtScreenShotInterval.Text, cbScreenShotIntervalUnit.Text));

                    _state = State.Starting;

                    _converterThread = new Thread(ConverterThread);
                    _converterThread.Start();

                    UpdateEnabledState();
                }
                else
                {
                    Stop(false, false, Resources.info_StopReasonAborted);
                }
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        #endregion Control Events

        #region Private Methods

        private void UpdateEnabledState()
        {
            try
            {
                if (_loading)
                {
                    return;
                }

                if (_state == State.Starting)
                {
                    tsStatus.Text = Resources.info_StatusStarting;
                }
                else if (_state == State.Stopping)
                {
                    tsStatus.Text = Resources.info_StatusStopping;
                }

                if (_state != State.Stopped)
                {
                    foreach (Control control in gbOptions.Controls)
                    {
                        if (control != btnOpenVideoInputDir &&
                            control != btnOpenVideoOutputDir)
                        {
                            control.Enabled = false;
                        }
                    }

                    foreach (Control control in gbCommand.Controls)
                    {
                        control.Enabled = false;
                    }

                    btnStartStop.Text = Resources.info_Stop;
                    return;
                }

                foreach (Control control in gbOptions.Controls)
                {
                    control.Enabled = true;
                }

                foreach (Control control in gbCommand.Controls)
                {
                    control.Enabled = true;
                }

                btnStartStop.Text = Resources.info_Start;

                bool startEnabled = true;
                _startTooltip = null;

                if (File.Exists(txtVideoConverterPath.Text))
                {
                    Settings.Default.OptionVideoConverterPath = txtVideoConverterPath.Text;
                }
                else
                {
                    startEnabled = false;
                    _startTooltip = Resources.tooltip_VideoConverterPathNotFound;
                }

                Settings.Default.OptionVideoFilePattern = txtVideoFilePattern.Text;
                Settings.Default.OptionVideoSubDirsType = cbVideoSubDirsType.SelectedIndex;

                if (float.TryParse(txtVideoImagesPerSecond.Text, out float frameRate) && frameRate >= 0)
                {
                    Settings.Default.OptionVideoImagesPerSecond = frameRate;
                }
                else
                {
                    startEnabled = false;
                    _startTooltip = Resources.tooltip_InvalidFramerate;
                }

                VideoScaleTypeItem scaleItem = (VideoScaleTypeItem) cbVideoScaleType.SelectedItem;
                Settings.Default.OptionVideoScaleType = (int) scaleItem.Value;

                if (scaleItem.Value == VideoScaleType.Height)
                {
                    txtVideoScale.Enabled = true;

                    if (int.TryParse(txtVideoScale.Text, out int scale) && scale > 0)
                    {
                        Settings.Default.OptionVideoScale = scale;
                    }
                    else
                    {
                        startEnabled = false;
                        _startTooltip = Resources.tooltip_InvalidScale;
                    }
                }
                else
                {
                    txtVideoScale.Enabled = false;
                }

                _currentVideoInputDir = txtVideoInputDir.Text;
                if (!Directory.Exists(_currentVideoInputDir))
                {
                    startEnabled = false;
                    _startTooltip ??= Resources.tooltip_InvalidVideoInputDir;
                }

                _currentVideoOutputDir = txtVideoOutputDir.Text;
                Settings.Default.OptionVideoOutputDir = txtVideoOutputDir.Text;

                if (string.IsNullOrWhiteSpace(_currentVideoOutputDir))
                {
                    _currentVideoOutputDir = _currentVideoInputDir;
                }
                else if (!Path.IsPathRooted(_currentVideoOutputDir))
                {
                    _currentVideoOutputDir = Path.Combine(_currentVideoInputDir, _currentVideoOutputDir);
                }

                if (!Directory.Exists(_currentVideoOutputDir))
                {
                    startEnabled = false;
                    _startTooltip ??= Resources.tooltip_VideoOutpuDirDoesNotExist;
                }

                if (string.IsNullOrEmpty(txtVideoOutputFilename.Text))
                {
                    startEnabled = false;
                    _startTooltip ??= Resources.tooltip_InvalidVideoOutputFilename;
                }
                else
                {
                    Settings.Default.OptionVideoOutputFilename = txtVideoOutputFilename.Text;
                }

                if (chkUseVideoConverterCustomCommand.Checked)
                {
                    txtVideoConverterCommand.ReadOnly = false;
                    txtVideoConverterCommand.BackColor = Color.White;

                    Settings.Default.OptionVideoConverterCustomCommand = txtVideoConverterCommand.Text;
                    Settings.Default.OptionUseVideoConverterCustomCommand = true;
                }
                else
                {
                    txtVideoConverterCommand.ReadOnly = true;
                    txtVideoConverterCommand.BackColor = SystemColors.Control;

                    string converterPath = txtVideoConverterPath.Text.Trim();
                    txtVideoConverterCommand.Text = $"\"{converterPath}\" {GetArguments()}";

                    Settings.Default.OptionUseVideoConverterCustomCommand = false;
                }

                Settings.Default.Save();

                if (TryGetInputFilesInfo(txtVideoInputDir.Text, txtVideoFilePattern.Text, (VideoSubDirsType) cbVideoSubDirsType.SelectedIndex, out int inputFileCount, out string firstInputFilePath, out int width, out int height))
                {
                    txtFileCount.Text = inputFileCount.ToString();
                    txtFirstFile.Text = Path.GetFileName(firstInputFilePath);
                    txtFirstFileDimensions.Text = $"{width} x {height}";

                    if (frameRate > 0)
                    {
                        int totalMsecs = (int) (inputFileCount / frameRate * 1000f);

                        Tools.GetReadableDuration(totalMsecs, totalMsecs < 10000.0, out string duration);
                        txtEstimatedDuration.Text = duration;
                    }
                    else
                    {
                        txtEstimatedDuration.Text = string.Empty;
                    }
                }
                else
                {
                    txtFileCount.Text = "0";
                    txtFirstFile.Text = string.Empty;
                    txtFirstFileDimensions.Text = string.Empty;
                    txtEstimatedDuration.Text = string.Empty;
                }

                if (startEnabled)
                {
                    btnStartStop.Enabled = true;

                    toolTipStart.Active = false;
                }
                else
                {
                    btnStartStop.Enabled = false;

                    Debug.Assert(!string.IsNullOrEmpty(_startTooltip), "Start button tooltip must be set.");
                }

                if (_stoppingReason != null)
                {
                    if (_stoppingReasonIsError)
                    {
                        tsStatus.Text = Resources.info_StatusError.Swap(_stoppingReason);
                    }
                    else
                    {
                        tsStatus.Text = _stoppingReason;
                    }

                    // BUG: Text in Status is not visible when too long: https://stackoverflow.com/q/38155313/998987 
                    if (tsStatus.Text.Length > 100)
                    {
                        tsStatus.Text = tsStatus.Text.Substring(0, 100) + "...";
                    }

                    _stoppingReasonIsError = false;
                    _stoppingReason = null;
                }
                else if (startEnabled)
                {
                    tsStatus.Text = Resources.info_StatusReady;
                }
                else
                {
                    tsStatus.Text = _startTooltip;
                }
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private string GetArguments()
        {
            Tools.TryApplyFormat(Settings.Default.OptionVideoOutputFilename, DateTime.Now, 1, out string outputFilename, out _);

            string outputPath = Path.Combine(_currentVideoOutputDir, outputFilename);

            return GetArguments(outputPath);
        }

        private string GetArguments(string outputPath)
        {
            string arguments = string.Empty;

            // Can't use glob_pattern. Not available on Windows.
            // Can't use static pattern like "Screenshot_%d5.png". Does not work for all cases.
            // Let's use an input file instead!

            arguments += " -f concat -i input.txt";

            arguments += " -codec:v libx264 -pix_fmt yuv420p -b:v 2500k -minrate 1500k -maxrate 4000k -bufsize 5000k";

            if ((VideoScaleType) Settings.Default.OptionVideoScaleType == VideoScaleType.Height)
            {
                if (Settings.Default.OptionVideoScale > 0)
                {
                    arguments += $" -vf scale=-1:{Settings.Default.OptionVideoScale}";
                }
            }

            if (!string.IsNullOrWhiteSpace(outputPath))
            {
                arguments += $" \"{outputPath.Trim()}\"";
            }

            return arguments.Trim();
        }

        private List<string> GetInputFilePaths(string inputDir, string pattern, VideoSubDirsType subDirsType)
        {
            if (!Directory.Exists(inputDir))
            {
                return new List<string>();
            }

            if (string.IsNullOrWhiteSpace(pattern))
            {
                pattern = "*.*";
            }

            List<string> inputFilePaths;

            if (subDirsType == VideoSubDirsType.NoSubDirs)
            {
                inputFilePaths = Directory.GetFiles(inputDir, pattern).ToList();
                inputFilePaths.Sort();
            }
            else if (subDirsType == VideoSubDirsType.IncludeSubDirs)
            {
                inputFilePaths = Directory.GetFiles(inputDir, pattern, SearchOption.AllDirectories).ToList();
                inputFilePaths.Sort();
            }
            else
            {
                List<string> subDirs = Directory.GetDirectories(inputDir).ToList();

                subDirs.Sort();

                inputFilePaths = new List<string>();
                foreach (string subDir in subDirs)
                {
                    List<string> currentPaths = Directory.GetFiles(subDir, pattern).ToList();
                    currentPaths.Sort();

                    inputFilePaths.AddRange(currentPaths);
                }
            }

            if (inputFilePaths.Count == 0)
            {
                return inputFilePaths;
            }

            return inputFilePaths;
        }

        private bool TryGetInputFilesInfo(string inputDir, string pattern, VideoSubDirsType subDirsType, out int inputFileCount, out string firstInputFilePath, out int width, out int height)
        {
            inputFileCount = 0;
            firstInputFilePath = null;
            width = 0;
            height = 0;

            List<string> inputFilePaths = GetInputFilePaths(inputDir, pattern, subDirsType);

            if (inputFilePaths.Count == 0)
            {
                return false;
            }

            inputFileCount = inputFilePaths.Count;
            firstInputFilePath = inputFilePaths[0];

            try
            {
                using (Bitmap bitmap = new Bitmap(firstInputFilePath))
                {
                    width = bitmap.Width;
                    height = bitmap.Height;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
            }

            return true;
        }

        private void ConverterThread()
        {
            string concatInputPath = null;
            Process converterProcess = null;

            try
            {
                DateTime now = new DateTime();

                _stoppingReason = null;
                _stoppingReasonIsError = false;

                UpdateStatus(0);

                string workingDirectory = Path.GetFullPath(_currentVideoInputDir.Trim());

                List<string> inputFilePaths = GetInputFilePaths(_currentVideoInputDir, Settings.Default.OptionVideoFilePattern, (VideoSubDirsType) Settings.Default.OptionVideoSubDirsType);

                if (inputFilePaths.Count == 0)
                {
                    throw new Exception(Resources.warn_NoInputFilesFound);
                }

                if (_state == State.Stopping)
                {
                    Stop(true, false, Resources.info_StopReasonAborted);
                    return;
                }

                concatInputPath = Path.Combine(_currentVideoInputDir, "input.txt");
                float fps = 1f / Settings.Default.OptionVideoImagesPerSecond;

                using (StreamWriter writer = new StreamWriter(File.Open(concatInputPath, FileMode.Create)))
                {
                    string relativePath;
                    foreach (string inputPath in inputFilePaths)
                    {
                        relativePath = inputPath.Substring(workingDirectory.Length).TrimStart(Path.DirectorySeparatorChar).Replace('\\', '/');

                        writer.WriteLine($"file '{relativePath}'");
                        writer.WriteLine($"duration {fps:0.#####}");

                        if (_state == State.Stopping)
                        {
                            Stop(true, false, Resources.info_StopReasonAborted);
                            return;
                        }
                    }

                    relativePath = inputFilePaths.Last().Substring(workingDirectory.Length).TrimStart(Path.DirectorySeparatorChar).Replace('\\', '/');;

                    // Necessary 'quirk' according to ffmpeg concat documentation...
                    writer.WriteLine($"file '{relativePath}'");
                }


                int fileCounter = 1;
                Tools.TryApplyFormat(Settings.Default.OptionVideoOutputFilename, now, fileCounter, out string outputFilename, out bool hasCounter);

                string outputPath = Path.Combine(_currentVideoOutputDir, outputFilename);
                if (File.Exists(outputPath))
                {
                    if (hasCounter)
                    {
                        while(true)
                        {
                            fileCounter += 1;
                            if (!Tools.TryApplyFormat(Settings.Default.OptionVideoOutputFilename, now, fileCounter, out outputFilename, out _))
                            {
                                break;
                            }

                            outputPath = Path.Combine(_currentVideoOutputDir, outputFilename);

                            if (!File.Exists(outputPath))
                            {
                                break;
                            }
                        }
                    }
                }

                if (File.Exists(outputPath))
                {
                    if (!AskFileOverwrite(outputPath))
                    {
                        throw new Exception(Resources.error_FileAlreadyExists.Swap(Path.GetFileName(outputPath)));
                    }

                    File.Delete(outputPath);
                }

                if (Settings.Default.OptionUseVideoConverterCustomCommand)
                {
                    converterProcess = Process.Start(
                        new ProcessStartInfo(Settings.Default.OptionVideoConverterCustomCommand)
                        {
                            //UseShellExecute = true,
                            CreateNoWindow = true,
                            WorkingDirectory = workingDirectory,
                            //RedirectStandardOutput = true,
                            RedirectStandardError = true
                        });
                }
                else
                {
                    converterProcess = Process.Start(
                        new ProcessStartInfo(Settings.Default.OptionVideoConverterPath.Trim(), GetArguments(outputPath))
                        {
                            //UseShellExecute = true,
                            CreateNoWindow = true,
                            WorkingDirectory = workingDirectory,
                            //RedirectStandardOutput = true,
                            RedirectStandardError = true
                        });
                }

                if (converterProcess == null)
                {
                    throw new Exception(Resources.error_FailedToCreateProcess);
                }

                float totalDurationSecs = (float) inputFilePaths.Count / Settings.Default.OptionVideoImagesPerSecond;

                while (!converterProcess.StandardError.EndOfStream)
                {
                    string line = converterProcess.StandardError.ReadLine();
                    _log.WriteInfo(Resources.info_ConverterOutput.Swap(line));

                    Match match = _regexConverterTime.Match(line);
                    if (match.Success)
                    {
                        int hours = int.Parse(match.Groups["hours"].Value);
                        int minutes = int.Parse(match.Groups["minutes"].Value);
                        int secs = int.Parse(match.Groups["secs"].Value);
                        int msecs = int.Parse(match.Groups["msecs"].Value) * 10;

                        TimeSpan current = new TimeSpan(0, hours, minutes, secs, msecs);
                        float currentDurationSecs = (float) current.TotalSeconds;

                        float progressPercent = currentDurationSecs / totalDurationSecs * 100f;
                        UpdateStatus(progressPercent);
                    }

                    if (_state == State.Stopping)
                    {
                        Stop(true, false, Resources.info_StopReasonAborted);
                        return;
                    }
                }

                if (converterProcess.ExitCode == 0)
                {
                    Stop(true, false, Resources.info_VideoConverterDone);
                }
                else
                {
                    Stop(true, true, Resources.error_VideoConverterReturnsError);
                }
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Stop(true, true, ex.Message);
                
                Debug.Fail(ex.ToString());
            }
            finally
            {
                try
                {
                    if (converterProcess != null && !converterProcess.WaitForExit(250))
                    {
                        converterProcess.Kill();
                        converterProcess.WaitForExit(250);
                    }

                    if (concatInputPath != null && File.Exists(concatInputPath))
                    {
                        File.Delete(concatInputPath);
                    }
                }
                catch (Exception ex)
                {
                    _log.WriteWarning(ex);
                    Debug.Fail(ex.ToString());
                }

                _log.WriteInfo(Resources.info_VideoConverterStopped);
            }
        }

        private void UpdateStatus(float progressPercent)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdateStatus(progressPercent)));
                return;
            }

            if (_state == State.Stopping)
            {
                return;
            }

            if (progressPercent <= 0)
            {
                tsStatus.Text = Resources.info_StatusRunning;
            }
            else
            {
                if (progressPercent > 100f)
                {
                    progressPercent = 100f;
                }


                tsStatus.Text = $"{Resources.info_StatusRunning} ({progressPercent:0.0}%)";
            }
        }

        private bool AskFileOverwrite(string outputPath)
        {
            if (InvokeRequired)
            {
                bool overwrite = false;
                Invoke(new Action(() => { overwrite = AskFileOverwrite(outputPath); }));
                return overwrite;
            }

            return MessageBox.Show(
                this,
                Resources.info_FileExistsOverwrite.Swap(outputPath),
                Resources.caption_Overwrite,
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question) == DialogResult.OK;
        }

        private void Stop(bool fromThread, bool isError, string reason)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Stop(fromThread, isError, reason)));
                return;
            }

            if (_state == State.Stopped)
            {
                return;
            }

            if (isError)
            {
                _log.WriteError(Resources.info_VideoConverterStopping.Swap(reason));
            }
            else
            {
                _log.WriteInfo(Resources.info_VideoConverterStopping.Swap(reason));
            }

            if (fromThread)
            {
                _state = State.Stopped;
            }
            else
            {
                _state = State.Stopping;
            }

            _stoppingReasonIsError = isError;
            _stoppingReason = reason;

            UpdateEnabledState();
        }

        #endregion Private Methods
    }
}
