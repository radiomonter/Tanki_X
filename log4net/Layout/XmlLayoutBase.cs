namespace log4net.Layout
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.IO;
    using System.Xml;

    public abstract class XmlLayoutBase : LayoutSkeleton
    {
        private bool m_locationInfo;
        private string m_invalidCharReplacement;

        protected XmlLayoutBase() : this(false)
        {
            this.IgnoresException = false;
        }

        protected XmlLayoutBase(bool locationInfo)
        {
            this.m_invalidCharReplacement = "?";
            this.IgnoresException = false;
            this.m_locationInfo = locationInfo;
        }

        public override void ActivateOptions()
        {
        }

        public override void Format(TextWriter writer, LoggingEvent loggingEvent)
        {
            if (loggingEvent == null)
            {
                throw new ArgumentNullException("loggingEvent");
            }
            XmlTextWriter writer2 = new XmlTextWriter(new ProtectCloseTextWriter(writer)) {
                Formatting = Formatting.None,
                Namespaces = false
            };
            this.FormatXml(writer2, loggingEvent);
            writer2.WriteWhitespace(SystemInfo.NewLine);
            writer2.Close();
        }

        protected abstract void FormatXml(XmlWriter writer, LoggingEvent loggingEvent);

        public bool LocationInfo
        {
            get => 
                this.m_locationInfo;
            set => 
                this.m_locationInfo = value;
        }

        public string InvalidCharReplacement
        {
            get => 
                this.m_invalidCharReplacement;
            set => 
                this.m_invalidCharReplacement = value;
        }

        public override string ContentType =>
            "text/xml";
    }
}

