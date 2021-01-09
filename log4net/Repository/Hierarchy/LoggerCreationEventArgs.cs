namespace log4net.Repository.Hierarchy
{
    using System;

    public class LoggerCreationEventArgs : EventArgs
    {
        private log4net.Repository.Hierarchy.Logger m_log;

        public LoggerCreationEventArgs(log4net.Repository.Hierarchy.Logger log)
        {
            this.m_log = log;
        }

        public log4net.Repository.Hierarchy.Logger Logger =>
            this.m_log;
    }
}

