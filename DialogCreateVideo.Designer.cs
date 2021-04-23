
namespace ScreenShotBot
{
    partial class DialogCreateVideo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogCreateVideo));
            this.gbOptions = new System.Windows.Forms.GroupBox();
            this.btnOutputFilenameAddVariable = new System.Windows.Forms.Button();
            this.txtVideoOutputFilename = new System.Windows.Forms.TextBox();
            this.lblVideoOutputFilename = new System.Windows.Forms.Label();
            this.cbVideoSubDirsType = new System.Windows.Forms.ComboBox();
            this.btnOpenVideoOutputDir = new System.Windows.Forms.Button();
            this.btnOpenVideoInputDir = new System.Windows.Forms.Button();
            this.btnSelectVideoOutputDir = new System.Windows.Forms.Button();
            this.txtVideoOutputDir = new System.Windows.Forms.TextBox();
            this.lblVideoOutputDir = new System.Windows.Forms.Label();
            this.btnSelectVideoInputDir = new System.Windows.Forms.Button();
            this.txtVideoInputDir = new System.Windows.Forms.TextBox();
            this.lblVideoInputDir = new System.Windows.Forms.Label();
            this.cbVideoScaleType = new System.Windows.Forms.ComboBox();
            this.txtVideoScale = new System.Windows.Forms.TextBox();
            this.lblVideoScale = new System.Windows.Forms.Label();
            this.txtVideoImagesPerSecond = new System.Windows.Forms.TextBox();
            this.lblVideoImagesPerSecond = new System.Windows.Forms.Label();
            this.txtVideoFilePattern = new System.Windows.Forms.TextBox();
            this.lblVideoFilePattern = new System.Windows.Forms.Label();
            this.btnSelectVideoConverterPath = new System.Windows.Forms.Button();
            this.txtVideoConverterPath = new System.Windows.Forms.TextBox();
            this.lblVideoConverterPath = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.LinkLabel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tsStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.gbStatus = new System.Windows.Forms.GroupBox();
            this.txtEstimatedDuration = new System.Windows.Forms.TextBox();
            this.txtFirstFileDimensions = new System.Windows.Forms.TextBox();
            this.txtFirstFile = new System.Windows.Forms.TextBox();
            this.txtFileCount = new System.Windows.Forms.TextBox();
            this.lblEstimatedDuration = new System.Windows.Forms.Label();
            this.lblFirstFileDimensions = new System.Windows.Forms.Label();
            this.lblFirstFile = new System.Windows.Forms.Label();
            this.lblFileCount = new System.Windows.Forms.Label();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.toolTipStart = new System.Windows.Forms.ToolTip(this.components);
            this.gbCommand = new System.Windows.Forms.GroupBox();
            this.chkUseVideoConverterCustomCommand = new System.Windows.Forms.CheckBox();
            this.lblVideoConverterCustomCommand = new System.Windows.Forms.Label();
            this.txtVideoConverterCommand = new System.Windows.Forms.TextBox();
            this.gbOptions.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.gbStatus.SuspendLayout();
            this.gbCommand.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbOptions
            // 
            resources.ApplyResources(this.gbOptions, "gbOptions");
            this.gbOptions.Controls.Add(this.btnOutputFilenameAddVariable);
            this.gbOptions.Controls.Add(this.txtVideoOutputFilename);
            this.gbOptions.Controls.Add(this.lblVideoOutputFilename);
            this.gbOptions.Controls.Add(this.cbVideoSubDirsType);
            this.gbOptions.Controls.Add(this.btnOpenVideoOutputDir);
            this.gbOptions.Controls.Add(this.btnOpenVideoInputDir);
            this.gbOptions.Controls.Add(this.btnSelectVideoOutputDir);
            this.gbOptions.Controls.Add(this.txtVideoOutputDir);
            this.gbOptions.Controls.Add(this.lblVideoOutputDir);
            this.gbOptions.Controls.Add(this.btnSelectVideoInputDir);
            this.gbOptions.Controls.Add(this.txtVideoInputDir);
            this.gbOptions.Controls.Add(this.lblVideoInputDir);
            this.gbOptions.Controls.Add(this.cbVideoScaleType);
            this.gbOptions.Controls.Add(this.txtVideoScale);
            this.gbOptions.Controls.Add(this.lblVideoScale);
            this.gbOptions.Controls.Add(this.txtVideoImagesPerSecond);
            this.gbOptions.Controls.Add(this.lblVideoImagesPerSecond);
            this.gbOptions.Controls.Add(this.txtVideoFilePattern);
            this.gbOptions.Controls.Add(this.lblVideoFilePattern);
            this.gbOptions.Controls.Add(this.btnSelectVideoConverterPath);
            this.gbOptions.Controls.Add(this.txtVideoConverterPath);
            this.gbOptions.Controls.Add(this.lblVideoConverterPath);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.TabStop = false;
            // 
            // btnOutputFilenameAddVariable
            // 
            resources.ApplyResources(this.btnOutputFilenameAddVariable, "btnOutputFilenameAddVariable");
            this.btnOutputFilenameAddVariable.Name = "btnOutputFilenameAddVariable";
            this.btnOutputFilenameAddVariable.UseVisualStyleBackColor = true;
            this.btnOutputFilenameAddVariable.Click += new System.EventHandler(this.btnOutputFilenameAddVariable_Click);
            // 
            // txtVideoOutputFilename
            // 
            resources.ApplyResources(this.txtVideoOutputFilename, "txtVideoOutputFilename");
            this.txtVideoOutputFilename.HideSelection = false;
            this.txtVideoOutputFilename.Name = "txtVideoOutputFilename";
            this.txtVideoOutputFilename.TextChanged += new System.EventHandler(this.txtVideoOutputFilename_TextChanged);
            // 
            // lblVideoOutputFilename
            // 
            resources.ApplyResources(this.lblVideoOutputFilename, "lblVideoOutputFilename");
            this.lblVideoOutputFilename.Name = "lblVideoOutputFilename";
            // 
            // cbVideoSubDirsType
            // 
            resources.ApplyResources(this.cbVideoSubDirsType, "cbVideoSubDirsType");
            this.cbVideoSubDirsType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVideoSubDirsType.FormattingEnabled = true;
            this.cbVideoSubDirsType.Name = "cbVideoSubDirsType";
            this.cbVideoSubDirsType.SelectedIndexChanged += new System.EventHandler(this.cbVideoSubDirsType_SelectedIndexChanged);
            // 
            // btnOpenVideoOutputDir
            // 
            resources.ApplyResources(this.btnOpenVideoOutputDir, "btnOpenVideoOutputDir");
            this.btnOpenVideoOutputDir.Name = "btnOpenVideoOutputDir";
            this.btnOpenVideoOutputDir.UseVisualStyleBackColor = true;
            this.btnOpenVideoOutputDir.Click += new System.EventHandler(this.btnOpenVideoOutputDir_Click);
            // 
            // btnOpenVideoInputDir
            // 
            resources.ApplyResources(this.btnOpenVideoInputDir, "btnOpenVideoInputDir");
            this.btnOpenVideoInputDir.Name = "btnOpenVideoInputDir";
            this.btnOpenVideoInputDir.UseVisualStyleBackColor = true;
            this.btnOpenVideoInputDir.Click += new System.EventHandler(this.btnOpenVideoInputDir_Click);
            // 
            // btnSelectVideoOutputDir
            // 
            resources.ApplyResources(this.btnSelectVideoOutputDir, "btnSelectVideoOutputDir");
            this.btnSelectVideoOutputDir.Name = "btnSelectVideoOutputDir";
            this.btnSelectVideoOutputDir.UseVisualStyleBackColor = true;
            this.btnSelectVideoOutputDir.Click += new System.EventHandler(this.btnSelectVideoOutputDir_Click);
            // 
            // txtVideoOutputDir
            // 
            resources.ApplyResources(this.txtVideoOutputDir, "txtVideoOutputDir");
            this.txtVideoOutputDir.Name = "txtVideoOutputDir";
            this.txtVideoOutputDir.TextChanged += new System.EventHandler(this.txtVideoOutputDir_TextChanged);
            // 
            // lblVideoOutputDir
            // 
            resources.ApplyResources(this.lblVideoOutputDir, "lblVideoOutputDir");
            this.lblVideoOutputDir.Name = "lblVideoOutputDir";
            // 
            // btnSelectVideoInputDir
            // 
            resources.ApplyResources(this.btnSelectVideoInputDir, "btnSelectVideoInputDir");
            this.btnSelectVideoInputDir.Name = "btnSelectVideoInputDir";
            this.btnSelectVideoInputDir.UseVisualStyleBackColor = true;
            this.btnSelectVideoInputDir.Click += new System.EventHandler(this.btnSelectVideoInputDir_Click);
            // 
            // txtVideoInputDir
            // 
            resources.ApplyResources(this.txtVideoInputDir, "txtVideoInputDir");
            this.txtVideoInputDir.Name = "txtVideoInputDir";
            this.txtVideoInputDir.TextChanged += new System.EventHandler(this.txtVideoInputDir_TextChanged);
            // 
            // lblVideoInputDir
            // 
            resources.ApplyResources(this.lblVideoInputDir, "lblVideoInputDir");
            this.lblVideoInputDir.Name = "lblVideoInputDir";
            // 
            // cbVideoScaleType
            // 
            resources.ApplyResources(this.cbVideoScaleType, "cbVideoScaleType");
            this.cbVideoScaleType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVideoScaleType.FormattingEnabled = true;
            this.cbVideoScaleType.Name = "cbVideoScaleType";
            this.cbVideoScaleType.SelectedIndexChanged += new System.EventHandler(this.cbVideoScaleType_SelectedIndexChanged);
            // 
            // txtVideoScale
            // 
            resources.ApplyResources(this.txtVideoScale, "txtVideoScale");
            this.txtVideoScale.Name = "txtVideoScale";
            this.txtVideoScale.TextChanged += new System.EventHandler(this.txtVideoScale_TextChanged);
            // 
            // lblVideoScale
            // 
            resources.ApplyResources(this.lblVideoScale, "lblVideoScale");
            this.lblVideoScale.Name = "lblVideoScale";
            // 
            // txtVideoImagesPerSecond
            // 
            resources.ApplyResources(this.txtVideoImagesPerSecond, "txtVideoImagesPerSecond");
            this.txtVideoImagesPerSecond.Name = "txtVideoImagesPerSecond";
            this.txtVideoImagesPerSecond.TextChanged += new System.EventHandler(this.txtVideoFramerate_TextChanged);
            // 
            // lblVideoImagesPerSecond
            // 
            resources.ApplyResources(this.lblVideoImagesPerSecond, "lblVideoImagesPerSecond");
            this.lblVideoImagesPerSecond.Name = "lblVideoImagesPerSecond";
            // 
            // txtVideoFilePattern
            // 
            resources.ApplyResources(this.txtVideoFilePattern, "txtVideoFilePattern");
            this.txtVideoFilePattern.Name = "txtVideoFilePattern";
            this.txtVideoFilePattern.TextChanged += new System.EventHandler(this.txtVideoFileFilter_TextChanged);
            // 
            // lblVideoFilePattern
            // 
            resources.ApplyResources(this.lblVideoFilePattern, "lblVideoFilePattern");
            this.lblVideoFilePattern.Name = "lblVideoFilePattern";
            // 
            // btnSelectVideoConverterPath
            // 
            resources.ApplyResources(this.btnSelectVideoConverterPath, "btnSelectVideoConverterPath");
            this.btnSelectVideoConverterPath.Name = "btnSelectVideoConverterPath";
            this.btnSelectVideoConverterPath.UseVisualStyleBackColor = true;
            this.btnSelectVideoConverterPath.Click += new System.EventHandler(this.btnSelectVideoConverterPath_Click);
            // 
            // txtVideoConverterPath
            // 
            resources.ApplyResources(this.txtVideoConverterPath, "txtVideoConverterPath");
            this.txtVideoConverterPath.Name = "txtVideoConverterPath";
            this.txtVideoConverterPath.TextChanged += new System.EventHandler(this.txtVideoConverterPath_TextChanged);
            // 
            // lblVideoConverterPath
            // 
            resources.ApplyResources(this.lblVideoConverterPath, "lblVideoConverterPath");
            this.lblVideoConverterPath.Name = "lblVideoConverterPath";
            // 
            // lblInfo
            // 
            resources.ApplyResources(this.lblInfo, "lblInfo");
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.TabStop = true;
            this.lblInfo.UseCompatibleTextRendering = true;
            this.lblInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblInfo_LinkClicked);
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
            // gbStatus
            // 
            resources.ApplyResources(this.gbStatus, "gbStatus");
            this.gbStatus.Controls.Add(this.txtEstimatedDuration);
            this.gbStatus.Controls.Add(this.txtFirstFileDimensions);
            this.gbStatus.Controls.Add(this.txtFirstFile);
            this.gbStatus.Controls.Add(this.txtFileCount);
            this.gbStatus.Controls.Add(this.lblEstimatedDuration);
            this.gbStatus.Controls.Add(this.lblFirstFileDimensions);
            this.gbStatus.Controls.Add(this.lblFirstFile);
            this.gbStatus.Controls.Add(this.lblFileCount);
            this.gbStatus.Name = "gbStatus";
            this.gbStatus.TabStop = false;
            // 
            // txtEstimatedDuration
            // 
            resources.ApplyResources(this.txtEstimatedDuration, "txtEstimatedDuration");
            this.txtEstimatedDuration.Name = "txtEstimatedDuration";
            this.txtEstimatedDuration.ReadOnly = true;
            // 
            // txtFirstFileDimensions
            // 
            resources.ApplyResources(this.txtFirstFileDimensions, "txtFirstFileDimensions");
            this.txtFirstFileDimensions.Name = "txtFirstFileDimensions";
            this.txtFirstFileDimensions.ReadOnly = true;
            // 
            // txtFirstFile
            // 
            resources.ApplyResources(this.txtFirstFile, "txtFirstFile");
            this.txtFirstFile.Name = "txtFirstFile";
            this.txtFirstFile.ReadOnly = true;
            // 
            // txtFileCount
            // 
            resources.ApplyResources(this.txtFileCount, "txtFileCount");
            this.txtFileCount.Name = "txtFileCount";
            this.txtFileCount.ReadOnly = true;
            // 
            // lblEstimatedDuration
            // 
            resources.ApplyResources(this.lblEstimatedDuration, "lblEstimatedDuration");
            this.lblEstimatedDuration.Name = "lblEstimatedDuration";
            // 
            // lblFirstFileDimensions
            // 
            resources.ApplyResources(this.lblFirstFileDimensions, "lblFirstFileDimensions");
            this.lblFirstFileDimensions.Name = "lblFirstFileDimensions";
            // 
            // lblFirstFile
            // 
            resources.ApplyResources(this.lblFirstFile, "lblFirstFile");
            this.lblFirstFile.Name = "lblFirstFile";
            // 
            // lblFileCount
            // 
            resources.ApplyResources(this.lblFileCount, "lblFileCount");
            this.lblFileCount.Name = "lblFileCount";
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
            // gbCommand
            // 
            resources.ApplyResources(this.gbCommand, "gbCommand");
            this.gbCommand.Controls.Add(this.chkUseVideoConverterCustomCommand);
            this.gbCommand.Controls.Add(this.lblVideoConverterCustomCommand);
            this.gbCommand.Controls.Add(this.txtVideoConverterCommand);
            this.gbCommand.Name = "gbCommand";
            this.gbCommand.TabStop = false;
            // 
            // chkUseVideoConverterCustomCommand
            // 
            resources.ApplyResources(this.chkUseVideoConverterCustomCommand, "chkUseVideoConverterCustomCommand");
            this.chkUseVideoConverterCustomCommand.Name = "chkUseVideoConverterCustomCommand";
            this.chkUseVideoConverterCustomCommand.UseVisualStyleBackColor = true;
            this.chkUseVideoConverterCustomCommand.CheckedChanged += new System.EventHandler(this.chkVideoConverterCustomCommand_CheckedChanged);
            // 
            // lblVideoConverterCustomCommand
            // 
            resources.ApplyResources(this.lblVideoConverterCustomCommand, "lblVideoConverterCustomCommand");
            this.lblVideoConverterCustomCommand.Name = "lblVideoConverterCustomCommand";
            // 
            // txtVideoConverterCommand
            // 
            resources.ApplyResources(this.txtVideoConverterCommand, "txtVideoConverterCommand");
            this.txtVideoConverterCommand.Name = "txtVideoConverterCommand";
            this.txtVideoConverterCommand.TextChanged += new System.EventHandler(this.txtVideoConverterCommand_TextChanged);
            // 
            // DialogCreateVideo
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbCommand);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.gbStatus);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.gbOptions);
            this.Name = "DialogCreateVideo";
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DialogCreateVideo_FormClosing);
            this.Load += new System.EventHandler(this.DialogCreateVideo_Load);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DialogCreateVideo_MouseMove);
            this.gbOptions.ResumeLayout(false);
            this.gbOptions.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.gbStatus.ResumeLayout(false);
            this.gbStatus.PerformLayout();
            this.gbCommand.ResumeLayout(false);
            this.gbCommand.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbOptions;
        private System.Windows.Forms.LinkLabel lblInfo;
        private System.Windows.Forms.Label lblVideoConverterPath;
        private System.Windows.Forms.TextBox txtVideoConverterPath;
        private System.Windows.Forms.Button btnSelectVideoConverterPath;
        private System.Windows.Forms.TextBox txtVideoScale;
        private System.Windows.Forms.Label lblVideoScale;
        private System.Windows.Forms.TextBox txtVideoImagesPerSecond;
        private System.Windows.Forms.Label lblVideoImagesPerSecond;
        private System.Windows.Forms.TextBox txtVideoFilePattern;
        private System.Windows.Forms.Label lblVideoFilePattern;
        private System.Windows.Forms.ComboBox cbVideoScaleType;
        private System.Windows.Forms.Button btnSelectVideoOutputDir;
        private System.Windows.Forms.TextBox txtVideoOutputDir;
        private System.Windows.Forms.Label lblVideoOutputDir;
        private System.Windows.Forms.Button btnSelectVideoInputDir;
        private System.Windows.Forms.TextBox txtVideoInputDir;
        private System.Windows.Forms.Label lblVideoInputDir;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel tsStatus;
        private System.Windows.Forms.GroupBox gbStatus;
        private System.Windows.Forms.TextBox txtEstimatedDuration;
        private System.Windows.Forms.TextBox txtFirstFileDimensions;
        private System.Windows.Forms.TextBox txtFirstFile;
        private System.Windows.Forms.TextBox txtFileCount;
        private System.Windows.Forms.Label lblEstimatedDuration;
        private System.Windows.Forms.Label lblFirstFileDimensions;
        private System.Windows.Forms.Label lblFirstFile;
        private System.Windows.Forms.Label lblFileCount;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.ToolTip toolTipStart;
        private System.Windows.Forms.GroupBox gbCommand;
        private System.Windows.Forms.CheckBox chkUseVideoConverterCustomCommand;
        private System.Windows.Forms.Label lblVideoConverterCustomCommand;
        private System.Windows.Forms.TextBox txtVideoConverterCommand;
        private System.Windows.Forms.Button btnOpenVideoInputDir;
        private System.Windows.Forms.Button btnOpenVideoOutputDir;
        private System.Windows.Forms.ComboBox cbVideoSubDirsType;
        private System.Windows.Forms.Button btnOutputFilenameAddVariable;
        private System.Windows.Forms.TextBox txtVideoOutputFilename;
        private System.Windows.Forms.Label lblVideoOutputFilename;
    }
}