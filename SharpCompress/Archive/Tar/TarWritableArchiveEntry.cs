namespace SharpCompress.Archive.Tar
{
    using SharpCompress.Common;
    using SharpCompress.IO;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal class TarWritableArchiveEntry : TarArchiveEntry
    {
        private string path;
        private long size;
        private DateTime? lastModified;
        private bool closeStream;

        internal TarWritableArchiveEntry(TarArchive archive, System.IO.Stream stream, CompressionType compressionType, string path, long size, DateTime? lastModified, bool closeStream) : base(archive, null, compressionType)
        {
            this.Stream = stream;
            this.path = path;
            this.size = size;
            this.lastModified = lastModified;
            this.closeStream = closeStream;
        }

        internal override void Close()
        {
            if (this.closeStream)
            {
                this.Stream.Dispose();
            }
        }

        public override System.IO.Stream OpenEntryStream() => 
            new NonDisposingStream(this.Stream);

        public override uint Crc =>
            0;

        public override string FilePath =>
            this.path;

        public override long CompressedSize =>
            0L;

        public override long Size =>
            this.size;

        public override DateTime? LastModifiedTime =>
            this.lastModified;

        public override DateTime? CreatedTime =>
            null;

        public override DateTime? LastAccessedTime =>
            null;

        public override DateTime? ArchivedTime =>
            null;

        public override bool IsEncrypted =>
            false;

        public override bool IsDirectory =>
            false;

        public override bool IsSplit =>
            false;

        internal override IEnumerable<FilePart> Parts
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        internal System.IO.Stream Stream { get; private set; }
    }
}

