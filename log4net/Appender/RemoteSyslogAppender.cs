namespace log4net.Appender
{
    using log4net.Core;
    using log4net.Layout;
    using log4net.Util;
    using System;
    using System.Net;
    using System.Text;

    public class RemoteSyslogAppender : UdpAppender
    {
        private const int DefaultSyslogPort = 0x202;
        private SyslogFacility m_facility = SyslogFacility.User;
        private PatternLayout m_identity;
        private LevelMapping m_levelMapping = new LevelMapping();
        private const int c_renderBufferSize = 0x100;
        private const int c_renderBufferMaxCapacity = 0x400;

        public RemoteSyslogAppender()
        {
            base.RemotePort = 0x202;
            base.RemoteAddress = IPAddress.Parse("127.0.0.1");
            base.Encoding = Encoding.ASCII;
        }

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            this.m_levelMapping.ActivateOptions();
        }

        public void AddMapping(LevelSeverity mapping)
        {
            this.m_levelMapping.Add(mapping);
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            try
            {
                byte[] buffer;
                int num = GeneratePriority(this.m_facility, this.GetSeverity(loggingEvent.Level));
                string str = (this.m_identity == null) ? loggingEvent.Domain : this.m_identity.Format(loggingEvent);
                string str2 = base.RenderLoggingEvent(loggingEvent);
                int num2 = 0;
                StringBuilder builder = new StringBuilder();
                goto TR_000F;
            TR_0002:
                buffer = base.Encoding.GetBytes(builder.ToString());
                base.Client.Send(buffer, buffer.Length, base.RemoteEndPoint);
            TR_000F:
                while (true)
                {
                    if (num2 < str2.Length)
                    {
                        builder.Length = 0;
                        builder.Append('<');
                        builder.Append(num);
                        builder.Append('>');
                        builder.Append(str);
                        builder.Append(": ");
                        while (num2 < str2.Length)
                        {
                            char ch = str2[num2];
                            if ((ch >= ' ') && (ch <= '~'))
                            {
                                builder.Append(ch);
                            }
                            else if ((ch == '\r') || (ch == '\n'))
                            {
                                if ((str2.Length > (num2 + 1)) && ((str2[num2 + 1] == '\r') || (str2[num2 + 1] == '\n')))
                                {
                                    num2++;
                                }
                                num2++;
                                break;
                            }
                            num2++;
                        }
                    }
                    else
                    {
                        return;
                    }
                    break;
                }
                goto TR_0002;
            }
            catch (Exception exception)
            {
                object[] objArray1 = new object[] { "Unable to send logging event to remote syslog ", base.RemoteAddress.ToString(), " on port ", base.RemotePort, "." };
                this.ErrorHandler.Error(string.Concat(objArray1), exception, ErrorCode.WriteFailure);
            }
        }

        public static int GeneratePriority(SyslogFacility facility, SyslogSeverity severity)
        {
            if ((facility < SyslogFacility.Kernel) || (facility > SyslogFacility.Local7))
            {
                throw new ArgumentException("SyslogFacility out of range", "facility");
            }
            if ((severity < SyslogSeverity.Emergency) || (severity > SyslogSeverity.Debug))
            {
                throw new ArgumentException("SyslogSeverity out of range", "severity");
            }
            return (int) ((facility * SyslogFacility.Uucp) + ((SyslogFacility) ((int) severity)));
        }

        protected virtual SyslogSeverity GetSeverity(Level level)
        {
            LevelSeverity severity = this.m_levelMapping.Lookup(level) as LevelSeverity;
            return ((severity == null) ? ((level < Level.Alert) ? ((level < Level.Critical) ? ((level < Level.Error) ? ((level < Level.Warn) ? ((level < Level.Notice) ? ((level < Level.Info) ? SyslogSeverity.Debug : SyslogSeverity.Informational) : SyslogSeverity.Notice) : SyslogSeverity.Warning) : SyslogSeverity.Error) : SyslogSeverity.Critical) : SyslogSeverity.Alert) : severity.Severity);
        }

        public PatternLayout Identity
        {
            get => 
                this.m_identity;
            set => 
                this.m_identity = value;
        }

        public SyslogFacility Facility
        {
            get => 
                this.m_facility;
            set => 
                this.m_facility = value;
        }

        public class LevelSeverity : LevelMappingEntry
        {
            private RemoteSyslogAppender.SyslogSeverity m_severity;

            public RemoteSyslogAppender.SyslogSeverity Severity
            {
                get => 
                    this.m_severity;
                set => 
                    this.m_severity = value;
            }
        }

        public enum SyslogFacility
        {
            Kernel,
            User,
            Mail,
            Daemons,
            Authorization,
            Syslog,
            Printer,
            News,
            Uucp,
            Clock,
            Authorization2,
            Ftp,
            Ntp,
            Audit,
            Alert,
            Clock2,
            Local0,
            Local1,
            Local2,
            Local3,
            Local4,
            Local5,
            Local6,
            Local7
        }

        public enum SyslogSeverity
        {
            Emergency,
            Alert,
            Critical,
            Error,
            Warning,
            Notice,
            Informational,
            Debug
        }
    }
}

