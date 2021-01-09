namespace Platform.Library.ClientUnityIntegration.Impl
{
    using log4net;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientLogger.API;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using UnityEngine;

    public class CrashReporter : DefaultActivator<AutoCompleting>
    {
        private const string OUTPUT_LOG_FILENAME = "output_log.txt";
        private const string ERROR_LOG_FILENAME = "error.log";
        private const string CRASH_DUMP_FILENAME = "crash.dmp";
        private const string REPORTED_FILENAME = "ReportedToServer.txt";
        private static ILog log;

        protected override void Activate()
        {
            SendCrashReports();
        }

        private static bool ContainsCrashReportFiles(string dirWithSeparator) => 
            (File.Exists(dirWithSeparator + "output_log.txt") && File.Exists(dirWithSeparator + "error.log")) && File.Exists(dirWithSeparator + "crash.dmp");

        private static bool ContainsReportedFile(string dirWithSeparator) => 
            File.Exists(dirWithSeparator + "ReportedToServer.txt");

        private static List<string> GetCrashReportDirs()
        {
            string path = Application.dataPath + "/..";
            try
            {
                return new List<string>(Directory.GetDirectories(path, "????-??-??_??????", SearchOption.TopDirectoryOnly));
            }
            catch (IOException exception)
            {
                GetLog().WarnFormat("GetCrashReportDirs {0}", exception);
                return new List<string>();
            }
        }

        private static ILog GetLog()
        {
            log ??= LoggerProvider.GetLogger(typeof(CrashReporter));
            return log;
        }

        private static bool IsNameFormattedWithDate(string dir, out DateTime parsedDate)
        {
            string[] formats = new string[] { "yyyy-MM-dd_HHmmss" };
            return DateTime.TryParseExact(new FileInfo(dir).Name, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate);
        }

        private static bool IsOutdated(DateTime dateTime) => 
            (DateTime.Now - dateTime).TotalDays >= 7.0;

        private static void ProcessDirectory(string dir)
        {
            try
            {
                DateTime time;
                GetLog().InfoFormat("Processing {0}", dir);
                string dirWithSeparator = dir + Path.DirectorySeparatorChar;
                if (!IsNameFormattedWithDate(dir, out time))
                {
                    GetLog().InfoFormat("Skip IsNameFormattedWithDate {0}", dir);
                }
                else if (IsOutdated(time))
                {
                    GetLog().InfoFormat("Skip IsOutdated {0}", dir);
                }
                else if (!ContainsCrashReportFiles(dirWithSeparator))
                {
                    GetLog().InfoFormat("Skip ContainsCrashReportFiles {0}", dir);
                }
                else if (ContainsReportedFile(dirWithSeparator))
                {
                    GetLog().InfoFormat("Skip ContainsReportedFile {0}", dir);
                }
                else
                {
                    Report(dirWithSeparator, time);
                }
            }
            catch (IOException exception)
            {
                GetLog().WarnFormat("ProcessDirectory {0}", exception);
            }
        }

        private static void Report(string dirWithSeparator, DateTime date)
        {
            string str = File.ReadAllText(dirWithSeparator + "output_log.txt", new UTF8Encoding(false, false));
            GetLog().ErrorFormat("CrashReport {0:yyyy-MM-dd HH:mm:ss} UTC\n{1}", date.ToUniversalTime(), str);
            File.Create(dirWithSeparator + "ReportedToServer.txt");
        }

        public static void SendCrashReports()
        {
            foreach (string str in GetCrashReportDirs())
            {
                ProcessDirectory(str);
            }
        }
    }
}

