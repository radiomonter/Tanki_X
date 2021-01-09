namespace Platform.Library.ClientUnityIntegration.API
{
    using System;
    using System.Runtime.CompilerServices;

    public class StartupConfiguration
    {
        public static StartupConfiguration Config { get; set; }

        public string InitUrl { get; set; }

        public string StateUrl { get; set; }

        public string CurrentClientVersion { get; set; }
    }
}

