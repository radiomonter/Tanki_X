namespace Platform.Tool.ClientUnityLogger.API
{
    using log4net.Util;
    using System;
    using System.IO;
    using UnityEngine;

    public class DeviceNameConverter : PatternConverter
    {
        public const string KEY = "deviceName";

        protected override void Convert(TextWriter writer, object state)
        {
            writer.Write(SystemInfo.deviceName);
        }
    }
}

