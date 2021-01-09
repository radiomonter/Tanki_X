namespace SharpCompress.Writer.Tar
{
    using SharpCompress;
    using SharpCompress.Common;
    using SharpCompress.Common.Tar.Headers;
    using SharpCompress.Compressor;
    using SharpCompress.Compressor.BZip2;
    using SharpCompress.Compressor.Deflate;
    using SharpCompress.Writer;
    using System;
    using System.IO;

    public class TarWriter : AbstractWriter
    {
        public TarWriter(Stream destination, CompressionInfo compressionInfo) : base(ArchiveType.Tar)
        {
            if (!destination.CanWrite)
            {
                throw new ArgumentException("Tars require writable streams.");
            }
            switch (compressionInfo.Type)
            {
                case CompressionType.None:
                    break;

                case CompressionType.GZip:
                    destination = new GZipStream(destination, CompressionMode.Compress, false);
                    break;

                case CompressionType.BZip2:
                    destination = new BZip2Stream(destination, CompressionMode.Compress, false, false);
                    break;

                default:
                    throw new InvalidFormatException("Tar does not support compression: " + compressionInfo.Type);
            }
            base.InitalizeStream(destination, false);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                this.PadTo512(0L, true);
                this.PadTo512(0L, true);
                base.OutputStream.Dispose();
            }
            base.Dispose(isDisposing);
        }

        private string NormalizeFilename(string filename)
        {
            filename = filename.Replace('\\', '/');
            int index = filename.IndexOf(':');
            if (index >= 0)
            {
                filename = filename.Remove(0, index + 1);
            }
            char[] trimChars = new char[] { '/' };
            return filename.Trim(trimChars);
        }

        private void PadTo512(long size, bool forceZeros)
        {
            int count = ((int) size) % 0x200;
            if ((count != 0) || forceZeros)
            {
                count = 0x200 - count;
                base.OutputStream.Write(new byte[count], 0, count);
            }
        }

        public override void Write(string filename, Stream source, DateTime? modificationTime)
        {
            long? size = null;
            this.Write(filename, source, modificationTime, size);
        }

        public void Write(string filename, Stream source, DateTime? modificationTime, long? size)
        {
            if (!source.CanSeek && (size == null))
            {
                throw new ArgumentException("Seekable stream is required if no size is given.");
            }
            long num = (size == null) ? source.Length : size.Value;
            new TarHeader { 
                LastModifiedTime = (modificationTime == null) ? TarHeader.Epoch : modificationTime.Value,
                Name = this.NormalizeFilename(filename),
                Size = num
            }.Write(base.OutputStream);
            size = new long?(source.TransferTo(base.OutputStream));
            this.PadTo512(size.Value, false);
        }
    }
}

