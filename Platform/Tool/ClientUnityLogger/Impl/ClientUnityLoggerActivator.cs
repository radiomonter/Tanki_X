namespace Platform.Tool.ClientUnityLogger.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientLogger.API;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using System;
    using System.IO;
    using UnityEngine;

    public class ClientUnityLoggerActivator : DefaultActivator<AutoCompleting>
    {
        protected override void Activate()
        {
            if (!Application.isEditor)
            {
                InitLogger();
            }
        }

        public static void InitLogger()
        {
            string url = ConfigPath.ConvertToUrl(Application.streamingAssetsPath + "/" + ConfigPath.CLIENT_LOGGER_CONFIG);
            Console.WriteLine("Load client logger config: " + url);
            WWW www = new WWW(url);
            while (!www.isDone)
            {
            }
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError("Error loading logger config from: " + url + " Error: " + www.error);
            }
            else
            {
                LoggerProvider.LoadConfiguration(new MemoryStream(www.bytes));
            }
        }
    }
}

