using Aplikacia_Motion_Detect.Interfaces.Enums;
using Aplikacia_Motion_Detect.Interfaces.Extensions;
using Destructurama;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacia_Motion_Detect.Interfaces
{
    class LoggerInit
    {

        public static readonly DateTime DateStart = DateTime.Now;
        public static readonly Guid ApplicationId = Guid.NewGuid();
        public static readonly string RoamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.None);

        public static string ApplicationName { get; set; }

        public static string Version { get; set; }

        public static ILogger InitializeApplicationLogger(LoggingLevelSwitch loggingLevelSwitch, CultureInfo culture, CultureInfo uiCulture)
        {
            if (string.IsNullOrWhiteSpace(ApplicationName))
            {
                throw new ArgumentNullException(nameof(ApplicationName));
            }
            if (string.IsNullOrWhiteSpace(ApplicationName))
            {
                throw new ArgumentNullException(nameof(Version));
            }
            var logPathFoldere = Path.Combine(RoamingPath, ApplicationName);

            if (!Directory.Exists(logPathFoldere))
            {
                Directory.CreateDirectory(logPathFoldere);
            }

            var logPath = Path.Combine(logPathFoldere, ApplicationName + "-{Date}.log");

            loggingLevelSwitch.MinimumLevel = LogEventLevel.Debug;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(loggingLevelSwitch)
                .Enrich.WithThreadId()
                .Enrich.WithMachineName()
                .Enrich.WithExceptionDetails()
                .Enrich.WithProperty("ApplicationId", ApplicationId)
                .Enrich.WithProperty("Version", Version)
                .WriteTo.RollingFile(logPath, LogEventLevel.Verbose, retainedFileCountLimit: 60, outputTemplate: Constants.FileLogFormat)
                .WriteTo.Console()
                .Destructure.UsingAttributes()
                .Destructure.AsScalar<CultureInfo>()
                .CreateLogger();

            Log.Logger.Information(ApplicationEvents.ApplicationStarted,
                "Application started at {DateTime}, Version {Version:l}, Logging level: {level}, CurrentCulture: {CurrentCulture}, CurrentUICulture: {CurrentUICulture}",
                DateStart, Version, loggingLevelSwitch.MinimumLevel, culture, uiCulture);
            return Log.Logger;
        }
    }
}
