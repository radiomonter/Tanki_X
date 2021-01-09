namespace SharpCompress.Archive
{
    using SharpCompress.Common;
    using System;
    using System.IO;

    public interface IArchiveEntry : IEntry
    {
        Stream OpenEntryStream();
        void WriteTo(Stream stream);

        bool IsComplete { get; }
    }
}

