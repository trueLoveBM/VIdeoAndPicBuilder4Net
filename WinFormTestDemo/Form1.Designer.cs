namespace WinFormTestDemo
{
    partial class Form1
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.camera = new System.Windows.Forms.Panel();
            this.btnTakePic = new System.Windows.Forms.Button();
            this.btnOpenCapture = new System.Windows.Forms.Button();
            this.btnCloseCapture = new System.Windows.Forms.Button();
            this.btnTakeVideo = new System.Windows.Forms.Button();
            this.btnStopTakeVideo = new System.Windows.Forms.Button();
            this.labelTip = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // camera
            // 
            this.camera.Dock = System.Windows.Forms.DockStyle.Right;
            this.camera.Location = new System.Drawing.Point(180, 0);
            this.camera.Name = "camera";
            this.camera.Size = new System.Drawing.Size(620, 450);
            this.camera.TabIndex = 0;
            // 
            // btnTakePic
            // 
            this.btnTakePic.Location = new System.Drawing.Point(12, 128);
            this.btnTakePic.Name = "btnTakePic";
            this.btnTakePic.Size = new System.Drawing.Size(75, 23);
            this.btnTakePic.TabIndex = 1;
            this.btnTakePic.Text = "拍照";
            this.btnTakePic.UseVisualStyleBackColor = true;
            // 
            // btnOpenCapture
            // 
            this.btnOpenCapture.Location = new System.Drawing.Point(12, 12);
            this.btnOpenCapture.Name = "btnOpenCapture";
            this.btnOpenCapture.Size = new System.Drawing.Size(75, 23);
            this.btnOpenCapture.TabIndex = 2;
            this.btnOpenCapture.Text = "打开摄像头";
            this.btnOpenCapture.UseVisualStyleBackColor = true;
            this.btnOpenCapture.Click += new System.EventHandler(this.btnOpenCapture_Click);
            // 
            // btnCloseCapture
            // 
            this.btnCloseCapture.Location = new System.Drawing.Point(12, 41);
            this.btnCloseCapture.Name = "btnCloseCapture";
            this.btnCloseCapture.Size = new System.Drawing.Size(75, 23);
            this.btnCloseCapture.TabIndex = 3;
            this.btnCloseCapture.Text = "关闭摄像头";
            this.btnCloseCapture.UseVisualStyleBackColor = true;
            this.btnCloseCapture.Click += new System.EventHandler(this.btnCloseCapture_Click);
            // 
            // btnTakeVideo
            // 
            this.btnTakeVideo.Location = new System.Drawing.Point(12, 70);
            this.btnTakeVideo.Name = "btnTakeVideo";
            this.btnTakeVideo.Size = new System.Drawing.Size(75, 23);
            this.btnTakeVideo.TabIndex = 4;
            this.btnTakeVideo.Text = "开始摄像";
            this.btnTakeVideo.UseVisualStyleBackColor = true;
            this.btnTakeVideo.Click += new System.EventHandler(this.btnTakeVideo_Click);
            // 
            // btnStopTakeVideo
            // 
            this.btnStopTakeVideo.Enabled = false;
            this.btnStopTakeVideo.Location = new System.Drawing.Point(12, 99);
            this.btnStopTakeVideo.Name = "btnStopTakeVideo";
            this.btnStopTakeVideo.Size = new System.Drawing.Size(75, 23);
            this.btnStopTakeVideo.TabIndex = 5;
            this.btnStopTakeVideo.Text = "停止摄像";
            this.btnStopTakeVideo.UseVisualStyleBackColor = true;
            this.btnStopTakeVideo.Click += new System.EventHandler(this.btnStopTakeVideo_Click);
            // 
            // labelTip
            // 
            this.labelTip.AutoSize = true;
            this.labelTip.Location = new System.Drawing.Point(13, 426);
            this.labelTip.Name = "labelTip";
            this.labelTip.Size = new System.Drawing.Size(65, 12);
            this.labelTip.TabIndex = 6;
            this.labelTip.Text = "提示信息！";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.labelTip);
            this.Controls.Add(this.btnStopTakeVideo);
            this.Controls.Add(this.btnTakeVideo);
            this.Controls.Add(this.btnCloseCapture);
            this.Controls.Add(this.btnOpenCapture);
            this.Controls.Add(this.btnTakePic);
            this.Controls.Add(this.camera);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel camera;
        private System.Windows.Forms.Button btnTakePic;
        private System.Windows.Forms.Button btnOpenCapture;
        private System.Windows.Forms.Button btnCloseCapture;
        private System.Windows.Forms.Button btnTakeVideo;
        private System.Windows.Forms.Button btnStopTakeVideo;
        private System.Windows.Forms.Label labelTip;
    }
}

