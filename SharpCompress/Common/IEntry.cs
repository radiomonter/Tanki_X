namespace SharpCompress.Common
{
    using System;

    public interface IEntry
    {
        SharpCompress.Common.CompressionType CompressionType { get; }

        DateTime? ArchivedTime { get; }

        long CompressedSize { get; }

        uint Crc { get; }

        DateTime? CreatedTime { get; }

        string FilePath { get; }

        bool IsDirectory { get; }

        bool IsEncrypted { get; }

        bool IsSplit { get; }

        DateTime? LastAccessedTime { get; }

        DateTime? LastModifiedTime { get; }

        long Size { get; }
    }
}

