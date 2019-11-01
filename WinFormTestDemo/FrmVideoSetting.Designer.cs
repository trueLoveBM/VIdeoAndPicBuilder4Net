namespace WinFormTestDemo
{
    partial class FrmVideoSetting
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbSettingName = new System.Windows.Forms.TextBox();
            this.cbAsDefaultSetting = new System.Windows.Forms.CheckBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "方案名";
            // 
            // tbSettingName
            // 
            this.tbSettingName.Location = new System.Drawing.Point(12, 24);
            this.tbSettingName.Name = "tbSettingName";
            this.tbSettingName.Size = new System.Drawing.Size(229, 21);
            this.tbSettingName.TabIndex = 1;
            // 
            // cbAsDefaultSetting
            // 
            this.cbAsDefaultSetting.AutoSize = true;
            this.cbAsDefaultSetting.Location = new System.Drawing.Point(12, 67);
            this.cbAsDefaultSetting.Name = "cbAsDefaultSetting";
            this.cbAsDefaultSetting.Size = new System.Drawing.Size(108, 16);
            this.cbAsDefaultSetting.TabIndex = 2;
            this.cbAsDefaultSetting.Text = "保存为默认配置";
            this.cbAsDefaultSetting.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(14, 115);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(191, 115);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmVideoSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 150);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.cbAsDefaultSetting);
            this.Controls.Add(this.tbSettingName);
            this.Controls.Add(this.label1);
            this.Name = "FrmVideoSetting";
            this.Text = "FrmVideoSetting";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbSettingName;
        private System.Windows.Forms.CheckBox cbAsDefaultSetting;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}