using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Xml;
using Aplikacia_Motion_Detect.Interfaces.Convertors;
using Aplikacia_Motion_Detect.Interfaces.Interface.Services;
using Aplikacia_Motion_Detect.Interfaces.Messages;
using Aplikacia_Motion_Detect.Interfaces.Models;
using DTKVideoCapLib;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Win32;

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

        public VideoCaptureUtils utils = new VideoCaptureUtils();
        public List<VideoInfoDataGridModel> _videoCaptureList;

        private string configFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                        "\\CCSIPRO\\" + LoggerInit.ApplicationName + "\\DTKVideoCapture.xml";

        private string _developerKey = "";

        public List<VideoInfoDataGridModel> VideoCaptureList
        {
            get { return _videoCaptureList; }
            set { _videoCaptureList = value; }
        }

        public string DeveloperKey
        {
            get { return _developerKey; }
            set { _developerKey = value; }
        }

        public VideoService()
        {
            this.VideoCaptureList = new List<VideoInfoDataGridModel>();
            this.LoadConfig();
        }

        public void AddVideoCapture(VideoInfoDataGridModel videoDevice)
        {
            videoDevice.VideoCapture.FrameReceived += FrameReceived;
            videoDevice.VideoCapture.StateChanged += VideoCaptureStateChanged;
            videoDevice.VideoCapture.Error += VideoCaptureError;

            VideoCaptureList.Add(videoDevice);
            SetInfoData();
            SaveConfig();
        }

        private void FrameReceived(VideoCapture vidCap, FrameImage frame)
        {
            FrameImageInfo frameInfo = new FrameImageInfo();
            frameInfo.width = frame.Width;
            frameInfo.height = frame.Height;
            frameInfo.pixelFormat = frame.PixelFormat;

            SetInfoDataOne(vidCap, frameInfo);

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
                        if (error.Equals("FFFFFFFB") || error.Equals("80008004"))
                        {
                            Task.Run(() => { ReconnectDevice(vidCap); });

                        }

                        pom.LastError = "Error " + "0x" + error;
                    }
                });

        }

        private void ReconnectDevice(VideoCapture vidCap)
        {
            while (vidCap.State != VideoCaptureStateEnum.VCS_Started)
            {
                System.Threading.Thread.Sleep(1000);
                vidCap.StartCapture();
                System.Threading.Thread.Sleep(4000);
            }
        }

        public void ModifyVideoCapture(VideoInfoDataGridModel videoDevice)
        {
            foreach (var item in VideoCaptureList)
            {
                if (object.ReferenceEquals(item, videoDevice))
                {
                    ChangeVideoSource(item, videoDevice);
                    SetInfoData();
                    return;
                }
            }
        }

        public void DeleteVideoCapture(VideoInfoDataGridModel videoDevice)
        {
            foreach (var item in VideoCaptureList)
            {
                if (object.ReferenceEquals(item, videoDevice))
                {
                    DeleteVideoSource(item);
                    return;
                }
            }
        }

        private void ChangeVideoSource(VideoInfoDataGridModel videoDeviceOld, VideoInfoDataGridModel videoDeviceNew)
        {
            videoDeviceOld = videoDeviceNew;
            SaveConfig();
        }

        private void DeleteVideoSource(VideoInfoDataGridModel videoDevice)
        {
            VideoCaptureList.Remove(videoDevice);
            SaveConfig();
        }

        public void StartCaptureOne(VideoInfoDataGridModel videoSource)
        {
            foreach (var item in VideoCaptureList)
            {
                if (object.ReferenceEquals(item, videoSource) && item.Enable)
                {
                    try
                    {

                        item.VideoCapture.StartCapture();
                        UpdateState(item);

                    }
                    catch (COMException)
                    {
                        Messenger.Default.Send<NotifiMessage>(new NotifiMessage()
                        {
                            Msg = "Error: " + item.VideoCapture.LastErrorCode
                        });
                    }

                    return;
                }
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
            foreach (var item in VideoCaptureList)
            {
                if (object.ReferenceEquals(item, videoSource))
                {
                    try
                    {
                        item.VideoCapture.StopCapture();

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    return;
                }
            }
        }

        public void StopCaptureAll()
        {
            foreach (var item in VideoCaptureList)
            {
                item.VideoCapture.StopCapture();

            }
        }

        public void SaveConfig()
        {
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
                var enableNode = xmlDoc.CreateNode(XmlNodeType.Element, "Enable", "");
                enableNode.InnerText = (string)boolConvert.Convert(row.Enable);
                node.AppendChild(enableNode);
                rootNode.AppendChild(node);
                i++;
            }
            xmlDoc.AppendChild(rootNode);

            xmlDoc.Save(configFilePath);
        }

        private void LoadConfig()
        {
            if (File.Exists(configFilePath))
            {
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
                            VideoCapture = videoCapture
                        };
                        foreach (XmlNode enable in node.ChildNodes)
                        {
                            if (enable.Name == "Enable")
                            {
                                var a = boolConvert.ConvertBack(enable.InnerText);
                                if (a == null)
                                {
                                    pom.Enable = false;
                                }
                                else
                                {
                                    pom.Enable = (bool)a;
                                }
                            }
                        }
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

        private VideoInfoDataGridModel FoundEqualsVideoCapture(VideoCapture vidCap)
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
            string strState = vidCap.VideoCapture.State.ToString();
            //remove prefix from the state contatnt name 
            strState = strState.Substring(4, strState.Length - 4);
            vidCap.State = strState;
        }


    }
}
