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
    public partial class FrmVideoShower : Form
    {
        private string videoPath;
        public FrmVideoShower(string videoPath)
        {
            InitializeComponent();
            this.videoPath = videoPath;
        }

        private void FrmVideoShower_Load(object sender, EventArgs e)
        {
            this.axWindowsMediaPlayer1.URL = videoPath;
            axWindowsMediaPlayer1.Visible = true;
            axWindowsMediaPlayer1.BringToFront();
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void FrmVideoShower_FormClosing(object sender, FormClosingEventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.pause();
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }
    }
}
