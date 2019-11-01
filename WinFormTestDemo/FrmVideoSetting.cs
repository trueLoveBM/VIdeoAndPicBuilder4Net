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
    public partial class FrmVideoSetting : Form
    {
        public string SettingName { get; set; }

        public bool CbDefaultSetting { get; set; }

        public FrmVideoSetting()
        {
            InitializeComponent();
            this.FormClosing += FrmVideoSetting_FormClosing;
        }

        private void FrmVideoSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            SettingName = this.tbSettingName.Text;
            CbDefaultSetting = this.cbAsDefaultSetting.Checked;
        }
    }
}
