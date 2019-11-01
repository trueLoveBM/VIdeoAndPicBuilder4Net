using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaseDirectShow.SharePreferences;
using BaseDirectShow.Entity;

namespace BaseDirectShow
{
    public partial class VideoSettingSecView : UserControl
    {
        public VideoSettingSecView()
        {
            InitializeComponent();
        }

        private void VideoSettingView_Load(object sender, EventArgs e)
        {
            var VideoSetting = VideoSettingUtils.Instance.GetAllVideoSettings();
            this.cbVideoSetting.DataSource = VideoSetting;
        }

        private void cbVideoSetting_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbVideoSetting.SelectedItem is VideoSetting)
                DirectShow.Instance.SetSettingValue(cbVideoSetting.SelectedItem as VideoSetting, true);
        }
    }
}
