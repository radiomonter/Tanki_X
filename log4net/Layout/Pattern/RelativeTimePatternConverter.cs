namespace log4net.Layout.Pattern
{
    using log4net.Core;
    using System;
    using System.Globalization;
    using System.IO;

    internal sealed class RelativeTimePatternConverter : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            writer.Write(TimeDifferenceInMillis(LoggingEvent.StartTime, loggingEvent.TimeStamp).ToString(NumberFormatInfo.InvariantInfo));
        }

        private static long TimeDifferenceInMillis(DateTime start, DateTime end) => 
            (long) (end.ToUniversalTime() - start.ToUniversalTime()).TotalMilliseconds;
    }
}

