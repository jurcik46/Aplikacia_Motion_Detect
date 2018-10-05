using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using Aplikacia_Motion_Detect.UI.ViewModels.MainWindow;
using Aplikacia_Motion_Detect.UI.ViewModels.AddVideoDevice;
using GalaSoft.MvvmLight;
using Aplikacia_Motion_Detect.Interfaces.Interface;
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
            }
            else
            {
                // Create run time view services and models
                //SimpleIoc.Default.Register<ITestService, TestService>();

            }

            RegisterViewModels();

        }

        internal static void RegisterViewModels()
        {
            if (!SimpleIoc.Default.IsRegistered<MainViewModel>())
            {
                SimpleIoc.Default.Register<MainViewModel>();
            }

            if (!SimpleIoc.Default.IsRegistered<VideoViewModel>())
            {
                SimpleIoc.Default.Register<VideoViewModel>();
            }

            if (!SimpleIoc.Default.IsRegistered<VideoSettingViewModel>())
            {
                SimpleIoc.Default.Register<VideoSettingViewModel>();
            }


            //if (!SimpleIoc.Default.IsRegistered<OptionsViewModel>())
            //{
            //    SimpleIoc.Default.Register(() => new OptionsViewModel(ServiceLocator.Current.GetInstance<IOptionsService>()));
            //}


            //if (!SimpleIoc.Default.IsRegistered<VideoSettingViewModel>())
            //{
            //    SimpleIoc.Default.Register<VideoSettingViewModel>();
            //}
        }




        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }

        public static LoggingLevelSwitch LoggingLevelSwitch => _loggingLevelSwitch ?? (_loggingLevelSwitch = new LoggingLevelSwitch());

        public static MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();
        public static VideoViewModel VideoViewModel => ServiceLocator.Current.GetInstance<VideoViewModel>();
        public static VideoSettingViewModel VideoSettingViewModel => ServiceLocator.Current.GetInstance<VideoSettingViewModel>();
        //  public static VideoSettingViewModel VideoSettingViewModel => ServiceLocator.Current.GetInstance<VideoSettingViewModel>();


    }

}

