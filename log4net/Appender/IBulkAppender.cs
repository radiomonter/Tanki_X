namespace log4net.Appender
{
    using log4net.Core;
    using System;

    public interface IBulkAppender : IAppender
    {
        void DoAppend(LoggingEvent[] loggingEvents);
    }
}

