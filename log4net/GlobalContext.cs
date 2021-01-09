namespace log4net
{
    using log4net.Util;
    using System;

    public sealed class GlobalContext
    {
        private static readonly GlobalContextProperties s_properties = new GlobalContextProperties();

        static GlobalContext()
        {
            Properties["log4net:HostName"] = SystemInfo.HostName;
        }

        private GlobalContext()
        {
        }

        public static GlobalContextProperties Properties =>
            s_properties;
    }
}

