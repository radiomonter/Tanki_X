namespace log4net.Layout
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.Collections;
    using System.Xml;

    public class XmlLayoutSchemaLog4j : XmlLayoutBase
    {
        private static readonly DateTime s_date1970 = new DateTime(0x7b2, 1, 1);

        public XmlLayoutSchemaLog4j()
        {
        }

        public XmlLayoutSchemaLog4j(bool locationInfo) : base(locationInfo)
        {
        }

        protected override void FormatXml(XmlWriter writer, LoggingEvent loggingEvent)
        {
            if ((loggingEvent.LookupProperty("log4net:HostName") != null) && (loggingEvent.LookupProperty("log4jmachinename") == null))
            {
                loggingEvent.GetProperties()["log4jmachinename"] = loggingEvent.LookupProperty("log4net:HostName");
            }
            if ((loggingEvent.LookupProperty("log4japp") == null) && ((loggingEvent.Domain != null) && (loggingEvent.Domain.Length > 0)))
            {
                loggingEvent.GetProperties()["log4japp"] = loggingEvent.Domain;
            }
            if ((loggingEvent.Identity != null) && ((loggingEvent.Identity.Length > 0) && (loggingEvent.LookupProperty("log4net:Identity") == null)))
            {
                loggingEvent.GetProperties()["log4net:Identity"] = loggingEvent.Identity;
            }
            if ((loggingEvent.UserName != null) && ((loggingEvent.UserName.Length > 0) && (loggingEvent.LookupProperty("log4net:UserName") == null)))
            {
                loggingEvent.GetProperties()["log4net:UserName"] = loggingEvent.UserName;
            }
            writer.WriteStartElement("log4j:event");
            writer.WriteAttributeString("logger", loggingEvent.LoggerName);
            TimeSpan span = loggingEvent.TimeStamp.ToUniversalTime() - s_date1970;
            writer.WriteAttributeString("timestamp", XmlConvert.ToString((long) span.TotalMilliseconds));
            writer.WriteAttributeString("level", loggingEvent.Level.DisplayName);
            writer.WriteAttributeString("thread", loggingEvent.ThreadName);
            writer.WriteStartElement("log4j:message");
            Transform.WriteEscapedXmlString(writer, loggingEvent.RenderedMessage, base.InvalidCharReplacement);
            writer.WriteEndElement();
            object property = loggingEvent.LookupProperty("NDC");
            if (property != null)
            {
                string textData = loggingEvent.Repository.RendererMap.FindAndRender(property);
                if ((textData != null) && (textData.Length > 0))
                {
                    writer.WriteStartElement("log4j:NDC");
                    Transform.WriteEscapedXmlString(writer, textData, base.InvalidCharReplacement);
                    writer.WriteEndElement();
                }
            }
            PropertiesDictionary properties = loggingEvent.GetProperties();
            if (properties.Count > 0)
            {
                writer.WriteStartElement("log4j:properties");
                IEnumerator enumerator = ((IEnumerable) properties).GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                        writer.WriteStartElement("log4j:data");
                        writer.WriteAttributeString("name", (string) current.Key);
                        string str2 = loggingEvent.Repository.RendererMap.FindAndRender(current.Value);
                        writer.WriteAttributeString("value", str2);
                        writer.WriteEndElement();
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
                writer.WriteEndElement();
            }
            string exceptionString = loggingEvent.GetExceptionString();
            if ((exceptionString != null) && (exceptionString.Length > 0))
            {
                writer.WriteStartElement("log4j:throwable");
                Transform.WriteEscapedXmlString(writer, exceptionString, base.InvalidCharReplacement);
                writer.WriteEndElement();
            }
            if (base.LocationInfo)
            {
                LocationInfo locationInformation = loggingEvent.LocationInformation;
                writer.WriteStartElement("log4j:locationInfo");
                writer.WriteAttributeString("class", locationInformation.ClassName);
                writer.WriteAttributeString("method", locationInformation.MethodName);
                writer.WriteAttributeString("file", locationInformation.FileName);
                writer.WriteAttributeString("line", locationInformation.LineNumber);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        public string Version
        {
            get => 
                "1.2";
            set
            {
                if (value != "1.2")
                {
                    throw new ArgumentException("Only version 1.2 of the log4j schema is currently supported");
                }
            }
        }
    }
}

