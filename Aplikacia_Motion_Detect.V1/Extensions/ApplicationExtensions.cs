using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplikacia_Motion_Detect.Interfaces;
using Aplikacia_Motion_Detect.UI;
using System.Threading;
using Serilog;
using Aplikacia_Motion_Detect.Interfaces.Enums;
using GalaSoft.MvvmLight.Threading;
using Aplikacia_Motion_Detect.Interfaces.Extensions;
using System.Windows.Threading;
using System.Windows;

namespace Aplikacia_Motion_Detect.V1.Extensions
{
    class ApplicationExtensions
    {

        public static void InitializeApplication(string appName, Application app, ILogger logger)
        {
            LoggerInit.ApplicationName = appName.Replace(" ", string.Empty);
            //LoggerExtensions.DiagnosticsFunc = ErrorExtensions.DiagnosticsFunc;
            //var dbLogger = LoggerInitializer.InitializeDatabaseLogger(assembly);
            //var dbInitialized = DatabaseInitializer.Initialize(ViewModelLocator.DatabaseService, ViewModelLocator.DialogService, dbLogger);
            //if (!dbInitialized)
            //{
            //    app.Shutdown(3);
            //}
            ViewModelLocator.LoggingLevelSwitch.MinimumLevel = LogEventLevel.Information;
            //var applicationDeployment = ApplicationDeployment.IsNetworkDeployed ? ApplicationDeployment.CurrentDeployment : null;
            var currentCulture = Thread.CurrentThread.CurrentCulture;
            var currentUICulture = Thread.CurrentThread.CurrentUICulture;
            Log.Logger = LoggerInit.InitializeApplicationLogger(ViewModelLocator.LoggingLevelSwitch, currentCulture, currentUICulture);
            logger.Debug(ApplicationEvents.DispatcherThread, "Dispatcher thread: {ThreadId}", DispatcherHelper.UIDispatcher.Thread.ManagedThreadId);
            var errorsTimer = new DispatcherTimer(Constants.ErrorCountInterval, DispatcherPriority.ApplicationIdle, (sender, args) => Interfaces.Extensions.LoggerExtensions.SendErrorsWarningMessage(logger), DispatcherHelper.UIDispatcher);
            errorsTimer.Start();

        }

        public static void OnExit(ILogger logger)
        {
            logger.Information(ApplicationEvents.ApplicationEnded, "Application ended at {DateTime}", DateTime.Now);
            ((IDisposable)Log.Logger).Dispose();
        }
    }
}
