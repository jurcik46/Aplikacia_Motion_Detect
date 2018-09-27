using Serilog;
using Microsoft.Shell;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Threading;
using System.Reflection;
using Aplikacia_Motion_Detect.V1.Extensions;

namespace Aplikacia_Motion_Detect.V1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public ILogger Logger => Log.Logger.ForContext<App>();



        protected override void OnStartup(StartupEventArgs e)
        {

            base.OnStartup(e);
            DispatcherHelper.Initialize();

            var appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            var app = Current;
            var logger = Logger;
            ApplicationExtensions.InitializeApplication(appName, app, logger);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            ApplicationExtensions.OnExit(Logger);
        }

    }
}
