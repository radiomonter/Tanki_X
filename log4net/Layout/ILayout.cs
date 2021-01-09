namespace log4net.Layout
{
    using log4net.Core;
    using System;
    using System.IO;

    public interface ILayout
    {
        void Format(TextWriter writer, LoggingEvent loggingEvent);

        string ContentType { get; }

        string Header { get; }

        string Footer { get; }

        bool IgnoresException { get; }
    }
}

