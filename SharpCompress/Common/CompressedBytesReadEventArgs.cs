namespace SharpCompress.Common
{
    using System;
    using System.Runtime.CompilerServices;

    public class CompressedBytesReadEventArgs : EventArgs
    {
        public long CompressedBytesRead { get; internal set; }

        public long CurrentFilePartCompressedBytesRead { get; internal set; }
    }
}

