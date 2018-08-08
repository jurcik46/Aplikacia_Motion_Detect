using Aplikacia_Motion_Detect.Interfaces.Interface;
using Aplikacia_Motion_Detect.Backend.Design;
using Aplikacia_Motion_Detect.Backend.Service;
using GalaSoft.MvvmLight;
using CommonServiceLocator;

namespace Aplikacia_Motion_Detect.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {



        public MainViewModel()
        {



        }

        public static ITestService TestService => ServiceLocator.Current.GetInstance<ITestService>();

    }
}
