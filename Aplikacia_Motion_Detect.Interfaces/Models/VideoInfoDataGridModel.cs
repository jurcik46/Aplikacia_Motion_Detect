using System;
using Aplikacia_Motion_Detect.Interfaces.Messages;
using DTKVideoCapLib;

namespace Aplikacia_Motion_Detect.Interfaces.Models
{
    public class VideoInfoDataGridModel
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public string LastError { get; set; }
        public string Type { get; set; }
        public string Resolution { get; set; }
        public string Pixel { get; set; }
        public int Frames { get; set; }
        public int FPS { get; set; }
        public VideoCapture VideoCapture { get; set; }

        public static implicit operator VideoInfoDataGridModel(ModifyVideoCapture v)
        {
            throw new NotImplementedException();
        }
    }
}
