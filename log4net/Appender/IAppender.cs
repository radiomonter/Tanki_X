namespace log4net.Appender
{
    using log4net.Core;
    using System;

    public interface IAppender
    {
        void Close();
        void DoAppend(LoggingEvent loggingEvent);

        string Name { get; set; }
    }
}

