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
    public partial class FrmImageShower : Form
    {
        public FrmImageShower(string path)
        {
            InitializeComponent();
            this.pictureBox1.Image = Image.FromFile(path);
        }
    }
}
