﻿namespace log4net.Layout
{
    using log4net.Core;
    using System;
    using System.IO;

    public class ExceptionLayout : LayoutSkeleton
    {
        public ExceptionLayout()
        {
            this.IgnoresException = false;
        }

        public override void ActivateOptions()
        {
        }

        public override void Format(TextWriter writer, LoggingEvent loggingEvent)
        {
            if (loggingEvent == null)
            {
                throw new ArgumentNullException("loggingEvent");
            }
            writer.Write(loggingEvent.GetExceptionString());
        }
    }
}

