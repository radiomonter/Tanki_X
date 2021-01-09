namespace Platform.Tool.ClientUnityLogger.API
{
    using log4net.Util;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.IO;

    public class ServerUrlConverter : PatternConverter
    {
        public const string KEY = "InitUrl";

        protected override void Convert(TextWriter writer, object state)
        {
            if (StartupConfiguration.Config != null)
            {
                writer.Write(StartupConfiguration.Config.InitUrl);
            }
        }
    }
}

