namespace log4net.Util.PatternStringConverters
{
    using log4net.Util;
    using System;
    using System.IO;

    internal sealed class IdentityPatternConverter : PatternConverter
    {
        private static readonly Type declaringType = typeof(IdentityPatternConverter);

        protected override void Convert(TextWriter writer, object state)
        {
            writer.Write(SystemInfo.NotAvailableText);
        }
    }
}

