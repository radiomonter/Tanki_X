namespace Platform.Library.ClientProtocol.Impl
{
    using System;
    using System.IO;

    public class BigEndianBinaryWriter : BinaryWriter
    {
        private byte[] numBuffer;

        public BigEndianBinaryWriter(Stream output) : base(output)
        {
            this.numBuffer = new byte[8];
        }

        private void ReverseIfNeedAndWrite(byte[] bytes)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            this.Write(bytes, 0, bytes.Length);
        }

        public override void Write(double value)
        {
            this.ReverseIfNeedAndWrite(BitConverter.GetBytes(value));
        }

        public override void Write(short value)
        {
            this.Write((ushort) value);
        }

        public override void Write(int value)
        {
            this.Write((uint) value);
        }

        public override void Write(long value)
        {
            this.Write((ulong) value);
        }

        public override void Write(float value)
        {
            this.ReverseIfNeedAndWrite(BitConverter.GetBytes(value));
        }

        public override void Write(ushort value)
        {
            this.numBuffer[0] = (byte) (value >> 8);
            this.numBuffer[1] = (byte) value;
            this.Write(this.numBuffer, 0, 2);
        }

        public override void Write(uint value)
        {
            this.numBuffer[0] = (byte) (value >> 0x18);
            this.numBuffer[1] = (byte) (value >> 0x10);
            this.numBuffer[2] = (byte) (value >> 8);
            this.numBuffer[3] = (byte) value;
            this.Write(this.numBuffer, 0, 4);
        }

        public override void Write(ulong value)
        {
            this.numBuffer[0] = (byte) (value >> 0x38);
            this.numBuffer[1] = (byte) (value >> 0x30);
            this.numBuffer[2] = (byte) (value >> 40);
            this.numBuffer[3] = (byte) (value >> 0x20);
            this.numBuffer[4] = (byte) (value >> 0x18);
            this.numBuffer[5] = (byte) (value >> 0x10);
            this.numBuffer[6] = (byte) (value >> 8);
            this.numBuffer[7] = (byte) value;
            this.Write(this.numBuffer, 0, 8);
        }
    }
}

