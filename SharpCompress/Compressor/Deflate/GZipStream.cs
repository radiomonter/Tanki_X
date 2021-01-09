namespace SharpCompress.Compressor.Deflate
{
    using SharpCompress.Common;
    using SharpCompress.Compressor;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    public class GZipStream : Stream
    {
        internal static readonly DateTime UnixEpoch = new DateTime(0x7b2, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private string comment;
        private int crc32;
        private string fileName;
        internal ZlibBaseStream BaseStream;
        private bool disposed;
        private bool firstReadDone;
        private int headerByteCount;

        public GZipStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
        {
        }

        public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
        {
        }

        public GZipStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
        {
        }

        public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
        {
            this.BaseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.GZIP, leaveOpen);
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (!this.disposed)
                {
                    if (disposing && (this.BaseStream != null))
                    {
                        this.BaseStream.Dispose();
                        this.crc32 = this.BaseStream.Crc32;
                    }
                    this.disposed = true;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        private int EmitHeader()
        {
            byte[] sourceArray = (this.Comment != null) ? ArchiveEncoding.Default.GetBytes(this.Comment) : null;
            byte[] buffer2 = (this.FileName != null) ? ArchiveEncoding.Default.GetBytes(this.FileName) : null;
            int num = (this.Comment != null) ? (sourceArray.Length + 1) : 0;
            int num2 = (this.FileName != null) ? (buffer2.Length + 1) : 0;
            byte[] destinationArray = new byte[(10 + num) + num2];
            int destinationIndex = 0;
            destinationArray[destinationIndex++] = 0x1f;
            destinationArray[destinationIndex++] = 0x8b;
            destinationArray[destinationIndex++] = 8;
            byte num5 = 0;
            if (this.Comment != null)
            {
                num5 = (byte) (num5 ^ 0x10);
            }
            if (this.FileName != null)
            {
                num5 = (byte) (num5 ^ 8);
            }
            destinationArray[destinationIndex++] = num5;
            if (this.LastModified == null)
            {
                this.LastModified = new DateTime?(DateTime.Now);
            }
            Array.Copy(BitConverter.GetBytes((int) (this.LastModified.Value - UnixEpoch).TotalSeconds), 0, destinationArray, destinationIndex, 4);
            destinationIndex += 4;
            destinationArray[destinationIndex++] = 0;
            destinationArray[destinationIndex++] = 0xff;
            if (num2 != 0)
            {
                Array.Copy(buffer2, 0, destinationArray, destinationIndex, num2 - 1);
                destinationIndex += num2 - 1;
                destinationArray[destinationIndex++] = 0;
            }
            if (num != 0)
            {
                Array.Copy(sourceArray, 0, destinationArray, destinationIndex, num - 1);
                destinationIndex += num - 1;
                destinationArray[destinationIndex++] = 0;
            }
            this.BaseStream._stream.Write(destinationArray, 0, destinationArray.Length);
            return destinationArray.Length;
        }

        public override void Flush()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("GZipStream");
            }
            this.BaseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("GZipStream");
            }
            int num = this.BaseStream.Read(buffer, offset, count);
            if (!this.firstReadDone)
            {
                this.firstReadDone = true;
                this.FileName = this.BaseStream._GzipFileName;
                this.Comment = this.BaseStream._GzipComment;
            }
            return num;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("GZipStream");
            }
            if (this.BaseStream._streamMode == ZlibBaseStream.StreamMode.Undefined)
            {
                if (!this.BaseStream._wantCompress)
                {
                    throw new InvalidOperationException();
                }
                this.headerByteCount = this.EmitHeader();
            }
            this.BaseStream.Write(buffer, offset, count);
        }

        public DateTime? LastModified { get; set; }

        public virtual FlushType FlushMode
        {
            get => 
                this.BaseStream._flushMode;
            set
            {
                if (this.disposed)
                {
                    throw new ObjectDisposedException("GZipStream");
                }
                this.BaseStream._flushMode = value;
            }
        }

        public int BufferSize
        {
            get => 
                this.BaseStream._bufferSize;
            set
            {
                if (this.disposed)
                {
                    throw new ObjectDisposedException("GZipStream");
                }
                if (this.BaseStream._workingBuffer != null)
                {
                    throw new ZlibException("The working buffer is already set.");
                }
                if (value < 0x400)
                {
                    throw new ZlibException($"Don't be silly. {value} bytes?? Use a bigger buffer, at least {0x400}.");
                }
                this.BaseStream._bufferSize = value;
            }
        }

        internal virtual long TotalIn =>
            this.BaseStream._z.TotalBytesIn;

        internal virtual long TotalOut =>
            this.BaseStream._z.TotalBytesOut;

        public override bool CanRead
        {
            get
            {
                if (this.disposed)
                {
                    throw new ObjectDisposedException("GZipStream");
                }
                return this.BaseStream._stream.CanRead;
            }
        }

        public override bool CanSeek =>
            false;

        public override bool CanWrite
        {
            get
            {
                if (this.disposed)
                {
                    throw new ObjectDisposedException("GZipStream");
                }
                return this.BaseStream._stream.CanWrite;
            }
        }

        public override long Length
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override long Position
        {
            get => 
                (this.BaseStream._streamMode != ZlibBaseStream.StreamMode.Writer) ? ((this.BaseStream._streamMode != ZlibBaseStream.StreamMode.Reader) ? 0L : (this.BaseStream._z.TotalBytesIn + this.BaseStream._gzipHeaderByteCount)) : (this.BaseStream._z.TotalBytesOut + this.headerByteCount);
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Comment
        {
            get => 
                this.comment;
            set
            {
                if (this.disposed)
                {
                    throw new ObjectDisposedException("GZipStream");
                }
                this.comment = value;
            }
        }

        public string FileName
        {
            get => 
                this.fileName;
            set
            {
                if (this.disposed)
                {
                    throw new ObjectDisposedException("GZipStream");
                }
                this.fileName = value;
                if (this.fileName != null)
                {
                    if (this.fileName.IndexOf("/") != -1)
                    {
                        this.fileName = this.fileName.Replace("/", @"\");
                    }
                    if (this.fileName.EndsWith(@"\"))
                    {
                        throw new InvalidOperationException("Illegal filename");
                    }
                    if (this.fileName.IndexOf(@"\") != -1)
                    {
                        this.fileName = Path.GetFileName(this.fileName);
                    }
                }
            }
        }

        public int Crc32 =>
            this.crc32;
    }
}

