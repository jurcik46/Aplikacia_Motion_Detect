using System.Reflection;
using System.Resources;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using Aplikacia_Motion_Detect.UI.ViewModels.MainWindow;
using Aplikacia_Motion_Detect.UI.ViewModels.AddVideoDevice;
using GalaSoft.MvvmLight;
using Aplikacia_Motion_Detect.Interfaces.Service;
using Aplikacia_Motion_Detect.Interfaces.Interface.Services;
using Aplikacia_Motion_Detect.UI.ViewModels.DeveloperKey;
using Aplikacia_Motion_Detect.UI.ViewModels.MotionZones;
using Serilog.Core;

namespace Aplikacia_Motion_Detect.UI
{
    public static class ViewModelLocator
    {

        private static LoggingLevelSwitch _loggingLevelSwitch;

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
                //SimpleIoc.Default.Register<ITestService, DesignTestService>();
                SimpleIoc.Default.Register<IVideoService, VideoService>();

            }
            else
            {
                // Create run time view services and models
                //SimpleIoc.Default.Register<ITestService, TestService>();
                SimpleIoc.Default.Register<IVideoService, VideoService>();

            }

            RegisterViewModels();

        }

        internal static void RegisterViewModels()
        {
            if (!SimpleIoc.Default.IsRegistered<MainViewModel>())
            {
                SimpleIoc.Default.Register<MainViewModel>();
            }

            if (!SimpleIoc.Default.IsRegistered<VideoCaptureViewModel>())
            {
                SimpleIoc.Default.Register<VideoCaptureViewModel>();
            }

            if (!SimpleIoc.Default.IsRegistered<DeveloperKeyViewModel>())
            {
                SimpleIoc.Default.Register<DeveloperKeyViewModel>();
            }

            if (!SimpleIoc.Default.IsRegistered<MotionZonesViewModel>())
            {
                SimpleIoc.Default.Register<MotionZonesViewModel>();
            }

        }




        public static void CleanupVideoCaptureVieModel()
        {
            if (SimpleIoc.Default.IsRegistered<VideoCaptureViewModel>())
            {
                SimpleIoc.Default.Unregister<VideoCaptureViewModel>();
            }

            if (!SimpleIoc.Default.IsRegistered<VideoCaptureViewModel>())
            {
                SimpleIoc.Default.Register<VideoCaptureViewModel>();
            }
            // TODO Clear the ViewModels
        }

        public static LoggingLevelSwitch LoggingLevelSwitch => _loggingLevelSwitch ?? (_loggingLevelSwitch = new LoggingLevelSwitch());

        public static MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();
        public static VideoCaptureViewModel VideoCaptureViewModel => ServiceLocator.Current.GetInstance<VideoCaptureViewModel>();
        public static DeveloperKeyViewModel DeveloperKeyViewModel => ServiceLocator.Current.GetInstance<DeveloperKeyViewModel>();
        public static MotionZonesViewModel MotionZonesViewModel => ServiceLocator.Current.GetInstance<MotionZonesViewModel>();
        public static IVideoService VideoService => ServiceLocator.Current.GetInstance<IVideoService>();
        public static ResourceManager rm = new ResourceManager("Aplikacia_Motion_Detect.Resources.Languages.sk-Sk", Assembly.GetExecutingAssembly());


    }

}

