namespace log4net.Core
{
    using log4net.Repository;
    using System;

    public class LoggerRepositoryCreationEventArgs : EventArgs
    {
        private ILoggerRepository m_repository;

        public LoggerRepositoryCreationEventArgs(ILoggerRepository repository)
        {
            this.m_repository = repository;
        }

        public ILoggerRepository LoggerRepository =>
            this.m_repository;
    }
}

