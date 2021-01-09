namespace Platform.Library.ClientLogger.API
{
    using log4net;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;

    public class BaseTestLogger
    {
        public BaseTestLogger()
        {
            LoggerProvider.Init();
        }

        [Inject]
        public static ILog Log { get; set; }
    }
}

