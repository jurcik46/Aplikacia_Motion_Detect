using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using Aplikacia_Motion_Detect.Interfaces.Interface.Services;
using Aplikacia_Motion_Detect.Interfaces.Messages;
using Aplikacia_Motion_Detect.Interfaces.Models;
using DTKVideoCapLib;
using GalaSoft.MvvmLight.Messaging;

namespace Aplikacia_Motion_Detect.Interfaces.Service
{
    public class VideoService : IVideoService
    {

        public List<VideoInfoDataGridModel> _videoCaptureList;
        public VideoInfoDataGridModel _videoDevice;

        private string configFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\DTKVideoCaptureDemo.xml";

        private string developerKey = "";

        public VideoService()
        {
            this.VideoCaptureList = new List<VideoInfoDataGridModel>();
            this.LoadConfig();
            //this.VideoDevice = null;
        }

        public void ModifyVideoCapture()
        {
            foreach (var item in VideoCaptureList)
            {
                if (object.ReferenceEquals(item, VideoDevice))
                {
                    ChangeVideoSource(item);
                    return;
                }
            }
        }

        public void DeleteVideoCapture()
        {
            foreach (var item in VideoCaptureList)
            {
                if (object.ReferenceEquals(item, VideoDevice))
                {
                    DeleteVideoSource(item);
                    return;
                }
            }
        }

        private void ChangeVideoSource(VideoInfoDataGridModel videSource)
        {
            videSource = VideoDevice;
            VideoDevice = null;
        }

        private void DeleteVideoSource(VideoInfoDataGridModel videSource)
        {
            VideoCaptureList.Remove(videSource);
            VideoDevice = null;
        }

        public void StartCaptureOne(VideoInfoDataGridModel videoSource)
        {
            foreach (var item in VideoCaptureList)
            {
                if (object.ReferenceEquals(item, videoSource))
                {
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

                    return;
                }
            }
        }

        public void StartCaptureAll()
        {
            foreach (var item in VideoCaptureList)
            {
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
                    item.VideoCapture.StopCapture();

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

        private void SaveConfig()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateNode(XmlNodeType.Element, "VideoSources", "");
            int i = 0;

            XmlNode node = xmlDoc.CreateNode(XmlNodeType.Element, "DeveloperKey", "");
            node.InnerText = this.developerKey;
            rootNode.AppendChild(node);

            foreach (var row in VideoCaptureList)
            {
                VideoCapture videoCapture = row.VideoCapture;

                // get configuration XML string
                string xmlConfig;
                videoCapture.GetConfigXml(out xmlConfig);

                node = xmlDoc.CreateNode(XmlNodeType.Element, "VideoCapture", "");
                node.InnerText = xmlConfig;
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

                XmlNode root = xmlDoc.DocumentElement;
                foreach (XmlNode node in root.ChildNodes)
                {
                    if (node.Name == "VideoCapture")
                    {
                        VideoCapture videoCapture = new VideoCapture();
                        videoCapture.SetConfigXml(node.InnerText);
                        VideoCaptureList.Add(new VideoInfoDataGridModel()
                        {
                            VideoCapture = videoCapture
                        });
                    }
                    if (node.Name == "DeveloperKey")
                    {
                        //this.developerKey = node.InnerText;
                        //if (this.developerKey.Length > 0)
                        //    utils.SetDeveloperLicenseKey(this.developerKey);
                    }
                }
            }

        }

        public List<VideoInfoDataGridModel> VideoCaptureList
        {
            get { return _videoCaptureList; }
            set { _videoCaptureList = value; }
        }

        public VideoInfoDataGridModel VideoDevice
        {
            get { return _videoDevice; }
            set { _videoDevice = value; }
        }
    }
}
