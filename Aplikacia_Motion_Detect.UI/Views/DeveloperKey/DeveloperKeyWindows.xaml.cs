using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Aplikacia_Motion_Detect.Interfaces.Interface;

namespace Aplikacia_Motion_Detect.UI.Views.DeveloperKey
{
    /// <summary>
    /// Interaction logic for DeveloperKeyWindows.xaml
    /// </summary>
    public partial class DeveloperKeyWindows : Window, IClosable
    {
        public DeveloperKeyWindows()
        {
            InitializeComponent();
            this.DataContext = ViewModelLocator.DeveloperKeyViewModel;
        }
    }
}
