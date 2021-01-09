namespace SharpCompress.Writer
{
    using SharpCompress.Common;
    using SharpCompress.Writer.Tar;
    using System;
    using System.IO;

    public static class WriterFactory
    {
        public static IWriter Open(Stream stream, ArchiveType archiveType, CompressionInfo compressionInfo)
        {
            if (archiveType != ArchiveType.Tar)
            {
                throw new NotSupportedException("Archive Type does not have a Writer: " + archiveType);
            }
            return new TarWriter(stream, compressionInfo);
        }

        public static IWriter Open(Stream stream, ArchiveType archiveType, CompressionType compressionType)
        {
            CompressionInfo compressionInfo = new CompressionInfo {
                Type = compressionType
            };
            return Open(stream, archiveType, compressionInfo);
        }
    }
}

