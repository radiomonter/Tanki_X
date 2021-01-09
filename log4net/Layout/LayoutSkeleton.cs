namespace log4net.Layout
{
    using log4net.Core;
    using System;
    using System.Globalization;
    using System.IO;

    public abstract class LayoutSkeleton : ILayout, IOptionHandler
    {
        private string m_header;
        private string m_footer;
        private bool m_ignoresException = true;

        protected LayoutSkeleton()
        {
        }

        public abstract void ActivateOptions();
        public string Format(LoggingEvent loggingEvent)
        {
            StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
            this.Format(writer, loggingEvent);
            return writer.ToString();
        }

        public abstract void Format(TextWriter writer, LoggingEvent loggingEvent);

        public virtual string ContentType =>
            "text/plain";

        public virtual string Header
        {
            get => 
                this.m_header;
            set => 
                this.m_header = value;
        }

        public virtual string Footer
        {
            get => 
                this.m_footer;
            set => 
                this.m_footer = value;
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

