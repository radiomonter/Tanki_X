namespace SharpCompress.Common
{
    using System;
    using System.Runtime.CompilerServices;

    public class ArchiveExtractionEventArgs<T> : EventArgs
    {
        internal ArchiveExtractionEventArgs(T entry)
        {
            this.Item = entry;
        }

        public T Item { get; private set; }
    }
}

