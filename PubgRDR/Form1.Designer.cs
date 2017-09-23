namespace PubgTriggr
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnSelectKillMessage = new System.Windows.Forms.Button();
            this.pBKillMessage = new System.Windows.Forms.PictureBox();
            this.btnSelRedKill = new System.Windows.Forms.Button();
            this.btnWhiteKillCounter = new System.Windows.Forms.Button();
            this.pBRedKill = new System.Windows.Forms.PictureBox();
            this.pBKillCounterTopRight = new System.Windows.Forms.PictureBox();
            this.Scanner_RedKill = new System.ComponentModel.BackgroundWorker();
            this.tBLog = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.Scanner_KillNumber = new System.ComponentModel.BackgroundWorker();
            this.btnPickReferenceRed = new System.Windows.Forms.Button();
            this.btnPickReferenceWhite = new System.Windows.Forms.Button();
            this.btnRedKillNumber = new System.Windows.Forms.Button();
            this.pBCenterKillNumber = new System.Windows.Forms.PictureBox();
            this.btnRescanAll = new System.Windows.Forms.Button();
            this.tBColorThreshold = new System.Windows.Forms.TrackBar();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.Worker_LearnThresholds = new System.ComponentModel.BackgroundWorker();
            this.btnLearnRedThresholds = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.tsLblKills = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsLblAlive = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsLblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.Worker_KillMessageCleaner = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.pBKillMessage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBRedKill)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBKillCounterTopRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBCenterKillNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBColorThreshold)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSelectKillMessage
            // 
            this.btnSelectKillMessage.Location = new System.Drawing.Point(12, 175);
            this.btnSelectKillMessage.Name = "btnSelectKillMessage";
            this.btnSelectKillMessage.Size = new System.Drawing.Size(357, 23);
            this.btnSelectKillMessage.TabIndex = 0;
            this.btnSelectKillMessage.Text = "SelectKillMessage";
            this.btnSelectKillMessage.UseVisualStyleBackColor = true;
            this.btnSelectKillMessage.Click += new System.EventHandler(this.BtnSelectKillMessage_Click);
            // 
            // pBKillMessage
            // 
            this.pBKillMessage.Location = new System.Drawing.Point(12, 204);
            this.pBKillMessage.Name = "pBKillMessage";
            this.pBKillMessage.Size = new System.Drawing.Size(474, 35);
            this.pBKillMessage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pBKillMessage.TabIndex = 1;
            this.pBKillMessage.TabStop = false;
            // 
            // btnSelRedKill
            // 
            this.btnSelRedKill.Location = new System.Drawing.Point(12, 31);
            this.btnSelRedKill.Name = "btnSelRedKill";
            this.btnSelRedKill.Size = new System.Drawing.Size(115, 23);
            this.btnSelRedKill.TabIndex = 6;
            this.btnSelRedKill.Text = "RedKill";
            this.btnSelRedKill.UseVisualStyleBackColor = true;
            this.btnSelRedKill.Click += new System.EventHandler(this.BtnSelRedKill_Click);
            // 
            // btnWhiteKillCounter
            // 
            this.btnWhiteKillCounter.Location = new System.Drawing.Point(254, 31);
            this.btnWhiteKillCounter.Name = "btnWhiteKillCounter";
            this.btnWhiteKillCounter.Size = new System.Drawing.Size(115, 23);
            this.btnWhiteKillCounter.TabIndex = 7;
            this.btnWhiteKillCounter.Text = "KillCounterTop";
            this.btnWhiteKillCounter.UseVisualStyleBackColor = true;
            this.btnWhiteKillCounter.Click += new System.EventHandler(this.BtnWhiteKillCounter_Click);
            // 
            // pBRedKill
            // 
            this.pBRedKill.Location = new System.Drawing.Point(12, 60);
            this.pBRedKill.Name = "pBRedKill";
            this.pBRedKill.Size = new System.Drawing.Size(115, 88);
            this.pBRedKill.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pBRedKill.TabIndex = 8;
            this.pBRedKill.TabStop = false;
            // 
            // pBKillCounterTopRight
            // 
            this.pBKillCounterTopRight.Location = new System.Drawing.Point(254, 60);
            this.pBKillCounterTopRight.Name = "pBKillCounterTopRight";
            this.pBKillCounterTopRight.Size = new System.Drawing.Size(115, 88);
            this.pBKillCounterTopRight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pBKillCounterTopRight.TabIndex = 9;
            this.pBKillCounterTopRight.TabStop = false;
            // 
            // Scanner_RedKill
            // 
            this.Scanner_RedKill.WorkerReportsProgress = true;
            this.Scanner_RedKill.WorkerSupportsCancellation = true;
            this.Scanner_RedKill.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Scanner_RedKill_DoWork);
            this.Scanner_RedKill.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.Scanner_RedKill_ProgressChanged);
            // 
            // tBLog
            // 
            this.tBLog.Location = new System.Drawing.Point(12, 245);
            this.tBLog.Multiline = true;
            this.tBLog.Name = "tBLog";
            this.tBLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tBLog.Size = new System.Drawing.Size(474, 76);
            this.tBLog.TabIndex = 13;
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.LightGreen;
            this.btnStart.Location = new System.Drawing.Point(375, 60);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(111, 23);
            this.btnStart.TabIndex = 14;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // Scanner_KillNumber
            // 
            this.Scanner_KillNumber.WorkerReportsProgress = true;
            this.Scanner_KillNumber.WorkerSupportsCancellation = true;
            this.Scanner_KillNumber.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Scanner_KillNumber_DoWork);
            this.Scanner_KillNumber.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.Scanner_KillNumber_ProgressChanged);
            this.Scanner_KillNumber.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Scanner_KillNumber_RunWorkerCompleted);
            // 
            // btnPickReferenceRed
            // 
            this.btnPickReferenceRed.Location = new System.Drawing.Point(375, 31);
            this.btnPickReferenceRed.Name = "btnPickReferenceRed";
            this.btnPickReferenceRed.Size = new System.Drawing.Size(54, 23);
            this.btnPickReferenceRed.TabIndex = 19;
            this.btnPickReferenceRed.Text = "Red";
            this.btnPickReferenceRed.UseVisualStyleBackColor = true;
            this.btnPickReferenceRed.Click += new System.EventHandler(this.BtnPickReferenceRed_Click);
            // 
            // btnPickReferenceWhite
            // 
            this.btnPickReferenceWhite.Location = new System.Drawing.Point(432, 31);
            this.btnPickReferenceWhite.Name = "btnPickReferenceWhite";
            this.btnPickReferenceWhite.Size = new System.Drawing.Size(54, 23);
            this.btnPickReferenceWhite.TabIndex = 22;
            this.btnPickReferenceWhite.Text = "White";
            this.btnPickReferenceWhite.UseVisualStyleBackColor = true;
            this.btnPickReferenceWhite.Click += new System.EventHandler(this.BtnPickReferenceWhite_Click);
            // 
            // btnRedKillNumber
            // 
            this.btnRedKillNumber.Location = new System.Drawing.Point(133, 31);
            this.btnRedKillNumber.Name = "btnRedKillNumber";
            this.btnRedKillNumber.Size = new System.Drawing.Size(115, 23);
            this.btnRedKillNumber.TabIndex = 24;
            this.btnRedKillNumber.Text = "RedKillNumber";
            this.btnRedKillNumber.UseVisualStyleBackColor = true;
            this.btnRedKillNumber.Click += new System.EventHandler(this.BtnRedKillNumber_Click);
            // 
            // pBCenterKillNumber
            // 
            this.pBCenterKillNumber.Location = new System.Drawing.Point(133, 60);
            this.pBCenterKillNumber.Name = "pBCenterKillNumber";
            this.pBCenterKillNumber.Size = new System.Drawing.Size(115, 88);
            this.pBCenterKillNumber.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pBCenterKillNumber.TabIndex = 23;
            this.pBCenterKillNumber.TabStop = false;
            // 
            // btnRescanAll
            // 
            this.btnRescanAll.Location = new System.Drawing.Point(375, 89);
            this.btnRescanAll.Name = "btnRescanAll";
            this.btnRescanAll.Size = new System.Drawing.Size(111, 23);
            this.btnRescanAll.TabIndex = 25;
            this.btnRescanAll.Text = "Rescan";
            this.btnRescanAll.UseVisualStyleBackColor = true;
            this.btnRescanAll.Click += new System.EventHandler(this.BtnRescanAll_Click);
            // 
            // tBColorThreshold
            // 
            this.tBColorThreshold.Location = new System.Drawing.Point(9, 142);
            this.tBColorThreshold.Maximum = 100;
            this.tBColorThreshold.Name = "tBColorThreshold";
            this.tBColorThreshold.Size = new System.Drawing.Size(474, 56);
            this.tBColorThreshold.TabIndex = 26;
            this.tBColorThreshold.ValueChanged += new System.EventHandler(this.TBColorThreshold_ValueChanged);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Location = new System.Drawing.Point(375, 125);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(111, 23);
            this.btnSaveSettings.TabIndex = 27;
            this.btnSaveSettings.Text = "Save";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.BtnSaveSettings_Click);
            // 
            // Worker_LearnThresholds
            // 
            this.Worker_LearnThresholds.WorkerReportsProgress = true;
            this.Worker_LearnThresholds.WorkerSupportsCancellation = true;
            this.Worker_LearnThresholds.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Worker_LearnThresholds_DoWork);
            this.Worker_LearnThresholds.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.Worker_LearnThresholds_ProgressChanged);
            this.Worker_LearnThresholds.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Worker_LearnThresholds_RunWorkerCompleted);
            // 
            // btnLearnRedThresholds
            // 
            this.btnLearnRedThresholds.Location = new System.Drawing.Point(375, 175);
            this.btnLearnRedThresholds.Name = "btnLearnRedThresholds";
            this.btnLearnRedThresholds.Size = new System.Drawing.Size(111, 23);
            this.btnLearnRedThresholds.TabIndex = 28;
            this.btnLearnRedThresholds.Text = "Learn Red Thresholds";
            this.btnLearnRedThresholds.UseVisualStyleBackColor = true;
            this.btnLearnRedThresholds.Click += new System.EventHandler(this.btnLearnRedThresholds_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(491, 28);
            this.menuStrip1.TabIndex = 29;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(62, 24);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.tsLblKills,
            this.tsLblAlive,
            this.tsLblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 346);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(491, 25);
            this.statusStrip1.TabIndex = 30;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.MarqueeAnimationSpeed = 50;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 19);
            this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // tsLblKills
            // 
            this.tsLblKills.Name = "tsLblKills";
            this.tsLblKills.Size = new System.Drawing.Size(51, 20);
            this.tsLblKills.Text = "Kills: 0";
            // 
            // tsLblAlive
            // 
            this.tsLblAlive.Name = "tsLblAlive";
            this.tsLblAlive.Size = new System.Drawing.Size(73, 20);
            this.tsLblAlive.Text = "Alive: 100";
            // 
            // tsLblStatus
            // 
            this.tsLblStatus.Name = "tsLblStatus";
            this.tsLblStatus.Size = new System.Drawing.Size(0, 20);
            // 
            // Worker_KillMessageCleaner
            // 
            this.Worker_KillMessageCleaner.WorkerReportsProgress = true;
            this.Worker_KillMessageCleaner.WorkerSupportsCancellation = true;
            this.Worker_KillMessageCleaner.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Worker_KillMessageCleaner_DoWork);
            this.Worker_KillMessageCleaner.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.Worker_KillMessageCleaner_ProgressChanged);
            this.Worker_KillMessageCleaner.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Worker_KillMessageCleaner_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 371);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnSelectKillMessage);
            this.Controls.Add(this.btnLearnRedThresholds);
            this.Controls.Add(this.btnSaveSettings);
            this.Controls.Add(this.btnRescanAll);
            this.Controls.Add(this.btnRedKillNumber);
            this.Controls.Add(this.pBCenterKillNumber);
            this.Controls.Add(this.btnPickReferenceWhite);
            this.Controls.Add(this.btnPickReferenceRed);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.tBLog);
            this.Controls.Add(this.pBKillCounterTopRight);
            this.Controls.Add(this.pBRedKill);
            this.Controls.Add(this.btnWhiteKillCounter);
            this.Controls.Add(this.btnSelRedKill);
            this.Controls.Add(this.pBKillMessage);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.tBColorThreshold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PUBG-TRIGGR by PippoFPS";
            ((System.ComponentModel.ISupportInitialize)(this.pBKillMessage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBRedKill)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBKillCounterTopRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBCenterKillNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBColorThreshold)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectKillMessage;
        private System.Windows.Forms.PictureBox pBKillMessage;
        private System.Windows.Forms.Button btnSelRedKill;
        private System.Windows.Forms.Button btnWhiteKillCounter;
        private System.Windows.Forms.PictureBox pBRedKill;
        private System.Windows.Forms.PictureBox pBKillCounterTopRight;
        private System.ComponentModel.BackgroundWorker Scanner_RedKill;
        private System.Windows.Forms.TextBox tBLog;
        private System.Windows.Forms.Button btnStart;
        private System.ComponentModel.BackgroundWorker Scanner_KillNumber;
        private System.Windows.Forms.Button btnPickReferenceRed;
        private System.Windows.Forms.Button btnPickReferenceWhite;
        private System.Windows.Forms.Button btnRedKillNumber;
        private System.Windows.Forms.PictureBox pBCenterKillNumber;
        private System.Windows.Forms.Button btnRescanAll;
        private System.Windows.Forms.TrackBar tBColorThreshold;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.ComponentModel.BackgroundWorker Worker_LearnThresholds;
        private System.Windows.Forms.Button btnLearnRedThresholds;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsLblStatus;
        private System.Windows.Forms.ToolStripStatusLabel tsLblKills;
        private System.Windows.Forms.ToolStripStatusLabel tsLblAlive;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.ComponentModel.BackgroundWorker Worker_KillMessageCleaner;
    }
}

