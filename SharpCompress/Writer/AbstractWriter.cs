namespace SharpCompress.Writer
{
    using SharpCompress.Common;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    public abstract class AbstractWriter : IWriter, IDisposable
    {
        private bool closeStream;
        private bool isDisposed;

        protected AbstractWriter(ArchiveType type)
        {
            this.WriterType = type;
        }

        public void Dispose()
        {
            if (!this.isDisposed)
            {
                GC.SuppressFinalize(this);
                this.Dispose(true);
                this.isDisposed = true;
            }
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing && this.closeStream)
            {
                this.OutputStream.Dispose();
            }
        }

        ~AbstractWriter()
        {
            if (!this.isDisposed)
            {
                this.Dispose(false);
                this.isDisposed = true;
            }
        }

        protected void InitalizeStream(Stream stream, bool closeStream)
        {
            this.OutputStream = stream;
            this.closeStream = closeStream;
        }

        public abstract void Write(string filename, Stream source, DateTime? modificationTime);

        protected Stream OutputStream { get; private set; }

        public ArchiveType WriterType { get; private set; }
    }
}

