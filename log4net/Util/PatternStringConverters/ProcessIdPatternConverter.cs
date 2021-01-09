namespace log4net.Util.PatternStringConverters
{
    using log4net.Util;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Security;

    internal sealed class ProcessIdPatternConverter : PatternConverter
    {
        private static readonly Type declaringType = typeof(ProcessIdPatternConverter);

        protected override void Convert(TextWriter writer, object state)
        {
            try
            {
                writer.Write(Process.GetCurrentProcess().Id);
            }
            catch (SecurityException)
            {
                LogLog.Debug(declaringType, "Security exception while trying to get current process id. Error Ignored.");
                writer.Write(SystemInfo.NotAvailableText);
            }
        }
    }
}

