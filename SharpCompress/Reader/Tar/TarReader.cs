namespace SharpCompress.Reader.Tar
{
    using SharpCompress;
    using SharpCompress.Archive.Tar;
    using SharpCompress.Common;
    using SharpCompress.Common.Tar;
    using SharpCompress.Compressor;
    using SharpCompress.Compressor.BZip2;
    using SharpCompress.Compressor.Deflate;
    using SharpCompress.IO;
    using SharpCompress.Reader;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;

    public class TarReader : AbstractReader<TarEntry, TarVolume>
    {
        private readonly TarVolume volume;
        private readonly CompressionType compressionType;

        internal TarReader(Stream stream, CompressionType compressionType, Options options) : base(options, ArchiveType.Tar)
        {
            this.compressionType = compressionType;
            this.volume = new TarVolume(stream, options);
        }

        internal override IEnumerable<TarEntry> GetEntries(Stream stream) => 
            TarEntry.GetEntries(StreamingMode.Streaming, stream, this.compressionType);

        public static TarReader Open(Stream stream, Options options = 1)
        {
            stream.CheckNotNull("stream");
            RewindableStream stream2 = new RewindableStream(stream);
            stream2.StartRecording();
            stream2.Rewind(false);
            if (!BZip2Stream.IsBZip2(stream2))
            {
                stream2.Rewind(true);
                return new TarReader(stream2, CompressionType.None, options);
            }
            stream2.Rewind(false);
            if (!TarArchive.IsTarFile(new BZip2Stream(stream2, CompressionMode.Decompress, false, false)))
            {
                throw new InvalidFormatException("Not a tar file.");
            }
            stream2.Rewind(true);
            return new TarReader(stream2, CompressionType.BZip2, options);
        }

        internal override Stream RequestInitialStream()
        {
            Stream stream = base.RequestInitialStream();
            switch (this.compressionType)
            {
                case CompressionType.None:
                    return stream;

                case CompressionType.GZip:
                    return new GZipStream(stream, CompressionMode.Decompress);

                case CompressionType.BZip2:
                    return new BZip2Stream(stream, CompressionMode.Decompress, false, false);
            }
            throw new NotSupportedException("Invalid compression type: " + this.compressionType);
        }

        public override TarVolume Volume =>
            this.volume;
    }
}

