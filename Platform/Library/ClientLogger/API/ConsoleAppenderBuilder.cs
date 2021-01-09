namespace Platform.Library.ClientLogger.API
{
    using log4net.Appender;
    using Platform.Library.ClientLogger.Impl;
    using System;

    public class ConsoleAppenderBuilder : AppenderBuilder
    {
        public ConsoleAppenderBuilder()
        {
            base.Init(new ConsoleAppender());
        }
    }
}

