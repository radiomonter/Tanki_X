namespace log4net.Util
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public sealed class LogLog
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static LogReceivedEventHandler LogReceived;
        private readonly Type source;
        private readonly DateTime timeStamp = DateTime.Now;
        private readonly string prefix;
        private readonly string message;
        private readonly System.Exception exception;
        private static bool s_debugEnabled;
        private static bool s_quietMode;
        private static bool s_emitInternalMessages = true;
        private const string PREFIX = "log4net: ";
        private const string ERR_PREFIX = "log4net:ERROR ";
        private const string WARN_PREFIX = "log4net:WARN ";

        public static event LogReceivedEventHandler LogReceived
        {
            add
            {
                LogReceivedEventHandler logReceived = LogReceived;
                while (true)
                {
                    LogReceivedEventHandler objB = logReceived;
                    logReceived = Interlocked.CompareExchange<LogReceivedEventHandler>(ref LogReceived, objB + value, logReceived);
                    if (ReferenceEquals(logReceived, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                LogReceivedEventHandler logReceived = LogReceived;
                while (true)
                {
                    LogReceivedEventHandler objB = logReceived;
                    logReceived = Interlocked.CompareExchange<LogReceivedEventHandler>(ref LogReceived, objB - value, logReceived);
                    if (ReferenceEquals(logReceived, objB))
                    {
                        return;
                    }
                }
            }
        }

        public LogLog(Type source, string prefix, string message, System.Exception exception)
        {
            this.source = source;
            this.prefix = prefix;
            this.message = message;
            this.exception = exception;
        }

        public static void Debug(Type source, string message)
        {
            if (IsDebugEnabled)
            {
                if (EmitInternalMessages)
                {
                    EmitOutLine("log4net: " + message);
                }
                OnLogReceived(source, "log4net: ", message, null);
            }
        }

        public static void Debug(Type source, string message, System.Exception exception)
        {
            if (IsDebugEnabled)
            {
                if (EmitInternalMessages)
                {
                    EmitOutLine("log4net: " + message);
                    if (exception != null)
                    {
                        EmitOutLine(exception.ToString());
                    }
                }
                OnLogReceived(source, "log4net: ", message, exception);
            }
        }

        private static void EmitErrorLine(string message)
        {
            try
            {
                Console.Error.WriteLine(message);
            }
            catch
            {
            }
        }

        private static void EmitOutLine(string message)
        {
            try
            {
                Console.Out.WriteLine(message);
            }
            catch
            {
            }
        }

        public static void Error(Type source, string message)
        {
            if (IsErrorEnabled)
            {
                if (EmitInternalMessages)
                {
                    EmitErrorLine("log4net:ERROR " + message);
                }
                OnLogReceived(source, "log4net:ERROR ", message, null);
            }
        }

        public static void Error(Type source, string message, System.Exception exception)
        {
            if (IsErrorEnabled)
            {
                if (EmitInternalMessages)
                {
                    EmitErrorLine("log4net:ERROR " + message);
                    if (exception != null)
                    {
                        EmitErrorLine(exception.ToString());
                    }
                }
                OnLogReceived(source, "log4net:ERROR ", message, exception);
            }
        }

        public static void OnLogReceived(Type source, string prefix, string message, System.Exception exception)
        {
            if (LogReceived != null)
            {
                LogReceived(null, new LogReceivedEventArgs(new LogLog(source, prefix, message, exception)));
            }
        }

        public override string ToString() => 
            this.Prefix + this.Source.Name + ": " + this.Message;

        public static void Warn(Type source, string message)
        {
            if (IsWarnEnabled)
            {
                if (EmitInternalMessages)
                {
                    EmitErrorLine("log4net:WARN " + message);
                }
                OnLogReceived(source, "log4net:WARN ", message, null);
            }
        }

        public static void Warn(Type source, string message, System.Exception exception)
        {
            if (IsWarnEnabled)
            {
                if (EmitInternalMessages)
                {
                    EmitErrorLine("log4net:WARN " + message);
                    if (exception != null)
                    {
                        EmitErrorLine(exception.ToString());
                    }
                }
                OnLogReceived(source, "log4net:WARN ", message, exception);
            }
        }

        public Type Source =>
            this.source;

        public DateTime TimeStamp =>
            this.timeStamp;

        public string Prefix =>
            this.prefix;

        public string Message =>
            this.message;

        public System.Exception Exception =>
            this.exception;

        public static bool InternalDebugging
        {
            get => 
                s_debugEnabled;
            set => 
                s_debugEnabled = value;
        }

        public static bool QuietMode
        {
            get => 
                s_quietMode;
            set => 
                s_quietMode = value;
        }

        public static bool EmitInternalMessages
        {
            get => 
                s_emitInternalMessages;
            set => 
                s_emitInternalMessages = value;
        }

        public static bool IsDebugEnabled =>
            s_debugEnabled && !s_quietMode;

        public static bool IsWarnEnabled =>
            !s_quietMode;

        public static bool IsErrorEnabled =>
            !s_quietMode;

        public class LogReceivedAdapter : IDisposable
        {
            private readonly IList items;
            private readonly LogReceivedEventHandler handler;

            public LogReceivedAdapter(IList items)
            {
                this.items = items;
                this.handler = new LogReceivedEventHandler(this.LogLog_LogReceived);
                LogLog.LogReceived += this.handler;
            }

            public void Dispose()
            {
                LogLog.LogReceived -= this.handler;
            }

            private void LogLog_LogReceived(object source, LogReceivedEventArgs e)
            {
                this.items.Add(e.LogLog);
            }

            public IList Items =>
                this.items;
        }
    }
}

