namespace Platform.Library.ClientLogger.API
{
    using log4net.Appender;
    using Platform.Library.ClientLogger.Impl;
    using System;
    using System.Runtime.InteropServices;

    public class FileAppenderBuilder : AppenderBuilder
    {
        public FileAppenderBuilder(string path, bool appendToFile = false)
        {
            FileAppender appender = new FileAppender {
                AppendToFile = true,
                LockingModel = new FileAppender.MinimalLock(),
                File = path
            };
            appender.AppendToFile = appendToFile;
            appender.ActivateOptions();
            base.Init(appender);
        }
    }
}

