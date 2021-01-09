namespace Platform.Tool.ClientUnityLogger.API
{
    using log4net.Util;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.IO;

    public class ECSSessionIdConverter : PatternConverter
    {
        public const string KEY = "ECSSessionId";

        protected override void Convert(TextWriter writer, object state)
        {
            writer.Write(ECStoLoggerGateway.ClientSessionId);
        }
    }
}

