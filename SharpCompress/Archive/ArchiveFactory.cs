namespace SharpCompress.Archive
{
    using SharpCompress;
    using SharpCompress.Archive.Tar;
    using SharpCompress.Common;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    public class ArchiveFactory
    {
        public static IArchive Create(ArchiveType type)
        {
            if (type != ArchiveType.Tar)
            {
                throw new NotSupportedException("Cannot create Archives of type: " + type);
            }
            return TarArchive.Create();
        }

        public static IArchive Open(Stream stream, Options options = 1)
        {
            stream.CheckNotNull("stream");
            if (!stream.CanRead || !stream.CanSeek)
            {
                throw new ArgumentException("Stream should be readable and seekable");
            }
            stream.Seek(0L, SeekOrigin.Begin);
            if (TarArchive.IsTarFile(stream))
            {
                stream.Seek(0L, SeekOrigin.Begin);
                return TarArchive.Open(stream, options);
            }
            stream.Seek(0L, SeekOrigin.Begin);
            throw new InvalidOperationException("Cannot determine compressed stream type.");
        }
    }
}

