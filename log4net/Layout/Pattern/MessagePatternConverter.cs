namespace log4net.Layout.Pattern
{
    using log4net.Core;
    using System;
    using System.IO;

    internal sealed class MessagePatternConverter : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            loggingEvent.WriteRenderedMessage(writer);
        }
    }
}

