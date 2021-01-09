namespace Platform.Tool.ClientUnityLogger.API
{
    using log4net.Core;
    using log4net.Layout.Pattern;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class SyslogLevelConverter : PatternLayoutConverter
    {
        private Dictionary<Level, SyslogSeverity> log4net2SyslogLevelMap;
        public const string KEY = "syslogLevel";

        public SyslogLevelConverter()
        {
            Dictionary<Level, SyslogSeverity> dictionary = new Dictionary<Level, SyslogSeverity> {
                { 
                    Level.Alert,
                    SyslogSeverity.Alert
                },
                { 
                    Level.Critical,
                    SyslogSeverity.Critical
                },
                { 
                    Level.Fatal,
                    SyslogSeverity.Critical
                },
                { 
                    Level.Debug,
                    SyslogSeverity.Debug
                },
                { 
                    Level.Emergency,
                    SyslogSeverity.Emergency
                },
                { 
                    Level.Error,
                    SyslogSeverity.Error
                },
                { 
                    Level.Info,
                    SyslogSeverity.Informational
                },
                { 
                    Level.Off,
                    SyslogSeverity.Informational
                },
                { 
                    Level.Notice,
                    SyslogSeverity.Notice
                },
                { 
                    Level.Verbose,
                    SyslogSeverity.Notice
                },
                { 
                    Level.Trace,
                    SyslogSeverity.Notice
                },
                { 
                    Level.Severe,
                    SyslogSeverity.Emergency
                },
                { 
                    Level.Warn,
                    SyslogSeverity.Warning
                }
            };
            this.log4net2SyslogLevelMap = dictionary;
        }

        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            writer.Write((int) this.GetSyslogSeverity(loggingEvent.Level));
        }

        private byte GetSyslogSeverity(Level level)
        {
            SyslogSeverity severity;
            return (!this.log4net2SyslogLevelMap.TryGetValue(level, out severity) ? 7 : ((byte) severity));
        }

        private enum SyslogSeverity : byte
        {
            Emergency = 0,
            Alert = 1,
            Critical = 2,
            Error = 3,
            Warning = 4,
            Notice = 5,
            Informational = 6,
            Debug = 7
        }
    }
}

