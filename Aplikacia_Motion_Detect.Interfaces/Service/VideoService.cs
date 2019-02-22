using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Xml;
using Aplikacia_Motion_Detect.Interfaces.Convertors;
using Aplikacia_Motion_Detect.Interfaces.Enums;
using Aplikacia_Motion_Detect.Interfaces.Extensions;
using Aplikacia_Motion_Detect.Interfaces.Interface.Services;
using Aplikacia_Motion_Detect.Interfaces.Messages;
using Aplikacia_Motion_Detect.Interfaces.Models;
using DTKVideoCapLib;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Serilog;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

namespace Aplikacia_Motion_Detect.Interfaces.Service
{
    public class VideoService : IVideoService
    {
        struct FrameImageInfo
        {
            public int width;
            public int height;
            public PixelFormatEnum pixelFormat;
        }

        public ILogger Logger => Log.Logger.ForContext<VideoService>();


        public VideoCaptureUtils utils = new VideoCaptureUtils();
        public List<VideoInfoDataGridModel> _videoCaptureList;

        private string configFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                        "\\CCSIPRO\\" + LoggerInit.ApplicationName + "\\DTKVideoCapture.xml";

        //private string pa = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
        //                    "\\CCSIPRO\\" + LoggerInit.ApplicationName + "\\img";

        private string _developerKey = "";

        public List<VideoInfoDataGridModel> VideoCaptureList
        {
            get { return _videoCaptureList; }
            set { _videoCaptureList = value; }
        }

        public string DeveloperKey
        {
            get { return _developerKey; }
            set
            {
                _developerKey = value;
                Logger.Information(VideoServiceEvents.SetDeveloperKey, $"Setting developer key to {value}");
            }
        }

        public VideoService()
        {
            Logger.Debug(VideoServiceEvents.Create, "Creating new instance of VideoService");
            this.VideoCaptureList = new List<VideoInfoDataGridModel>();
            this.LoadConfig();
        }

        public void AddVideoCapture(VideoInfoDataGridModel videoDevice)
        {
            Logger.Information(VideoServiceEvents.AddVideoCaptureDevice, $"Adding new video device whit name {videoDevice.Name} and type {videoDevice.Type}");
            videoDevice.VideoCapture.FrameReceived += FrameReceived;
            videoDevice.VideoCapture.StateChanged += VideoCaptureStateChanged;
            videoDevice.VideoCapture.Error += VideoCaptureError;

            VideoCaptureList.Add(videoDevice);

            SetInfoData();
            SaveConfig();
            //if (!Directory.Exists(pa))
            //{
            //    Directory.CreateDirectory(pa);
            //}
        }

        private void FrameReceived(VideoCapture vidCap, FrameImage frame)
        {
            //Logger.Debug(VideoServiceEvents.FrameReceived, $"Width {frame.Width} Height {frame.Height} PixelFormat {frame.PixelFormat} from device  {vidCap.Name} ");
            FrameImageInfo frameInfo = new FrameImageInfo();
            frameInfo.width = frame.Width;
            frameInfo.height = frame.Height;
            frameInfo.pixelFormat = frame.PixelFormat;
            SetInfoDataOne(vidCap, frameInfo);

            //Task.Run(() =>
            //{
            //    FrameImage f = frame;
            //    long k = 0;
            //    f.GetHBitmap(out k);
            //    IntPtr test = new IntPtr(k);

            //    using (Bitmap bmp = Image.FromHbitmap(test))
            //    {
            //        bmp.Save(pa + "\\ " + f.Timestamp + ".jpg", ImageFormat.Jpeg);
            //    }
            //    test = IntPtr.Zero;
            //    k = 0;
            //});

            Marshal.ReleaseComObject(frame);

        }


        private void VideoCaptureStateChanged(VideoCapture vidCap, VideoCaptureStateEnum State)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(
                () =>
                {
                    var pom = FoundEqualsVideoCapture(vidCap);
                    if (pom != null)
                    {
                        Logger.Debug(VideoServiceEvents.VideoCaptureStateChanged, $"Video device {vidCap.Name} changed state from {pom.State} to {State}");
                        UpdateState(pom);
                        if (State == VideoCaptureStateEnum.VCS_Stopped)
                        {
                            // reset data 
                            pom.FPS = 0;
                            pom.Resolution = "";
                            pom.Frames = 0;
                            pom.Pixel = "";
                        }
                        if (State == VideoCaptureStateEnum.VCS_Starting)
                        {
                            pom.LastError = "";
                        }
                    }
                });
        }

        private void VideoCaptureError(VideoCapture vidCap, int errorCode)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(
                () =>
                {
                    var pom = FoundEqualsVideoCapture(vidCap);
                    if (pom != null)
                    {
                        string error = errorCode.ToString("X8");
                        Logger.Error(VideoServiceEvents.VideoCaptureError, $"Video device {vidCap.Name} raised error {error} ");
                        //if (error.Equals("FFFFFFFB") || error.Equals("80008004"))
                        //{
                            Logger.Warning(VideoServiceEvents.VideoDeviceDisconnected, $"Video device {vidCap.Name}");
                            Task.Run(() => { ReconnectDevice(vidCap); });
                        //}

                        pom.LastError = "Error " + "0x" + error;
                    }
                });
        }

        private void ReconnectDevice(VideoCapture vidCap)
        {
            while (vidCap.State != VideoCaptureStateEnum.VCS_Started)
            {
                System.Threading.Thread.Sleep(1000);
                Logger.Warning(VideoServiceEvents.TryingRestartCapturing, $"Video device {vidCap.Name}");
                vidCap.StartCapture();
                System.Threading.Thread.Sleep(4000);
            }
        }

        public void ModifyVideoCapture(VideoInfoDataGridModel videoDevice)
        {
            SetInfoData();
            SaveConfig();
            Logger.Information(VideoServiceEvents.ModifyVideoCapture, "Video device {Name} modified setting \n{@videoDevice}", videoDevice.Name, videoDevice);
        }

        public void DeleteVideoCapture(VideoInfoDataGridModel videoDevice)
        {
            Logger.Information(VideoServiceEvents.DeleteVideoCapture, "Video device {Name}  \n{@videoDevice}", videoDevice.Name, videoDevice);
            VideoCaptureList.Remove(videoDevice);
            SaveConfig();
        }

        public void StartCaptureOne(VideoInfoDataGridModel videoSource)
        {
            try
            {
                if (!videoSource.Enable)
                    return;
                Logger.Information(VideoServiceEvents.StartCapture, "Video device {Name} ", videoSource.Name);
                videoSource.VideoCapture.StartCapture();
            }
            catch (COMException)
            {
                Messenger.Default.Send(new NotifiMessage()
                {
                    Msg = "Error: " + videoSource.VideoCapture.LastErrorCode
                });
            }
        }

        public void StartCaptureAll()
        {
            foreach (var item in VideoCaptureList)
            {
                if (!item.Enable)
                    continue;
                try
                {
                    Logger.Information(VideoServiceEvents.StartCapture, "Video device {Name} ", item.Name);
                    item.VideoCapture.StartCapture();
                }
                catch (COMException)
                {
                    Messenger.Default.Send<NotifiMessage>(new NotifiMessage()
                    {
                        Msg = "Error: " + item.VideoCapture.LastErrorCode
                    });
                }
            }
        }

        public void StopCaptureOne(VideoInfoDataGridModel videoSource)
        {
            if (!videoSource.Enable)
                return;
            Logger.Information(VideoServiceEvents.StopCapture, "Video device {Name} ", videoSource.Name);

            videoSource.VideoCapture.FrameReceived -= FrameReceived;
            videoSource.VideoCapture.StateChanged -= VideoCaptureStateChanged;
            videoSource.VideoCapture.Error -= VideoCaptureError;
            Thread.Sleep(100);
            videoSource.VideoCapture.StopCapture();
            Thread.Sleep(100);

            videoSource.VideoCapture.FrameReceived += FrameReceived;
            videoSource.VideoCapture.StateChanged += VideoCaptureStateChanged;
            videoSource.VideoCapture.Error += VideoCaptureError;

            UpdateState(videoSource);
        }

        public void StopCaptureAll()
        {
            foreach (var item in VideoCaptureList)
            {
                if (!item.Enable)
                    continue;
                Logger.Information(VideoServiceEvents.StopCapture, "Video device {Name} ", item.Name);
                StopCaptureOne(item);
                //item.VideoCapture.StopCapture();

            }
        }

        public void SaveConfig()
        {
            Logger.Information(VideoServiceEvents.SavingConfig);
            XmlDocument xmlDoc = new XmlDocument();
            BoolToTextConverter boolConvert = new BoolToTextConverter();
            XmlNode rootNode = xmlDoc.CreateNode(XmlNodeType.Element, "VideoSources", "");
            int i = 0;

            XmlNode node = xmlDoc.CreateNode(XmlNodeType.Element, "DeveloperKey", "");
            node.InnerText = this.DeveloperKey;
            rootNode.AppendChild(node);

            foreach (var row in VideoCaptureList)
            {
                VideoCapture videoCapture = row.VideoCapture;
                // get configuration XML string
                string xmlConfig;
                videoCapture.GetConfigXml(out xmlConfig);

                node = xmlDoc.CreateNode(XmlNodeType.Element, "VideoCapture", "");
                node.InnerText = xmlConfig;
                var motionZonesNode = xmlDoc.CreateNode(XmlNodeType.Element, "MotionZones", "");

                if (row.MotionZones != null && row.MotionZones.Count != 0){
                    foreach (var zone in row.MotionZones) {
                        var zoneNode = xmlDoc.CreateNode(XmlNodeType.Element, "Zone", "");
                        var number = xmlDoc.CreateNode(XmlNodeType.Element, "Number", "");
                        number.InnerText = zone.Number.ToString();
                        zoneNode.AppendChild(number);
                        var name = xmlDoc.CreateNode(XmlNodeType.Element, "Name", "");
                        name.InnerText = zone.Name;
                        zoneNode.AppendChild(name);
                        var timer = xmlDoc.CreateNode(XmlNodeType.Element, "Timer", "");
                        timer.InnerText = zone.Timer.ToString();
                        zoneNode.AppendChild(timer);
                        motionZonesNode.AppendChild(zoneNode);
                    }
                }
                node.AppendChild(motionZonesNode);
                var enableNode = xmlDoc.CreateNode(XmlNodeType.Element, "Enable", "");
                enableNode.InnerText = (string)boolConvert.Convert(row.Enable);
                node.AppendChild(enableNode);
                rootNode.AppendChild(node);
                i++;
            }
            xmlDoc.AppendChild(rootNode);

            xmlDoc.Save(configFilePath);
        }

        public void LoadConfig()
        {
            if (File.Exists(configFilePath))
            {
                Logger.Information(VideoServiceEvents.LoadingConfig);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(configFilePath);

                BoolToTextConverter boolConvert = new BoolToTextConverter();
                XmlNode root = xmlDoc.DocumentElement;

                foreach (XmlNode node in root.ChildNodes)
                {
                    if (node.Name == "VideoCapture")
                    {
                        VideoCapture videoCapture = new VideoCapture();
                        videoCapture.SetConfigXml(node.InnerText);
                        var pom = new VideoInfoDataGridModel()
                        {
                            VideoCapture = videoCapture,
                            MotionZones = new List<MotionZoneInfoDataGridModel>()
                        };
                        foreach (XmlNode childNode in node.ChildNodes)
                        {
                            if (childNode.Name == "Enable")
                            {
                                var a = boolConvert.ConvertBack(childNode.InnerText);
                                if (a == null)
                                {
                                    pom.Enable = false;
                                }
                                else
                                {
                                    pom.Enable = (bool)a;
                                }
                            }
                            if(childNode.Name == "MotionZones")
                            {
                                foreach(XmlNode motionZ in childNode.ChildNodes)
                                {
                                    var pomZone = new MotionZoneInfoDataGridModel();
                                    foreach (XmlNode zoneData in motionZ.ChildNodes) {
                                        switch (zoneData.Name) {
                                            case "Number":
                                                pomZone.Number = int.Parse(zoneData.InnerText);
                                                break;
                                            case "Name":
                                                pomZone.Name = zoneData.InnerText;
                                                break;
                                            case "Timer":
                                                pomZone.Timer = int.Parse(zoneData.InnerText);
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    pom.MotionZones.Add(pomZone);
                                }
                            }
                        }
                        for (int i = 0; i < pom.VideoCapture.MotionZones.Count; i++)
                        {
                            pom.MotionZones[i].Zone = pom.VideoCapture.MotionZones.Item[i];
                        };
                        AddVideoCapture(pom);
                    }
                    if (node.Name == "DeveloperKey")
                    {
                        this.DeveloperKey = node.InnerText;
                        if (this.DeveloperKey.Length > 0)
                            utils.SetDeveloperLicenseKey(this._developerKey);
                    }
                }
            }
            SetInfoData();
        }

        private void SetInfoData()
        {
            Logger.Debug(VideoServiceEvents.SettingGUIDeviceDataAll);

            foreach (var row in VideoCaptureList)
            {
                row.Name = row.VideoCapture.Name;
                string strState = row.VideoCapture.State.ToString();
                //remove prefix from the state contatnt name 
                strState = strState.Substring(4, strState.Length - 4);
                row.State = strState;

                if (row.VideoCapture.VideoSource is IPCamera)
                {
                    IPCamera ipPCamera = (IPCamera)row.VideoCapture.VideoSource;
                    row.Description = ipPCamera.IPCameraURL;
                    row.Type = "IP Camera";
                }
                else if (row.VideoCapture.VideoSource is VideoDevice)
                {
                    VideoDevice videoDevice = (VideoDevice)row.VideoCapture.VideoSource;
                    row.Description = videoDevice.DisplayName;
                    row.Type = "Video Device";
                }
                else if (row.VideoCapture.VideoSource is VideoFile)
                {
                    VideoFile videoFile = (VideoFile)row.VideoCapture.VideoSource;
                    row.Description = videoFile.FileName;
                    row.Type = "Video File";
                }

                row.Frames = 0;
                row.FPS = 0;
                row.Resolution = "";
            }
        }

        private void SetInfoDataOne(VideoCapture vidCap, FrameImageInfo frameInfo)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(
                () =>
                {

                    var pom = FoundEqualsVideoCapture(vidCap);
                    if (pom != null)
                    {

                        // set resulution 
                        pom.Resolution = frameInfo.width.ToString() + "x" + frameInfo.height.ToString();

                        // set pixel format 
                        switch (frameInfo.pixelFormat)
                        {
                            case PixelFormatEnum.PIXFMT_GRAYSCALE:
                                pom.Pixel = "GrayScale";
                                break;
                            case PixelFormatEnum.PIXFMT_RGB24:
                                pom.Pixel = "RGB24";
                                break;
                            case PixelFormatEnum.PIXFMT_YUV420:
                                pom.Pixel = "YUV420";
                                break;
                            default:
                                break;
                        }

                        // increment frames count 
                        //
                        int frames = pom.Frames + 1;
                        pom.Frames = frames;
                        // calculate fps
                        //
                        int fps = 0;
                        if (pom.FrameTick == null)
                            pom.FrameTick = new Queue<int>();
                        Queue<int> ticksQueue = (Queue<int>)pom.FrameTick;
                        int curTicks = Environment.TickCount;
                        ticksQueue.Enqueue(curTicks);
                        if (ticksQueue.Count > 10)
                        {
                            int t = curTicks - ticksQueue.First();
                            while (t > 5000) // calc FPS for last 5 sec
                            {
                                ticksQueue.Dequeue();
                                if (ticksQueue.Count > 0)
                                    t = curTicks - ticksQueue.First();
                                else
                                    t = 0;
                            }
                            if (t > 0)
                                fps = (int)Math.Round((float)ticksQueue.Count * 1000 / t);
                            else if (t < 0)
                                ticksQueue.Clear();
                        }
                        pom.FPS = fps;
                    }
                });
        }

        public VideoInfoDataGridModel FoundEqualsVideoCapture(VideoCapture vidCap)
        {
            foreach (var row in VideoCaptureList)
            {
                if (object.ReferenceEquals(row.VideoCapture, vidCap))
                {
                    return row;
                }
            }
            return null;
        }

        private void UpdateState(VideoInfoDataGridModel vidCap)
        {
            Logger.Debug(VideoServiceEvents.UpdateStateVideoDevice);
            string strState = vidCap.VideoCapture.State.ToString();
            //remove prefix from the state contatnt name 
            strState = strState.Substring(4, strState.Length - 4);
            vidCap.State = strState;
        }


    }
}
