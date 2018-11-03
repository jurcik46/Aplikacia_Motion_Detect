using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DTKVideoCapLib;

namespace Aplikacia_Motion_Detect.UI.Views.MotionZones
{
    /// <summary>
    /// Interaction logic for VideoDisplayMotionZoneControl.xaml
    /// </summary>
    public partial class VideoDisplayMotionZoneControl : UserControl
    {
        public VideoDisplayMotionZoneControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty VideoCaptureProperty = DependencyProperty.Register(
            "VideoCapture", typeof(VideoCapture), typeof(VideoDisplayMotionZoneControl), new FrameworkPropertyMetadata(
                default(VideoCapture),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                VideoCaptureOnPropertyChanged));

        public VideoCapture VideoCapture
        {
            get { return (VideoCapture)GetValue(VideoCaptureProperty); }
            set { SetValue(VideoCaptureProperty, value); }
        }

        private static void VideoCaptureOnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VideoDisplayMotionZoneControl thisVidContorl = (VideoDisplayMotionZoneControl)d;
            WindowsFormsHost axVideoDisplayHost = thisVidContorl.VideoDisplayHostMotionZone;
            VideoDisplayMotionZones axVideoDisplay = (VideoDisplayMotionZones)axVideoDisplayHost.Child;
            axVideoDisplay.SetVideoCapture((VideoCapture)e.NewValue);
        }



    }
}
