namespace log4net.Core
{
    using System;

    public interface ITriggeringEventEvaluator
    {
        bool IsTriggeringEvent(LoggingEvent loggingEvent);
    }
}

