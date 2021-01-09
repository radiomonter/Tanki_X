namespace log4net.Appender
{
    using log4net.Core;
    using log4net.Util;
    using System;

    public class BufferingForwardingAppender : BufferingAppenderSkeleton, IAppenderAttachable
    {
        private AppenderAttachedImpl m_appenderAttachedImpl;

        public virtual void AddAppender(IAppender newAppender)
        {
            if (newAppender == null)
            {
                throw new ArgumentNullException("newAppender");
            }
            lock (this)
            {
                this.m_appenderAttachedImpl ??= new AppenderAttachedImpl();
                this.m_appenderAttachedImpl.AddAppender(newAppender);
            }
        }

        public virtual IAppender GetAppender(string name)
        {
            lock (this)
            {
                return (((this.m_appenderAttachedImpl == null) || (name == null)) ? null : this.m_appenderAttachedImpl.GetAppender(name));
            }
        }

        protected override void OnClose()
        {
            lock (this)
            {
                base.OnClose();
                if (this.m_appenderAttachedImpl != null)
                {
                    this.m_appenderAttachedImpl.RemoveAllAppenders();
                }
            }
        }

        public virtual void RemoveAllAppenders()
        {
            lock (this)
            {
                if (this.m_appenderAttachedImpl != null)
                {
                    this.m_appenderAttachedImpl.RemoveAllAppenders();
                    this.m_appenderAttachedImpl = null;
                }
            }
        }

        public virtual IAppender RemoveAppender(IAppender appender)
        {
            lock (this)
            {
                if ((appender != null) && (this.m_appenderAttachedImpl != null))
                {
                    return this.m_appenderAttachedImpl.RemoveAppender(appender);
                }
            }
            return null;
        }

        public virtual IAppender RemoveAppender(string name)
        {
            lock (this)
            {
                if ((name != null) && (this.m_appenderAttachedImpl != null))
                {
                    return this.m_appenderAttachedImpl.RemoveAppender(name);
                }
            }
            return null;
        }

        protected override void SendBuffer(LoggingEvent[] events)
        {
            if (this.m_appenderAttachedImpl != null)
            {
                this.m_appenderAttachedImpl.AppendLoopOnAppenders(events);
            }
        }

        public virtual AppenderCollection Appenders
        {
            get
            {
                lock (this)
                {
                    return ((this.m_appenderAttachedImpl != null) ? this.m_appenderAttachedImpl.Appenders : AppenderCollection.EmptyCollection);
                }
            }
        }
    }
}

