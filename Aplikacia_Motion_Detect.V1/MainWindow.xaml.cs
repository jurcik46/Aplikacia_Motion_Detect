using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using Aplikacia_Motion_Detect.Interfaces.Messages;
namespace Aplikacia_Motion_Detect.V1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<Testmessage>(new Testmessage() { ButtonText = "1111111111111111111111111111111" });
        }
    }
}
