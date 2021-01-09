namespace SharpCompress.Archive
{
    using SharpCompress;
    using SharpCompress.Common;
    using SharpCompress.Reader;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public abstract class AbstractArchive<TEntry, TVolume> : IArchive, IStreamListener, IDisposable where TEntry: IArchiveEntry where TVolume: IVolume
    {
        private readonly LazyReadOnlyCollection<TVolume> lazyVolumes;
        private readonly LazyReadOnlyCollection<TEntry> lazyEntries;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EventHandler<ArchiveExtractionEventArgs<IArchiveEntry>> EntryExtractionBegin;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EventHandler<ArchiveExtractionEventArgs<IArchiveEntry>> EntryExtractionEnd;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EventHandler<CompressedBytesReadEventArgs> CompressedBytesRead;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EventHandler<FilePartExtractionBeginEventArgs> FilePartExtractionBegin;
        private bool disposed;
        [CompilerGenerated]
        private static Func<Stream, Stream> <>f__mg$cache0;
        [CompilerGenerated]
        private static Func<long, TEntry, long> <>f__am$cache0;
        [CompilerGenerated]
        private static Action<TVolume> <>f__am$cache1;
        [CompilerGenerated]
        private static Action<Entry> <>f__am$cache2;
        [CompilerGenerated]
        private static Func<TEntry, bool> <>f__am$cache3;

        public event EventHandler<CompressedBytesReadEventArgs> CompressedBytesRead
        {
            add
            {
                EventHandler<CompressedBytesReadEventArgs> compressedBytesRead = this.CompressedBytesRead;
                while (true)
                {
                    EventHandler<CompressedBytesReadEventArgs> objB = compressedBytesRead;
                    compressedBytesRead = Interlocked.CompareExchange<EventHandler<CompressedBytesReadEventArgs>>(ref this.CompressedBytesRead, objB + value, compressedBytesRead);
                    if (ReferenceEquals(compressedBytesRead, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                EventHandler<CompressedBytesReadEventArgs> compressedBytesRead = this.CompressedBytesRead;
                while (true)
                {
                    EventHandler<CompressedBytesReadEventArgs> objB = compressedBytesRead;
                    compressedBytesRead = Interlocked.CompareExchange<EventHandler<CompressedBytesReadEventArgs>>(ref this.CompressedBytesRead, objB - value, compressedBytesRead);
                    if (ReferenceEquals(compressedBytesRead, objB))
                    {
                        return;
                    }
                }
            }
        }

        public event EventHandler<ArchiveExtractionEventArgs<IArchiveEntry>> EntryExtractionBegin
        {
            add
            {
                EventHandler<ArchiveExtractionEventArgs<IArchiveEntry>> entryExtractionBegin = this.EntryExtractionBegin;
                while (true)
                {
                    EventHandler<ArchiveExtractionEventArgs<IArchiveEntry>> objB = entryExtractionBegin;
                    entryExtractionBegin = Interlocked.CompareExchange<EventHandler<ArchiveExtractionEventArgs<IArchiveEntry>>>(ref this.EntryExtractionBegin, objB + value, entryExtractionBegin);
                    if (ReferenceEquals(entryExtractionBegin, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                EventHandler<ArchiveExtractionEventArgs<IArchiveEntry>> entryExtractionBegin = this.EntryExtractionBegin;
                while (true)
                {
                    EventHandler<ArchiveExtractionEventArgs<IArchiveEntry>> objB = entryExtractionBegin;
                    entryExtractionBegin = Interlocked.CompareExchange<EventHandler<ArchiveExtractionEventArgs<IArchiveEntry>>>(ref this.EntryExtractionBegin, objB - value, entryExtractionBegin);
                    if (ReferenceEquals(entryExtractionBegin, objB))
                    {
                        return;
                    }
                }
            }
        }

        public event EventHandler<ArchiveExtractionEventArgs<IArchiveEntry>> EntryExtractionEnd
        {
            add
            {
                EventHandler<ArchiveExtractionEventArgs<IArchiveEntry>> entryExtractionEnd = this.EntryExtractionEnd;
                while (true)
                {
                    EventHandler<ArchiveExtractionEventArgs<IArchiveEntry>> objB = entryExtractionEnd;
                    entryExtractionEnd = Interlocked.CompareExchange<EventHandler<ArchiveExtractionEventArgs<IArchiveEntry>>>(ref this.EntryExtractionEnd, objB + value, entryExtractionEnd);
                    if (ReferenceEquals(entryExtractionEnd, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                EventHandler<ArchiveExtractionEventArgs<IArchiveEntry>> entryExtractionEnd = this.EntryExtractionEnd;
                while (true)
                {
                    EventHandler<ArchiveExtractionEventArgs<IArchiveEntry>> objB = entryExtractionEnd;
                    entryExtractionEnd = Interlocked.CompareExchange<EventHandler<ArchiveExtractionEventArgs<IArchiveEntry>>>(ref this.EntryExtractionEnd, objB - value, entryExtractionEnd);
                    if (ReferenceEquals(entryExtractionEnd, objB))
                    {
                        return;
                    }
                }
            }
        }

        public event EventHandler<FilePartExtractionBeginEventArgs> FilePartExtractionBegin
        {
            add
            {
                EventHandler<FilePartExtractionBeginEventArgs> filePartExtractionBegin = this.FilePartExtractionBegin;
                while (true)
                {
                    EventHandler<FilePartExtractionBeginEventArgs> objB = filePartExtractionBegin;
                    filePartExtractionBegin = Interlocked.CompareExchange<EventHandler<FilePartExtractionBeginEventArgs>>(ref this.FilePartExtractionBegin, objB + value, filePartExtractionBegin);
                    if (ReferenceEquals(filePartExtractionBegin, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                EventHandler<FilePartExtractionBeginEventArgs> filePartExtractionBegin = this.FilePartExtractionBegin;
                while (true)
                {
                    EventHandler<FilePartExtractionBeginEventArgs> objB = filePartExtractionBegin;
                    filePartExtractionBegin = Interlocked.CompareExchange<EventHandler<FilePartExtractionBeginEventArgs>>(ref this.FilePartExtractionBegin, objB - value, filePartExtractionBegin);
                    if (ReferenceEquals(filePartExtractionBegin, objB))
                    {
                        return;
                    }
                }
            }
        }

        internal AbstractArchive(ArchiveType type)
        {
            this.Type = type;
            this.lazyVolumes = new LazyReadOnlyCollection<TVolume>(Enumerable.Empty<TVolume>());
            this.lazyEntries = new LazyReadOnlyCollection<TEntry>(Enumerable.Empty<TEntry>());
        }

        internal AbstractArchive(ArchiveType type, IEnumerable<Stream> streams, Options options)
        {
            this.Type = type;
            if (AbstractArchive<TEntry, TVolume>.<>f__mg$cache0 == null)
            {
                AbstractArchive<TEntry, TVolume>.<>f__mg$cache0 = new Func<Stream, Stream>(AbstractArchive<TEntry, TVolume>.CheckStreams);
            }
            this.lazyVolumes = new LazyReadOnlyCollection<TVolume>(this.LoadVolumes(streams.Select<Stream, Stream>(AbstractArchive<TEntry, TVolume>.<>f__mg$cache0), options));
            this.lazyEntries = new LazyReadOnlyCollection<TEntry>(this.LoadEntries(this.Volumes));
        }

        private static Stream CheckStreams(Stream stream)
        {
            if (!stream.CanSeek || !stream.CanRead)
            {
                throw new ArgumentException("Archive streams must be Readable and Seekable");
            }
            return stream;
        }

        protected abstract IReader CreateReaderForSolidExtraction();
        public void Dispose()
        {
            if (!this.disposed)
            {
                if (AbstractArchive<TEntry, TVolume>.<>f__am$cache1 == null)
                {
                    AbstractArchive<TEntry, TVolume>.<>f__am$cache1 = v => v.Dispose();
                }
                this.lazyVolumes.ForEach<TVolume>(AbstractArchive<TEntry, TVolume>.<>f__am$cache1);
                AbstractArchive<TEntry, TVolume>.<>f__am$cache2 ??= x => x.Close();
                this.lazyEntries.GetLoaded().Cast<Entry>().ForEach<Entry>(AbstractArchive<TEntry, TVolume>.<>f__am$cache2);
                this.disposed = true;
            }
        }

        internal void EnsureEntriesLoaded()
        {
            this.lazyEntries.EnsureFullyLoaded();
            this.lazyVolumes.EnsureFullyLoaded();
        }

        public IReader ExtractAllEntries()
        {
            this.EnsureEntriesLoaded();
            return this.CreateReaderForSolidExtraction();
        }

        internal void FireEntryExtractionBegin(IArchiveEntry entry)
        {
            if (this.EntryExtractionBegin != null)
            {
                this.EntryExtractionBegin(this, new ArchiveExtractionEventArgs<IArchiveEntry>(entry));
            }
        }

        internal void FireEntryExtractionEnd(IArchiveEntry entry)
        {
            if (this.EntryExtractionEnd != null)
            {
                this.EntryExtractionEnd(this, new ArchiveExtractionEventArgs<IArchiveEntry>(entry));
            }
        }

        protected abstract IEnumerable<TEntry> LoadEntries(IEnumerable<TVolume> volumes);
        protected abstract IEnumerable<TVolume> LoadVolumes(IEnumerable<Stream> streams, Options options);
        void IStreamListener.FireCompressedBytesRead(long currentPartCompressedBytes, long compressedReadBytes)
        {
            if (this.CompressedBytesRead != null)
            {
                CompressedBytesReadEventArgs e = new CompressedBytesReadEventArgs {
                    CurrentFilePartCompressedBytesRead = currentPartCompressedBytes,
                    CompressedBytesRead = compressedReadBytes
                };
                this.CompressedBytesRead(this, e);
            }
        }

        void IStreamListener.FireFilePartExtractionBegin(string name, long size, long compressedSize)
        {
            if (this.FilePartExtractionBegin != null)
            {
                FilePartExtractionBeginEventArgs e = new FilePartExtractionBeginEventArgs {
                    CompressedSize = compressedSize,
                    Size = size,
                    Name = name
                };
                this.FilePartExtractionBegin(this, e);
            }
        }

        IEnumerable<IArchiveEntry> IArchive.Entries =>
            this.lazyEntries.Cast<IArchiveEntry>();

        IEnumerable<IVolume> IArchive.Volumes =>
            this.lazyVolumes.Cast<IVolume>();

        public virtual ICollection<TEntry> Entries =>
            this.lazyEntries;

        public ICollection<TVolume> Volumes =>
            this.lazyVolumes;

        public long TotalSize
        {
            get
            {
                if (AbstractArchive<TEntry, TVolume>.<>f__am$cache0 == null)
                {
                    AbstractArchive<TEntry, TVolume>.<>f__am$cache0 = (total, cf) => total + cf.CompressedSize;
                }
                return this.Entries.Aggregate<TEntry, long>(0L, AbstractArchive<TEntry, TVolume>.<>f__am$cache0);
            }
        }

        public ArchiveType Type { get; private set; }

        public virtual bool IsSolid =>
            false;

        public bool IsComplete
        {
            get
            {
                this.EnsureEntriesLoaded();
                if (AbstractArchive<TEntry, TVolume>.<>f__am$cache3 == null)
                {
                    AbstractArchive<TEntry, TVolume>.<>f__am$cache3 = x => x.IsComplete;
                }
                return this.Entries.All<TEntry>(AbstractArchive<TEntry, TVolume>.<>f__am$cache3);
            }
        }
    }
}

