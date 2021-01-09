namespace log4net.Layout.Pattern
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.IO;

    internal sealed class NdcPatternConverter : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            WriteObject(writer, loggingEvent.Repository, loggingEvent.LookupProperty("NDC"));
        }
    }
}

