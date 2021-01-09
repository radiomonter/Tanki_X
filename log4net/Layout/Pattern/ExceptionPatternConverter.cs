namespace log4net.Layout.Pattern
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.IO;

    internal sealed class ExceptionPatternConverter : PatternLayoutConverter
    {
        public ExceptionPatternConverter()
        {
            this.IgnoresException = false;
        }

        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            if ((loggingEvent.ExceptionObject == null) || ((this.Option == null) || (this.Option.Length <= 0)))
            {
                string exceptionString = loggingEvent.GetExceptionString();
                if ((exceptionString != null) && (exceptionString.Length > 0))
                {
                    writer.WriteLine(exceptionString);
                }
            }
            else
            {
                string str = this.Option.ToLower();
                if (str != null)
                {
                    if (str == "message")
                    {
                        WriteObject(writer, loggingEvent.Repository, loggingEvent.ExceptionObject.Message);
                    }
                    else if (str == "source")
                    {
                        WriteObject(writer, loggingEvent.Repository, loggingEvent.ExceptionObject.Source);
                    }
                    else if (str == "stacktrace")
                    {
                        WriteObject(writer, loggingEvent.Repository, loggingEvent.ExceptionObject.StackTrace);
                    }
                    else if (str == "targetsite")
                    {
                        WriteObject(writer, loggingEvent.Repository, loggingEvent.ExceptionObject.TargetSite);
                    }
                    else if (str == "helplink")
                    {
                        WriteObject(writer, loggingEvent.Repository, loggingEvent.ExceptionObject.HelpLink);
                    }
                }
            }
        }
    }
}

