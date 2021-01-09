namespace SharpCompress.Common
{
    using SharpCompress.IO;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    public abstract class Volume : IVolume, IDisposable
    {
        private readonly System.IO.Stream actualStream;
        private bool disposed;

        internal Volume(System.IO.Stream stream, SharpCompress.Common.Options options)
        {
            this.actualStream = stream;
            this.Options = options;
        }

        public void Dispose()
        {
            if (!this.Options.HasFlag(SharpCompress.Common.Options.KeepStreamsOpen) && !this.disposed)
            {
                this.actualStream.Dispose();
                this.disposed = true;
            }
        }

        internal System.IO.Stream Stream =>
            new NonDisposingStream(this.actualStream);

        internal SharpCompress.Common.Options Options { get; private set; }

        public abstract bool IsFirstVolume { get; }

        public abstract bool IsMultiVolume { get; }
    }
}

