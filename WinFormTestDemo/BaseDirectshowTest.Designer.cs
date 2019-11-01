namespace WinFormTestDemo
{
    partial class BaseDirectshowTest
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
            this.panelView = new System.Windows.Forms.Panel();
            this.lblVideoRecordingTime = new System.Windows.Forms.Label();
            this.btnVideoTape = new System.Windows.Forms.Button();
            this.btnCameraSetting = new System.Windows.Forms.Button();
            this.process1 = new System.Diagnostics.Process();
            this.label1 = new System.Windows.Forms.Label();
            this.tbarLight = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.tbarContrast = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.tbarSaturation = new System.Windows.Forms.TrackBar();
            this.btnSaveSetting = new System.Windows.Forms.Button();
            this.lvSettings = new System.Windows.Forms.ListBox();
            this.videoSettingBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lbDefaultSettingName = new System.Windows.Forms.Label();
            this.btnReadAndSave = new System.Windows.Forms.Button();
            this.btnTakePic = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tbarLight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbarContrast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbarSaturation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.videoSettingBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panelView
            // 
            this.panelView.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelView.Location = new System.Drawing.Point(394, 0);
            this.panelView.Name = "panelView";
            this.panelView.Size = new System.Drawing.Size(406, 450);
            this.panelView.TabIndex = 0;
            // 
            // lblVideoRecordingTime
            // 
            this.lblVideoRecordingTime.AutoSize = true;
            this.lblVideoRecordingTime.Location = new System.Drawing.Point(12, 429);
            this.lblVideoRecordingTime.Name = "lblVideoRecordingTime";
            this.lblVideoRecordingTime.Size = new System.Drawing.Size(41, 12);
            this.lblVideoRecordingTime.TabIndex = 1;
            this.lblVideoRecordingTime.Text = "label1";
            // 
            // btnVideoTape
            // 
            this.btnVideoTape.Location = new System.Drawing.Point(12, 12);
            this.btnVideoTape.Name = "btnVideoTape";
            this.btnVideoTape.Size = new System.Drawing.Size(75, 23);
            this.btnVideoTape.TabIndex = 2;
            this.btnVideoTape.Text = "录像";
            this.btnVideoTape.UseVisualStyleBackColor = true;
            this.btnVideoTape.Click += new System.EventHandler(this.btnVideoTape_Click);
            // 
            // btnCameraSetting
            // 
            this.btnCameraSetting.Location = new System.Drawing.Point(12, 41);
            this.btnCameraSetting.Name = "btnCameraSetting";
            this.btnCameraSetting.Size = new System.Drawing.Size(75, 23);
            this.btnCameraSetting.TabIndex = 3;
            this.btnCameraSetting.Text = "摄像头设置";
            this.btnCameraSetting.UseVisualStyleBackColor = true;
            this.btnCameraSetting.Click += new System.EventHandler(this.btnCameraSetting_Click);
            // 
            // process1
            // 
            this.process1.StartInfo.Domain = "";
            this.process1.StartInfo.LoadUserProfile = false;
            this.process1.StartInfo.Password = null;
            this.process1.StartInfo.StandardErrorEncoding = null;
            this.process1.StartInfo.StandardOutputEncoding = null;
            this.process1.StartInfo.UserName = "";
            this.process1.SynchronizingObject = this;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "亮度";
            // 
            // tbarLight
            // 
            this.tbarLight.Location = new System.Drawing.Point(15, 107);
            this.tbarLight.Maximum = 255;
            this.tbarLight.Name = "tbarLight";
            this.tbarLight.Size = new System.Drawing.Size(155, 45);
            this.tbarLight.TabIndex = 6;
            this.tbarLight.ValueChanged += new System.EventHandler(this.tbarLight_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "对比度";
            // 
            // tbarContrast
            // 
            this.tbarContrast.Location = new System.Drawing.Point(13, 175);
            this.tbarContrast.Maximum = 255;
            this.tbarContrast.Name = "tbarContrast";
            this.tbarContrast.Size = new System.Drawing.Size(157, 45);
            this.tbarContrast.TabIndex = 8;
            this.tbarContrast.ValueChanged += new System.EventHandler(this.tbarContrast_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 227);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "饱和度";
            // 
            // tbarSaturation
            // 
            this.tbarSaturation.Location = new System.Drawing.Point(12, 242);
            this.tbarSaturation.Maximum = 255;
            this.tbarSaturation.Name = "tbarSaturation";
            this.tbarSaturation.Size = new System.Drawing.Size(158, 45);
            this.tbarSaturation.TabIndex = 10;
            this.tbarSaturation.ValueChanged += new System.EventHandler(this.tbarSaturation_ValueChanged);
            // 
            // btnSaveSetting
            // 
            this.btnSaveSetting.Location = new System.Drawing.Point(12, 293);
            this.btnSaveSetting.Name = "btnSaveSetting";
            this.btnSaveSetting.Size = new System.Drawing.Size(158, 23);
            this.btnSaveSetting.TabIndex = 12;
            this.btnSaveSetting.Text = "保存";
            this.btnSaveSetting.UseVisualStyleBackColor = true;
            this.btnSaveSetting.Click += new System.EventHandler(this.btnSaveSetting_Click);
            // 
            // lvSettings
            // 
            this.lvSettings.DataSource = this.videoSettingBindingSource;
            this.lvSettings.DisplayMember = "VideoSettingName";
            this.lvSettings.FormattingEnabled = true;
            this.lvSettings.ItemHeight = 12;
            this.lvSettings.Location = new System.Drawing.Point(176, 7);
            this.lvSettings.Name = "lvSettings";
            this.lvSettings.Size = new System.Drawing.Size(212, 448);
            this.lvSettings.TabIndex = 13;
            this.lvSettings.SelectedIndexChanged += new System.EventHandler(this.lvSettings_SelectedIndexChanged);
            // 
            // videoSettingBindingSource
            // 
            this.videoSettingBindingSource.DataSource = typeof(BaseDirectShow.Entity.VideoSetting);
            // 
            // lbDefaultSettingName
            // 
            this.lbDefaultSettingName.AutoSize = true;
            this.lbDefaultSettingName.Location = new System.Drawing.Point(10, 337);
            this.lbDefaultSettingName.Name = "lbDefaultSettingName";
            this.lbDefaultSettingName.Size = new System.Drawing.Size(77, 12);
            this.lbDefaultSettingName.TabIndex = 14;
            this.lbDefaultSettingName.Text = "默认设置：无";
            // 
            // btnReadAndSave
            // 
            this.btnReadAndSave.Location = new System.Drawing.Point(95, 41);
            this.btnReadAndSave.Name = "btnReadAndSave";
            this.btnReadAndSave.Size = new System.Drawing.Size(75, 23);
            this.btnReadAndSave.TabIndex = 15;
            this.btnReadAndSave.Text = "读取并保存";
            this.btnReadAndSave.UseVisualStyleBackColor = true;
            this.btnReadAndSave.Click += new System.EventHandler(this.btnReadAndSave_Click);
            // 
            // btnTakePic
            // 
            this.btnTakePic.Location = new System.Drawing.Point(95, 13);
            this.btnTakePic.Name = "btnTakePic";
            this.btnTakePic.Size = new System.Drawing.Size(75, 23);
            this.btnTakePic.TabIndex = 16;
            this.btnTakePic.Text = "拍照";
            this.btnTakePic.UseVisualStyleBackColor = true;
            this.btnTakePic.Click += new System.EventHandler(this.btnTakePic_Click);
            // 
            // BaseDirectshowTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnTakePic);
            this.Controls.Add(this.btnReadAndSave);
            this.Controls.Add(this.lbDefaultSettingName);
            this.Controls.Add(this.lvSettings);
            this.Controls.Add(this.btnSaveSetting);
            this.Controls.Add(this.tbarSaturation);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbarContrast);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbarLight);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCameraSetting);
            this.Controls.Add(this.btnVideoTape);
            this.Controls.Add(this.lblVideoRecordingTime);
            this.Controls.Add(this.panelView);
            this.Name = "BaseDirectshowTest";
            this.Text = "BaseDirectshowTest";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.BaseDirectshowTest_FormClosed);
            this.Load += new System.EventHandler(this.BaseDirectshowTest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tbarLight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbarContrast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbarSaturation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.videoSettingBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelView;
        private System.Windows.Forms.Label lblVideoRecordingTime;
        private System.Windows.Forms.Button btnVideoTape;
        private System.Windows.Forms.Button btnCameraSetting;
        private System.Diagnostics.Process process1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar tbarLight;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar tbarContrast;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar tbarSaturation;
        private System.Windows.Forms.Button btnSaveSetting;
        private System.Windows.Forms.ListBox lvSettings;
        private System.Windows.Forms.BindingSource videoSettingBindingSource;
        private System.Windows.Forms.Label lbDefaultSettingName;
        private System.Windows.Forms.Button btnReadAndSave;
        private System.Windows.Forms.Button btnTakePic;
    }
}