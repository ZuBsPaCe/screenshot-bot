using ScreenShotBot.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using IntervalItem = ScreenShotBot.ComboItem<ScreenShotBot.IntervalUnit>;
using ScreenItem = ScreenShotBot.ComboItem<System.Windows.Forms.Screen>;

namespace ScreenShotBot
{
    public partial class FormMain : Form
    {
        private readonly ILog _log;

        private bool _loading = true;

        private State _state = State.Stopped;
        private string _stoppingError;

        private bool _countdownShowMsecs;
        private int _countdownMsecs;
        private readonly Stopwatch _countdownStopwatch = new();

        private int _notifyIconNum = 1;
        private readonly Stopwatch _changeNotifyIconStopwatch = new();

        private readonly Thread _screenShotThread;
        private readonly AutoResetEvent _screenShotResetEvent = new(true);
        private string _lastSaveSubDir;
        private bool _exitThread;

        private Screen _selectedScreen;

        private readonly ToolStripDropDownMenu _tsSubdirectoryAddVariable = new();
        private readonly ToolStripDropDownMenu _tsFilenamesAddVariable = new();

        private string _startTooltip;

        private FormWindowState _preferredWindowState = FormWindowState.Normal;

        public FormMain(ILog log)
        {
            _log = log;
            _screenShotThread = new Thread(ScreenShotThread);

            InitializeComponent();
        }

        #region Form Events

        private void FormMain_Load(object sender, EventArgs e)
        {
            try
            {
                tsFile.DropDownItemClicked += menuStrip_ItemClicked;
                tsEdit.DropDownItemClicked += menuStrip_ItemClicked;
                tsTools.DropDownItemClicked += menuStrip_ItemClicked;
                tsHelp.DropDownItemClicked += menuStrip_ItemClicked;

                txtScreenShotDirectory.Text = Settings.Default.OptionScreenShotDir;

                int preferredScreenIndex = -1;
                for (var i = 0; i < Screen.AllScreens.Length; i++)
                {
                    Screen screen = Screen.AllScreens[i];
                    string displayName;
                    if (screen.Primary)
                    {
                        displayName = Resources.info_ScreenPrimary.Swap(i);
                        preferredScreenIndex = i;
                    }
                    else
                    {
                        displayName = Resources.info_Screen.Swap(i);
                    }

                    cbScreen.Items.Add(new ScreenItem(displayName, screen));
                }

                if (Settings.Default.OptionScreenIndex >= 0 && Settings.Default.OptionScreenIndex < cbScreen.Items.Count)
                {
                    cbScreen.SelectedIndex = Settings.Default.OptionScreenIndex;
                }
                else if (preferredScreenIndex >= 0)
                {
                    cbScreen.SelectedIndex = preferredScreenIndex;
                }
                else
                {
                    cbScreen.Items.Add(new ScreenItem(Resources.info_NoScreensDetected, null));
                    cbScreen.SelectedIndex = 0;
                }


                Tools.AddFormats(_tsSubdirectoryAddVariable.Items); 
                _tsSubdirectoryAddVariable.ItemClicked += TsSubdirectoryAddVariableOnItemClicked;

                txtScreenShotSubDir.Text = Settings.Default.OptionScreenShotSubDir;
                txtScreenShotSubDir.SelectionStart = txtScreenShotSubDir.Text.Length;


                Tools.AddFormats(_tsFilenamesAddVariable.Items);
                _tsFilenamesAddVariable.ItemClicked += TsFilenamesAddVariableOnItemClicked;

                txtScreenShotFilename.Text = Settings.Default.OptionScreenShotFilename;
                txtScreenShotFilename.SelectionStart = txtScreenShotFilename.Text.Length;


                if (Settings.Default.OptionScreenShotInterval >= 0)
                {
                    txtScreenShotInterval.Text = Settings.Default.OptionScreenShotInterval.ToString("0.###");
                }
                else
                {
                    txtScreenShotInterval.Text = "10";
                }

                foreach (IntervalUnit intervalUnit in Enum.GetValues<IntervalUnit>())
                {
                    cbScreenShotIntervalUnit.Items.Add(new IntervalItem(Tools.GetEnumString(Resources.ResourceManager, intervalUnit), intervalUnit));
                }

                if (Settings.Default.OptionScreenShotIntervalUnits >= 0 && Settings.Default.OptionScreenShotIntervalUnits < cbScreenShotIntervalUnit.Items.Count)
                {
                    cbScreenShotIntervalUnit.SelectedIndex = Settings.Default.OptionScreenShotIntervalUnits;
                }
                else
                {
                    cbScreenShotIntervalUnit.SelectedIndex = (int) IntervalUnit.Secs;
                }

                timerUpdateCountdown.Start();
                _changeNotifyIconStopwatch.Start();

                _screenShotThread.Start();
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

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
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
                }

                _log.WriteDebug(Resources.debug_ApplicationFormClosingStarted);

                _exitThread = true;
                _screenShotResetEvent.Set();
                _screenShotThread.Join(4000);

                _log.WriteDebug(Resources.debug_ApplicationFormClosingDone);
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void FormMain_MouseMove(object sender, MouseEventArgs e)
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

        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                if (WindowState == FormWindowState.Normal)
                {
                    _preferredWindowState = FormWindowState.Normal;
                }
                else if (WindowState == FormWindowState.Maximized)
                {
                    _preferredWindowState = FormWindowState.Maximized;
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

        private void menuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                if (e.ClickedItem == miExit)
                {
                    Close();
                }
                else if (e.ClickedItem == miPreferences)
                {

                }
                else if (e.ClickedItem == miCreateVideo)
                {
                    try
                    {
                        notifyIcon.ContextMenuStrip.Enabled = false;
                        DialogCreateVideo dlg = new DialogCreateVideo(_log);
                        dlg.ShowDialog(this);
                    }
                    finally
                    {
                        notifyIcon.ContextMenuStrip.Enabled = true;
                    }
                }
                else if (e.ClickedItem == miAbout)
                {

                }
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void cbScreen_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbScreen.SelectedIndex >= 0)
                {
                    _selectedScreen = ((ScreenItem) cbScreen.SelectedItem).Value;
                }
                else
                {
                    _selectedScreen = null;
                }
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void txtScreenShotDirectory_TextChanged(object sender, EventArgs e)
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

        private void btnSelectScreenShotDirectory_Click(object sender, EventArgs e)
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
                    if (txtScreenShotDirectory.Text != dlg.SelectedPath)
                    {
                        txtScreenShotDirectory.Text = dlg.SelectedPath;
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

        private void btnOpenScreenShotDirectory_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    string dir = txtScreenShotDirectory.Text;
                    if (!dir.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    {
                        dir += Path.DirectorySeparatorChar;
                    }

                    if (_lastSaveSubDir == null || !Directory.Exists(Path.Combine(dir, _lastSaveSubDir)))
                    {
                        Process.Start("explorer.exe", dir);
                    }
                    else
                    {
                        var info = new ProcessStartInfo
                        {
                            FileName = "explorer.exe", 
                            Arguments = $"/select, \"{Path.Combine(dir, _lastSaveSubDir)}\"" 
                        };
                        Process.Start(info);
                    }
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

        private void txtScreenShotSubDir_TextChanged(object sender, EventArgs e)
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

        private void btnSubdirectoryAddVariable_Click(object sender, EventArgs e)
        {
            try
            {
                _tsSubdirectoryAddVariable.Show(btnSubdirectoryAddVariable, new Point(0, btnSubdirectoryAddVariable.Height), ToolStripDropDownDirection.BelowRight);
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void TsSubdirectoryAddVariableOnItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                int startIndex = e.ClickedItem.Text.IndexOf('{');
                int endIndex = e.ClickedItem.Text.IndexOf('}');
                txtScreenShotSubDir.Paste(e.ClickedItem.Text.Substring(startIndex, endIndex - startIndex + 1));
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void txtScreenShotFilenames_TextChanged(object sender, EventArgs e)
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

        private void btnFilenamesAddVariable_Click(object sender, EventArgs e)
        {
            try
            {
                _tsFilenamesAddVariable.Show(btnFilenamesAddVariable, new Point(0, btnSubdirectoryAddVariable.Height), ToolStripDropDownDirection.BelowRight);
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void TsFilenamesAddVariableOnItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                int startIndex = e.ClickedItem.Text.IndexOf('{');
                int endIndex = e.ClickedItem.Text.IndexOf('}');
                txtScreenShotFilename.Paste(e.ClickedItem.Text.Substring(startIndex, endIndex - startIndex + 1));
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void txtScreenShotInterval_TextChanged(object sender, EventArgs e)
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

        private void cbScreenShotIntervalUnit_SelectedIndexChanged(object sender, EventArgs e)
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

        private void timerUpdateCountdown_Tick(object sender, EventArgs e)
        {
            try
            {
                if (_state == State.Running)
                {
                    int currentMsecs = _countdownMsecs - (int) _countdownStopwatch.ElapsedMilliseconds;
                    if (currentMsecs < 0)
                    {
                        currentMsecs = 0;
                    }

                    Tools.GetReadableDuration(currentMsecs, _countdownShowMsecs, out string countdown);

                    txtCountdown.Text = countdown;

                    if (_changeNotifyIconStopwatch.ElapsedMilliseconds > 2000)
                    {
                        _changeNotifyIconStopwatch.Restart();

                        if (_notifyIconNum == 1)
                        {
                            Icon = notifyIcon.Icon = Resources.IconRed2;
                            _notifyIconNum = 2;
                        }
                        else if (_notifyIconNum == 2)
                        {
                            Icon = notifyIcon.Icon = Resources.IconRed3;
                            _notifyIconNum = 3;
                        }
                        else if (_notifyIconNum == 3)
                        {
                            Icon = notifyIcon.Icon = Resources.IconRed4;
                            _notifyIconNum = 4;
                        }
                        else
                        {
                            Icon = notifyIcon.Icon = Resources.IconRed1;
                            _notifyIconNum = 1;
                        }
                    }
                }
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
                    _log.WriteInfo(Resources.info_StartScreenShots);

                    ScreenItem screenItem = (ScreenItem) cbScreen.SelectedItem;
                    _log.WriteInfo(
                        Resources.info_DumpScreen.Swap(cbScreen.SelectedIndex, screenItem.Value.Primary, screenItem.Value.DeviceName, screenItem.Value.Bounds, screenItem.Value.BitsPerPixel));

                    _log.WriteInfo(Resources.info_DumpDirectory.Swap(txtScreenShotDirectory.Text));
                    _log.WriteInfo(Resources.info_DumpInterval.Swap(txtScreenShotInterval.Text, cbScreenShotIntervalUnit.Text));

                    _state = State.Starting;

                    Icon = notifyIcon.Icon = Resources.IconRed1;
                    _notifyIconNum = 1;
                    _changeNotifyIconStopwatch.Restart();

                    _screenShotResetEvent.Set();

                    UpdateEnabledState();
                }
                else
                {
                    Stop(false, Resources.info_StopReasonAborted);
                }
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void notifyIcon_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    ShowWindow();
                }
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void contextMenuNotifyIcon_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                if (e.ClickedItem == miNotifyIconShow)
                {
                    ShowWindow();
                }
                else if (e.ClickedItem == mitNotifyIconStartStop)
                {
                    btnStartStop_Click(mitNotifyIconStartStop, EventArgs.Empty);
                }
                else if (e.ClickedItem == miNotifyIconExit)
                {
                    Close();
                }
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Debug.Fail(ex.ToString());
            }
        }

        private void miAbout_Click(object sender, EventArgs e)
        {
            try
            {
                var dlg = new DialogAbout();
                dlg.ShowDialog(this);
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
                    _countdownStopwatch.Stop();

                    _state = State.Stopped;
                }

                if (_state != State.Stopped)
                {
                    foreach (Control control in gbOptions.Controls)
                    {
                        if (control != btnOpenScreenShotDirectory)
                        {
                            control.Enabled = false;
                        }
                    }

                    btnStartStop.Text = Resources.info_Stop;
                    mitNotifyIconStartStop.Text = Resources.info_Stop;
                    miCreateVideo.Enabled = false;
                    return;
                }

                foreach (Control control in gbOptions.Controls)
                {
                    control.Enabled = true;
                }

                btnStartStop.Text = Resources.info_Start;
                mitNotifyIconStartStop.Text = Resources.info_Start;
                miCreateVideo.Enabled = true;

                Icon = notifyIcon.Icon = Resources.IconGreen;

                bool startEnabled = true;
                _startTooltip = null;

                if (Directory.Exists(txtScreenShotDirectory.Text))
                {
                    Settings.Default.OptionScreenShotDir = txtScreenShotDirectory.Text;
                    btnOpenScreenShotDirectory.Enabled = true;
                }
                else
                {
                    startEnabled = false;
                    _startTooltip = Resources.tooltip_ScreenShotDirectoryNotFound;
                    btnOpenScreenShotDirectory.Enabled = false;
                }

                ScreenItem screenItem = (ScreenItem) cbScreen.SelectedItem;
                if (screenItem.Value != null)
                {
                    Settings.Default.OptionScreenIndex = cbScreen.SelectedIndex;
                }
                else
                {
                    startEnabled = false;
                    _startTooltip ??= Resources.tooltip_InvalidScreen;
                }

                if (string.IsNullOrWhiteSpace(txtScreenShotSubDir.Text) ||
                    TryFormatScreenshotSubDir(txtScreenShotSubDir.Text, DateTime.Now, 1, out _, out _))
                {
                    Settings.Default.OptionScreenShotSubDir = txtScreenShotSubDir.Text;
                }
                else
                {
                    startEnabled = false;
                    _startTooltip ??= Resources.tooltip_InvalidScreenShotSubDir;
                }


                if (!string.IsNullOrWhiteSpace(txtScreenShotFilename.Text) &&
                    TryFormatScreenshotFilename(txtScreenShotFilename.Text, DateTime.Now, 1, out _, out _, out var extension))
                {
                    string format = txtScreenShotFilename.Text.Trim();
                    Settings.Default.OptionScreenShotFilename = format;
                    Settings.Default.OptionScreenShotExtension = (int) extension;
                }
                else
                {
                    startEnabled = false;
                    _startTooltip ??= Resources.tooltip_InvalidScreenShotFilename;
                }


                Settings.Default.OptionScreenShotIntervalUnits = cbScreenShotIntervalUnit.SelectedIndex;

                if (!string.IsNullOrWhiteSpace(txtScreenShotInterval.Text) &&
                    float.TryParse(txtScreenShotInterval.Text, out float screenShotInterval) &&
                    TryGetIntervalMsecs(screenShotInterval, (IntervalUnit) cbScreenShotIntervalUnit.SelectedIndex, out _))
                {
                    Settings.Default.OptionScreenShotInterval = screenShotInterval;
                }
                else
                {
                    startEnabled = false;
                    _startTooltip ??= Resources.tooltip_InvalidScreenShotInterval;
                }


                Settings.Default.Save();

                if (startEnabled)
                {
                    btnStartStop.Enabled = true;
                    mitNotifyIconStartStop.Enabled = true;

                    toolTipStart.Active = false;
                }
                else
                {
                    btnStartStop.Enabled = false;
                    mitNotifyIconStartStop.Enabled = false;

                    Debug.Assert(!string.IsNullOrEmpty(_startTooltip), "Start button tooltip must be set.");
                }

                if (_stoppingError != null)
                {
                    tsStatus.Text = Resources.info_StatusError.Swap(_stoppingError);
                    _stoppingError = null;
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

        private bool TryGetIntervalMsecs(float interval, IntervalUnit unit, out int intervalMsecs)
        {
            try
            {
                switch (unit)
                {
                    case IntervalUnit.MSecs:
                        intervalMsecs = (int) interval;
                        break;

                    case IntervalUnit.Minutes:
                        intervalMsecs = (int) (interval * 60f * 1000f);
                        break;

                    case IntervalUnit.Hours:
                        intervalMsecs = (int) (interval * 60f * 60f * 1000f);
                        break;

                    case IntervalUnit.Days:
                        intervalMsecs = (int) (interval * 24f * 60f * 60f * 1000f);
                        break;

                    default:
                        Debug.Assert(unit == IntervalUnit.Secs, $"Unknown unit [{unit}]");
                        intervalMsecs = (int) (interval * 1000f);
                        break;
                }

                return intervalMsecs >= 0 && intervalMsecs <= float.MaxValue;
            }
            catch
            {
                intervalMsecs = -1;
                return false;
            }
        }

        private bool TryFormatScreenshotSubDir(string format, DateTime dateTime, int counter, out string result, out bool hasCounter)
        {
            return Tools.TryApplyFormat(format, dateTime, counter, out result, out hasCounter);
        }

        private bool TryFormatScreenshotFilename(string format, DateTime dateTime, int counter, out string result, out bool hasCounter, out ScreenShotExtension extension)
        {
            extension = ScreenShotExtension.PNG;

            if (!Tools.TryApplyFormat(format, dateTime, counter, out result, out hasCounter))
            {
                return false;
            }

            string ext = Path.GetExtension(result);

            if (string.IsNullOrWhiteSpace(ext) || ext.Length < 4 || !ext.StartsWith('.'))
            {
                result = null;
                return false;
            }

            if (!Enum.TryParse(ext.Substring(1), true, out extension))
            {
                result = null;
                return false;
            }

            return true;
        }

        private void Stop(bool isError, string reason)
        {

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => Stop(isError, reason)));
                return;
            }

            if (_state == State.Stopped)
            {
                return;
            }

            if (isError)
            {
                _log.WriteError(Resources.info_ScreenshotCaptureStopping.Swap(reason));
            }
            else
            {
                _log.WriteInfo(Resources.info_ScreenshotCaptureStopping.Swap(reason));
            }

            _state = State.Stopping;

            if (isError)
            {
                _stoppingError = reason;
            }


            UpdateEnabledState();
        }

        private void UpdateStatus(int countdownMsecs, string lastFile, int savedFiles, long lastFileSizeBytes, long totalSizeBytes)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdateStatus(countdownMsecs, lastFile, savedFiles, lastFileSizeBytes, totalSizeBytes)));
                return;
            }

            tsStatus.Text = Resources.info_StatusRunning;

            _countdownShowMsecs = countdownMsecs <= 10000;

            _countdownMsecs = countdownMsecs;
            _countdownStopwatch.Restart();

            Tools.GetReadableDuration(countdownMsecs, _countdownShowMsecs, out string countdown);

            txtCountdown.Text = countdown;
            txtLastFile.Text = lastFile;
            txtSavedFiles.Text = savedFiles.ToString();

            GetReadableFileSize(lastFileSizeBytes, out string lastFileSize, out string lastFileSizeUnit);
            GetReadableFileSize(totalSizeBytes, out string totalFileSize, out string totalFileSizeUnit);

            if (savedFiles > 1)
            {
                txtTotalSize.Text = $"{totalFileSize} {totalFileSizeUnit} (+{lastFileSize} {lastFileSizeUnit})";
            }
            else
            {
                txtTotalSize.Text = $"{totalFileSize} {totalFileSizeUnit}";
            }
        }

        private void GetReadableFileSize(long bytes, out string value, out string unit)
        {
            if (bytes > 1024 * 1024 * 1024)
            {
                value = (bytes / (1024.0 * 1024.0 * 1024.0)).ToString("0.000");
                unit = "GB";
            }
            else if (bytes > 1024 * 1024)
            {
                value = (bytes / (1024.0 * 1024.0)).ToString("0.0");
                unit = "MB";
            }
            else if (bytes > 1024)
            {
                value = (bytes / 1024.0).ToString("0");
                unit = "KB";
            }
            else
            {
                value = bytes.ToString();
                unit = "Bytes";
            }
        }

        private void ScreenShotThread()
        {
            int subDirCounter = 1;
            int screenShotCounter = 1;

            string lastDir = string.Empty;
            string currentDir = string.Empty;

            string lastFile = string.Empty;
            int savedFiles = 0;
            long totalFileSizeBytes = 0;
            long lastFileSizeBytes = 0;

            bool screenLocked = false;

            while (!_exitThread)
            {
                try
                {
                    _log.WriteDebug(Resources.debug_ThreadSleep);

                    int waitMsecs = 1000;
                    if (_state == State.Running)
                    {
                        if (TryGetIntervalMsecs(Settings.Default.OptionScreenShotInterval, (IntervalUnit) Settings.Default.OptionScreenShotIntervalUnits, out waitMsecs))
                        {
                            UpdateStatus(waitMsecs, lastFile, savedFiles, lastFileSizeBytes, totalFileSizeBytes);
                        }
                        else
                        {
                            waitMsecs = 1000;

                            Stop(true, Resources.tooltip_InvalidScreenShotInterval);
                        }
                    }

                    _screenShotResetEvent.WaitOne(waitMsecs);

                    _log.WriteDebug(Resources.debug_ThreadWokeUp);

                    if (_exitThread)
                    {
                        _log.WriteDebug(Resources.debug_ThreadStopping);
                        return;
                    }

                    Screen screen = _selectedScreen;

                    if (screen == null)
                    {
                        _log.WriteDebug(Resources.debug_ThreadNoScreen);
                        continue;
                    }

                    DateTime dateTime = DateTime.Now;

                    if (_state == State.Starting)
                    {
                        _state = State.Running;

                        if (Settings.Default.OptionResetStatusAfterStart)
                        {
                            lastFile = string.Empty;
                            savedFiles = 0;
                            totalFileSizeBytes = 0;
                            lastFileSizeBytes = 0;
                        }

                        _lastSaveSubDir = null;

                        if (TryFormatScreenshotSubDir(Settings.Default.OptionScreenShotSubDir, dateTime, subDirCounter, out string subDir, out bool hasSubDirCounter))
                        {
                            currentDir = Path.Combine(Settings.Default.OptionScreenShotDir, subDir);

                            if (hasSubDirCounter)
                            {
                                while (Directory.Exists(currentDir))
                                {
                                    subDirCounter += 1;
                                    TryFormatScreenshotSubDir(Settings.Default.OptionScreenShotSubDir, dateTime, subDirCounter, out subDir, out _);
                                    currentDir = Path.Combine(Settings.Default.OptionScreenShotDir, subDir);
                                }
                            }

                            _lastSaveSubDir = subDir;
                        }
                        else
                        {
                            currentDir = Settings.Default.OptionScreenShotDir;
                        }

                        // If the directory did not change, we can continue from current screenShotCounter value.
                        // If the directory already exists, we maybe need to initialize screenShotCounter correctly.
                        // If the directory does not exists, screenShotCounter can start from 1.

                        if (lastDir != currentDir)
                        {
                            lastDir = currentDir;

                            if (Directory.Exists(currentDir))
                            {
                                TryFormatScreenshotFilename(Settings.Default.OptionScreenShotFilename, dateTime, screenShotCounter, out string filename, out bool hasFileCounter, out _);

                                if (hasFileCounter)
                                {
                                    string path = Path.Combine(currentDir, filename);

                                    while (File.Exists(path))
                                    {
                                        screenShotCounter += 1;
                                        TryFormatScreenshotFilename(Settings.Default.OptionScreenShotFilename, dateTime, screenShotCounter, out filename, out _, out _);
                                        path = Path.Combine(currentDir, filename);
                                    }
                                }
                            }
                            else
                            {
                                screenShotCounter = 1;
                            }
                        }

                        Directory.CreateDirectory(currentDir);
                    }

                    Bitmap bitmap = new Bitmap(screen.Bounds.Width, screen.Bounds.Height, PixelFormat.Format24bppRgb);
                    using (Graphics captureGraphic = Graphics.FromImage(bitmap))
                    {
                        _log.WriteDebug(Resources.debug_ThreadGettingScreenShot);

                        captureGraphic.CopyFromScreen(screen.Bounds.X, screen.Bounds.Y, 0, 0, bitmap.Size);
                    }

                    screenLocked = false;

                    if (_state == State.Running)
                    {
                        TryFormatScreenshotFilename(Settings.Default.OptionScreenShotFilename, dateTime, screenShotCounter, out string filename, out bool hasCounter, out _);

                        string path = Path.Combine(currentDir, filename);

                        if (!File.Exists(path))
                        {
                            ImageFormat imageFormat = ImageFormat.Png;

                            string extension = Path.GetExtension(path);
                            if (!string.IsNullOrEmpty(extension))
                            {
                                switch (extension.ToLower())
                                {
                                    case @".bmp":
                                        imageFormat = ImageFormat.Bmp;
                                        break;

                                    case @".jpg":
                                    case @".jpeg":
                                        imageFormat = ImageFormat.Jpeg;
                                        break;

                                    case @".png":
                                        imageFormat = ImageFormat.Png;
                                        break;

                                    case @".tif":
                                    case @".tiff":
                                        imageFormat = ImageFormat.Tiff;
                                        break;
                                }
                            }

                            bitmap.Save(path, imageFormat);

                            savedFiles += 1;

                            FileInfo fileInfo = new FileInfo(path);
                            lastFile = fileInfo.Name;
                            lastFileSizeBytes = fileInfo.Length;
                            totalFileSizeBytes += lastFileSizeBytes;

                            if (hasCounter)
                            {
                                screenShotCounter += 1;
                            }
                        }
                        else
                        {
                            Stop(true, Resources.error_FileAlreadyExists.Swap(Path.GetFileName(path)));
                        }
                    }

                    _log.WriteDebug(Resources.debug_ThreadUpdatingPreview);

                    UpdatePreview(bitmap);
                }
                catch (Win32Exception ex)
                {
                    // Desktop could be locked. Also happens when an UAC Dialog is shown. Log once.
                    if (!screenLocked)
                    {
                        screenLocked = true;
                        _log.WriteWarning(Resources.warn_ScreenCannotBeCapturedNow.Swap(ex.Message));
                    }
                }
                catch (Exception ex)
                {
                    Stop(true, ex.Message);
                    Debug.Fail(ex.ToString());
                }
            }
        }

        private void UpdatePreview(Bitmap bitmap)
        {
            if (InvokeRequired)
            {
                // Delayed invoke. Reason: Deadlock, if FormMain_FormClosing is waiting
                // for ScreenShotThread to end.
                BeginInvoke(new Action(() => UpdatePreview(bitmap)));
                return;
            }

            Image oldImage = pbPreview.Image;

            if (!_exitThread)
            {
                pbPreview.Image = bitmap;
            }
            else
            {
                bitmap.Dispose();
            }

            oldImage?.Dispose();
        }

        private void ShowWindow()
        {
            Activate();
            WindowState = _preferredWindowState;
            BringToFront();
        }

        #endregion Private Methods
    }
}
