﻿namespace log4net.Layout.Pattern
{
    using log4net.Core;
    using System;
    using System.IO;

    internal sealed class MethodLocationPatternConverter : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            writer.Write(loggingEvent.LocationInformation.MethodName);
        }
    }
}

