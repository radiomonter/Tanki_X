namespace log4net.Core
{
    using System;

    public class TimeEvaluator : ITriggeringEventEvaluator
    {
        private int m_interval;
        private DateTime m_lasttime;
        private const int DEFAULT_INTERVAL = 0;

        public TimeEvaluator() : this(0)
        {
        }

        public TimeEvaluator(int interval)
        {
            this.m_interval = interval;
            this.m_lasttime = DateTime.Now;
        }

        public bool IsTriggeringEvent(LoggingEvent loggingEvent)
        {
            bool flag;
            if (loggingEvent == null)
            {
                throw new ArgumentNullException("loggingEvent");
            }
            if (this.m_interval == 0)
            {
                return false;
            }
            lock (this)
            {
                if (DateTime.Now.Subtract(this.m_lasttime).TotalSeconds <= this.m_interval)
                {
                    flag = false;
                }
                else
                {
                    this.m_lasttime = DateTime.Now;
                    flag = true;
                }
            }
            return flag;
        }

        public int Interval
        {
            get => 
                this.m_interval;
            set => 
                this.m_interval = value;
        }
    }
}

