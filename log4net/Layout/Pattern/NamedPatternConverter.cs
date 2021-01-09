namespace log4net.Layout.Pattern
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.IO;

    public abstract class NamedPatternConverter : PatternLayoutConverter, IOptionHandler
    {
        private int m_precision;
        private static readonly Type declaringType = typeof(NamedPatternConverter);
        private const string DOT = ".";

        protected NamedPatternConverter()
        {
        }

        public void ActivateOptions()
        {
            this.m_precision = 0;
            if (this.Option != null)
            {
                string s = this.Option.Trim();
                if (s.Length > 0)
                {
                    int num;
                    if (!SystemInfo.TryParse(s, out num))
                    {
                        LogLog.Error(declaringType, "NamedPatternConverter: Precision option \"" + s + "\" not a decimal integer.");
                    }
                    else if (num <= 0)
                    {
                        LogLog.Error(declaringType, "NamedPatternConverter: Precision option (" + s + ") isn't a positive integer.");
                    }
                    else
                    {
                        this.m_precision = num;
                    }
                }
            }
        }

        protected sealed override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            string fullyQualifiedName = this.GetFullyQualifiedName(loggingEvent);
            if ((this.m_precision <= 0) || ((fullyQualifiedName == null) || (fullyQualifiedName.Length < 2)))
            {
                writer.Write(fullyQualifiedName);
            }
            else
            {
                int length = fullyQualifiedName.Length;
                string str2 = string.Empty;
                if (fullyQualifiedName.EndsWith("."))
                {
                    str2 = ".";
                    fullyQualifiedName = fullyQualifiedName.Substring(0, length - 1);
                    length--;
                }
                int num2 = fullyQualifiedName.LastIndexOf(".");
                int num3 = 1;
                while (true)
                {
                    if ((num2 <= 0) || (num3 >= this.m_precision))
                    {
                        if (num2 == -1)
                        {
                            writer.Write(fullyQualifiedName + str2);
                        }
                        else
                        {
                            writer.Write(fullyQualifiedName.Substring(num2 + 1, (length - num2) - 1) + str2);
                        }
                        break;
                    }
                    num2 = fullyQualifiedName.LastIndexOf('.', num2 - 1);
                    num3++;
                }
            }
        }

        protected abstract string GetFullyQualifiedName(LoggingEvent loggingEvent);
    }
}

