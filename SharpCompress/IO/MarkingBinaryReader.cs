namespace SharpCompress.IO
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal class MarkingBinaryReader : BinaryReader
    {
        public MarkingBinaryReader(Stream stream) : base(stream)
        {
        }

        public void Mark()
        {
            this.CurrentReadByteCount = 0L;
        }

        public override int Read()
        {
            this.CurrentReadByteCount += 4L;
            return base.Read();
        }

        public override int Read(byte[] buffer, int index, int count)
        {
            this.CurrentReadByteCount += count;
            return base.Read(buffer, index, count);
        }

        public override int Read(char[] buffer, int index, int count)
        {
            throw new NotImplementedException();
        }

        public override bool ReadBoolean()
        {
            this.CurrentReadByteCount += 1L;
            return base.ReadBoolean();
        }

        public override byte ReadByte()
        {
            this.CurrentReadByteCount += 1L;
            return base.ReadByte();
        }

        public override byte[] ReadBytes(int count)
        {
            this.CurrentReadByteCount += count;
            return base.ReadBytes(count);
        }

        public override char ReadChar()
        {
            throw new NotImplementedException();
        }

        public override char[] ReadChars(int count)
        {
            throw new NotImplementedException();
        }

        public override decimal ReadDecimal()
        {
            this.CurrentReadByteCount += 0x10;
            return base.ReadDecimal();
        }

        public override double ReadDouble()
        {
            this.CurrentReadByteCount += 8L;
            return base.ReadDouble();
        }

        public override short ReadInt16()
        {
            this.CurrentReadByteCount += 2L;
            return base.ReadInt16();
        }

        public override int ReadInt32()
        {
            this.CurrentReadByteCount += 4L;
            return base.ReadInt32();
        }

        public override long ReadInt64()
        {
            this.CurrentReadByteCount += 8L;
            return base.ReadInt64();
        }

        public override sbyte ReadSByte()
        {
            this.CurrentReadByteCount += 1L;
            return base.ReadSByte();
        }

        public override float ReadSingle()
        {
            this.CurrentReadByteCount += 4L;
            return base.ReadSingle();
        }

        public override string ReadString()
        {
            throw new NotImplementedException();
        }

        public override ushort ReadUInt16()
        {
            this.CurrentReadByteCount += 2L;
            return base.ReadUInt16();
        }

        public override uint ReadUInt32()
        {
            this.CurrentReadByteCount += 4L;
            return base.ReadUInt32();
        }

        public override ulong ReadUInt64()
        {
            this.CurrentReadByteCount += 8L;
            return base.ReadUInt64();
        }

        public long CurrentReadByteCount { get; private set; }
    }
}

