namespace log4net.Util.PatternStringConverters
{
    using log4net.Util;
    using System;
    using System.IO;
    using System.Security;

    internal sealed class EnvironmentPatternConverter : PatternConverter
    {
        private static readonly Type declaringType = typeof(EnvironmentPatternConverter);

        protected override void Convert(TextWriter writer, object state)
        {
            try
            {
                if ((this.Option != null) && (this.Option.Length > 0))
                {
                    string environmentVariable = Environment.GetEnvironmentVariable(this.Option);
                    environmentVariable ??= Environment.GetEnvironmentVariable(this.Option, EnvironmentVariableTarget.User);
                    environmentVariable ??= Environment.GetEnvironmentVariable(this.Option, EnvironmentVariableTarget.Machine);
                    if ((environmentVariable != null) && (environmentVariable.Length > 0))
                    {
                        writer.Write(environmentVariable);
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

