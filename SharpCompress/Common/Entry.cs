namespace SharpCompress.Common
{
    using System;
    using System.Collections.Generic;

    public abstract class Entry : IEntry
    {
        protected Entry()
        {
        }

        internal abstract void Close();

        public abstract uint Crc { get; }

        public abstract string FilePath { get; }

        public abstract long CompressedSize { get; }

        public abstract SharpCompress.Common.CompressionType CompressionType { get; }

        public abstract long Size { get; }

        public abstract DateTime? LastModifiedTime { get; }

        public abstract DateTime? CreatedTime { get; }

        public abstract DateTime? LastAccessedTime { get; }

        public abstract DateTime? ArchivedTime { get; }

        public abstract bool IsEncrypted { get; }

        public abstract bool IsDirectory { get; }

        public abstract bool IsSplit { get; }

        internal abstract IEnumerable<FilePart> Parts { get; }
    }
}

