﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacia_Motion_Detect.Interfaces
{
    public class Constants
    {
        public const string NotAvailable = @"N\A";
        public const string FileLogFormat = @"{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {MachineName}:{ThreadId} [{Level}] {Message}{NewLine}{Exception}";
        public const string EmailLogFormat = @"{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {MachineName}:{ThreadId} [{Level}] {Message}{NewLine}{Exception}{NewLine}{@Diagnostics}";
        public const string ReceivedMessageFormat = @"Message: {@message} Target: {target}";
        public const string DateTimeFormatService = "yyyy-MM-dd HH:mm:ss";
        public const int MinimalErrorCountToWarning = 10;
        public static TimeSpan ErrorCountInterval = TimeSpan.FromMinutes(1);
        public static TimeSpan UpdateInterval = TimeSpan.FromMinutes(30);


    }
}

