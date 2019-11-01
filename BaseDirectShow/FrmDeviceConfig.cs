using DirectShowLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseDirectShow
{
    public partial class FrmDeviceConfig : Form
    {
        private string _VideoDeviceName;
        public string VideoDeviceName
        {
            get
            {
                return this._VideoDeviceName;
            }
            set
            {
                this._VideoDeviceName = value;
            }
        }

        private string _AudioInputDeviceName;
        public string AudioInputDeviceName
        {
            get
            {
                return this._AudioInputDeviceName;
            }
            set
            {
                this._AudioInputDeviceName = value;
            }
        }

        private int _Frames;
        public int Frames
        {
            get
            {
                return this._Frames;
            }
            set
            {
                this._Frames = value;
            }
        }

        private string _Resolution;
        public string Resolution
        {
            get
            {
                return this._Resolution;
            }
            set
            {
                this._Resolution = value;
            }
        }

        public FrmDeviceConfig()
        {
            InitializeComponent();
        }

        private void FrmDeviceConfig_Load(object sender, EventArgs e)
        {
            this.BindDevice(this.cmbCamera, FilterCategory.VideoInputDevice);
            this.BindDevice(this.cmbMicrophone, FilterCategory.AudioInputDevice);
            this.BindFrame();
        }


        private void BindDevice(ComboBox cmb, Guid category)
        {
            cmb.Items.Clear();
            DsDevice[] devicesOfCat = DsDevice.GetDevicesOfCat(category);
            bool flag = devicesOfCat != null;
            if (flag)
            {
                foreach (DsDevice dsDevice in devicesOfCat)
                {
                    cmb.Items.Add(dsDevice.Name);
                }
            }
            cmb.SelectedIndex = ((cmb.Items.Count > 0) ? 0 : -1);
        }


        private void BindFrame()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Number");
            dataTable.Columns.Add("Frames");
            DataRow dataRow = dataTable.NewRow();
            dataRow["Number"] = "0";
            dataRow["Frames"] = "<None>";
            dataTable.Rows.Add(dataRow);
            dataRow = dataTable.NewRow();
            dataRow["Number"] = "15";
            dataRow["Frames"] = "15 fps";
            dataTable.Rows.Add(dataRow);
            dataRow = dataTable.NewRow();
            dataRow["Number"] = "20";
            dataRow["Frames"] = "20 fps";
            dataTable.Rows.Add(dataRow);
            dataRow = dataTable.NewRow();
            dataRow["Number"] = "30";
            dataRow["Frames"] = "30 fps";
            dataTable.Rows.Add(dataRow);
            dataRow = dataTable.NewRow();
            dataRow["Number"] = "60";
            dataRow["Frames"] = "60 fps";
            dataTable.Rows.Add(dataRow);
            this.cmbFrame.DataSource = dataTable;
            this.cmbFrame.DisplayMember = "Frames";
            this.cmbFrame.ValueMember = "Number";
            this.cmbFrame.SelectedIndex = 0;
        }


        private void cmbCamera_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = (this.cmbCamera.Tag == null) ? string.Empty : this.cmbCamera.Tag.ToString();
            bool flag = text.Equals(this.cmbCamera.Text.Trim());
            if (!flag)
            {
                this.cmbCamera.Tag = this.cmbCamera.Text.Trim();
                this.GetCaptureSupportSize(this.cmbCamera.Text);
            }
        }


        public void GetCaptureSupportSize(string sFriendlyName)
        {
            ICaptureGraphBuilder2 captureGraphBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            IBaseFilter pbf = this.CreateFilter(FilterCategory.VideoInputDevice, sFriendlyName);
            object obj;
            int hr = captureGraphBuilder.FindInterface(PinCategory.Capture, MediaType.Video, pbf, typeof(IAMStreamConfig).GUID, out obj);
            DsError.ThrowExceptionForHR(hr);
            IAMStreamConfig iamstreamConfig = obj as IAMStreamConfig;
            bool flag = iamstreamConfig == null;
            if (flag)
            {
                MessageBox.Show("获取IAMStreamConfig失败！");
            }
            else
            {
                VideoStreamConfigCaps structure = new VideoStreamConfigCaps();
                int num;
                int num2;
                hr = iamstreamConfig.GetNumberOfCapabilities(out num, out num2);
                DsError.ThrowExceptionForHR(hr);
                bool flag2 = Marshal.SizeOf(structure) != num2;
                if (flag2)
                {
                    MessageBox.Show("获取分辨率失败！");
                }
                else
                {
                    this.cmbResolution.Items.Clear();
                    for (int i = 0; i < num; i++)
                    {
                        IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(structure));
                        AMMediaType ammediaType = new AMMediaType();
                        hr = iamstreamConfig.GetStreamCaps(i, out ammediaType, intPtr);
                        DsError.ThrowExceptionForHR(hr);
                        bool flag3 = ammediaType.majorType == MediaType.Video && ammediaType.formatType == FormatType.VideoInfo;
                        if (flag3)
                        {
                            Marshal.StructureToPtr(structure, intPtr, false);
                            VideoInfoHeader videoInfoHeader = new VideoInfoHeader();
                            Marshal.PtrToStructure(ammediaType.formatPtr, videoInfoHeader);
                            int width = videoInfoHeader.BmiHeader.Width;
                            int height = videoInfoHeader.BmiHeader.Height;
                            this.cmbResolution.Items.Add(width + "*" + height);
                        }
                        Marshal.FreeCoTaskMem(intPtr);
                        DsUtils.FreeAMMediaType(ammediaType);
                    }
                    this.cmbResolution.SelectedIndex = ((this.cmbResolution.Items.Count > 0) ? 0 : -1);
                }
            }
        }


        private IBaseFilter CreateFilter(Guid category, string sFriendlyName)
        {
            object obj = null;
            Guid guid = typeof(IBaseFilter).GUID;
            ArrayList arrayList = new ArrayList();
            foreach (DsDevice dsDevice in DsDevice.GetDevicesOfCat(category))
            {
                bool flag = dsDevice.Name.CompareTo(sFriendlyName) == 0;
                if (flag)
                {
                    dsDevice.Mon.BindToObject(null, null, ref guid, out obj);
                    break;
                }
            }
            return (IBaseFilter)obj;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this._VideoDeviceName = this.cmbCamera.Text;
            this._AudioInputDeviceName = this.cmbMicrophone.Text;
            this._Resolution = this.cmbResolution.Text;
            this._Frames = ((this.cmbFrame.SelectedValue != null && !string.IsNullOrEmpty(this.cmbFrame.SelectedValue.ToString())) ? int.Parse(this.cmbFrame.SelectedValue.ToString()) : 0);
            base.DialogResult = DialogResult.OK;
        }
    }
}
