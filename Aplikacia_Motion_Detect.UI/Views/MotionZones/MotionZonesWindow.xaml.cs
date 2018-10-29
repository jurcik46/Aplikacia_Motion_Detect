﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Aplikacia_Motion_Detect.UI.Views.MotionZones
{
    /// <summary>
    /// Interaction logic for MotionZonesWindow.xaml
    /// </summary>
    public partial class MotionZonesWindow : Window, IClosable
    {
        public MotionZonesWindow()
        {
            InitializeComponent();
            this.DataContext = ViewModelLocator.MotionZonesViewModel;
        }

        private void MotionZonesWindow_OnClosing(object sender, CancelEventArgs e)
        {
            ViewModelLocator.VideoService.VideoDevice = null;
            ViewModelLocator.VideoService.SaveConfig();
            ViewModelLocator.CleanupMotionZonesViewModel();
        }
    }
}
