namespace log4net.Core
{
    using log4net.Repository;
    using System;

    public interface ILogger
    {
        bool IsEnabledFor(Level level);
        void Log(LoggingEvent logEvent);
        void Log(Type callerStackBoundaryDeclaringType, Level level, object message, Exception exception);

        string Name { get; }

        ILoggerRepository Repository { get; }
    }
}

