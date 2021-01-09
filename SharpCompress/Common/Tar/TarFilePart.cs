namespace SharpCompress.Common.Tar
{
    using SharpCompress.Common;
    using SharpCompress.Common.Tar.Headers;
    using SharpCompress.IO;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal class TarFilePart : FilePart
    {
        private Stream seekableStream;

        internal TarFilePart(TarHeader header, Stream seekableStream)
        {
            this.seekableStream = seekableStream;
            this.Header = header;
        }

        internal override Stream GetStream()
        {
            if (this.seekableStream == null)
            {
                return this.Header.PackedStream;
            }
            this.seekableStream.Position = this.Header.DataStartPosition.Value;
            return new ReadOnlySubStream(this.seekableStream, this.Header.Size);
        }

        internal TarHeader Header { get; private set; }

        internal override string FilePartName =>
            this.Header.Name;
    }
}

