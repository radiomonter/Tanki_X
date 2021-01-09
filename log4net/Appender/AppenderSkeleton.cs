namespace log4net.Appender
{
    using log4net.Core;
    using log4net.Filter;
    using log4net.Layout;
    using log4net.Util;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;

    public abstract class AppenderSkeleton : IAppender, IBulkAppender, IOptionHandler
    {
        private ILayout m_layout;
        private string m_name;
        private Level m_threshold;
        private IErrorHandler m_errorHandler;
        private IFilter m_headFilter;
        private IFilter m_tailFilter;
        private bool m_closed;
        private bool m_recursiveGuard;
        private ReusableStringWriter m_renderWriter;
        private const int c_renderBufferSize = 0x100;
        private const int c_renderBufferMaxCapacity = 0x400;
        private static readonly Type declaringType = typeof(AppenderSkeleton);

        protected AppenderSkeleton()
        {
            this.m_errorHandler = new OnlyOnceErrorHandler(base.GetType().Name);
        }

        public virtual void ActivateOptions()
        {
        }

        public virtual void AddFilter(IFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter param must not be null");
            }
            if (this.m_headFilter == null)
            {
                this.m_headFilter = this.m_tailFilter = filter;
            }
            else
            {
                this.m_tailFilter.Next = filter;
                this.m_tailFilter = filter;
            }
        }

        protected abstract void Append(LoggingEvent loggingEvent);
        protected virtual void Append(LoggingEvent[] loggingEvents)
        {
            foreach (LoggingEvent event2 in loggingEvents)
            {
                this.Append(event2);
            }
        }

        public virtual void ClearFilters()
        {
            IFilter filter;
            this.m_tailFilter = (IFilter) (filter = null);
            this.m_headFilter = filter;
        }

        public void Close()
        {
            lock (this)
            {
                if (!this.m_closed)
                {
                    this.OnClose();
                    this.m_closed = true;
                }
            }
        }

        public void DoAppend(LoggingEvent loggingEvent)
        {
            lock (this)
            {
                if (this.m_closed)
                {
                    this.ErrorHandler.Error("Attempted to append to closed appender named [" + this.m_name + "].");
                }
                else if (!this.m_recursiveGuard)
                {
                    try
                    {
                        this.m_recursiveGuard = true;
                        if (this.FilterEvent(loggingEvent) && this.PreAppendCheck())
                        {
                            this.Append(loggingEvent);
                        }
                    }
                    catch (Exception exception)
                    {
                        this.ErrorHandler.Error("Failed in DoAppend", exception);
                    }
                    finally
                    {
                        this.m_recursiveGuard = false;
                    }
                }
            }
        }

        public void DoAppend(LoggingEvent[] loggingEvents)
        {
            lock (this)
            {
                if (this.m_closed)
                {
                    this.ErrorHandler.Error("Attempted to append to closed appender named [" + this.m_name + "].");
                }
                else if (!this.m_recursiveGuard)
                {
                    try
                    {
                        this.m_recursiveGuard = true;
                        ArrayList list = new ArrayList(loggingEvents.Length);
                        LoggingEvent[] eventArray = loggingEvents;
                        int index = 0;
                        while (true)
                        {
                            if (index >= eventArray.Length)
                            {
                                if ((list.Count > 0) && this.PreAppendCheck())
                                {
                                    this.Append((LoggingEvent[]) list.ToArray(typeof(LoggingEvent)));
                                }
                                break;
                            }
                            LoggingEvent loggingEvent = eventArray[index];
                            if (this.FilterEvent(loggingEvent))
                            {
                                list.Add(loggingEvent);
                            }
                            index++;
                        }
                    }
                    catch (Exception exception)
                    {
                        this.ErrorHandler.Error("Failed in Bulk DoAppend", exception);
                    }
                    finally
                    {
                        this.m_recursiveGuard = false;
                    }
                }
            }
        }

        protected virtual bool FilterEvent(LoggingEvent loggingEvent)
        {
            if (!this.IsAsSevereAsThreshold(loggingEvent.Level))
            {
                return false;
            }
            IFilter filterHead = this.FilterHead;
            while (filterHead != null)
            {
                FilterDecision decision = filterHead.Decide(loggingEvent);
                if (decision == FilterDecision.Deny)
                {
                    return false;
                }
                if (decision == FilterDecision.Accept)
                {
                    filterHead = null;
                    continue;
                }
                if (decision == FilterDecision.Neutral)
                {
                    filterHead = filterHead.Next;
                }
            }
            return true;
        }

        ~AppenderSkeleton()
        {
            if (!this.m_closed)
            {
                LogLog.Debug(declaringType, "Finalizing appender named [" + this.m_name + "].");
                this.Close();
            }
        }

        protected virtual bool IsAsSevereAsThreshold(Level level) => 
            (this.m_threshold == null) || (level >= this.m_threshold);

        protected virtual void OnClose()
        {
        }

        protected virtual bool PreAppendCheck()
        {
            if ((this.m_layout != null) || !this.RequiresLayout)
            {
                return true;
            }
            this.ErrorHandler.Error("AppenderSkeleton: No layout set for the appender named [" + this.m_name + "].");
            return false;
        }

        protected string RenderLoggingEvent(LoggingEvent loggingEvent)
        {
            this.m_renderWriter ??= new ReusableStringWriter(CultureInfo.InvariantCulture);
            lock (this.m_renderWriter)
            {
                this.m_renderWriter.Reset(0x400, 0x100);
                this.RenderLoggingEvent(this.m_renderWriter, loggingEvent);
                return this.m_renderWriter.ToString();
            }
        }

        protected void RenderLoggingEvent(TextWriter writer, LoggingEvent loggingEvent)
        {
            if (this.m_layout == null)
            {
                throw new InvalidOperationException("A layout must be set");
            }
            if (!this.m_layout.IgnoresException)
            {
                this.m_layout.Format(writer, loggingEvent);
            }
            else
            {
                string exceptionString = loggingEvent.GetExceptionString();
                if ((exceptionString == null) || (exceptionString.Length <= 0))
                {
                    this.m_layout.Format(writer, loggingEvent);
                }
                else
                {
                    this.m_layout.Format(writer, loggingEvent);
                    writer.WriteLine(exceptionString);
                }
            }
        }

        public Level Threshold
        {
            get => 
                this.m_threshold;
            set => 
                this.m_threshold = value;
        }

        public virtual IErrorHandler ErrorHandler
        {
            get => 
                this.m_errorHandler;
            set
            {
                lock (this)
                {
                    if (value == null)
                    {
                        LogLog.Warn(declaringType, "You have tried to set a null error-handler.");
                    }
                    else
                    {
                        this.m_errorHandler = value;
                    }
                }
            }
        }

        public virtual IFilter FilterHead =>
            this.m_headFilter;

        public virtual ILayout Layout
        {
            get => 
                this.m_layout;
            set => 
                this.m_layout = value;
        }

        public string Name
        {
            get => 
                this.m_name;
            set => 
                this.m_name = value;
        }

        protected virtual bool RequiresLayout =>
            false;
    }
}

