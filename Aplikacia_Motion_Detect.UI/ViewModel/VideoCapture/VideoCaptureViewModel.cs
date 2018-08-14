using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Aplikacia_Motion_Detect.Interfaces.Messages;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
namespace Aplikacia_Motion_Detect.UI.ViewModel.VideoCapture
{
    public class VideoCaptureViewModel
    {

        private RelayCommand _closingWindow;

        public RelayCommand ClosingWindow { get => _closingWindow; set => _closingWindow = value; }

        public VideoCaptureViewModel()
        {

            this.CommandInit();

        }

        public void CommandInit()
        {
            this.ClosingWindow = new RelayCommand(this.CloseWindow);

        }

        public void CloseWindow()
        {
            Messenger.Default.Send<ClosedWindowMessage>(new ClosedWindowMessage() { Closed = true });

        }


    }



}
