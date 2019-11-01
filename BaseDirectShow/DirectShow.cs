using System;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using BaseDirectShow.Entity;
using BaseDirectShow.SharePreferences;
using DirectShowLib;

namespace BaseDirectShow
{
    public class DirectShow : ISampleGrabberCB
    {
        private static readonly object locker = new object();// 定义一个标识确保线程同步
        private static DirectShow _instance;
        public static DirectShow Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new DirectShow();
                        }
                    }
                }
                return _instance;
            }
        }

        #region 属性
        private string _VideoDeviceName;
        /// <summary>
        /// 视频设备名称（为空系统自动加载）
        /// </summary>
        public string VideoDeviceName
        {
            get { return _VideoDeviceName; }
            set { _VideoDeviceName = value; }
        }

        private string _VideoCompressorCategory;
        /// <summary>
        /// 视频压缩器（为空系统自动加载）
        /// </summary>
        public string VideoCompressorCategory
        {
            get { return _VideoCompressorCategory; }
            set { _VideoCompressorCategory = value; }
        }

        private string _AudioInputDeviceName;
        /// <summary>
        /// 语音设备名称（为空系统自动加载）
        /// </summary>
        public string AudioInputDeviceName
        {
            get { return _AudioInputDeviceName; }
            set { _AudioInputDeviceName = value; }
        }

        private string _AudioCompressor;
        /// <summary>
        /// 语音压缩器（为空系统自动加载）
        /// </summary>
        public string AudioCompressor
        {
            get { return _AudioCompressor; }
            set { _AudioCompressor = value; }
        }
        private int _Frames;
        /// <summary>
        /// 帧数
        /// </summary>
        public int Frames
        {
            get { return _Frames; }
            set { _Frames = value; }
        }

        private string _Resolution;
        /// <summary>
        /// 分辨率
        /// </summary>
        public string Resolution
        {
            get { return _Resolution; }
            set { _Resolution = value; }
        }
        #endregion

        private Control ctVideo;//显示视频的控件对象
        IFilterGraph2 graphBuilder = null;
        /// <summary> capture graph builder interface.捕捉图表构建器接口 </summary>
        ICaptureGraphBuilder2 captureGraphBuilder = null;
        /// <summary>samp Grabber interface 采样器接口</summary>
        ISampleGrabber sampleGrabber = null;
        /// <summary> control interface.媒体控制器接口 </summary>
        IMediaControl mediaControl = null;

        #region 录像变量
        DsDevice[] dsVideoDevice;//所有的视频设备
        DsDevice[] dsVideoCompressorCategory;//所有视频解码器
        DsDevice[] dsAudioInputDevice;//所有的音频设备
        DsDevice[] dsAudioCompressorCategory;//所有的音频解码器
        /// <summary> 选择摄像头 </summary>
        IBaseFilter theDevice = null;
        /// <summary> 选择视频压缩器 </summary>
        IBaseFilter theDeviceCompressor = null;
        /// <summary> 选择声音设备 </summary>
        IBaseFilter theAudio = null;
        /// <summary> 选择声音压缩器 </summary>
        IBaseFilter theAudioCompressor = null;

        //SetOutputFileName为我们创建图形的文件编写器部分，并返回mux和sink
        IBaseFilter mux;
        IFileSinkFilter sink;
        #endregion

        private int _VideoWidth;//视频宽度
        private int _VideoHeight;//视频高度
        private int _VideoBitCount;//视频比特
        private int _ImageSize;//图片大小
        private string sVideoType = "wmv";//录制视频格式

        private bool BSamll = false;//是否拍摄小图
        private IntPtr m_ipBuffer = IntPtr.Zero;
        private volatile ManualResetEvent m_PictureReady = null;
        private volatile bool m_bWantOneFrame = false;

        #region APIs
        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory")]
        private static extern void CopyMemory(IntPtr Destination, IntPtr Source, [MarshalAs(UnmanagedType.U4)] int Length);
        #endregion

        /// <summary> sample callback, NOT USED. </summary>
        public int SampleCB(double SampleTime, IMediaSample pSample)
        {
            Marshal.ReleaseComObject(pSample);
            return 0;
        }

        /// <summary> 采集回调：buffer callback, COULD BE FROM FOREIGN THREAD. 缓冲区回调，可以来自外线程</summary>
        public int BufferCB(double SampleTime, IntPtr pBuffer, int BufferLen)
        {
            Debug.Assert(BufferLen == Math.Abs(_VideoBitCount / 8 * _VideoWidth) * _VideoHeight, "错误的 buffer 长度");
            if (m_bWantOneFrame)
            {
                m_bWantOneFrame = false;
                Debug.Assert(m_ipBuffer != IntPtr.Zero, "空的 buffer");
                // Save the buffer
                CopyMemory(m_ipBuffer, pBuffer, BufferLen);
                // Picture is ready.
                m_PictureReady.Set();
            }
            return 0;
        }


        #region 预览视频

        /// <summary>
        /// 预览视频
        /// </summary>
        /// <param name="clVideo">视频显示的控件对象</param>
        public bool Preview(Control clVideo)
        {
            if (!InitDevice())//初始化设备
            {
                return false;
            }
            StopRun();//先停止播放
            ctVideo = clVideo;
            SetVideoShow(string.Empty, 0);
            return StartRun();
        }

        /// <summary>
        /// 初始化设备
        /// </summary>
        public bool InitDevice()
        {
            try
            {
                if (!IsCorrectDirectXVersion())//检查DirectX版本
                {
                    throw new Exception("未安装DirectX 8.1！");
                }

                dsVideoDevice = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);//读摄像头信息
                if (dsVideoDevice == null || dsVideoDevice.Length == 0)//如果摄像头不存在则抛出异常
                {
                    throw new Exception("未找到摄像头设备！");
                }

                if (!string.IsNullOrEmpty(_VideoDeviceName))
                {
                    theDevice = CreateFilter(dsVideoDevice, _VideoDeviceName);
                    if (theDevice == null)
                    {
                        MessageBox.Show("输入摄像机名称不存在！");
                    }
                }
                bool blManualSettings = false;//是否手动设置
                if (theDevice == null)
                {
                    if (dsVideoDevice.Length == 1)
                    {
                        theDevice = CreateFilter(dsVideoDevice, dsVideoDevice[0].Name);
                    }
                    else
                    {
                        //这是选择摄像头
                        FrmDeviceConfig DeviceConfig = new FrmDeviceConfig();
                        if (DeviceConfig.ShowDialog() == DialogResult.OK)
                        {
                            theDevice = CreateFilter(dsVideoDevice, DeviceConfig.VideoDeviceName);
                            DsDevice[] dsArray = DsDevice.GetDevicesOfCat(FilterCategory.AudioInputDevice);
                            theAudio = CreateFilter(dsArray, DeviceConfig.AudioInputDeviceName);
                            _Frames = DeviceConfig.Frames;
                            _Resolution = DeviceConfig.Resolution;
                        }
                        blManualSettings = true;//添加摄像机设置
                    }
                }
                if (!blManualSettings)
                {
                    //dsVideoCompressorCategory = DsDevice.GetDevicesOfCat(FilterCategory.VideoCompressorCategory);//读视频压缩器信息
                    //theDeviceCompressor = CreateFilter(dsVideoCompressorCategory, _VideoCompressorCategory);//获取视频压缩器IBaseFilter
                    dsAudioInputDevice= DsDevice.GetDevicesOfCat(FilterCategory.AudioInputDevice);//读取音频设备信息
                    theAudio = CreateFilter(dsAudioInputDevice, _AudioInputDeviceName);//获取音频设备IBaseFilter

                    //dsAudioCompressorCategory = DsDevice.GetDevicesOfCat(FilterCategory.AudioCompressorCategory);//读取音频压缩器信息
                    //theDeviceCompressor = CreateFilter(dsAudioCompressorCategory, _AudioCompressor);//获取音频压缩器IBaseFilter
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("初始化设备失败：" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 验证是否安装DirectX
        /// </summary>
        /// <returns></returns>
        public bool IsCorrectDirectXVersion()
        {
            return File.Exists(Path.Combine(Environment.SystemDirectory, "dpnhpast.dll"));
        }

        /// <summary>
        /// 根据设备集合、设备名称获取指定设备的BaseFilter
        /// </summary>
        /// <param name="dsDevice">设备集合</param>
        /// <param name="friendlyname">设备名</param>
        /// <returns></returns>
        private IBaseFilter CreateFilter(DsDevice[] dsDevice, string friendlyname)
        {
            if (dsDevice == null || dsDevice.Length == 0)
            {
                return null;
            }
            object source = null;
            Guid gbf = typeof(IBaseFilter).GUID;
            if (string.IsNullOrEmpty(friendlyname))//如果名字为空则默认第一个
            {
                dsDevice[0].Mon.BindToObject(null, null, ref gbf, out source);
                return (IBaseFilter)source;
            }
            bool blExis = false;//是否存在
            foreach (DsDevice device in dsDevice)
            {
                if (device.Name.Equals(friendlyname))
                {
                    blExis = true;
                    device.Mon.BindToObject(null, null, ref gbf, out source);
                    break;
                }
            }
            if (blExis)
            {
                return (IBaseFilter)source;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///设置视频显示
        /// </summary>
        public void SetVideoShow(string Resolution, int Frames)
        {
            if (theDevice == null)
            {
                return;
            }
            if (graphBuilder != null)
            {
                mediaControl.Stop();
            }
            CreateGraph(Resolution, Frames);
            //渲染设备的任何预览引脚 并且把sampleGrabber添加到预览
            int hr = captureGraphBuilder.RenderStream(PinCategory.Preview, MediaType.Video, theDevice, (sampleGrabber as IBaseFilter), null);
            //DsError.ThrowExceptionForHR(hr);
            hr = captureGraphBuilder.RenderStream(PinCategory.Preview, MediaType.Audio, theAudio, null, null);
            //DsError.ThrowExceptionForHR(hr);
            GetVideoHeaderInfo(sampleGrabber);//获取视频头文件信息

            //从图中获取视频窗口
            IVideoWindow videoWindow = null;
            videoWindow = (IVideoWindow)graphBuilder;

            //将视频窗口的所有者设置为某种IntPtr（任何控件的句柄 - 可以是窗体/按钮等）
            hr = videoWindow.put_Owner(ctVideo.Handle);
            //DsError.ThrowExceptionForHR(hr);

            //设置视频窗口的样式
            hr = videoWindow.put_WindowStyle(WindowStyle.Child |WindowStyle.ClipChildren);
            //DsError.ThrowExceptionForHR(hr);

            //在主应用程序窗口的客户端中定位视频窗口
            hr = videoWindow.SetWindowPosition(0, 0, ctVideo.Width, ctVideo.Height);
            //DsError.ThrowExceptionForHR(hr);

            //使视频窗口可见
            hr = videoWindow.put_Visible(OABool.True);
            //DsError.ThrowExceptionForHR(hr);
        }

        /// <summary>
        /// 构建捕获图
        /// </summary>
        public void CreateGraph(string Resolution, int Frames)
        {
            if (graphBuilder != null)
            {
                return;
            }
            graphBuilder = (IFilterGraph2)new FilterGraph();// 获取IFilterGraph2接口对象
            captureGraphBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();//获取ICaptureGraphBuilder2接口对象


            int hr = captureGraphBuilder.SetFiltergraph(this.graphBuilder);//将过滤器图形附加到捕获图
            DsError.ThrowExceptionForHR(hr);

            //将视频输入设备添加到图形
            hr = graphBuilder.AddFilter(theDevice, "source filter");
            DsError.ThrowExceptionForHR(hr);

            //将视频压缩器过滤器添加到图形
            if (theDeviceCompressor != null)
            {
                hr = graphBuilder.AddFilter(theDeviceCompressor, "devicecompressor filter");
                DsError.ThrowExceptionForHR(hr);
            }
            //将音频输入设备添加到图形
            if (theAudio != null)
            {
                hr = graphBuilder.AddFilter(theAudio, "audio filter");
                DsError.ThrowExceptionForHR(hr);
            }
            //将音频压缩器过滤器添加到图形
            if (theAudioCompressor != null)
            {
                hr = graphBuilder.AddFilter(theAudioCompressor, "audiocompressor filter");
                DsError.ThrowExceptionForHR(hr);
            }
            mediaControl = (IMediaControl)this.graphBuilder;//获取IMediaControl接口对象

            m_PictureReady = new ManualResetEvent(false);

            sampleGrabber = new SampleGrabber() as ISampleGrabber;//添加采样器接口.
            ConfigureSampleGrabber(sampleGrabber);// 配置SampleGrabber。添加预览回调
            hr = this.graphBuilder.AddFilter(sampleGrabber as IBaseFilter, "Frame Callback");// 将SampleGrabber添加到图形.
            DsError.ThrowExceptionForHR(hr);


            //读取摄像头配置信息
            AMMediaType mediaType = new AMMediaType();
            object oVideoStreamConfig;//视频流配置信息
            hr = captureGraphBuilder.FindInterface(PinCategory.Capture, MediaType.Video, theDevice, typeof(IAMStreamConfig).GUID, out oVideoStreamConfig);
            if (!(oVideoStreamConfig is IAMStreamConfig videoStreamConfig))
            {
                throw new Exception("Failed to get IAMStreamConfig");
            }

            //test Failed
            //读取摄像头中的配置
            //int iCount;
            //int iSize;
            //hr = videoStreamConfig.GetNumberOfCapabilities(out iCount, out iSize);
            //if (hr != 0)
            //    Marshal.ThrowExceptionForHR(hr);
            //if (iSize == Marshal.SizeOf(typeof(VideoStreamConfigCaps)))//?? sizeof
            //{
            //    IntPtr sccPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(VideoStreamConfigCaps)));
            //    for (int iFormat = 0; iFormat < iCount; iFormat++)
            //    {
            //        VideoStreamConfigCaps scc=new VideoStreamConfigCaps(); 
            //        IntPtr pmtConfigIntPtr;
            //        AMMediaType pmtConfig = new AMMediaType() ; 
            //        hr = videoStreamConfig.GetStreamCaps(iFormat, out pmtConfigIntPtr, sccPtr);
            //        if (hr != 0)
            //            Marshal.ThrowExceptionForHR(hr);
            //        Marshal.PtrToStructure(pmtConfigIntPtr, pmtConfig);
            //        //读取配置值
            //        if (pmtConfig.majorType == MediaType.Video && pmtConfig.subType== MediaSubType.RGB24 && pmtConfig.formatType == FormatType.VideoInfo)
            //        {


            //        }
            //    }
            //}
            //test end


            hr = videoStreamConfig.GetFormat(out mediaType);
            if (hr != 0)
                Marshal.ThrowExceptionForHR(hr);
          



            DsError.ThrowExceptionForHR(hr);


            // The formatPtr member points to different structures
            // dependingon the formatType
            object formatStruct;
            if (mediaType.formatType == FormatType.WaveEx)
                formatStruct = new WaveFormatEx();
            else if (mediaType.formatType == FormatType.VideoInfo)
                formatStruct = new VideoInfoHeader();
            else if (mediaType.formatType == FormatType.VideoInfo2)
                formatStruct = new VideoInfoHeader2();
            else
                throw new NotSupportedException("This device does not support a recognized format block.");

            // Retrieve the nested structure
            Marshal.PtrToStructure(mediaType.formatPtr, formatStruct);


            if (formatStruct is VideoInfoHeader)
            {
                VideoInfoHeader videoInfoHeader = formatStruct as VideoInfoHeader;
                // 设置帧率
                if (Frames > 0)
                {
                    videoInfoHeader.AvgTimePerFrame = 10000000 / Frames;
                }
                // 设置宽度 设置高度
                if (!string.IsNullOrEmpty(Resolution) && Resolution.Split('*').Length > 1)
                {
                    videoInfoHeader.BmiHeader.Width = Convert.ToInt32(Resolution.Split('*')[0]);
                    videoInfoHeader.BmiHeader.Height = Convert.ToInt32(Resolution.Split('*')[1]);
                }
                // 复制媒体结构
                Marshal.StructureToPtr(videoInfoHeader, mediaType.formatPtr, false);
            }
            else if (formatStruct is VideoInfoHeader2)
            {
                VideoInfoHeader2 videoInfoHeader = formatStruct as VideoInfoHeader2;
                // 设置帧率
                if (Frames > 0)
                {
                    videoInfoHeader.AvgTimePerFrame = 10000000 / Frames;
                }
                // 设置宽度 设置高度
                if (!string.IsNullOrEmpty(Resolution) && Resolution.Split('*').Length > 1)
                {
                    videoInfoHeader.BmiHeader.Width = Convert.ToInt32(Resolution.Split('*')[0]);
                    videoInfoHeader.BmiHeader.Height = Convert.ToInt32(Resolution.Split('*')[1]);
                }
                // 复制媒体结构
                Marshal.StructureToPtr(videoInfoHeader, mediaType.formatPtr, false);
            }


            //VideoInfoHeader videoInfoHeader = new VideoInfoHeader();
            //Marshal.PtrToStructure(mediaType.formatPtr, videoInfoHeader);


            //if (Frames > 0)
            //{
            //    videoInfoHeader.AvgTimePerFrame = 10000000 / Frames;
            //}
            //// 设置宽度 设置高度
            //if (!string.IsNullOrEmpty(Resolution) && Resolution.Split('*').Length > 1)
            //{
            //    videoInfoHeader.BmiHeader.Width = Convert.ToInt32(Resolution.Split('*')[0]);
            //    videoInfoHeader.BmiHeader.Height = Convert.ToInt32(Resolution.Split('*')[1]);
            //}
            //// 复制媒体结构
            //Marshal.StructureToPtr(videoInfoHeader, mediaType.formatPtr, false);
            // 设置新的视频格式
            hr = videoStreamConfig.SetFormat(mediaType);
            DsError.ThrowExceptionForHR(hr);
            DsUtils.FreeAMMediaType(mediaType);
            mediaType = null;
        }

        /// <summary>
        /// 配置SampleGrabber
        /// </summary>
        /// <param name="sampGrabber"></param>
        private void ConfigureSampleGrabber(ISampleGrabber sampGrabber)
        {
            AMMediaType media = new AMMediaType();
            media.majorType = MediaType.Video;
            media.subType = MediaSubType.RGB24;
            media.formatType = FormatType.VideoInfo;
            int hr = sampGrabber.SetMediaType(media);
            DsError.ThrowExceptionForHR(hr);
            DsUtils.FreeAMMediaType(media);
            media = null;
            hr = sampGrabber.SetCallback(this, 1);
            DsError.ThrowExceptionForHR(hr);
        }

        /// <summary>
        /// 读取视频头文件信息
        /// </summary>
        /// <param name="sampleGrabber"></param>
        private void GetVideoHeaderInfo(ISampleGrabber sampleGrabber)
        {
            // Get the media type from the SampleGrabber
            AMMediaType media = new AMMediaType();
            int hr = sampleGrabber.GetConnectedMediaType(media);//读取视频文件信息
            DsError.ThrowExceptionForHR(hr);
            if ((media.formatType != FormatType.VideoInfo) || (media.formatPtr == IntPtr.Zero))
            {
                throw new NotSupportedException("Unknown Grabber Media Format");
            }
            // 获取视频文件信息
            VideoInfoHeader videoInfoHeader = (VideoInfoHeader)Marshal.PtrToStructure(media.formatPtr, typeof(VideoInfoHeader));
            _VideoWidth = videoInfoHeader.BmiHeader.Width;
            _VideoHeight = videoInfoHeader.BmiHeader.Height;
            _VideoBitCount = videoInfoHeader.BmiHeader.BitCount;
            _ImageSize = videoInfoHeader.BmiHeader.ImageSize;
            DsUtils.FreeAMMediaType(media);
            media = null;
        }

        /// <summary>
        /// 开始运行
        /// </summary>
        public bool StartRun()
        {
            if (mediaControl == null)
            {
                MessageBox.Show("播放对象为空！");
                return false;
            }
            int iRun = mediaControl.Run();//运行图表
            if (iRun != 1)
            {
                MessageBox.Show("播放失败！");
                return false;
            }
            return true;
        }
        #endregion

        #region 抓拍图片


        /// <summary>
        /// 抓拍照片
        /// </summary>
        /// <param name="path">抓拍照片完整路径，包含文件名</param>
        /// <param name="iImageWidth">抓怕图片宽度，默认获取视频信息宽度</param>
        /// <param name="iImageHeight">抓拍图片高度，默认获取视频信息高度</param>
        /// <param name="bSamll">是否抓小图，默认不抓拍</param>
        /// <param name="samllPath">抓拍小图路径，抓拍小图时则必须填写</param>
        public bool Cap(string path, int iImageWidth = 0, int iImageHeight = 0, bool bSamll = false, string samllPath = "")
        {
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("请输入抓拍照片完整路径！");
                return false;
            }
            if (bSamll && string.IsNullOrEmpty(samllPath))
            {
                MessageBox.Show("请输入抓拍小照片完整路径！");
                return false;
            }
            if (!VerStarPreview())
            {
                MessageBox.Show("请先开始预览！");
                return false;
            }
            IntPtr ip = GetNextFrame();
            int iCapWidth = 0; int iCapHeight = 0;
            if (iImageWidth == 0 && iImageHeight == 0)
            {
                iCapWidth = _VideoWidth;
                iCapHeight = _VideoHeight;
            }
            else
            {
                iCapWidth = iImageWidth;
                iCapHeight = iImageHeight;
            }
            if (((iCapWidth & 0x03) != 0) || (iCapWidth < 32) || (iCapWidth > 4096) || (iCapHeight < 32) || (iCapHeight > 4096))
            {
                MessageBox.Show("输入的宽度或高度必须在32到4096之间");
                return false;
            }
            BSamll = bSamll;
            Bitmap bitmap = new Bitmap(iCapWidth, iCapHeight, (_VideoBitCount / 8) * iCapWidth, PixelFormat.Format24bppRgb, ip);

            Bitmap bitmap_clone = bitmap.Clone(new Rectangle(0, 0, _VideoWidth, _VideoHeight), PixelFormat.Format24bppRgb);
            bitmap_clone.RotateFlip(RotateFlipType.RotateNoneFlipY);

            // Release any previous buffer
            if (ip != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(ip);
                ip = IntPtr.Zero;
            }
            bitmap.Dispose();
            bitmap = null;
            bitmap_clone.Save(path, ImageFormat.Jpeg);
            //保存小图片
            if (BSamll)
            {
                Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                Image myThumbnail = bitmap_clone.GetThumbnailImage(120, 120, myCallback, IntPtr.Zero);
                myThumbnail.Save(samllPath);
            }
            return true;
        }

        /// <summary>
        /// 获取视频下一帧
        /// </summary>
        /// <returns></returns>
        private IntPtr GetNextFrame()
        {
            // get ready to wait for new image
            m_PictureReady.Reset();
            m_ipBuffer = Marshal.AllocCoTaskMem(Math.Abs(_VideoBitCount / 8 * _VideoWidth) * _VideoHeight);
            try
            {
                m_bWantOneFrame = true;
                if (!m_PictureReady.WaitOne(5000, false))// 开始等待
                {
                    throw new Exception("获取图片超时");
                }
            }
            catch
            {
                Marshal.FreeCoTaskMem(m_ipBuffer);
                m_ipBuffer = IntPtr.Zero;
                throw;
            }
            // 返回图片
            return m_ipBuffer;
        }
        public bool ThumbnailCallback()
        {
            return false;
        }

        /// <summary>
        /// 验证是否开始预览
        /// </summary>
        /// <returns></returns>
        private bool VerStarPreview()
        {
            if (theDevice == null || graphBuilder == null)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 开始录制
        /// <summary>
        /// 开始录制
        /// </summary>
        public bool StartMonitorRecord(string sViewPath)
        {
            if (!InitGraph(sViewPath))
            {
                return false;
            }
            return StartRun();
        }

        /// <summary>
        /// 初始化图表
        /// </summary>
        public bool InitGraph(string sViewPath)
        {
            if (!VerStarPreview())
            {
                MessageBox.Show("请先开始预览！");
                return false;
            }
            mediaControl.Stop();
            CreateGraph(_Resolution, _Frames);
            int hr;
            if (sVideoType == "wmv")
            {
                hr = captureGraphBuilder.SetOutputFileName(MediaSubType.Asf, sViewPath, out mux, out sink);
                Marshal.ThrowExceptionForHR(hr);
                try
                {
                    IConfigAsfWriter lConfig = mux as IConfigAsfWriter;

                    // Windows Media Video (audio, 700 Kbps)
                    // READ THE README for info about using guids
                    Guid cat = new Guid("ec298949-639b-45e2-96fd-4ab32d5919c2");
                    hr = lConfig.ConfigureFilterUsingProfileGuid(cat);
                    Marshal.ThrowExceptionForHR(hr);
                }
                finally
                {
                    Marshal.ReleaseComObject(sink);
                }
            }
            else if (sVideoType == "avi")
            {
                hr = captureGraphBuilder.SetOutputFileName(MediaSubType.Avi, sViewPath, out mux, out sink);
                DsError.ThrowExceptionForHR(hr);
            }
            //将设备和压缩器连接到mux，以呈现图形的捕获部分
            hr = captureGraphBuilder.RenderStream(PinCategory.Capture, MediaType.Interleaved, theDevice, theDeviceCompressor, mux);
            //DsError.ThrowExceptionForHR(hr);
            if (hr < 0)
            {
                hr = captureGraphBuilder.RenderStream(PinCategory.Capture, MediaType.Video, theDevice, theDeviceCompressor, mux);
                if (hr < 0) { DsError.ThrowExceptionForHR(hr); }
            }

            //将音频添加进去
            hr = captureGraphBuilder.RenderStream(PinCategory.Capture, MediaType.Audio, theAudio, theAudioCompressor, mux);
            DsError.ThrowExceptionForHR(hr);

            Marshal.ReleaseComObject(mux);
            Marshal.ReleaseComObject(sink);
            return true;
        }
        #endregion

        #region 停止录制
        /// <summary>
        /// 停止录制
        /// </summary>
        public void StopMonitorRecord()
        {
            if (!VerStarPreview())
            {
                MessageBox.Show("请先开始预览！");
                return;
            }
            StopRun();
            SetVideoShow(_Resolution, _Frames);
            StartRun();
        }

        /// <summary>
        /// 停止播放
        /// </summary>
        public void StopRun()
        {
            if (mediaControl != null)
            {
                //Stop the Graph
                mediaControl.Stop();
            }
            if (graphBuilder != null)
            {
                //Release COM objects
                Marshal.ReleaseComObject(graphBuilder);
                graphBuilder = null;
            }
            if (captureGraphBuilder != null)
            {
                Marshal.ReleaseComObject(captureGraphBuilder);
                captureGraphBuilder = null;
            }
        }
        #endregion

        #region 关闭视频设备
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (mediaControl != null)
            {
                mediaControl.Stop();
                mediaControl = null;
            }
            if (graphBuilder != null)
            {
                Marshal.ReleaseComObject(graphBuilder);
                graphBuilder = null;
            }
            if (captureGraphBuilder != null)
            {
                Marshal.ReleaseComObject(captureGraphBuilder);
                captureGraphBuilder = null;
            }
            if (sampleGrabber != null)
            {
                Marshal.ReleaseComObject(sampleGrabber);
                sampleGrabber = null;
            }
            if (theDevice != null)
            {
                Marshal.ReleaseComObject(theDevice);
                theDevice = null;
            }
            if (theDeviceCompressor != null)
            {
                Marshal.ReleaseComObject(theDeviceCompressor);
                theDeviceCompressor = null;
            }
            if (theAudio != null)
            {
                Marshal.ReleaseComObject(theAudio);
                theAudio = null;
            }
            if (theAudioCompressor != null)
            {
                Marshal.ReleaseComObject(theAudioCompressor);
                theAudioCompressor = null;
            }
            if (mux != null)
            {
                Marshal.ReleaseComObject(mux);
                mux = null;
            }
            if (sink != null)
            {
                Marshal.ReleaseComObject(sink);
                sink = null;
            }
            DisposeDevice(dsVideoDevice);//释放视频设备
            DisposeDevice(dsVideoCompressorCategory);//释放视频解码器设备
            DisposeDevice(dsAudioInputDevice);//释放语音设备
            DisposeDevice(dsAudioCompressorCategory);//释放语音解码器设备
        }

        /// <summary>
        ///释放设备资源
        /// </summary>
        /// <param name="dsDevice"></param>
        private void DisposeDevice(DsDevice[] dsDevice)
        {
            if (dsDevice != null)
            {
                foreach (DsDevice ds in dsDevice)
                {
                    ds.Dispose();
                }
                dsDevice = null;
            }
        }
        #endregion

        #region 视频属性配置页面  调系统dll方式

        // ---------------- DLL Imports --------------------

        [DllImport("olepro32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern int OleCreatePropertyFrame(
            IntPtr hwndOwner, int x, int y,
            string lpszCaption, int cObjects,
            [In, MarshalAs(UnmanagedType.Interface)] ref object ppUnk,
            int cPages, IntPtr pPageClsID, int lcid, int dwReserved, IntPtr pvReserved);

        /// <summary> COM ISpecifyPropertyPages interface </summary>
        protected ISpecifyPropertyPages specifyPropertyPages;

        /// <summary>
        /// 视频属性页设置
        /// 原生方式
        /// </summary>
        public void changeCameraSetting(IntPtr Handle)
        {
            DsCAUUID cauuid = new DsCAUUID();
            try
            {
                specifyPropertyPages = theDevice as ISpecifyPropertyPages;
                if (specifyPropertyPages == null)
                {
                    MessageBox.Show("请先打开视频设备！");
                    return;
                }
                //返回filter所支持的属性页的CLSID
                int hr = specifyPropertyPages.GetPages(out cauuid);
                if (hr != 0) Marshal.ThrowExceptionForHR(hr);

                object o = specifyPropertyPages;
                //获取属性页
                hr = OleCreatePropertyFrame(Handle, 30, 30, null, 1,
                    ref o, cauuid.cElems, cauuid.pElems, 0, 0, IntPtr.Zero);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable display property page. Please submit a bug report.\n\n" + ex.Message + "\n\n" + ex.ToString());
            }
        }



        /// <summary>
        /// 保存当前的相机设置
        /// </summary>
        public void SaveCurrentOriginSetting(string VideoSettingName, bool AsDefault)
        {
            IAMVideoProcAmp videoProcAmp = theDevice as IAMVideoProcAmp;
            VideoProcAmpFlags flags = VideoProcAmpFlags.Manual;
            //亮度值 0到255
            int LightValue = 0;
            videoProcAmp.Get(VideoProcAmpProperty.Brightness, out LightValue, out flags);
            //对比度 0到255
            int ContrastValue = 0;
            videoProcAmp.Get(VideoProcAmpProperty.Contrast, out ContrastValue, out flags);
            //饱和度 0到255 
            int SaturationValue = 0;
            videoProcAmp.Get(VideoProcAmpProperty.Saturation, out SaturationValue, out flags);
            //色调 -127 到127
            int HueValue = 0;
            videoProcAmp.Get(VideoProcAmpProperty.Hue, out HueValue, out flags);
            //清晰度 0到15
            int SharpnessValue = 0;
            videoProcAmp.Get(VideoProcAmpProperty.Sharpness, out SharpnessValue, out flags);
            //伽玛 1到8
            int GammaValue = 0;
            videoProcAmp.Get(VideoProcAmpProperty.Gamma, out GammaValue, out flags);
            //启用颜色 不支持
            int ColorEnable = 0;
            videoProcAmp.Get(VideoProcAmpProperty.ColorEnable, out ColorEnable, out flags);
            //白平衡 不支持
            int WhiteBalanceValue = 0;
            videoProcAmp.Get(VideoProcAmpProperty.WhiteBalance, out WhiteBalanceValue, out flags);
            //背光补偿 1 到 5
            int BacklightCompensation = 0;
            videoProcAmp.Get(VideoProcAmpProperty.BacklightCompensation, out BacklightCompensation, out flags);
            //增益 不支持
            int Gain = 0;
            videoProcAmp.Get(VideoProcAmpProperty.Gain, out Gain, out flags);

            VideoSetting setting = new VideoSetting();
            setting.Brightness = LightValue;
            setting.VideoSettingName = VideoSettingName;
            setting.ContrastRatio = ContrastValue;
            setting.Saturation = SaturationValue;
            setting.Hue = HueValue;
            setting.Sharpness = SharpnessValue;
            setting.Gamma = GammaValue;
            setting.ColorEnable = Convert.ToBoolean(ColorEnable);
            setting.WhiteBalance = WhiteBalanceValue;
            setting.BacklightCompensation = BacklightCompensation;
            setting.Gain = Gain;
            setting.DefaultSetting = AsDefault;
            VideoSettingUtils.Instance.SaveVideoSetting(setting, AsDefault);
        }

        #endregion

        #region 视频属性配置页面 自定义
        /// <summary>
        /// 设置亮度
        /// </summary>
        /// <param name="lightValue">亮度值0 到 100</param>
        /// <returns></returns>
        public int SetLightValue(int lightValue)
        {
            int iResult = 0;
            IAMVideoProcAmp videoProcAmp = theDevice as IAMVideoProcAmp;
            if (videoProcAmp == null)
            {
                iResult = -1;
                return iResult;
            }
            int val;
            int min;
            int max;
            int step;
            int defaultValue;
            VideoProcAmpFlags flags = VideoProcAmpFlags.Manual;
            // 设置亮度
            if (lightValue != -1)
            {
                int hr = videoProcAmp.GetRange(VideoProcAmpProperty.Brightness, out min, out max, out step, out defaultValue, out flags);
                if (0 == hr)
                {
                    //videoProcAmp.Get(VideoProcAmpProperty.Brightness, out val, out flags);
                    //val = min + (max - min) * lightValue / 255;
                    iResult = videoProcAmp.Set(VideoProcAmpProperty.Brightness, lightValue, flags);
                }
            }


            return iResult;
        }


        /// <summary>
        /// 获取当前亮度值
        /// </summary>
        /// <returns></returns>
        public int GetLightValue()
        {
            int LightValue = 0;
            IAMVideoProcAmp videoProcAmp = theDevice as IAMVideoProcAmp;
            VideoProcAmpFlags flags = VideoProcAmpFlags.Manual;
            videoProcAmp.Get(VideoProcAmpProperty.Brightness, out LightValue, out flags);
            return LightValue;
        }

        /// <summary>
        /// 设置对比度
        /// </summary>
        /// <param name="ContrastValue">对比度值，0到100之间</param>
        /// <returns></returns>
        public int SetContrastValue(int ContrastValue)
        {
            int iResult = 0;
            IAMVideoProcAmp videoProcAmp = theDevice as IAMVideoProcAmp;
            if (videoProcAmp == null)
            {
                iResult = -1;
                return iResult;
            }
            int val;
            int min;
            int max;
            int step;
            int defaultValue;
            //设置对比度
            VideoProcAmpFlags flags = VideoProcAmpFlags.Manual;
            if (ContrastValue != -1)
            {
                int hr = videoProcAmp.GetRange(VideoProcAmpProperty.Contrast, out min, out max, out step, out defaultValue, out flags);
                if (0 == hr)
                {
                    //videoProcAmp.Get(VideoProcAmpProperty.Contrast, out val, out flags);
                    //val = min + (max - min) * ContrastValue / 100;
                    iResult = videoProcAmp.Set(VideoProcAmpProperty.Contrast, ContrastValue, flags);
                }
            }
            return iResult;
        }

        /// <summary>
        /// 获取当前对比度值
        /// </summary>
        /// <returns></returns>
        public int GetContrastValue()
        {
            int ContrastValue = 0;
            IAMVideoProcAmp videoProcAmp = theDevice as IAMVideoProcAmp;
            VideoProcAmpFlags flags = VideoProcAmpFlags.Manual;
            videoProcAmp.Get(VideoProcAmpProperty.Contrast, out ContrastValue, out flags);
            return ContrastValue;
        }

        /// <summary>
        /// 设置饱和度
        /// </summary>
        /// <param name="SaturationValue">饱和度 0到 100</param>
        /// <returns></returns>
        public int SetSaturationValue(int SaturationValue)
        {
            int iResult = 0;
            IAMVideoProcAmp videoProcAmp = theDevice as IAMVideoProcAmp;
            if (videoProcAmp == null)
            {
                iResult = -1;
                return iResult;
            }
            int val;
            int min;
            int max;
            int step;
            int defaultValue;
            VideoProcAmpFlags flags = VideoProcAmpFlags.Manual;
            //设置饱和度
            if (SaturationValue != -1)
            {
                int hr = videoProcAmp.GetRange(VideoProcAmpProperty.Saturation, out min, out max, out step, out defaultValue, out flags);
                if (0 == hr)
                {
                    //videoProcAmp.Get(VideoProcAmpProperty.Saturation, out val, out flags);
                    //val = min + (max - min) * SaturationValue / 100;
                    iResult = videoProcAmp.Set(VideoProcAmpProperty.Saturation, SaturationValue, flags);
                }
            }

            return iResult;
        }


        /// <summary>
        /// 获取当前饱和度值
        /// </summary>
        /// <returns></returns>
        public int GetSaturationValue()
        {
            int SaturationValue = 0;
            IAMVideoProcAmp videoProcAmp = theDevice as IAMVideoProcAmp;
            VideoProcAmpFlags flags = VideoProcAmpFlags.Manual;
            videoProcAmp.Get(VideoProcAmpProperty.Saturation, out SaturationValue, out flags);
            return SaturationValue;
        }


        /// <summary>
        /// 设置摄像头为此配置
        /// </summary>
        /// <param name="setting">摄像头的配置</param>
        /// <param name="asDefault">是否并设为默认</param>
        /// <returns></returns>
        public int SetSettingValue(VideoSetting setting, bool asDefault = false)
        {
            if (asDefault)
                VideoSettingUtils.Instance.SetDefaultSettings(setting);

            int iResult = 0;
            IAMVideoProcAmp videoProcAmp = theDevice as IAMVideoProcAmp;
            if (videoProcAmp == null)
            {
                iResult = -1;
                return iResult;
            }
            int val;
            int min;
            int max;
            int step;
            int defaultValue;
            VideoProcAmpFlags flags = VideoProcAmpFlags.Manual;
            // 设置亮度
            if (setting.Brightness != -1)
            {
                int hr = videoProcAmp.GetRange(VideoProcAmpProperty.Brightness, out min, out max, out step, out defaultValue, out flags);
                if (0 == hr)
                {
                    //videoProcAmp.Get(VideoProcAmpProperty.Brightness, out val, out flags);
                    //val = min + (max - min) * setting.Brightness / 255;
                    iResult = videoProcAmp.Set(VideoProcAmpProperty.Brightness, setting.Brightness, flags);
                }
            }
            //设置对比度
            if (setting.ContrastRatio != -1)
            {
                int hr = videoProcAmp.GetRange(VideoProcAmpProperty.Contrast, out min, out max, out step, out defaultValue, out flags);
                if (0 == hr)
                {
                    //videoProcAmp.Get(VideoProcAmpProperty.Contrast, out val, out flags);
                    //val = min + (max - min) * setting.ContrastRatio / 100;
                    iResult = videoProcAmp.Set(VideoProcAmpProperty.Contrast, setting.ContrastRatio, flags);
                }
            }//设置饱和度
            if (setting.Saturation != -1)
            {
                int hr = videoProcAmp.GetRange(VideoProcAmpProperty.Saturation, out min, out max, out step, out defaultValue, out flags);
                if (0 == hr)
                {
                    //videoProcAmp.Get(VideoProcAmpProperty.Saturation, out val, out flags);
                    //val = min + (max - min) * setting.Saturation / 100;
                    iResult = videoProcAmp.Set(VideoProcAmpProperty.Saturation, setting.Saturation, flags);
                }
            }
            //设置色调
            if (setting.Hue != -1)
            {
                int hr = videoProcAmp.GetRange(VideoProcAmpProperty.Hue, out min, out max, out step, out defaultValue, out flags);
                if (0 == hr)
                {
                    //videoProcAmp.Get(VideoProcAmpProperty.Saturation, out val, out flags);
                    //val = min + (max - min) * setting.Saturation / 100;
                    iResult = videoProcAmp.Set(VideoProcAmpProperty.Hue, setting.Hue, flags);
                }
            }
            //设置清晰度
            if (setting.Sharpness != -1)
            {
                int hr = videoProcAmp.GetRange(VideoProcAmpProperty.Sharpness, out min, out max, out step, out defaultValue, out flags);
                if (0 == hr)
                {
                    //videoProcAmp.Get(VideoProcAmpProperty.Saturation, out val, out flags);
                    //val = min + (max - min) * setting.Saturation / 100;
                    iResult = videoProcAmp.Set(VideoProcAmpProperty.Sharpness, setting.Sharpness, flags);
                }
            }
            //设置伽玛
            if (setting.Gamma != -1)
            {
                int hr = videoProcAmp.GetRange(VideoProcAmpProperty.Gamma, out min, out max, out step, out defaultValue, out flags);
                if (0 == hr)
                {
                    //videoProcAmp.Get(VideoProcAmpProperty.Saturation, out val, out flags);
                    //val = min + (max - min) * setting.Saturation / 100;
                    iResult = videoProcAmp.Set(VideoProcAmpProperty.Gamma, setting.Gamma, flags);
                }
            }
            //设置启用颜色
            if (setting.Gamma != -1)
            {
                int hr = videoProcAmp.GetRange(VideoProcAmpProperty.ColorEnable, out min, out max, out step, out defaultValue, out flags);
                if (0 == hr)
                {
                    //videoProcAmp.Get(VideoProcAmpProperty.Saturation, out val, out flags);
                    //val = min + (max - min) * setting.Saturation / 100;
                    iResult = videoProcAmp.Set(VideoProcAmpProperty.ColorEnable, Convert.ToInt32(setting.ColorEnable), flags);
                }
            }
            //白平衡
            if (setting.WhiteBalance != -1)
            {
                int hr = videoProcAmp.GetRange(VideoProcAmpProperty.WhiteBalance, out min, out max, out step, out defaultValue, out flags);
                if (0 == hr)
                {
                    //videoProcAmp.Get(VideoProcAmpProperty.Saturation, out val, out flags);
                    //val = min + (max - min) * setting.Saturation / 100;
                    iResult = videoProcAmp.Set(VideoProcAmpProperty.WhiteBalance, setting.WhiteBalance, flags);
                }
            }
            //背光补偿
            if (setting.WhiteBalance != -1)
            {
                int hr = videoProcAmp.GetRange(VideoProcAmpProperty.BacklightCompensation, out min, out max, out step, out defaultValue, out flags);
                if (0 == hr)
                {
                    //videoProcAmp.Get(VideoProcAmpProperty.Saturation, out val, out flags);
                    //val = min + (max - min) * setting.Saturation / 100;
                    iResult = videoProcAmp.Set(VideoProcAmpProperty.BacklightCompensation, setting.BacklightCompensation, flags);
                }
            }
            //增益
            if (setting.Gain != -1)
            {
                int hr = videoProcAmp.GetRange(VideoProcAmpProperty.Gain, out min, out max, out step, out defaultValue, out flags);
                if (0 == hr)
                {
                    //videoProcAmp.Get(VideoProcAmpProperty.Saturation, out val, out flags);
                    //val = min + (max - min) * setting.Saturation / 100;
                    iResult = videoProcAmp.Set(VideoProcAmpProperty.Gain, setting.Gain, flags);
                }
            }
            return iResult;
        }
        #endregion
    }
}
