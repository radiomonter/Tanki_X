namespace log4net.Util
{
    using System;

    public class LogReceivedEventArgs : EventArgs
    {
        private readonly log4net.Util.LogLog loglog;

        public LogReceivedEventArgs(log4net.Util.LogLog loglog)
        {
            this.loglog = loglog;
        }

        public log4net.Util.LogLog LogLog =>
            this.loglog;
    }
}

