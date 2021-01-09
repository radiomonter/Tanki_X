namespace log4net.Layout
{
    using log4net.Core;
    using System;

    public class RawUtcTimeStampLayout : IRawLayout
    {
        public virtual object Format(LoggingEvent loggingEvent) => 
            loggingEvent.TimeStamp.ToUniversalTime();
    }
}

