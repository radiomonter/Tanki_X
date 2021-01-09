namespace SharpCompress.Archive
{
    using SharpCompress.Common;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    public static class AbstractWritableArchiveExtensions
    {
        public static void SaveTo<TEntry, TVolume>(this AbstractWritableArchive<TEntry, TVolume> writableArchive, Stream stream, CompressionType compressionType) where TEntry: IArchiveEntry where TVolume: IVolume
        {
            CompressionInfo info = new CompressionInfo {
                Type = compressionType
            };
            writableArchive.SaveTo(stream, info);
        }
    }
}

