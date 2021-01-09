namespace log4net.Appender
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.Collections;

    public abstract class BufferingAppenderSkeleton : AppenderSkeleton
    {
        private const int DEFAULT_BUFFER_SIZE = 0x200;
        private int m_bufferSize;
        private CyclicBuffer m_cb;
        private ITriggeringEventEvaluator m_evaluator;
        private bool m_lossy;
        private ITriggeringEventEvaluator m_lossyEvaluator;
        private FixFlags m_fixFlags;
        private readonly bool m_eventMustBeFixed;

        protected BufferingAppenderSkeleton() : this(true)
        {
        }

        protected BufferingAppenderSkeleton(bool eventMustBeFixed)
        {
            this.m_bufferSize = 0x200;
            this.m_fixFlags = FixFlags.All;
            this.m_eventMustBeFixed = eventMustBeFixed;
        }

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            if (this.m_lossy && (this.m_evaluator == null))
            {
                this.ErrorHandler.Error("Appender [" + base.Name + "] is Lossy but has no Evaluator. The buffer will never be sent!");
            }
            this.m_cb = (this.m_bufferSize <= 1) ? null : new CyclicBuffer(this.m_bufferSize);
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if ((this.m_cb == null) || (this.m_bufferSize <= 1))
            {
                if ((!this.m_lossy || ((this.m_evaluator != null) && this.m_evaluator.IsTriggeringEvent(loggingEvent))) || ((this.m_lossyEvaluator != null) && this.m_lossyEvaluator.IsTriggeringEvent(loggingEvent)))
                {
                    if (this.m_eventMustBeFixed)
                    {
                        loggingEvent.Fix = this.Fix;
                    }
                    LoggingEvent[] events = new LoggingEvent[] { loggingEvent };
                    this.SendBuffer(events);
                }
            }
            else
            {
                loggingEvent.Fix = this.Fix;
                LoggingEvent firstLoggingEvent = this.m_cb.Append(loggingEvent);
                if (firstLoggingEvent == null)
                {
                    if ((this.m_evaluator != null) && this.m_evaluator.IsTriggeringEvent(loggingEvent))
                    {
                        this.SendFromBuffer(null, this.m_cb);
                    }
                }
                else if (!this.m_lossy)
                {
                    this.SendFromBuffer(firstLoggingEvent, this.m_cb);
                }
                else
                {
                    if ((this.m_lossyEvaluator == null) || !this.m_lossyEvaluator.IsTriggeringEvent(firstLoggingEvent))
                    {
                        firstLoggingEvent = null;
                    }
                    if ((this.m_evaluator != null) && this.m_evaluator.IsTriggeringEvent(loggingEvent))
                    {
                        this.SendFromBuffer(firstLoggingEvent, this.m_cb);
                    }
                    else if (firstLoggingEvent != null)
                    {
                        LoggingEvent[] events = new LoggingEvent[] { firstLoggingEvent };
                        this.SendBuffer(events);
                    }
                }
            }
        }

        public virtual void Flush()
        {
            this.Flush(false);
        }

        public virtual void Flush(bool flushLossyBuffer)
        {
            lock (this)
            {
                if ((this.m_cb != null) && (this.m_cb.Length > 0))
                {
                    if (!this.m_lossy)
                    {
                        this.SendFromBuffer(null, this.m_cb);
                    }
                    else if (flushLossyBuffer)
                    {
                        if (this.m_lossyEvaluator == null)
                        {
                            this.m_cb.Clear();
                        }
                        else
                        {
                            LoggingEvent[] eventArray = this.m_cb.PopAll();
                            ArrayList list = new ArrayList(eventArray.Length);
                            LoggingEvent[] eventArray2 = eventArray;
                            int index = 0;
                            while (true)
                            {
                                if (index >= eventArray2.Length)
                                {
                                    if (list.Count > 0)
                                    {
                                        this.SendBuffer((LoggingEvent[]) list.ToArray(typeof(LoggingEvent)));
                                    }
                                    break;
                                }
                                LoggingEvent loggingEvent = eventArray2[index];
                                if (this.m_lossyEvaluator.IsTriggeringEvent(loggingEvent))
                                {
                                    list.Add(loggingEvent);
                                }
                                index++;
                            }
                        }
                    }
                }
            }
        }

        protected override void OnClose()
        {
            this.Flush(true);
        }

        protected abstract void SendBuffer(LoggingEvent[] events);
        protected virtual void SendFromBuffer(LoggingEvent firstLoggingEvent, CyclicBuffer buffer)
        {
            LoggingEvent[] events = buffer.PopAll();
            if (firstLoggingEvent == null)
            {
                this.SendBuffer(events);
            }
            else if (events.Length == 0)
            {
                LoggingEvent[] eventArray1 = new LoggingEvent[] { firstLoggingEvent };
                this.SendBuffer(eventArray1);
            }
            else
            {
                LoggingEvent[] destinationArray = new LoggingEvent[] { firstLoggingEvent };
                Array.Copy(events, 0, destinationArray, 1, events.Length);
                this.SendBuffer(destinationArray);
            }
        }

        public bool Lossy
        {
            get => 
                this.m_lossy;
            set => 
                this.m_lossy = value;
        }

        public int BufferSize
        {
            get => 
                this.m_bufferSize;
            set => 
                this.m_bufferSize = value;
        }

        public ITriggeringEventEvaluator Evaluator
        {
            get => 
                this.m_evaluator;
            set => 
                this.m_evaluator = value;
        }

        public ITriggeringEventEvaluator LossyEvaluator
        {
            get => 
                this.m_lossyEvaluator;
            set => 
                this.m_lossyEvaluator = value;
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

