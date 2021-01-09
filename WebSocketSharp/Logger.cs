namespace WebSocketSharp
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;

    public class Logger
    {
        private volatile string _file;
        private volatile LogLevel _level;
        private Action<LogData, string> _output;
        private object _sync;
        [CompilerGenerated]
        private static Action<LogData, string> <>f__mg$cache0;
        [CompilerGenerated]
        private static Action<LogData, string> <>f__mg$cache1;

        public Logger() : this(LogLevel.Error, null, null)
        {
        }

        public Logger(LogLevel level) : this(level, null, null)
        {
        }

        public Logger(LogLevel level, string file, Action<LogData, string> output)
        {
            this._level = level;
            this._file = file;
            Action<LogData, string> action1 = output;
            if (output == null)
            {
                Action<LogData, string> local1 = output;
                if (<>f__mg$cache0 == null)
                {
                    <>f__mg$cache0 = new Action<LogData, string>(Logger.defaultOutput);
                }
                action1 = <>f__mg$cache0;
            }
            this._output = action1;
            this._sync = new object();
        }

        public void Debug(string message)
        {
            if (this._level <= 1)
            {
                this.output(message, LogLevel.Debug);
            }
        }

        private static void defaultOutput(LogData data, string path)
        {
            string str = data.ToString();
            Console.WriteLine(str);
            if ((path != null) && (path.Length > 0))
            {
                writeToFile(str, path);
            }
        }

        public void Error(string message)
        {
            if (this._level <= 4)
            {
                this.output(message, LogLevel.Error);
            }
        }

        public void Fatal(string message)
        {
            this.output(message, LogLevel.Fatal);
        }

        public void Info(string message)
        {
            if (this._level <= 2)
            {
                this.output(message, LogLevel.Info);
            }
        }

        private void output(string message, LogLevel level)
        {
            lock (this._sync)
            {
                if (((LogLevel) this._level) <= level)
                {
                    LogData data = null;
                    try
                    {
                        data = new LogData(level, new StackFrame(2, true), message);
                        this._output(data, this._file);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(new LogData(LogLevel.Fatal, new StackFrame(0, true), exception.Message).ToString());
                    }
                }
            }
        }

        public void Trace(string message)
        {
            if (this._level <= 0)
            {
                this.output(message, LogLevel.Trace);
            }
        }

        public void Warn(string message)
        {
            if (this._level <= 3)
            {
                this.output(message, LogLevel.Warn);
            }
        }

        private static void writeToFile(string value, string path)
        {
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                using (TextWriter writer2 = TextWriter.Synchronized(writer))
                {
                    writer2.WriteLine(value);
                }
            }
        }

        public string File
        {
            get => 
                this._file;
            set
            {
                lock (this._sync)
                {
                    this._file = value;
                    this.Warn($"The current path to the log file has been changed to {this._file}.");
                }
            }
        }

        public LogLevel Level
        {
            get => 
                this._level;
            set
            {
                lock (this._sync)
                {
                    this._level = value;
                    this.Warn($"The current logging level has been changed to {(LogLevel) this._level}.");
                }
            }
        }

        public Action<LogData, string> Output
        {
            get => 
                this._output;
            set
            {
                lock (this._sync)
                {
                    Action<LogData, string> action1 = value;
                    if (value == null)
                    {
                        Action<LogData, string> local1 = value;
                        if (<>f__mg$cache1 == null)
                        {
                            <>f__mg$cache1 = new Action<LogData, string>(Logger.defaultOutput);
                        }
                        action1 = <>f__mg$cache1;
                    }
                    this._output = action1;
                    this.Warn("The current output action has been changed.");
                }
            }
        }
    }
}

