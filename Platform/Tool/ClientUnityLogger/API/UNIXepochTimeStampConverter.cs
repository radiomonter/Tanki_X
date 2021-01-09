namespace Platform.Tool.ClientUnityLogger.API
{
    using log4net.Core;
    using log4net.Layout.Pattern;
    using System;
    using System.IO;

    public class UNIXepochTimeStampConverter : PatternLayoutConverter
    {
        public const string KEY = "UNIXepochTimeStamp";
        private static readonly DateTime UnixEpoch = new DateTime(0x7b2, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            TimeSpan span = loggingEvent.TimeStamp.ToUniversalTime().Subtract(UnixEpoch);
            writer.Write(span.TotalSeconds);
        }
    }
}

