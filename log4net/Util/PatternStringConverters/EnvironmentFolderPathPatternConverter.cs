namespace log4net.Util.PatternStringConverters
{
    using log4net.Util;
    using System;
    using System.IO;
    using System.Security;

    internal sealed class EnvironmentFolderPathPatternConverter : PatternConverter
    {
        private static readonly Type declaringType = typeof(EnvironmentFolderPathPatternConverter);

        protected override void Convert(TextWriter writer, object state)
        {
            try
            {
                if ((this.Option != null) && (this.Option.Length > 0))
                {
                    string folderPath = Environment.GetFolderPath((Environment.SpecialFolder) Enum.Parse(typeof(Environment.SpecialFolder), this.Option, true));
                    if ((folderPath != null) && (folderPath.Length > 0))
                    {
                        writer.Write(folderPath);
                    }
                }
            }
            catch (SecurityException exception)
            {
                LogLog.Debug(declaringType, "Security exception while trying to expand environment variables. Error Ignored. No Expansion.", exception);
            }
            catch (Exception exception2)
            {
                LogLog.Error(declaringType, "Error occurred while converting environment variable.", exception2);
            }
        }
    }
}

