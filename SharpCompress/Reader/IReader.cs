namespace SharpCompress.Reader
{
    using SharpCompress.Common;
    using System;
    using System.IO;

    public interface IReader : IDisposable
    {
        event EventHandler<CompressedBytesReadEventArgs> CompressedBytesRead;

        event EventHandler<FilePartExtractionBeginEventArgs> FilePartExtractionBegin;

        bool MoveToNextEntry();
        EntryStream OpenEntryStream();
        void WriteEntryTo(Stream writableStream);

        SharpCompress.Common.ArchiveType ArchiveType { get; }

        IEntry Entry { get; }
    }
}

