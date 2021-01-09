namespace SharpCompress.Archive.Tar
{
    using SharpCompress;
    using SharpCompress.Archive;
    using SharpCompress.Common;
    using SharpCompress.Common.Tar;
    using System;
    using System.IO;
    using System.Linq;

    public class TarArchiveEntry : TarEntry, IArchiveEntry, IEntry
    {
        private TarArchive archive;

        internal TarArchiveEntry(TarArchive archive, TarFilePart part, CompressionType compressionType) : base(part, compressionType)
        {
            this.archive = archive;
        }

        public virtual Stream OpenEntryStream() => 
            this.Parts.Single<FilePart>().GetStream();

        public void WriteTo(Stream streamToWriteTo)
        {
            if (this.IsEncrypted)
            {
                throw new PasswordProtectedException("Entry is password protected and cannot be extracted.");
            }
            this.Extract<TarArchiveEntry, TarVolume>(this.archive, streamToWriteTo);
        }

        public bool IsComplete =>
            true;
    }
}

