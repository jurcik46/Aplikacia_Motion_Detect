using System.Windows;
using System.Windows.Forms.Integration;
using DTKVideoCapLib;
using UserControl = System.Windows.Controls.UserControl;

namespace Aplikacia_Motion_Detect.UI.Views.MainWindow
{
    /// <summary>
    /// Interaction logic for VideoControl.xaml
    /// </summary>
    public partial class VideoControl : UserControl
    {

        private static bool shVideo = false;
        private static bool shZone = false;
        private static VideoCapture saveCapture = null;
        public VideoControl()
        {
            InitializeComponent();
        }

        private void VideoCheckboxChanged(object sender, RoutedEventArgs e)
        {
            shVideo = ShowVideoCheckBox.IsChecked ?? false;
            VideoControl thisVidContorl = (VideoControl)this;
            WindowsFormsHost axVideoDisplayHost = thisVidContorl.VideoDisplayHost;
            VideoDisplay axVideoDisplay = (VideoDisplay)axVideoDisplayHost.Child;
            if (shVideo)
            {
                axVideoDisplay.SetVideoCapt(saveCapture);
            }
            else
            {
                axVideoDisplay.SetVideoCapt(null);
            }
        }


        private void MotionZoneCheckBoxChanged(object sender, RoutedEventArgs e)
        {
            shZone = ShowMotionZoneCheckBox.IsChecked ?? false;
            SetCurrentValue(MotionZoneProperty, shZone);
        }

        public static readonly DependencyProperty MotionZoneProperty = DependencyProperty.Register(
            "MotionZone", typeof(bool), typeof(VideoControl), new FrameworkPropertyMetadata(
                default(bool),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                ZoneOnPropertyChanged));

        private static void ZoneOnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            VideoControl thisVidContorl = (VideoControl)d;
            WindowsFormsHost axVideoDisplayHost = thisVidContorl.VideoDisplayHost;
            VideoDisplay axVideoDisplay = (VideoDisplay)axVideoDisplayHost.Child;
            axVideoDisplay.SetMotionZone((bool)e.NewValue);
        }


        public bool MotionZone
        {
            get { return (bool)GetValue(MotionZoneProperty); }
            set { SetValue(MotionZoneProperty, value); }
        }

        public static readonly DependencyProperty VideoCapProperty = DependencyProperty.Register(
            "VideoCap", typeof(VideoCapture), typeof(VideoControl), new FrameworkPropertyMetadata(
                default(VideoCapture),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                VidOnPropertyChanged));


        private static void VidOnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            VideoControl thisVidContorl = (VideoControl)d;
            WindowsFormsHost axVideoDisplayHost = thisVidContorl.VideoDisplayHost;
            var pom = (VideoCapture)e.NewValue;
            saveCapture = pom;
            if (!shVideo)
            {
                pom = null;
            }
            VideoDisplay axVideoDisplay = (VideoDisplay)axVideoDisplayHost.Child;
            axVideoDisplay.SetVideoCapt(pom);
        }

        public VideoCapture VideoCap
        {
            get { return (VideoCapture)GetValue(VideoCapProperty); }
            set { SetValue(VideoCapProperty, value); }
        }
    }
}
