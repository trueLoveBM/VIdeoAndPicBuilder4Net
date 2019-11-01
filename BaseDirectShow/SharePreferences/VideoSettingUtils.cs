using BaseDirectShow.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BaseDirectShow.SharePreferences
{
    /// <summary>
    /// 视频参数设置
    /// </summary>
    public class VideoSettingUtils
    {

        public static VideoSettingUtils _instance;

        public static VideoSettingUtils Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new VideoSettingUtils();
                return _instance;
            }
        }

        /// <summary>
        /// 视频保存路径
        /// </summary>
        private string _VideoSettingFilePath;

        /// <summary>
        /// 配置文件名称
        /// </summary>
        private string _VideoSettingFileName;


        /// <summary>
        /// 文件的最终绝对路径
        /// </summary>
        private string _VideoSettingRealPath;

        private VideoSettingUtils()
        {
            _VideoSettingFilePath = ConfigurationManager.AppSettings["VideoSettingFilePath"];
            if (string.IsNullOrEmpty(_VideoSettingFilePath))
                //throw new Exception("未找到视频配置文件路径");
                _VideoSettingFilePath = "Config/VideoSetting";
            _VideoSettingFileName = ConfigurationManager.AppSettings["VideoSettingFileName"];
            if (string.IsNullOrEmpty(_VideoSettingFileName))
                //    throw new Exception("未找到视频配置文件名称");
                _VideoSettingFileName = "VideoSetting.xml";

            if (!Directory.Exists(System.Environment.CurrentDirectory + "/" + _VideoSettingFilePath))
            {
                Directory.CreateDirectory(System.Environment.CurrentDirectory + "/" + _VideoSettingFilePath);
            }

            _VideoSettingRealPath = System.Environment.CurrentDirectory + "/" + _VideoSettingFilePath + "/" + _VideoSettingFileName;
            //if (!File.Exists(_VideoSettingRealPath))
            //{
            //    File.Create(_VideoSettingRealPath);
            //}
        }


        public List<VideoSetting> GetAllVideoSettings()
        {
            List<VideoSetting> result = new List<VideoSetting>();
            #region 读取文件
            if (!File.Exists(_VideoSettingRealPath))
            {
                return null;
            }

            XmlTextReader reader = new XmlTextReader(_VideoSettingRealPath);
            VideoSetting setting = null;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == nameof(VideoSetting))
                    {
                        setting = new VideoSetting();
                        result.Add(setting);
                    }
                    if (reader.Name == nameof(setting.VideoSettingName))
                    {
                        setting.VideoSettingName = reader.ReadElementContentAsString();

                    }
                    else if (reader.Name == nameof(setting.Brightness))
                    {
                        if (setting != null)
                            setting.Brightness = reader.ReadElementContentAsInt();
                    }
                    else if (reader.Name == nameof(setting.AutoBrightness))
                    {
                        if (setting != null)
                            setting.AutoBrightness = reader.ReadElementContentAsBoolean();
                    }
                    else if (reader.Name == nameof(setting.ContrastRatio))
                    {
                        if (setting != null)
                            setting.ContrastRatio = reader.ReadElementContentAsInt();
                    }
                    else if (reader.Name == nameof(setting.AutoContrastRatio))
                    {
                        if (setting != null)
                            setting.AutoContrastRatio = reader.ReadElementContentAsBoolean();
                    }
                    else if (reader.Name == nameof(setting.Saturation))
                    {
                        if (setting != null)
                            setting.Saturation = reader.ReadElementContentAsInt();
                    }
                    else if (reader.Name == nameof(setting.AutoSaturation))
                    {
                        if (setting != null)
                            setting.AutoSaturation = reader.ReadElementContentAsBoolean();
                    }
                    else if (reader.Name == nameof(setting.DefaultSetting))
                    {
                        if (setting != null)
                            setting.DefaultSetting = reader.ReadElementContentAsBoolean();
                    }
                    else if (reader.Name == nameof(setting.Hue))
                    {
                        if (setting != null)
                            setting.Hue = reader.ReadElementContentAsInt();
                    }
                    else if (reader.Name == nameof(setting.AutoHue))
                    {
                        if (setting != null)
                            setting.AutoHue = reader.ReadElementContentAsBoolean();
                    }
                    else if (reader.Name == nameof(setting.Sharpness))
                    {
                        if (setting != null)
                            setting.Sharpness = reader.ReadElementContentAsInt();
                    }
                    else if (reader.Name == nameof(setting.AutoSharpness))
                    {
                        if (setting != null)
                            setting.AutoSharpness = reader.ReadElementContentAsBoolean();
                    }
                    else if (reader.Name == nameof(setting.Gamma))
                    {
                        if (setting != null)
                            setting.Gamma = reader.ReadElementContentAsInt();
                    }
                    else if (reader.Name == nameof(setting.AutoGamma))
                    {
                        if (setting != null)
                            setting.AutoGamma = reader.ReadElementContentAsBoolean();
                    }
                    else if (reader.Name == nameof(setting.ColorEnable))
                    {
                        if (setting != null)
                            setting.ColorEnable = reader.ReadElementContentAsBoolean();
                    }
                    else if (reader.Name == nameof(setting.WhiteBalance))
                    {
                        if (setting != null)
                            setting.WhiteBalance = reader.ReadElementContentAsInt();
                    }
                    else if (reader.Name == nameof(setting.AutoWhiteBalance))
                    {
                        if (setting != null)
                            setting.AutoWhiteBalance = reader.ReadElementContentAsBoolean();
                    }
                    else if (reader.Name == nameof(setting.BacklightCompensation))
                    {
                        if (setting != null)
                            setting.BacklightCompensation = reader.ReadElementContentAsInt();
                    }
                    else if (reader.Name == nameof(setting.AutoBacklightCompensation))
                    {
                        if (setting != null)
                            setting.AutoBacklightCompensation = reader.ReadElementContentAsBoolean();
                    }
                    else if (reader.Name == nameof(setting.Gain))
                    {
                        if (setting != null)
                            setting.Gain = reader.ReadElementContentAsInt();
                    }
                    else if (reader.Name == nameof(setting.AutoGain))
                    {
                        if (setting != null)
                            setting.AutoGain = reader.ReadElementContentAsBoolean();
                    }
                }
            }
            //关闭流
            reader.Close();
            reader = null;
            GC.Collect();
            #endregion
            return result;
        }

        /// <summary>
        /// 保存视频参数配置方案
        /// </summary>
        /// <param name="setting"></param>
        public void SaveVideoSetting(VideoSetting setting, bool AsDefault = false)
        {
            List<VideoSetting> settings = GetAllVideoSettings();
            if (settings == null)
                settings = new List<VideoSetting>();

            settings.ForEach(m => m.DefaultSetting = false);
            setting.DefaultSetting = true;
            if (setting != null)
                settings.Add(setting);
            FlushDataToXmlFile(settings);
        }


        /// <summary>
        /// 将配置持久化到文件中
        /// </summary>
        /// <param name="settings"></param>
        private void FlushDataToXmlFile(List<VideoSetting> settings)
        {
            #region 将配置持久化到文件中
            XmlTextWriter myXmlTextWriter = new XmlTextWriter(_VideoSettingRealPath, null);

            //使用 Formatting 属性指定希望将 XML 设定为何种格式。 这样，子元素就可以通过使用 Indentation 和 IndentChar 属性来缩进。
            myXmlTextWriter.Formatting = Formatting.Indented;
            myXmlTextWriter.WriteStartDocument(true);

            myXmlTextWriter.WriteStartElement("LiveChartsMap");
            myXmlTextWriter.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            myXmlTextWriter.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");

            myXmlTextWriter.WriteStartElement("VideoSettings");

            settings.ForEach(m =>
            {
                myXmlTextWriter.WriteStartElement(nameof(VideoSetting));

                myXmlTextWriter.WriteElementString(nameof(m.VideoSettingName), m.VideoSettingName);
                myXmlTextWriter.WriteElementString(nameof(m.Brightness), m.Brightness.ToString());
                myXmlTextWriter.WriteElementString(nameof(m.AutoBrightness), Convert.ToInt32(m.AutoBrightness).ToString());
                myXmlTextWriter.WriteElementString(nameof(m.ContrastRatio), m.ContrastRatio.ToString());
                myXmlTextWriter.WriteElementString(nameof(m.AutoContrastRatio), Convert.ToInt32(m.AutoContrastRatio).ToString());
                myXmlTextWriter.WriteElementString(nameof(m.Saturation), m.Saturation.ToString());
                myXmlTextWriter.WriteElementString(nameof(m.AutoSaturation), Convert.ToInt32(m.AutoSaturation).ToString());

                myXmlTextWriter.WriteElementString(nameof(m.Hue), m.Hue.ToString());
                myXmlTextWriter.WriteElementString(nameof(m.AutoHue), Convert.ToInt32(m.AutoHue).ToString());
                myXmlTextWriter.WriteElementString(nameof(m.Sharpness), m.Sharpness.ToString());
                myXmlTextWriter.WriteElementString(nameof(m.AutoSharpness), Convert.ToInt32(m.AutoSharpness).ToString());
                myXmlTextWriter.WriteElementString(nameof(m.Gamma), m.Gamma.ToString());
                myXmlTextWriter.WriteElementString(nameof(m.AutoGamma), Convert.ToInt32(m.AutoGamma).ToString());
                myXmlTextWriter.WriteElementString(nameof(m.ColorEnable), Convert.ToInt32(m.ColorEnable).ToString());
                myXmlTextWriter.WriteElementString(nameof(m.WhiteBalance), m.WhiteBalance.ToString());
                myXmlTextWriter.WriteElementString(nameof(m.AutoWhiteBalance), Convert.ToInt32(m.AutoWhiteBalance).ToString());
                myXmlTextWriter.WriteElementString(nameof(m.BacklightCompensation), m.BacklightCompensation.ToString());
                myXmlTextWriter.WriteElementString(nameof(m.AutoBacklightCompensation), Convert.ToInt32(m.AutoBacklightCompensation).ToString());
                myXmlTextWriter.WriteElementString(nameof(m.Gain), m.Gain.ToString());
                myXmlTextWriter.WriteElementString(nameof(m.AutoGain), Convert.ToInt32(m.AutoGain).ToString());

                myXmlTextWriter.WriteElementString(nameof(m.DefaultSetting), Convert.ToInt32(m.DefaultSetting).ToString());
                myXmlTextWriter.WriteEndElement();
            });
            myXmlTextWriter.WriteEndElement();
            myXmlTextWriter.WriteEndElement();
            myXmlTextWriter.Flush();
            myXmlTextWriter.Close();
            myXmlTextWriter = null;
            GC.Collect();
            #endregion
        }

        public bool SetDefaultSettings(VideoSetting setting)
        {
            if (setting == null)
                return false;
            List<VideoSetting> settings = GetAllVideoSettings();
            settings.ForEach(m => m.DefaultSetting = false);
            var defaultSetting = settings.Find(m => m.VideoSettingName == setting.VideoSettingName);
            defaultSetting.DefaultSetting = true;
            FlushDataToXmlFile(settings);
            return true;
        }
    }
}
