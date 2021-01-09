namespace SharpCompress.IO
{
    using SharpCompress;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal class RewindableStream : Stream
    {
        private Stream stream;
        private MemoryStream bufferStream;
        private bool isRewound;

        public RewindableStream(Stream stream)
        {
            this.bufferStream = new MemoryStream();
            this.stream = stream;
        }

        public RewindableStream(Stream stream, MemoryStream bufferStream)
        {
            this.bufferStream = new MemoryStream();
            RewindableStream stream2 = stream as RewindableStream;
            this.stream = ((stream2 == null) || stream2.isRewound) ? stream : stream2.stream;
            this.bufferStream = bufferStream;
            if (bufferStream.Position != bufferStream.Length)
            {
                this.isRewound = true;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                this.stream.Dispose();
            }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int num;
            if (!this.isRewound || (this.bufferStream.Position == this.bufferStream.Length))
            {
                num = this.stream.Read(buffer, offset, count);
                if (this.IsRecording)
                {
                    this.bufferStream.Write(buffer, offset, num);
                }
                return num;
            }
            num = this.bufferStream.Read(buffer, offset, count);
            if (num < count)
            {
                int num2 = this.stream.Read(buffer, offset + num, count - num);
                if (this.IsRecording)
                {
                    this.bufferStream.Write(buffer, offset + num, num2);
                }
                num += num2;
            }
            if ((this.bufferStream.Position == this.bufferStream.Length) && !this.IsRecording)
            {
                this.isRewound = false;
                this.bufferStream = new MemoryStream();
            }
            return num;
        }

        public void Rewind(bool stopRecording)
        {
            this.isRewound = true;
            this.IsRecording = !stopRecording;
            this.bufferStream.Position = 0L;
        }

        public void Rewind(MemoryStream buffer)
        {
            if (this.bufferStream.Position >= buffer.Length)
            {
                this.bufferStream.Position -= buffer.Length;
            }
            else
            {
                this.bufferStream.TransferTo(buffer);
                this.bufferStream = buffer;
                this.bufferStream.Position = 0L;
            }
            this.isRewound = true;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public void StartRecording()
        {
            if (this.bufferStream.Position != 0L)
            {
                byte[] buffer = this.bufferStream.ToArray();
                long position = this.bufferStream.Position;
                this.bufferStream = new MemoryStream();
                this.bufferStream.Write(buffer, (int) position, buffer.Length - ((int) position));
                this.bufferStream.Position = 0L;
            }
            this.IsRecording = true;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        internal bool IsRecording { get; private set; }

        public override bool CanRead =>
            true;

        public override bool CanSeek =>
            false;

        public override bool CanWrite =>
            false;

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
                (this.stream.Position + this.bufferStream.Position) - this.bufferStream.Length;
            set
            {
                if (!this.isRewound)
                {
                    this.stream.Position = value;
                }
                else if ((value >= (this.stream.Position - this.bufferStream.Length)) && (value < this.stream.Position))
                {
                    this.bufferStream.Position = (value - this.stream.Position) + this.bufferStream.Length;
                }
                else
                {
                    this.stream.Position = value;
                    this.isRewound = false;
                    this.bufferStream = new MemoryStream();
                }
            }
        }
    }
}

