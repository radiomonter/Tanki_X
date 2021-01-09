namespace SharpCompress.Compressor.LZMA.RangeCoder
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct BitEncoder
    {
        public const int kNumBitModelTotalBits = 11;
        public const uint kBitModelTotal = 0x800;
        private const int kNumMoveBits = 5;
        private const int kNumMoveReducingBits = 2;
        public const int kNumBitPriceShiftBits = 6;
        private uint Prob;
        private static uint[] ProbPrices;
        static BitEncoder()
        {
            ProbPrices = new uint[0x200];
            int num = 8;
            while (num >= 0)
            {
                uint num2 = (uint) (1 << (((9 - num) - 1) & 0x1f));
                uint num3 = (uint) (1 << ((9 - num) & 0x1f));
                uint index = num2;
                while (true)
                {
                    if (index >= num3)
                    {
                        num--;
                        break;
                    }
                    ProbPrices[index] = ((uint) (num << 6)) + (((num3 - index) << 6) >> (((9 - num) - 1) & 0x1f));
                    index++;
                }
            }
        }

        public void Init()
        {
            this.Prob = 0x400;
        }

        public void UpdateModel(uint symbol)
        {
            this.Prob = (symbol != 0) ? (this.Prob - (this.Prob >> 5)) : (this.Prob + ((uint) ((0x800 - this.Prob) >> 5)));
        }

        public void Encode(Encoder encoder, uint symbol)
        {
            uint num = (encoder.Range >> 11) * this.Prob;
            if (symbol == 0)
            {
                encoder.Range = num;
                this.Prob += (uint) ((0x800 - this.Prob) >> 5);
            }
            else
            {
                encoder.Low += num;
                encoder.Range -= num;
                this.Prob -= this.Prob >> 5;
            }
            if (encoder.Range < 0x1000000)
            {
                encoder.Range = encoder.Range << 8;
                encoder.ShiftLow();
            }
        }

        public uint GetPrice(uint symbol) => 
            ProbPrices[(int) ((IntPtr) ((((this.Prob - symbol) ^ -symbol) & 0x7ffL) >> 2))];

        public uint GetPrice0() => 
            ProbPrices[this.Prob >> 2];

        public uint GetPrice1() => 
            ProbPrices[(int) ((IntPtr) ((0x800 - this.Prob) >> 2))];
    }
}

