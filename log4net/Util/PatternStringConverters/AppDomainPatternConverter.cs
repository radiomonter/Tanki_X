namespace log4net.Util.PatternStringConverters
{
    using log4net.Util;
    using System;
    using System.IO;

    internal sealed class AppDomainPatternConverter : PatternConverter
    {
        protected override void Convert(TextWriter writer, object state)
        {
            writer.Write(SystemInfo.ApplicationFriendlyName);
        }
    }
}

