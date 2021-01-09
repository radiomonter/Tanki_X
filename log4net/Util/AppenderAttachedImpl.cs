namespace log4net.Util
{
    using log4net.Appender;
    using log4net.Core;
    using System;

    public class AppenderAttachedImpl : IAppenderAttachable
    {
        private AppenderCollection m_appenderList;
        private IAppender[] m_appenderArray;
        private static readonly Type declaringType = typeof(AppenderAttachedImpl);

        public void AddAppender(IAppender newAppender)
        {
            if (newAppender == null)
            {
                throw new ArgumentNullException("newAppender");
            }
            this.m_appenderArray = null;
            this.m_appenderList ??= new AppenderCollection(1);
            if (!this.m_appenderList.Contains(newAppender))
            {
                this.m_appenderList.Add(newAppender);
            }
        }

        public int AppendLoopOnAppenders(LoggingEvent loggingEvent)
        {
            if (loggingEvent == null)
            {
                throw new ArgumentNullException("loggingEvent");
            }
            if (this.m_appenderList == null)
            {
                return 0;
            }
            this.m_appenderArray ??= this.m_appenderList.ToArray();
            foreach (IAppender appender in this.m_appenderArray)
            {
                try
                {
                    appender.DoAppend(loggingEvent);
                }
                catch (Exception exception)
                {
                    LogLog.Error(declaringType, "Failed to append to appender [" + appender.Name + "]", exception);
                }
            }
            return this.m_appenderList.Count;
        }

        public int AppendLoopOnAppenders(LoggingEvent[] loggingEvents)
        {
            if (loggingEvents == null)
            {
                throw new ArgumentNullException("loggingEvents");
            }
            if (loggingEvents.Length == 0)
            {
                throw new ArgumentException("loggingEvents array must not be empty", "loggingEvents");
            }
            if (loggingEvents.Length == 1)
            {
                return this.AppendLoopOnAppenders(loggingEvents[0]);
            }
            if (this.m_appenderList == null)
            {
                return 0;
            }
            this.m_appenderArray ??= this.m_appenderList.ToArray();
            foreach (IAppender appender in this.m_appenderArray)
            {
                try
                {
                    CallAppend(appender, loggingEvents);
                }
                catch (Exception exception)
                {
                    LogLog.Error(declaringType, "Failed to append to appender [" + appender.Name + "]", exception);
                }
            }
            return this.m_appenderList.Count;
        }

        private static void CallAppend(IAppender appender, LoggingEvent[] loggingEvents)
        {
            IBulkAppender appender2 = appender as IBulkAppender;
            if (appender2 != null)
            {
                appender2.DoAppend(loggingEvents);
            }
            else
            {
                foreach (LoggingEvent event2 in loggingEvents)
                {
                    appender.DoAppend(event2);
                }
            }
        }

        public IAppender GetAppender(string name)
        {
            if ((this.m_appenderList != null) && (name != null))
            {
                AppenderCollection.IAppenderCollectionEnumerator enumerator = this.m_appenderList.GetEnumerator();
                try
                {
                    while (true)
                    {
                        if (!enumerator.MoveNext())
                        {
                            break;
                        }
                        IAppender current = enumerator.Current;
                        if (name == current.Name)
                        {
                            return current;
                        }
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
            }
            return null;
        }

        public void RemoveAllAppenders()
        {
            if (this.m_appenderList != null)
            {
                AppenderCollection.IAppenderCollectionEnumerator enumerator = this.m_appenderList.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        IAppender current = enumerator.Current;
                        try
                        {
                            current.Close();
                        }
                        catch (Exception exception)
                        {
                            LogLog.Error(declaringType, "Failed to Close appender [" + current.Name + "]", exception);
                        }
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
                this.m_appenderList = null;
                this.m_appenderArray = null;
            }
        }

        public IAppender RemoveAppender(IAppender appender)
        {
            if ((appender != null) && (this.m_appenderList != null))
            {
                this.m_appenderList.Remove(appender);
                if (this.m_appenderList.Count == 0)
                {
                    this.m_appenderList = null;
                }
                this.m_appenderArray = null;
            }
            return appender;
        }

        public IAppender RemoveAppender(string name) => 
            this.RemoveAppender(this.GetAppender(name));

        public AppenderCollection Appenders =>
            (this.m_appenderList != null) ? AppenderCollection.ReadOnly(this.m_appenderList) : AppenderCollection.EmptyCollection;
    }
}

