namespace BaseDirectShow
{
    partial class VideoSettingSecView
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cbVideoSetting = new System.Windows.Forms.ComboBox();
            this.videoSettingBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.videoSettingBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // cbVideoSetting
            // 
            this.cbVideoSetting.DataSource = this.videoSettingBindingSource;
            this.cbVideoSetting.DisplayMember = "VideoSettingName";
            this.cbVideoSetting.Dock = System.Windows.Forms.DockStyle.Right;
            this.cbVideoSetting.FormattingEnabled = true;
            this.cbVideoSetting.Location = new System.Drawing.Point(65, 0);
            this.cbVideoSetting.Name = "cbVideoSetting";
            this.cbVideoSetting.Size = new System.Drawing.Size(131, 20);
            this.cbVideoSetting.TabIndex = 0;
            this.cbVideoSetting.SelectedIndexChanged += new System.EventHandler(this.cbVideoSetting_SelectedIndexChanged);
            // 
            // videoSettingBindingSource
            // 
            this.videoSettingBindingSource.DataSource = typeof(BaseDirectShow.Entity.VideoSetting);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "选择设置";
            // 
            // VideoSettingSecView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbVideoSetting);
            this.Name = "VideoSettingSecView";
            this.Size = new System.Drawing.Size(196, 29);
            this.Load += new System.EventHandler(this.VideoSettingView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.videoSettingBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbVideoSetting;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.BindingSource videoSettingBindingSource;
    }
}
