using Aplikacia_Motion_Detect.Interfaces.Interface;
using Aplikacia_Motion_Detect.Interfaces.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using CommonServiceLocator;

namespace Aplikacia_Motion_Detect.UI.ViewModel
{

    public class MainViewModel : ViewModelBase
    {

        private string _testdata;

        public string TestData
        {
            get { return _testdata; }
            set
            {
                if (_testdata == value)
                    return;
                _testdata = value;
                RaisePropertyChanged();
            }
        }

        public MainViewModel()
        {
            ITestService a = ServiceLocator.Current.GetInstance<ITestService>();
            TestData = a.Test;

            Messenger.Default.Register<Testmessage>(this, (message) =>
            {

                TestData = message.ButtonText;

                // ITestService test = ServiceLocator.Current.GetInstance<ITestService>();
                // test.Test = message.ButtonText;
            });


        }

    }
}

