namespace SharpCompress.Archive
{
    using SharpCompress.Common;
    using SharpCompress.Reader;
    using System;
    using System.Collections.Generic;

    public interface IArchive : IDisposable
    {
        event EventHandler<CompressedBytesReadEventArgs> CompressedBytesRead;

        event EventHandler<ArchiveExtractionEventArgs<IArchiveEntry>> EntryExtractionBegin;

        event EventHandler<ArchiveExtractionEventArgs<IArchiveEntry>> EntryExtractionEnd;

        event EventHandler<FilePartExtractionBeginEventArgs> FilePartExtractionBegin;

        IReader ExtractAllEntries();

        IEnumerable<IArchiveEntry> Entries { get; }

        long TotalSize { get; }

        IEnumerable<IVolume> Volumes { get; }

        ArchiveType Type { get; }

        bool IsSolid { get; }

        bool IsComplete { get; }
    }
}

