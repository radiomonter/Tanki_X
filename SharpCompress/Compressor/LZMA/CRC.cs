namespace SharpCompress.Compressor.LZMA
{
    using System;

    internal class CRC
    {
        public static readonly uint[] Table = new uint[0x100];
        private uint _value = uint.MaxValue;

        static CRC()
        {
            uint index = 0;
            while (index < 0x100)
            {
                uint num2 = index;
                int num3 = 0;
                while (true)
                {
                    if (num3 >= 8)
                    {
                        Table[index] = num2;
                        index++;
                        break;
                    }
                    num2 = ((num2 & 1) == 0) ? (num2 >> 1) : ((num2 >> 1) ^ 0xedb88320);
                    num3++;
                }
            }
        }

        private static uint CalculateDigest(byte[] data, uint offset, uint size)
        {
            CRC crc = new CRC();
            crc.Update(data, offset, size);
            return crc.GetDigest();
        }

        public uint GetDigest() => 
            this._value ^ uint.MaxValue;

        public void Init()
        {
            this._value = uint.MaxValue;
        }

        public void Update(byte[] data, uint offset, uint size)
        {
            for (uint i = 0; i < size; i++)
            {
                this._value = Table[((byte) this._value) ^ data[offset + i]] ^ (this._value >> 8);
            }
        }

        public void UpdateByte(byte b)
        {
            this._value = Table[((byte) this._value) ^ b] ^ (this._value >> 8);
        }

        private static bool VerifyDigest(uint digest, byte[] data, uint offset, uint size) => 
            CalculateDigest(data, offset, size) == digest;
    }
}

