namespace Platform.Library.ClientProtocol.Impl
{
    using System;
    using System.IO;

    public class BigEndianBinaryReader : BinaryReader
    {
        private readonly byte[] numBuffer;

        public BigEndianBinaryReader(Stream input) : base(input)
        {
            this.numBuffer = new byte[8];
        }

        private void FillBufferAndReverseIfNeed(int numBytes)
        {
            this.FillNumBuffer(numBytes);
            if (BitConverter.IsLittleEndian)
            {
                this.Reverse(numBytes);
            }
        }

        private void FillNumBuffer(int numBytes)
        {
            int num;
            for (int i = 0; i < numBytes; i += num)
            {
                num = this.Read(this.numBuffer, i, numBytes - i);
                if (num == 0)
                {
                    throw new EndOfStreamException();
                }
            }
        }

        public override double ReadDouble()
        {
            this.FillBufferAndReverseIfNeed(8);
            return BitConverter.ToDouble(this.numBuffer, 0);
        }

        public override short ReadInt16() => 
            (short) this.ReadUInt16();

        public override int ReadInt32() => 
            (int) this.ReadUInt32();

        public override long ReadInt64() => 
            (long) this.ReadUInt64();

        public override float ReadSingle()
        {
            this.FillBufferAndReverseIfNeed(4);
            return BitConverter.ToSingle(this.numBuffer, 0);
        }

        public override ushort ReadUInt16()
        {
            this.FillNumBuffer(2);
            return (ushort) ((this.numBuffer[0] << 8) | this.numBuffer[1]);
        }

        public override uint ReadUInt32()
        {
            this.FillNumBuffer(4);
            return (uint) ((((this.numBuffer[0] << 0x18) | (this.numBuffer[1] << 0x10)) | (this.numBuffer[2] << 8)) | this.numBuffer[3]);
        }

        public override ulong ReadUInt64()
        {
            this.FillNumBuffer(8);
            uint num2 = (uint) ((((this.numBuffer[4] << 0x18) | (this.numBuffer[5] << 0x10)) | (this.numBuffer[6] << 8)) | this.numBuffer[7]);
            return ((((ulong) ((((this.numBuffer[0] << 0x18) | (this.numBuffer[1] << 0x10)) | (this.numBuffer[2] << 8)) | this.numBuffer[3])) << 0x20) | num2);
        }

        private void Reverse(int numBytes)
        {
            for (int i = 0; i < (numBytes / 2); i++)
            {
                byte num2 = this.numBuffer[i];
                int index = (numBytes - i) - 1;
                this.numBuffer[i] = this.numBuffer[index];
                this.numBuffer[index] = num2;
            }
        }
    }
}

