namespace log4net.Layout.Pattern
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.IO;

    internal class StackTracePatternConverter : PatternLayoutConverter, IOptionHandler
    {
        private int m_stackFrameLevel = 1;
        private static readonly Type declaringType = typeof(StackTracePatternConverter);

        public void ActivateOptions()
        {
            if (this.Option != null)
            {
                string s = this.Option.Trim();
                if (s.Length != 0)
                {
                    int num;
                    if (!SystemInfo.TryParse(s, out num))
                    {
                        LogLog.Error(declaringType, "StackTracePatternConverter: StackFrameLevel option \"" + s + "\" not a decimal integer.");
                    }
                    else if (num <= 0)
                    {
                        LogLog.Error(declaringType, "StackTracePatternConverter: StackeFrameLevel option (" + s + ") isn't a positive integer.");
                    }
                    else
                    {
                        this.m_stackFrameLevel = num;
                    }
                }
            }
        }

        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            StackFrameItem[] stackFrames = loggingEvent.LocationInformation.StackFrames;
            if ((stackFrames == null) || (stackFrames.Length <= 0))
            {
                LogLog.Error(declaringType, "loggingEvent.LocationInformation.StackFrames was null or empty.");
            }
            else
            {
                int index = this.m_stackFrameLevel - 1;
                while (index >= 0)
                {
                    if (index >= stackFrames.Length)
                    {
                        index--;
                        continue;
                    }
                    StackFrameItem item = stackFrames[index];
                    writer.Write("{0}.{1}", item.ClassName, this.GetMethodInformation(item.Method));
                    if (index > 0)
                    {
                        writer.Write(" > ");
                    }
                    index--;
                }
            }
        }

        internal virtual string GetMethodInformation(MethodItem method) => 
            method.Name;
    }
}

