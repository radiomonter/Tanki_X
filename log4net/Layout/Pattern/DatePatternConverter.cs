namespace log4net.Layout.Pattern
{
    using log4net.Core;
    using log4net.DateFormatter;
    using log4net.Util;
    using System;
    using System.Globalization;
    using System.IO;

    internal class DatePatternConverter : PatternLayoutConverter, IOptionHandler
    {
        protected IDateFormatter m_dateFormatter;
        private static readonly Type declaringType = typeof(DatePatternConverter);

        public void ActivateOptions()
        {
            string option = this.Option;
            option ??= "ISO8601";
            if (string.Compare(option, "ISO8601", true, CultureInfo.InvariantCulture) == 0)
            {
                this.m_dateFormatter = new Iso8601DateFormatter();
            }
            else if (string.Compare(option, "ABSOLUTE", true, CultureInfo.InvariantCulture) == 0)
            {
                this.m_dateFormatter = new AbsoluteTimeDateFormatter();
            }
            else if (string.Compare(option, "DATE", true, CultureInfo.InvariantCulture) == 0)
            {
                this.m_dateFormatter = new DateTimeDateFormatter();
            }
            else
            {
                try
                {
                    this.m_dateFormatter = new SimpleDateFormatter(option);
                }
                catch (Exception exception)
                {
                    LogLog.Error(declaringType, "Could not instantiate SimpleDateFormatter with [" + option + "]", exception);
                    this.m_dateFormatter = new Iso8601DateFormatter();
                }
            }
        }

        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            try
            {
                this.m_dateFormatter.FormatDate(loggingEvent.TimeStamp, writer);
            }
            catch (Exception exception)
            {
                LogLog.Error(declaringType, "Error occurred while converting date.", exception);
            }
        }
    }
}

