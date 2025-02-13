﻿namespace SharpCompress.Compressor.LZMA.RangeCoder
{
    using System;
    using System.IO;

    internal class Encoder
    {
        public const uint kTopValue = 0x1000000;
        private System.IO.Stream Stream;
        public ulong Low;
        public uint Range;
        private uint _cacheSize;
        private byte _cache;

        public void CloseStream()
        {
            this.Stream.Dispose();
        }

        public void Encode(uint start, uint size, uint total)
        {
            uint num;
            this.Range = num = this.Range / total;
            this.Low += start * num;
            this.Range *= size;
            while (this.Range < 0x1000000)
            {
                this.Range = this.Range << 8;
                this.ShiftLow();
            }
        }

        public void EncodeBit(uint size0, int numTotalBits, uint symbol)
        {
            uint num = (this.Range >> (numTotalBits & 0x1f)) * size0;
            if (symbol == 0)
            {
                this.Range = num;
            }
            else
            {
                this.Low += num;
                this.Range -= num;
            }
            while (this.Range < 0x1000000)
            {
                this.Range = this.Range << 8;
                this.ShiftLow();
            }
        }

        public void EncodeDirectBits(uint v, int numTotalBits)
        {
            for (int i = numTotalBits - 1; i >= 0; i--)
            {
                this.Range = this.Range >> 1;
                if (((v >> (i & 0x1f)) & 1) == 1)
                {
                    this.Low += this.Range;
                }
                if (this.Range < 0x1000000)
                {
                    this.Range = this.Range << 8;
                    this.ShiftLow();
                }
            }
        }

        public void FlushData()
        {
            for (int i = 0; i < 5; i++)
            {
                this.ShiftLow();
            }
        }

        public void FlushStream()
        {
            this.Stream.Flush();
        }

        public long GetProcessedSizeAdd() => 
            -1L;

        public void Init()
        {
            this.Low = 0UL;
            this.Range = uint.MaxValue;
            this._cacheSize = 1;
            this._cache = 0;
        }

        public void ReleaseStream()
        {
            this.Stream = null;
        }

        public void SetStream(System.IO.Stream stream)
        {
            this.Stream = stream;
        }

        public void ShiftLow()
        {
            if ((((uint) this.Low) < 0xff000000) || (((uint) (this.Low >> 0x20)) == 1))
            {
                byte num = this._cache;
                while (true)
                {
                    this.Stream.WriteByte((byte) (num + (this.Low >> 0x20)));
                    num = 0xff;
                    if (--this._cacheSize == 0)
                    {
                        this._cache = (byte) (((uint) this.Low) >> 0x18);
                        break;
                    }
                }
            }
            this._cacheSize++;
            this.Low = ((uint) this.Low) << 8;
        }
    }
}

