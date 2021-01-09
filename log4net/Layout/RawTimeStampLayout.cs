namespace log4net.Layout
{
    using log4net.Core;
    using System;

    public class RawTimeStampLayout : IRawLayout
    {
        public virtual object Format(LoggingEvent loggingEvent) => 
            loggingEvent.TimeStamp;
    }
}

