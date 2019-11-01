using BaseDirectShow;
using BaseDirectShow.Entity;
using BaseDirectShow.SharePreferences;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormTestDemo
{
    public partial class BaseDirectshowTest : Form
    {
        private Image tapeVideoImage = null;
        private Image tapingVideoImage = null;
        private Stopwatch sw = new Stopwatch(); //秒表对象
        Timer tVideoRecordingTime;

        public BaseDirectshowTest()
        {
            InitializeComponent();
            tVideoRecordingTime = new Timer();
        }

        private void BaseDirectshowTest_Load(object sender, EventArgs e)
        {
            MenuItem menu = new MenuItem();
            menu.Text = "设为默认配置";
            menu.Click += SetDefault_Click;
            lvSettings.ContextMenu = new ContextMenu();
            lvSettings.ContextMenu.MenuItems.Add(menu);

            DirectShow.Instance.Preview(panelView);
            RefreshSetting();

        }

        private void SetDefault_Click(object sender, EventArgs e)
        {
            DirectShow.Instance.SetSettingValue(lvSettings.SelectedItem as VideoSetting);
            //VideoSettingUtils.Instance.SetDefaultSettings(lvSettings.SelectedItem as VideoSetting);
            RefreshSetting();
        }

        private void BaseDirectshowTest_FormClosed(object sender, FormClosedEventArgs e)
        {
            DirectShow.Instance.Dispose();
        }

        /// <summary>
        /// 结束上次准备下次录像
        /// </summary>
        internal void RestartWaitVideoTape()
        {
            DirectShow.Instance.StopMonitorRecord();
            sw.Stop();
            
            this.tVideoRecordingTime.Stop();
            this.btnVideoTape.Image = tapeVideoImage;
            this.btnVideoTape.Text = "录像";
            if (this.lblVideoRecordingTime.Tag is string sFilePath
                && !string.IsNullOrEmpty(sFilePath))
            {
                //this.SaveVideoFile?.Invoke(new PFS_PathologicalFileDTO
                //{
                //    Filegroup = EnFileGroup.wmv.Int32String(),
                //    Filetag = this.lblVideoRecordingTime.Text,
                //    Flag_storedisk = SystemConst.Const1,
                //    Storepath = sFilePath,
                //    Createdoctor = CurrentCache.Instance.Name,
                //    Createtime = DateTime.Now.ToNowTimeString(),
                //    Flag_valid = SystemConst.Const0
                //});
            }
            this.lblVideoRecordingTime.Tag = null;
            new FrmVideoShower(this.sFilePath).ShowDialog();

        }

        private string sFilePath;

        internal void StartVideoTape()
        {
            this.sFilePath = string.Empty;
            string filePath = $@"{ConstVar.CacheVideo}/{DateTime.Now.ToString("yyyyMMdd")}/{Guid.NewGuid()}";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string sFilePath = filePath.GetUniqueFileName(".wmv");
            this.lblVideoRecordingTime.Text = "00:00";
            this.lblVideoRecordingTime.Tag = sFilePath;
            sw.Reset();
            this.sFilePath = sFilePath;
            DirectShow.Instance.StartMonitorRecord(sFilePath);
            sw.Start();
            this.tVideoRecordingTime.Start();
            this.btnVideoTape.Image = tapingVideoImage;
            this.btnVideoTape.Text = "录像中";
        }

        private void btnVideoTape_Click(object sender, EventArgs e)
        {
            if (this.btnVideoTape.Text == "录像")
            {
                this.StartVideoTape();
            }
            else
            {
                this.RestartWaitVideoTape();
            }
        }

        private void btnCameraSetting_Click(object sender, EventArgs e)
        {
            DirectShow.Instance.changeCameraSetting(this.Handle);
        }

        /// <summary>
        /// 亮度调节
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarLight_ValueChanged(object sender, EventArgs e)
        {
            int x = DirectShow.Instance.SetLightValue(tbarLight.Value);
            lblVideoRecordingTime.Text = "亮度设置" + x.ToString();
        }


        /// <summary>
        /// 对比度调节
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarContrast_ValueChanged(object sender, EventArgs e)
        {
            int x = DirectShow.Instance.SetContrastValue(tbarContrast.Value);
            lblVideoRecordingTime.Text = "对比度设置" + x.ToString();
        }


        /// <summary>
        /// 饱和度调节
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarSaturation_ValueChanged(object sender, EventArgs e)
        {
            int x = DirectShow.Instance.SetSaturationValue(tbarSaturation.Value);
            lblVideoRecordingTime.Text = "饱和度设置" + x.ToString();
        }


        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveSetting_Click(object sender, EventArgs e)
        {
            FrmVideoSetting settingView = new FrmVideoSetting();
            if (settingView.ShowDialog() == DialogResult.OK)
            {
                VideoSetting setting = new VideoSetting()
                {
                    Saturation = tbarSaturation.Value,
                    ContrastRatio = tbarContrast.Value,
                    Brightness = tbarLight.Value,
                    DefaultSetting = settingView.CbDefaultSetting,
                    VideoSettingName = settingView.SettingName
                };
                VideoSettingUtils.Instance.SaveVideoSetting(setting);
                ReadSetting();
            }

        }

        /// <summary>
        /// 重新获取配置相关功能
        /// </summary>
        public void RefreshSetting()
        {
            List<VideoSetting> videos = ReadSetting();
            this.lvSettings.DataSource = videos;
            if (videos != null)
            {
                var v = videos.Find(m => m.DefaultSetting == true);
                if (v != null)
                {
                    lvSettings.SelectedItem = v;

                    lbDefaultSettingName.Text = v.VideoSettingName;
                    //DirectShow.Instance.SetSettingValue(v);
                    tbarSaturation.Value = v.Saturation;
                    tbarContrast.Value = v.ContrastRatio;
                    tbarLight.Value = v.Brightness;
                    //lblVideoRecordingTime.Text = v.Brightness + " " + v.ContrastRatio + "  " + v.Saturation;
                }
                else
                {
                    lvSettings.SelectedItem = null;
                }
            }
        }

        public List<VideoSetting> ReadSetting()
        {
            List<VideoSetting> settings = VideoSettingUtils.Instance.GetAllVideoSettings();
            return settings;
        }

        private void lvSettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            DirectShow.Instance.SetSettingValue(lvSettings.SelectedItem as VideoSetting);
            //if (v != null)
            //{
            //    //DirectShow.Instance.SetSettingValue(v);
            //    tbarSaturation.Value = v.Saturation;
            //    tbarContrast.Value = v.ContrastRatio;
            //    tbarLight.Value = v.Brightness;
            //    //lblVideoRecordingTime.Text = v.Brightness + " " + v.ContrastRatio + "  " + v.Saturation;
            //}
        }

        /// <summary>
        /// 读取并保存当前摄像头的设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadAndSave_Click(object sender, EventArgs e)
        {
            FrmVideoSetting settingView = new FrmVideoSetting();
            if (settingView.ShowDialog() == DialogResult.OK)
            {
                DirectShow.Instance.SaveCurrentOriginSetting(settingView.SettingName, settingView.CbDefaultSetting);
                RefreshSetting();
            }


        }


        /// <summary>
        /// 拍照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTakePic_Click(object sender, EventArgs e)
        {
            string filePath = $@"{ConstVar.CacheImage}/{DateTime.Now.ToString("yyyyMMdd")}/{Guid.NewGuid()}";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string sFilePath = filePath.GetUniqueFileName(".png");
            //开始采集图片
            if (DirectShow.Instance.Cap(sFilePath))
                new FrmImageShower(sFilePath).ShowDialog();
        }
    }

    public class ConstVar
    {
        public const string CacheVideo = "Cache/Video";
        public const string CacheSound = "Cache/Sound";
        public const string CacheImage = "Cache/Image";

        /// <summary>
        /// 蜡块条码号前缀
        /// </summary>
        public const string ParaffinBarCodePrefix = "PB";

        /// <summary>
        /// 切片条码号前缀
        /// </summary>
        public const string SlideBarCodePrefix = "HE";

        /// <summary>
        /// 时间格式年月日Added by renyingjie
        /// </summary>
        public const string yyyyMMdd = "yyyy-MM-dd";

        /// <summary>
        /// 时间格式年月日时分Added by renyingjie
        /// </summary>
        public const string yyyyMMddHHmm = "yyyy-MM-dd HH:mm";
    }
}
