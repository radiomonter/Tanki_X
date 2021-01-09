namespace log4net.Layout.Pattern
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.IO;

    public abstract class PatternLayoutConverter : PatternConverter
    {
        private bool m_ignoresException = true;

        protected PatternLayoutConverter()
        {
        }

        protected abstract void Convert(TextWriter writer, LoggingEvent loggingEvent);
        protected override void Convert(TextWriter writer, object state)
        {
            LoggingEvent loggingEvent = state as LoggingEvent;
            if (loggingEvent == null)
            {
                throw new ArgumentException("state must be of type [" + typeof(LoggingEvent).FullName + "]", "state");
            }
            this.Convert(writer, loggingEvent);
        }

        public virtual bool IgnoresException
        {
            get => 
                this.m_ignoresException;
            set => 
                this.m_ignoresException = value;
        }
    }
}

