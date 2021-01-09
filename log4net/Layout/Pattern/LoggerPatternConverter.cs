namespace log4net.Layout.Pattern
{
    using log4net.Core;
    using System;

    internal sealed class LoggerPatternConverter : NamedPatternConverter
    {
        protected override string GetFullyQualifiedName(LoggingEvent loggingEvent) => 
            loggingEvent.LoggerName;
    }
}

