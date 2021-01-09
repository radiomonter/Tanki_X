namespace Platform.Tool.ClientUnityLogger.API
{
    using log4net.Core;
    using log4net.Layout.Pattern;
    using Platform.Tool.ClientUnityLogger.Impl;
    using System;
    using System.IO;

    public class MessageEscapeConvertor : PatternLayoutConverter
    {
        public const string KEY = "escapedMessage";

        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            writer.Write(JsonUtil.ToJSONString(loggingEvent.MessageObject.ToString()));
        }
    }
}

