namespace log4net.Core
{
    using System;

    public abstract class LoggerWrapperImpl : ILoggerWrapper
    {
        private readonly ILogger m_logger;

        protected LoggerWrapperImpl(ILogger logger)
        {
            this.m_logger = logger;
        }

        public virtual ILogger Logger =>
            this.m_logger;
    }
}

