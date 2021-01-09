namespace log4net.Filter
{
    using log4net.Core;
    using System;

    public interface IFilter : IOptionHandler
    {
        FilterDecision Decide(LoggingEvent loggingEvent);

        IFilter Next { get; set; }
    }
}

