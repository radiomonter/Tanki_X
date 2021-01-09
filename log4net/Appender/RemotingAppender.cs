namespace log4net.Appender
{
    using log4net.Core;
    using System;
    using System.Collections;
    using System.Threading;

    public class RemotingAppender : BufferingAppenderSkeleton
    {
        private string m_sinkUrl;
        private IRemoteLoggingSink m_sinkObj;
        private int m_queuedCallbackCount;
        private ManualResetEvent m_workQueueEmptyEvent = new ManualResetEvent(true);

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            IDictionary state = new Hashtable {
                ["typeFilterLevel"] = "Full"
            };
            this.m_sinkObj = (IRemoteLoggingSink) Activator.GetObject(typeof(IRemoteLoggingSink), this.m_sinkUrl, state);
        }

        private void BeginAsyncSend()
        {
            this.m_workQueueEmptyEvent.Reset();
            Interlocked.Increment(ref this.m_queuedCallbackCount);
        }

        private void EndAsyncSend()
        {
            if (Interlocked.Decrement(ref this.m_queuedCallbackCount) <= 0)
            {
                this.m_workQueueEmptyEvent.Set();
            }
        }

        protected override void OnClose()
        {
            base.OnClose();
            if (!this.m_workQueueEmptyEvent.WaitOne(0x7530, false))
            {
                this.ErrorHandler.Error("RemotingAppender [" + base.Name + "] failed to send all queued events before close, in OnClose.");
            }
        }

        protected override void SendBuffer(LoggingEvent[] events)
        {
            this.BeginAsyncSend();
            if (!ThreadPool.QueueUserWorkItem(new WaitCallback(this.SendBufferCallback), events))
            {
                this.EndAsyncSend();
                this.ErrorHandler.Error("RemotingAppender [" + base.Name + "] failed to ThreadPool.QueueUserWorkItem logging events in SendBuffer.");
            }
        }

        private void SendBufferCallback(object state)
        {
            try
            {
                LoggingEvent[] events = (LoggingEvent[]) state;
                this.m_sinkObj.LogEvents(events);
            }
            catch (Exception exception)
            {
                this.ErrorHandler.Error("Failed in SendBufferCallback", exception);
            }
            finally
            {
                this.EndAsyncSend();
            }
        }

        public string Sink
        {
            get => 
                this.m_sinkUrl;
            set => 
                this.m_sinkUrl = value;
        }

        public interface IRemoteLoggingSink
        {
            void LogEvents(LoggingEvent[] events);
        }
    }
}

