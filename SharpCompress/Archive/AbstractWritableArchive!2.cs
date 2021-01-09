namespace SharpCompress.Archive
{
    using SharpCompress.Common;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public abstract class AbstractWritableArchive<TEntry, TVolume> : AbstractArchive<TEntry, TVolume> where TEntry: IArchiveEntry where TVolume: IVolume
    {
        private readonly List<TEntry> newEntries;
        private readonly List<TEntry> removedEntries;
        private readonly List<TEntry> modifiedEntries;
        private bool hasModifications;
        private readonly bool anyNotWritable;
        [CompilerGenerated]
        private static Func<Stream, bool> <>f__am$cache0;

        internal AbstractWritableArchive(ArchiveType type) : base(type)
        {
            this.newEntries = new List<TEntry>();
            this.removedEntries = new List<TEntry>();
            this.modifiedEntries = new List<TEntry>();
        }

        internal AbstractWritableArchive(ArchiveType type, IEnumerable<Stream> streams, Options options) : base(type, streams, options)
        {
            this.newEntries = new List<TEntry>();
            this.removedEntries = new List<TEntry>();
            this.modifiedEntries = new List<TEntry>();
            if (AbstractWritableArchive<TEntry, TVolume>.<>f__am$cache0 == null)
            {
                AbstractWritableArchive<TEntry, TVolume>.<>f__am$cache0 = new Func<Stream, bool>(AbstractWritableArchive<TEntry, TVolume>.<AbstractWritableArchive>m__0);
            }
            if (streams.Any<Stream>(AbstractWritableArchive<TEntry, TVolume>.<>f__am$cache0))
            {
                this.anyNotWritable = true;
            }
        }

        [CompilerGenerated]
        private static bool <AbstractWritableArchive>m__0(Stream x) => 
            !x.CanWrite;

        public void AddEntry(string filePath, Stream source, long size = 0L, DateTime? modified = new DateTime?())
        {
            this.CheckWritable();
            this.newEntries.Add(this.CreateEntry(filePath, source, size, modified, false));
            this.RebuildModifiedCollection();
        }

        public void AddEntry(string filePath, Stream source, bool closeStream, long size = 0L, DateTime? modified = new DateTime?())
        {
            this.CheckWritable();
            this.newEntries.Add(this.CreateEntry(filePath, source, size, modified, closeStream));
            this.RebuildModifiedCollection();
        }

        private void CheckWritable()
        {
            if (this.anyNotWritable)
            {
                throw new ArchiveException("All Archive streams must be Writable to use Archive writing functionality.");
            }
        }

        protected abstract TEntry CreateEntry(string filePath, Stream source, long size, DateTime? modified, bool closeStream);
        private void RebuildModifiedCollection()
        {
            this.hasModifications = true;
            this.modifiedEntries.Clear();
            this.modifiedEntries.AddRange(this.OldEntries.Concat<TEntry>(this.newEntries));
        }

        public void RemoveEntry(TEntry entry)
        {
            this.CheckWritable();
            if (!this.removedEntries.Contains(entry))
            {
                this.removedEntries.Add(entry);
                this.RebuildModifiedCollection();
            }
        }

        public void SaveTo(Stream stream, CompressionInfo compressionType)
        {
            this.SaveTo(stream, compressionType, this.OldEntries, this.newEntries);
        }

        protected abstract void SaveTo(Stream stream, CompressionInfo compressionType, IEnumerable<TEntry> oldEntries, IEnumerable<TEntry> newEntries);

        public override ICollection<TEntry> Entries =>
            !this.hasModifications ? base.Entries : this.modifiedEntries;

        private IEnumerable<TEntry> OldEntries =>
            from x in base.Entries
                where !base.removedEntries.Contains(x)
                select x;
    }
}

