namespace log4net.Layout
{
    using log4net.Core;
    using System;
    using System.Globalization;
    using System.IO;

    public class Layout2RawLayoutAdapter : IRawLayout
    {
        private ILayout m_layout;

        public Layout2RawLayoutAdapter(ILayout layout)
        {
            this.m_layout = layout;
        }

        public virtual object Format(LoggingEvent loggingEvent)
        {
            StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
            this.m_layout.Format(writer, loggingEvent);
            return writer.ToString();
        }
    }
}

