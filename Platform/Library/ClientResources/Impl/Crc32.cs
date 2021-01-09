namespace Platform.Library.ClientResources.Impl
{
    using System;

    public class Crc32
    {
        private uint[] table;

        public Crc32()
        {
            uint num = 0xedb88320;
            this.table = new uint[0x100];
            uint num2 = 0;
            uint index = 0;
            while (index < this.table.Length)
            {
                num2 = index;
                int num4 = 8;
                while (true)
                {
                    if (num4 <= 0)
                    {
                        this.table[index] = num2;
                        index++;
                        break;
                    }
                    num2 = ((num2 & 1) != 1) ? (num2 >> 1) : ((num2 >> 1) ^ num);
                    num4--;
                }
            }
        }

        public uint Compute(byte[] bytes)
        {
            uint maxValue = uint.MaxValue;
            for (int i = 0; i < bytes.Length; i++)
            {
                byte index = (byte) ((maxValue & 0xff) ^ bytes[i]);
                maxValue = (maxValue >> 8) ^ this.table[index];
            }
            return ~maxValue;
        }
    }
}

