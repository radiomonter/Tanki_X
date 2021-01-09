namespace Platform.Tool.ClientUnityLogger.API
{
    using log4net.Util;
    using System;
    using System.IO;
    using UnityEngine;

    public class BuildGuidConverter : PatternConverter
    {
        public const string KEY = "buildGUID";

        protected override void Convert(TextWriter writer, object state)
        {
            writer.Write(Application.buildGUID);
        }
    }
}

