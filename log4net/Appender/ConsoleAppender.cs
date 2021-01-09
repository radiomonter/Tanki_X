namespace log4net.Appender
{
    using log4net.Core;
    using log4net.Layout;
    using System;
    using System.Globalization;

    public class ConsoleAppender : AppenderSkeleton
    {
        public const string ConsoleOut = "Console.Out";
        public const string ConsoleError = "Console.Error";
        private bool m_writeToErrorStream;

        public ConsoleAppender()
        {
        }

        [Obsolete("Instead use the default constructor and set the Layout property")]
        public ConsoleAppender(ILayout layout) : this(layout, false)
        {
        }

        [Obsolete("Instead use the default constructor and set the Layout & Target properties")]
        public ConsoleAppender(ILayout layout, bool writeToErrorStream)
        {
            this.Layout = layout;
            this.m_writeToErrorStream = writeToErrorStream;
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (this.m_writeToErrorStream)
            {
                Console.Error.Write(base.RenderLoggingEvent(loggingEvent));
            }
            else
            {
                Console.Write(base.RenderLoggingEvent(loggingEvent));
            }
        }

        public virtual string Target
        {
            get => 
                !this.m_writeToErrorStream ? "Console.Out" : "Console.Error";
            set
            {
                string strB = value.Trim();
                this.m_writeToErrorStream = string.Compare("Console.Error", strB, true, CultureInfo.InvariantCulture) == 0;
            }
        }

        protected override bool RequiresLayout =>
            true;
    }
}

