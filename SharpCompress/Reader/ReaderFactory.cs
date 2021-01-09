namespace SharpCompress.Reader
{
    using SharpCompress;
    using SharpCompress.Archive.Tar;
    using SharpCompress.Common;
    using SharpCompress.Compressor;
    using SharpCompress.Compressor.BZip2;
    using SharpCompress.IO;
    using SharpCompress.Reader.Tar;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    public static class ReaderFactory
    {
        public static IReader Open(Stream stream, Options options = 1)
        {
            stream.CheckNotNull("stream");
            RewindableStream stream2 = new RewindableStream(stream);
            stream2.StartRecording();
            stream2.Rewind(false);
            if (BZip2Stream.IsBZip2(stream2))
            {
                stream2.Rewind(false);
                if (TarArchive.IsTarFile(new BZip2Stream(stream2, CompressionMode.Decompress, false, false)))
                {
                    stream2.Rewind(true);
                    return new TarReader(stream2, CompressionType.BZip2, options);
                }
            }
            stream2.Rewind(false);
            if (!TarArchive.IsTarFile(stream2))
            {
                throw new InvalidOperationException("Cannot determine compressed stream type.");
            }
            stream2.Rewind(true);
            return TarReader.Open(stream2, options);
        }
    }
}

