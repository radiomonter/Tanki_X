namespace SharpCompress.Compressor.Deflate
{
    using System;

    internal class WorkItem
    {
        public byte[] buffer;
        public byte[] compressed;
        public int compressedBytesAvailable;
        public ZlibCodec compressor;
        public int crc;
        public int index;
        public int inputBytesAvailable;
        public int status;

        public WorkItem(int size, CompressionLevel compressLevel, CompressionStrategy strategy)
        {
            this.buffer = new byte[size];
            int num = size + ((((size / 0x8000) + 1) * 5) * 2);
            this.compressed = new byte[num];
            this.status = 0;
            this.compressor = new ZlibCodec();
            this.compressor.InitializeDeflate(compressLevel, false);
            this.compressor.OutputBuffer = this.compressed;
            this.compressor.InputBuffer = this.buffer;
        }

        internal enum Status
        {
            None,
            Filling,
            Filled,
            Compressing,
            Compressed,
            Writing,
            Done
        }
    }
}

