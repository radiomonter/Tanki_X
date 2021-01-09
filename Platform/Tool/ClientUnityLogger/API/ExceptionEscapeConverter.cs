namespace Platform.Tool.ClientUnityLogger.API
{
    using log4net.Core;
    using log4net.Layout.Pattern;
    using Platform.Tool.ClientUnityLogger.Impl;
    using System;
    using System.IO;

    public class ExceptionEscapeConverter : PatternLayoutConverter
    {
        public const string KEY = "escapedException";

        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            string exceptionString = loggingEvent.GetExceptionString();
            if (!string.IsNullOrEmpty(exceptionString))
            {
                writer.WriteLine(JsonUtil.ToJSONString(exceptionString));
            }
        }
    }
}

