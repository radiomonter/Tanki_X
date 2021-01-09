namespace Platform.Tool.ClientUnityLogger.API
{
    using log4net.Util;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.IO;

    public class UserUIDConverter : PatternConverter
    {
        public const string KEY = "UserUID";

        protected override void Convert(TextWriter writer, object state)
        {
            writer.Write(ECStoLoggerGateway.UserUID);
        }
    }
}

