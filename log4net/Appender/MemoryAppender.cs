namespace log4net.Appender
{
    using log4net.Core;
    using System;
    using System.Collections;

    public class MemoryAppender : AppenderSkeleton
    {
        protected ArrayList m_eventsList = new ArrayList();
        protected FixFlags m_fixFlags = FixFlags.All;

        protected override void Append(LoggingEvent loggingEvent)
        {
            loggingEvent.Fix = this.Fix;
            lock (this.m_eventsList.SyncRoot)
            {
                this.m_eventsList.Add(loggingEvent);
            }
        }

        public virtual void Clear()
        {
            lock (this.m_eventsList.SyncRoot)
            {
                this.m_eventsList.Clear();
            }
        }

        public virtual LoggingEvent[] GetEvents()
        {
            lock (this.m_eventsList.SyncRoot)
            {
                return (LoggingEvent[]) this.m_eventsList.ToArray(typeof(LoggingEvent));
            }
        }

        [Obsolete("Use Fix property")]
        public virtual bool OnlyFixPartialEventData
        {
            get => 
                this.Fix == FixFlags.Partial;
            set => 
                this.Fix = !value ? FixFlags.All : FixFlags.Partial;
        }

        public virtual FixFlags Fix
        {
            get => 
                this.m_fixFlags;
            set => 
                this.m_fixFlags = value;
        }
    }
}

