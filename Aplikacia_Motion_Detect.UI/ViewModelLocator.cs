using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using Aplikacia_Motion_Detect.UI.ViewModel.MainWindow;
using Aplikacia_Motion_Detect.UI.ViewModel;
using GalaSoft.MvvmLight;
using Aplikacia_Motion_Detect.Interfaces.Interface;
using Aplikacia_Motion_Detect.Backend.Design;
using Aplikacia_Motion_Detect.Backend.Service;



namespace Aplikacia_Motion_Detect.UI
{
    public static class ViewModelLocator
    {


        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
                SimpleIoc.Default.Register<ITestService, DesignTestService>();
            }
            else
            {
                // Create run time view services and models
                SimpleIoc.Default.Register<ITestService, TestService>();


            }

            RegisterViewModels();

        }

        internal static void RegisterViewModels()
        {
            if (!SimpleIoc.Default.IsRegistered<MainViewModel>())
            {
                SimpleIoc.Default.Register<MainViewModel>();
            }
        }





        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }

        public static MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();


    }

}

