namespace Platform.Library.ClientProtocol.Impl
{
    using System;
    using System.IO;

    public abstract class StreamData
    {
        private bool hasReader = false;
        private bool hasWriter = false;
        private BigEndianBinaryReader reader;
        private BigEndianBinaryWriter writer;

        protected StreamData()
        {
        }

        public void Clear()
        {
            this.Flip();
            this.SetLength(0L);
        }

        public void Flip()
        {
            this.Stream.Seek(0L, SeekOrigin.Begin);
        }

        public int Read(byte[] buffer, int offset, int count) => 
            this.Stream.Read(buffer, offset, count);

        public void SetLength(long value)
        {
            this.Stream.SetLength(value);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            this.Stream.Write(buffer, offset, count);
        }

        public void WriteByte(byte value)
        {
            this.Stream.WriteByte(value);
        }

        public abstract System.IO.Stream Stream { get; }

        public BigEndianBinaryReader Reader
        {
            get
            {
                if (!this.hasReader)
                {
                    this.reader = new BigEndianBinaryReader(this.Stream);
                    this.hasReader = true;
                }
                return this.reader;
            }
        }

        public BigEndianBinaryWriter Writer
        {
            get
            {
                if (!this.hasWriter)
                {
                    this.writer = new BigEndianBinaryWriter(this.Stream);
                    this.hasWriter = true;
                }
                return this.writer;
            }
        }

        public long Length =>
            this.Stream.Length;

        public long Position
        {
            get => 
                this.Stream.Position;
            set => 
                this.Stream.Position = value;
        }
    }
}

