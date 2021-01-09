namespace log4net.Util
{
    using System;
    using System.Threading;

    public sealed class ReaderWriterLock
    {
        private ReaderWriterLock m_lock = new ReaderWriterLock();

        public void AcquireReaderLock()
        {
            this.m_lock.AcquireReaderLock(-1);
        }

        public void AcquireWriterLock()
        {
            this.m_lock.AcquireWriterLock(-1);
        }

        public void ReleaseReaderLock()
        {
            this.m_lock.ReleaseReaderLock();
        }

        public void ReleaseWriterLock()
        {
            this.m_lock.ReleaseWriterLock();
        }
    }
}

