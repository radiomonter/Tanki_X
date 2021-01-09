namespace log4net.Layout.Pattern
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.IO;

    internal class UtcDatePatternConverter : DatePatternConverter
    {
        private static readonly Type declaringType = typeof(UtcDatePatternConverter);

        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            try
            {
                base.m_dateFormatter.FormatDate(loggingEvent.TimeStamp.ToUniversalTime(), writer);
            }
            catch (Exception exception)
            {
                LogLog.Error(declaringType, "Error occurred while converting date.", exception);
            }
        }
    }
}

