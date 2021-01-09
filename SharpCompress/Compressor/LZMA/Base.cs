namespace SharpCompress.Compressor.LZMA
{
    using System;
    using System.Runtime.InteropServices;

    internal abstract class Base
    {
        public const uint kNumRepDistances = 4;
        public const uint kNumStates = 12;
        public const int kNumPosSlotBits = 6;
        public const int kDicLogSizeMin = 0;
        public const int kNumLenToPosStatesBits = 2;
        public const uint kNumLenToPosStates = 4;
        public const uint kMatchMinLen = 2;
        public const int kNumAlignBits = 4;
        public const uint kAlignTableSize = 0x10;
        public const uint kAlignMask = 15;
        public const uint kStartPosModelIndex = 4;
        public const uint kEndPosModelIndex = 14;
        public const uint kNumPosModels = 10;
        public const uint kNumFullDistances = 0x80;
        public const uint kNumLitPosStatesBitsEncodingMax = 4;
        public const uint kNumLitContextBitsMax = 8;
        public const int kNumPosStatesBitsMax = 4;
        public const uint kNumPosStatesMax = 0x10;
        public const int kNumPosStatesBitsEncodingMax = 4;
        public const uint kNumPosStatesEncodingMax = 0x10;
        public const int kNumLowLenBits = 3;
        public const int kNumMidLenBits = 3;
        public const int kNumHighLenBits = 8;
        public const uint kNumLowLenSymbols = 8;
        public const uint kNumMidLenSymbols = 8;
        public const uint kNumLenSymbols = 0x110;
        public const uint kMatchMaxLen = 0x111;

        protected Base()
        {
        }

        public static uint GetLenToPosState(uint len)
        {
            len -= 2;
            return ((len >= 4) ? 3 : len);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct State
        {
            public uint Index;
            public void Init()
            {
                this.Index = 0;
            }

            public void UpdateChar()
            {
                this.Index = (this.Index >= 4) ? ((this.Index >= 10) ? (this.Index - 6) : (this.Index - 3)) : 0;
            }

            public void UpdateMatch()
            {
                this.Index = (this.Index >= 7) ? ((uint) 10) : 7;
            }

            public void UpdateRep()
            {
                this.Index = (this.Index >= 7) ? ((uint) 11) : 8;
            }

            public void UpdateShortRep()
            {
                this.Index = (this.Index >= 7) ? ((uint) 11) : ((uint) 9);
            }

            public bool IsCharState() => 
                this.Index < 7;
        }
    }
}

