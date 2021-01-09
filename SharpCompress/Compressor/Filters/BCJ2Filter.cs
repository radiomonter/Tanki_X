namespace SharpCompress.Compressor.Filters
{
    using System;
    using System.IO;

    public class BCJ2Filter : Stream
    {
        private Stream baseStream;
        private byte[] input;
        private int inputOffset;
        private int inputCount;
        private bool endReached;
        private long position;
        private byte[] output;
        private int outputOffset;
        private int outputCount;
        private byte[] control;
        private byte[] data1;
        private byte[] data2;
        private int controlPos;
        private int data1Pos;
        private int data2Pos;
        private ushort[] p;
        private uint range;
        private uint code;
        private byte prevByte;
        private const int kNumTopBits = 0x18;
        private const int kTopValue = 0x1000000;
        private const int kNumBitModelTotalBits = 11;
        private const int kBitModelTotal = 0x800;
        private const int kNumMoveBits = 5;

        public BCJ2Filter(byte[] control, byte[] data1, byte[] data2, Stream baseStream)
        {
            int num;
            this.input = new byte[0x1000];
            this.output = new byte[4];
            this.p = new ushort[0x102];
            this.control = control;
            this.data1 = data1;
            this.data2 = data2;
            this.baseStream = baseStream;
            for (num = 0; num < this.p.Length; num++)
            {
                this.p[num] = 0x400;
            }
            this.code = 0;
            this.range = uint.MaxValue;
            for (num = 0; num < 5; num++)
            {
                int num2;
                this.controlPos = (num2 = this.controlPos) + 1;
                this.code = (this.code << 8) | control[num2];
            }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        private static bool IsJ(byte b0, byte b1) => 
            ((b1 & 0xfe) == 0xe8) || IsJcc(b0, b1);

        private static bool IsJcc(byte b0, byte b1) => 
            (b0 == 15) && ((b1 & 240) == 0x80);

        public override unsafe int Read(byte[] buffer, int offset, int count)
        {
            int num = 0;
            byte num2 = 0;
            while (true)
            {
                if (this.endReached || (num >= count))
                {
                    break;
                }
                while (true)
                {
                    int num3;
                    if (this.outputOffset < this.outputCount)
                    {
                        this.outputOffset = (num3 = this.outputOffset) + 1;
                        num2 = this.output[num3];
                        buffer[offset++] = num2;
                        num++;
                        this.position += 1L;
                        this.prevByte = num2;
                        if (num == count)
                        {
                            return num;
                        }
                        continue;
                    }
                    if (this.inputOffset == this.inputCount)
                    {
                        this.inputOffset = 0;
                        this.inputCount = this.baseStream.Read(this.input, 0, this.input.Length);
                        if (this.inputCount == 0)
                        {
                            this.endReached = true;
                            return num;
                        }
                    }
                    this.inputOffset = (num3 = this.inputOffset) + 1;
                    num2 = this.input[num3];
                    buffer[offset++] = num2;
                    num++;
                    this.position += 1L;
                    if (!IsJ(this.prevByte, num2))
                    {
                        this.prevByte = num2;
                    }
                    else
                    {
                        int index = (num2 != 0xe8) ? ((num2 != 0xe9) ? 0x101 : 0x100) : this.prevByte;
                        uint num5 = (this.range >> 11) * this.p[index];
                        if (this.code < num5)
                        {
                            this.range = num5;
                            ushort* numPtr1 = &(this.p[index]);
                            numPtr1[0] = (ushort) (numPtr1[0] + ((ushort) ((0x800 - this.p[index]) >> 5)));
                            if (this.range < 0x1000000)
                            {
                                this.range = this.range << 8;
                                this.controlPos = (num3 = this.controlPos) + 1;
                                this.code = (this.code << 8) | this.control[num3];
                            }
                            this.prevByte = num2;
                        }
                        else
                        {
                            uint num6;
                            this.range -= num5;
                            this.code -= num5;
                            ushort* numPtr2 = &(this.p[index]);
                            numPtr2[0] = (ushort) (numPtr2[0] - ((ushort) (this.p[index] >> 5)));
                            if (this.range < 0x1000000)
                            {
                                this.range = this.range << 8;
                                this.controlPos = (num3 = this.controlPos) + 1;
                                this.code = (this.code << 8) | this.control[num3];
                            }
                            if (num2 == 0xe8)
                            {
                                this.data1Pos = (num3 = this.data1Pos) + 1;
                                int num1 = this.data1[num3] << 0x18;
                                this.data1Pos = (num3 = this.data1Pos) + 1;
                                int num7 = num1 | (this.data1[num3] << 0x10);
                                this.data1Pos = (num3 = this.data1Pos) + 1;
                                int num8 = num7 | (this.data1[num3] << 8);
                                this.data1Pos = (num3 = this.data1Pos) + 1;
                                num6 = (uint) (num8 | this.data1[num3]);
                            }
                            else
                            {
                                this.data2Pos = (num3 = this.data2Pos) + 1;
                                int num9 = this.data2[num3] << 0x18;
                                this.data2Pos = (num3 = this.data2Pos) + 1;
                                int num10 = num9 | (this.data2[num3] << 0x10);
                                this.data2Pos = (num3 = this.data2Pos) + 1;
                                int num11 = num10 | (this.data2[num3] << 8);
                                this.data2Pos = (num3 = this.data2Pos) + 1;
                                num6 = (uint) (num11 | this.data2[num3]);
                            }
                            num6 -= (uint) (this.position + 4L);
                            this.output[0] = (byte) num6;
                            this.output[1] = (byte) (num6 >> 8);
                            this.output[2] = (byte) (num6 >> 0x10);
                            this.output[3] = (byte) (num6 >> 0x18);
                            this.outputOffset = 0;
                            this.outputCount = 4;
                        }
                    }
                    break;
                }
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
            throw new NotImplementedException();
        }

        public override bool CanRead =>
            true;

        public override bool CanSeek =>
            false;

        public override bool CanWrite =>
            false;

        public override long Length =>
            (this.baseStream.Length + this.data1.Length) + this.data2.Length;

        public override long Position
        {
            get => 
                this.position;
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}

