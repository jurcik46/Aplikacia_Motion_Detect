﻿using Aplikacia_Motion_Detect.Interfaces.Interface;
using Aplikacia_Motion_Detect.Interfaces.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using CommonServiceLocator;
using GalaSoft.MvvmLight.CommandWpf;

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

        public RelayCommand TestCommand { get; private set; }


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

            this.TestCommand = new RelayCommand(this.DisplayMessage, this.CnaDisplayMessage);


        }

        public bool CnaDisplayMessage()
        {

            return true;
        }

        public void DisplayMessage()
        {
            Messenger.Default.Send<Testmessage>(new Testmessage() { ButtonText = "1111111111111111111111111111111" });

        }



    }
}

