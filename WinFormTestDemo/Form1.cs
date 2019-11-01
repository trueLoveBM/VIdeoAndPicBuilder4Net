//using DirectX.Capture;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormTestDemo
{
    public partial class Form1 : Form
    {
        //Filters filters;
        //Capture Capture;

        //摄像头打开次数
        private int openCount = 1;

        public Form1()
        {
            InitializeComponent();

            //filters = new Filters();
            //Capture = new Capture(filters.VideoInputDevices[0], filters.AudioInputDevices[0]);
        }

        /// <summary>
        /// 窗体加载完成
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {


        }

        /// <summary>
        /// 打开摄像头
        /// </summary>
        private void btnOpenCapture_Click(object sender, EventArgs e)
        {
            //labelTip.Text = "摄像头打开次数"+ (openCount++);
            //if (Capture != null)
            //{
            //    Capture.Stop();
            //    Capture.PreviewWindow = null;
            //    Capture = null;
            //}
            //Capture = new Capture(filters.VideoInputDevices[0], filters.AudioInputDevices[0]);

            //这里可以设置使用哪种压缩编码方式
            //Capture.VideoCompressor = filters.VideoCompressors[0];
            //Capture.AudioCompressor = filters.AudioCompressors[0];

            //Capture.FrameRate = 29.997;                 // NTSC
            //Capture.FrameSize = new Size(640, 480);   // 640x480
            //Capture.AudioSamplingRate = 44100;          // 44.1 kHz
            //Capture.AudioSampleSize = 16;               // 16-bit
            //Capture.AudioChannels = 1;                  // Mono
            //    Capture.Filename = @"‪C:\Users\zdt\Desktop\MyVideo.avi";

            //    Capture.PreviewWindow = camera;
        }

        /// <summary>
        /// 关闭摄像头
        /// </summary>
        private void btnCloseCapture_Click(object sender, EventArgs e)
        {
            if (Capture != null)
            {
                //Capture.Stop();
                //Capture.Dispose();
                //Capture = null;
            }
        }

        /// <summary>
        /// 开始摄像
        /// </summary>
        private void btnTakeVideo_Click(object sender, EventArgs e)
        {
            //if (Capture != null)
            //    Capture.Start();
            //btnTakeVideo.Enabled = false;
            //btnStopTakeVideo.Enabled = true;
        }

        /// <summary>
        /// 停止摄像
        /// </summary>
        private void btnStopTakeVideo_Click(object sender, EventArgs e)
        {

            //var source = Capture.VideoSource;

            //if (Capture != null)
            //    Capture.Stop();
            //btnTakeVideo.Enabled = true;
            //btnStopTakeVideo.Enabled = false;
        }
    }
}
