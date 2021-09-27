
namespace ScreenShotBot
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuNotifyIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miNotifyIconShow = new System.Windows.Forms.ToolStripMenuItem();
            this.mitNotifyIconStartStop = new System.Windows.Forms.ToolStripMenuItem();
            this.miNotifyIconSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.miNotifyIconExit = new System.Windows.Forms.ToolStripMenuItem();
            this.gbOptions = new System.Windows.Forms.GroupBox();
            this.btnFilenamesAddVariable = new System.Windows.Forms.Button();
            this.txtScreenShotFilename = new System.Windows.Forms.TextBox();
            this.lblFilenames = new System.Windows.Forms.Label();
            this.btnSubdirectoryAddVariable = new System.Windows.Forms.Button();
            this.txtScreenShotSubDir = new System.Windows.Forms.TextBox();
            this.lblSubdirectory = new System.Windows.Forms.Label();
            this.cbScreenShotIntervalUnit = new System.Windows.Forms.ComboBox();
            this.txtScreenShotInterval = new System.Windows.Forms.TextBox();
            this.lblScreenShotInterval = new System.Windows.Forms.Label();
            this.btnOpenScreenShotDirectory = new System.Windows.Forms.Button();
            this.btnSelectScreenShotDirectory = new System.Windows.Forms.Button();
            this.txtScreenShotDirectory = new System.Windows.Forms.TextBox();
            this.lblScreenShotDirectory = new System.Windows.Forms.Label();
            this.lblScreen = new System.Windows.Forms.Label();
            this.cbScreen = new System.Windows.Forms.ComboBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.tsFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.miMinimizeToTray = new System.Windows.Forms.ToolStripMenuItem();
            this.miCloseToTray = new System.Windows.Forms.ToolStripMenuItem();
            this.tsTools = new System.Windows.Forms.ToolStripMenuItem();
            this.miCreateVideo = new System.Windows.Forms.ToolStripMenuItem();
            this.tsHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.miAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.pbPreview = new System.Windows.Forms.PictureBox();
            this.gbPreview = new System.Windows.Forms.GroupBox();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.toolTipStart = new System.Windows.Forms.ToolTip(this.components);
            this.gbStatus = new System.Windows.Forms.GroupBox();
            this.txtTotalSize = new System.Windows.Forms.TextBox();
            this.txtSavedFiles = new System.Windows.Forms.TextBox();
            this.txtLastFile = new System.Windows.Forms.TextBox();
            this.txtCountdown = new System.Windows.Forms.TextBox();
            this.lblTotalSize = new System.Windows.Forms.Label();
            this.lblSavedFiles = new System.Windows.Forms.Label();
            this.lblLastFile = new System.Windows.Forms.Label();
            this.lblCountdown = new System.Windows.Forms.Label();
            this.timerUpdateCountdown = new System.Windows.Forms.Timer(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tsStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.contextMenuNotifyIcon.SuspendLayout();
            this.gbOptions.SuspendLayout();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.gbPreview.SuspendLayout();
            this.gbStatus.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuNotifyIcon;
            resources.ApplyResources(this.notifyIcon, "notifyIcon");
            this.notifyIcon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDown);
            // 
            // contextMenuNotifyIcon
            // 
            resources.ApplyResources(this.contextMenuNotifyIcon, "contextMenuNotifyIcon");
            this.contextMenuNotifyIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miNotifyIconShow,
            this.mitNotifyIconStartStop,
            this.miNotifyIconSeparator,
            this.miNotifyIconExit});
            this.contextMenuNotifyIcon.Name = "contextMenuNotifyIcon";
            this.contextMenuNotifyIcon.ShowImageMargin = false;
            this.contextMenuNotifyIcon.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuNotifyIcon_ItemClicked);
            // 
            // miNotifyIconShow
            // 
            this.miNotifyIconShow.Name = "miNotifyIconShow";
            resources.ApplyResources(this.miNotifyIconShow, "miNotifyIconShow");
            // 
            // mitNotifyIconStartStop
            // 
            this.mitNotifyIconStartStop.Name = "mitNotifyIconStartStop";
            resources.ApplyResources(this.mitNotifyIconStartStop, "mitNotifyIconStartStop");
            // 
            // miNotifyIconSeparator
            // 
            this.miNotifyIconSeparator.Name = "miNotifyIconSeparator";
            resources.ApplyResources(this.miNotifyIconSeparator, "miNotifyIconSeparator");
            // 
            // miNotifyIconExit
            // 
            this.miNotifyIconExit.Name = "miNotifyIconExit";
            resources.ApplyResources(this.miNotifyIconExit, "miNotifyIconExit");
            // 
            // gbOptions
            // 
            resources.ApplyResources(this.gbOptions, "gbOptions");
            this.gbOptions.Controls.Add(this.btnFilenamesAddVariable);
            this.gbOptions.Controls.Add(this.txtScreenShotFilename);
            this.gbOptions.Controls.Add(this.lblFilenames);
            this.gbOptions.Controls.Add(this.btnSubdirectoryAddVariable);
            this.gbOptions.Controls.Add(this.txtScreenShotSubDir);
            this.gbOptions.Controls.Add(this.lblSubdirectory);
            this.gbOptions.Controls.Add(this.cbScreenShotIntervalUnit);
            this.gbOptions.Controls.Add(this.txtScreenShotInterval);
            this.gbOptions.Controls.Add(this.lblScreenShotInterval);
            this.gbOptions.Controls.Add(this.btnOpenScreenShotDirectory);
            this.gbOptions.Controls.Add(this.btnSelectScreenShotDirectory);
            this.gbOptions.Controls.Add(this.txtScreenShotDirectory);
            this.gbOptions.Controls.Add(this.lblScreenShotDirectory);
            this.gbOptions.Controls.Add(this.lblScreen);
            this.gbOptions.Controls.Add(this.cbScreen);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.TabStop = false;
            // 
            // btnFilenamesAddVariable
            // 
            resources.ApplyResources(this.btnFilenamesAddVariable, "btnFilenamesAddVariable");
            this.btnFilenamesAddVariable.Name = "btnFilenamesAddVariable";
            this.btnFilenamesAddVariable.UseVisualStyleBackColor = true;
            this.btnFilenamesAddVariable.Click += new System.EventHandler(this.btnFilenamesAddVariable_Click);
            // 
            // txtScreenShotFilename
            // 
            resources.ApplyResources(this.txtScreenShotFilename, "txtScreenShotFilename");
            this.txtScreenShotFilename.HideSelection = false;
            this.txtScreenShotFilename.Name = "txtScreenShotFilename";
            this.txtScreenShotFilename.TextChanged += new System.EventHandler(this.txtScreenShotFilenames_TextChanged);
            // 
            // lblFilenames
            // 
            resources.ApplyResources(this.lblFilenames, "lblFilenames");
            this.lblFilenames.Name = "lblFilenames";
            // 
            // btnSubdirectoryAddVariable
            // 
            resources.ApplyResources(this.btnSubdirectoryAddVariable, "btnSubdirectoryAddVariable");
            this.btnSubdirectoryAddVariable.Name = "btnSubdirectoryAddVariable";
            this.btnSubdirectoryAddVariable.UseVisualStyleBackColor = true;
            this.btnSubdirectoryAddVariable.Click += new System.EventHandler(this.btnSubdirectoryAddVariable_Click);
            // 
            // txtScreenShotSubDir
            // 
            resources.ApplyResources(this.txtScreenShotSubDir, "txtScreenShotSubDir");
            this.txtScreenShotSubDir.HideSelection = false;
            this.txtScreenShotSubDir.Name = "txtScreenShotSubDir";
            this.txtScreenShotSubDir.TextChanged += new System.EventHandler(this.txtScreenShotSubDir_TextChanged);
            // 
            // lblSubdirectory
            // 
            resources.ApplyResources(this.lblSubdirectory, "lblSubdirectory");
            this.lblSubdirectory.Name = "lblSubdirectory";
            // 
            // cbScreenShotIntervalUnit
            // 
            resources.ApplyResources(this.cbScreenShotIntervalUnit, "cbScreenShotIntervalUnit");
            this.cbScreenShotIntervalUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbScreenShotIntervalUnit.FormattingEnabled = true;
            this.cbScreenShotIntervalUnit.Name = "cbScreenShotIntervalUnit";
            this.cbScreenShotIntervalUnit.SelectedIndexChanged += new System.EventHandler(this.cbScreenShotIntervalUnit_SelectedIndexChanged);
            // 
            // txtScreenShotInterval
            // 
            resources.ApplyResources(this.txtScreenShotInterval, "txtScreenShotInterval");
            this.txtScreenShotInterval.Name = "txtScreenShotInterval";
            this.txtScreenShotInterval.TextChanged += new System.EventHandler(this.txtScreenShotInterval_TextChanged);
            // 
            // lblScreenShotInterval
            // 
            resources.ApplyResources(this.lblScreenShotInterval, "lblScreenShotInterval");
            this.lblScreenShotInterval.Name = "lblScreenShotInterval";
            // 
            // btnOpenScreenShotDirectory
            // 
            resources.ApplyResources(this.btnOpenScreenShotDirectory, "btnOpenScreenShotDirectory");
            this.btnOpenScreenShotDirectory.Name = "btnOpenScreenShotDirectory";
            this.btnOpenScreenShotDirectory.UseVisualStyleBackColor = true;
            this.btnOpenScreenShotDirectory.Click += new System.EventHandler(this.btnOpenScreenShotDirectory_Click);
            // 
            // btnSelectScreenShotDirectory
            // 
            resources.ApplyResources(this.btnSelectScreenShotDirectory, "btnSelectScreenShotDirectory");
            this.btnSelectScreenShotDirectory.Name = "btnSelectScreenShotDirectory";
            this.btnSelectScreenShotDirectory.UseVisualStyleBackColor = true;
            this.btnSelectScreenShotDirectory.Click += new System.EventHandler(this.btnSelectScreenShotDirectory_Click);
            // 
            // txtScreenShotDirectory
            // 
            resources.ApplyResources(this.txtScreenShotDirectory, "txtScreenShotDirectory");
            this.txtScreenShotDirectory.Name = "txtScreenShotDirectory";
            this.txtScreenShotDirectory.TextChanged += new System.EventHandler(this.txtScreenShotDirectory_TextChanged);
            // 
            // lblScreenShotDirectory
            // 
            resources.ApplyResources(this.lblScreenShotDirectory, "lblScreenShotDirectory");
            this.lblScreenShotDirectory.Name = "lblScreenShotDirectory";
            // 
            // lblScreen
            // 
            resources.ApplyResources(this.lblScreen, "lblScreen");
            this.lblScreen.Name = "lblScreen";
            // 
            // cbScreen
            // 
            resources.ApplyResources(this.cbScreen, "cbScreen");
            this.cbScreen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbScreen.FormattingEnabled = true;
            this.cbScreen.Name = "cbScreen";
            this.cbScreen.SelectedIndexChanged += new System.EventHandler(this.cbScreen_SelectedIndexChanged);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsFile,
            this.tsSettings,
            this.tsTools,
            this.tsHelp});
            resources.ApplyResources(this.menuStrip, "menuStrip");
            this.menuStrip.Name = "menuStrip";
            // 
            // tsFile
            // 
            this.tsFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miExit});
            this.tsFile.Name = "tsFile";
            resources.ApplyResources(this.tsFile, "tsFile");
            // 
            // miExit
            // 
            this.miExit.Name = "miExit";
            resources.ApplyResources(this.miExit, "miExit");
            this.miExit.Click += new System.EventHandler(this.miExit_Click);
            // 
            // tsSettings
            // 
            this.tsSettings.CheckOnClick = true;
            this.tsSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMinimizeToTray,
            this.miCloseToTray});
            this.tsSettings.Name = "tsSettings";
            resources.ApplyResources(this.tsSettings, "tsSettings");
            // 
            // miMinimizeToTray
            // 
            this.miMinimizeToTray.CheckOnClick = true;
            this.miMinimizeToTray.Name = "miMinimizeToTray";
            resources.ApplyResources(this.miMinimizeToTray, "miMinimizeToTray");
            this.miMinimizeToTray.CheckedChanged += new System.EventHandler(this.miMinimizeToTray_CheckedChanged);
            // 
            // miCloseToTray
            // 
            this.miCloseToTray.CheckOnClick = true;
            this.miCloseToTray.Name = "miCloseToTray";
            resources.ApplyResources(this.miCloseToTray, "miCloseToTray");
            this.miCloseToTray.Click += new System.EventHandler(this.miCloseToTray_Click);
            // 
            // tsTools
            // 
            this.tsTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miCreateVideo});
            this.tsTools.Name = "tsTools";
            resources.ApplyResources(this.tsTools, "tsTools");
            // 
            // miCreateVideo
            // 
            this.miCreateVideo.Name = "miCreateVideo";
            resources.ApplyResources(this.miCreateVideo, "miCreateVideo");
            this.miCreateVideo.Click += new System.EventHandler(this.miCreateVideo_Click);
            // 
            // tsHelp
            // 
            this.tsHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAbout});
            this.tsHelp.Name = "tsHelp";
            resources.ApplyResources(this.tsHelp, "tsHelp");
            // 
            // miAbout
            // 
            this.miAbout.Name = "miAbout";
            resources.ApplyResources(this.miAbout, "miAbout");
            this.miAbout.Click += new System.EventHandler(this.miAbout_Click);
            // 
            // pbPreview
            // 
            resources.ApplyResources(this.pbPreview, "pbPreview");
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.TabStop = false;
            // 
            // gbPreview
            // 
            resources.ApplyResources(this.gbPreview, "gbPreview");
            this.gbPreview.Controls.Add(this.pbPreview);
            this.gbPreview.Name = "gbPreview";
            this.gbPreview.TabStop = false;
            // 
            // btnStartStop
            // 
            resources.ApplyResources(this.btnStartStop, "btnStartStop");
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // toolTipStart
            // 
            this.toolTipStart.AutomaticDelay = 0;
            this.toolTipStart.AutoPopDelay = 9999999;
            this.toolTipStart.InitialDelay = 0;
            this.toolTipStart.ReshowDelay = 0;
            this.toolTipStart.UseAnimation = false;
            this.toolTipStart.UseFading = false;
            // 
            // gbStatus
            // 
            resources.ApplyResources(this.gbStatus, "gbStatus");
            this.gbStatus.Controls.Add(this.txtTotalSize);
            this.gbStatus.Controls.Add(this.txtSavedFiles);
            this.gbStatus.Controls.Add(this.txtLastFile);
            this.gbStatus.Controls.Add(this.txtCountdown);
            this.gbStatus.Controls.Add(this.lblTotalSize);
            this.gbStatus.Controls.Add(this.lblSavedFiles);
            this.gbStatus.Controls.Add(this.lblLastFile);
            this.gbStatus.Controls.Add(this.lblCountdown);
            this.gbStatus.Name = "gbStatus";
            this.gbStatus.TabStop = false;
            // 
            // txtTotalSize
            // 
            resources.ApplyResources(this.txtTotalSize, "txtTotalSize");
            this.txtTotalSize.Name = "txtTotalSize";
            this.txtTotalSize.ReadOnly = true;
            // 
            // txtSavedFiles
            // 
            resources.ApplyResources(this.txtSavedFiles, "txtSavedFiles");
            this.txtSavedFiles.Name = "txtSavedFiles";
            this.txtSavedFiles.ReadOnly = true;
            // 
            // txtLastFile
            // 
            resources.ApplyResources(this.txtLastFile, "txtLastFile");
            this.txtLastFile.Name = "txtLastFile";
            this.txtLastFile.ReadOnly = true;
            // 
            // txtCountdown
            // 
            resources.ApplyResources(this.txtCountdown, "txtCountdown");
            this.txtCountdown.Name = "txtCountdown";
            this.txtCountdown.ReadOnly = true;
            // 
            // lblTotalSize
            // 
            resources.ApplyResources(this.lblTotalSize, "lblTotalSize");
            this.lblTotalSize.Name = "lblTotalSize";
            // 
            // lblSavedFiles
            // 
            resources.ApplyResources(this.lblSavedFiles, "lblSavedFiles");
            this.lblSavedFiles.Name = "lblSavedFiles";
            // 
            // lblLastFile
            // 
            resources.ApplyResources(this.lblLastFile, "lblLastFile");
            this.lblLastFile.Name = "lblLastFile";
            // 
            // lblCountdown
            // 
            resources.ApplyResources(this.lblCountdown, "lblCountdown");
            this.lblCountdown.Name = "lblCountdown";
            // 
            // timerUpdateCountdown
            // 
            this.timerUpdateCountdown.Tick += new System.EventHandler(this.timerUpdateCountdown_Tick);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsStatus});
            resources.ApplyResources(this.statusStrip, "statusStrip");
            this.statusStrip.Name = "statusStrip";
            // 
            // tsStatus
            // 
            this.tsStatus.Name = "tsStatus";
            resources.ApplyResources(this.tsStatus, "tsStatus");
            // 
            // FormMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.gbStatus);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.gbPreview);
            this.Controls.Add(this.gbOptions);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "FormMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.SizeChanged += new System.EventHandler(this.FormMain_SizeChanged);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseMove);
            this.contextMenuNotifyIcon.ResumeLayout(false);
            this.gbOptions.ResumeLayout(false);
            this.gbOptions.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.gbPreview.ResumeLayout(false);
            this.gbStatus.ResumeLayout(false);
            this.gbStatus.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.GroupBox gbOptions;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem miExit;
        private System.Windows.Forms.Label lblScreen;
        private System.Windows.Forms.ComboBox cbScreen;
        private System.Windows.Forms.PictureBox pbPreview;
        private System.Windows.Forms.GroupBox gbPreview;
        private System.Windows.Forms.Label lblScreenShotDirectory;
        private System.Windows.Forms.TextBox txtScreenShotDirectory;
        private System.Windows.Forms.Button btnSelectScreenShotDirectory;
        private System.Windows.Forms.Button btnOpenScreenShotDirectory;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.ToolTip toolTipStart;
        private System.Windows.Forms.ComboBox cbScreenShotIntervalUnit;
        private System.Windows.Forms.TextBox txtScreenShotInterval;
        private System.Windows.Forms.Label lblScreenShotInterval;
        private System.Windows.Forms.GroupBox gbStatus;
        private System.Windows.Forms.Label lblCountdown;
        private System.Windows.Forms.Label lblTotalSize;
        private System.Windows.Forms.Label lblSavedFiles;
        private System.Windows.Forms.Label lblLastFile;
        private System.Windows.Forms.TextBox txtTotalSize;
        private System.Windows.Forms.TextBox txtSavedFiles;
        private System.Windows.Forms.TextBox txtLastFile;
        private System.Windows.Forms.TextBox txtCountdown;
        private System.Windows.Forms.Timer timerUpdateCountdown;
        private System.Windows.Forms.ToolStripMenuItem tsFile;
        private System.Windows.Forms.ToolStripMenuItem tsSettings;
        private System.Windows.Forms.ToolStripMenuItem miMinimizeToTray;
        private System.Windows.Forms.ToolStripMenuItem tsHelp;
        private System.Windows.Forms.ToolStripMenuItem miAbout;
        private System.Windows.Forms.ToolStripMenuItem tsTools;
        private System.Windows.Forms.ToolStripMenuItem miCreateVideo;
        private System.Windows.Forms.Button btnFilenamesAddVariable;
        private System.Windows.Forms.TextBox txtScreenShotFilename;
        private System.Windows.Forms.Label lblFilenames;
        private System.Windows.Forms.Button btnSubdirectoryAddVariable;
        private System.Windows.Forms.TextBox txtScreenShotSubDir;
        private System.Windows.Forms.Label lblSubdirectory;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel tsStatus;
        private System.Windows.Forms.ContextMenuStrip contextMenuNotifyIcon;
        private System.Windows.Forms.ToolStripMenuItem miNotifyIconShow;
        private System.Windows.Forms.ToolStripMenuItem mitNotifyIconStartStop;
        private System.Windows.Forms.ToolStripSeparator miNotifyIconSeparator;
        private System.Windows.Forms.ToolStripMenuItem miNotifyIconExit;
        private System.Windows.Forms.ToolStripMenuItem miCloseToTray;
    }
}

