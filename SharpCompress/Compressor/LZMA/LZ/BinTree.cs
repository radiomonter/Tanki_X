namespace SharpCompress.Compressor.LZMA.LZ
{
    using SharpCompress.Compressor.LZMA;
    using System;
    using System.IO;

    internal class BinTree : InWindow
    {
        private uint _cyclicBufferPos;
        private uint _cyclicBufferSize;
        private uint _matchMaxLen;
        private uint[] _son;
        private uint[] _hash;
        private uint _cutValue = 0xff;
        private uint _hashMask;
        private uint _hashSizeSum;
        private bool HASH_ARRAY = true;
        private const uint kHash2Size = 0x400;
        private const uint kHash3Size = 0x10000;
        private const uint kBT2HashSize = 0x10000;
        private const uint kStartMaxLen = 1;
        private const uint kHash3Offset = 0x400;
        private const uint kEmptyHashValue = 0;
        private const uint kMaxValForNormalize = 0x7fffffff;
        private uint kNumHashDirectBytes;
        private uint kMinMatchCheck = 4;
        private uint kFixHashSize = 0x10400;

        public void Create(uint historySize, uint keepAddBufferBefore, uint matchMaxLen, uint keepAddBufferAfter)
        {
            uint num3;
            if (historySize > 0x7ffffeff)
            {
                throw new Exception();
            }
            this._cutValue = ((uint) 0x10) + (matchMaxLen >> 1);
            uint keepSizeReserv = ((((historySize + keepAddBufferBefore) + matchMaxLen) + keepAddBufferAfter) / 2) + 0x100;
            base.Create(historySize + keepAddBufferBefore, matchMaxLen + keepAddBufferAfter, keepSizeReserv);
            this._matchMaxLen = matchMaxLen;
            uint num2 = historySize + 1;
            if (this._cyclicBufferSize != num2)
            {
                this._cyclicBufferSize = num3 = num2;
                this._son = new uint[num3 * 2];
            }
            uint num4 = 0x10000;
            if (this.HASH_ARRAY)
            {
                num4 = historySize - 1;
                num4 |= num4 >> 1;
                num4 |= num4 >> 2;
                num4 |= num4 >> 4;
                num4 = ((num4 | (num4 >> 8)) >> 1) | 0xffff;
                if (num4 > 0x1000000)
                {
                    num4 = num4 >> 1;
                }
                this._hashMask = num4;
                num4 = (num4 + 1) + this.kFixHashSize;
            }
            if (num4 != this._hashSizeSum)
            {
                this._hashSizeSum = num3 = num4;
                this._hash = new uint[num3];
            }
        }

        public byte GetIndexByte(int index) => 
            base.GetIndexByte(index);

        public uint GetMatches(uint[] distances)
        {
            uint num;
            uint num6;
            uint num16;
            if ((base._pos + this._matchMaxLen) <= base._streamPos)
            {
                num = this._matchMaxLen;
            }
            else if ((base._streamPos - base._pos) < this.kMinMatchCheck)
            {
                this.MovePos();
                return 0;
            }
            uint num2 = 0;
            uint num3 = (base._pos <= this._cyclicBufferSize) ? 0 : (base._pos - this._cyclicBufferSize);
            uint index = base._bufferOffset + base._pos;
            uint num5 = 1;
            uint num7 = 0;
            uint num8 = 0;
            if (!this.HASH_ARRAY)
            {
                num6 = (uint) (base._bufferBase[index] ^ (base._bufferBase[(int) ((IntPtr) (index + 1))] << 8));
            }
            else
            {
                uint num9 = CRC.Table[base._bufferBase[index]] ^ base._bufferBase[(int) ((IntPtr) (index + 1))];
                num7 = num9 & 0x3ff;
                num9 ^= (uint) (base._bufferBase[(int) ((IntPtr) (index + 2))] << 8);
                num8 = num9 & 0xffff;
                num6 = (num9 ^ (CRC.Table[base._bufferBase[(int) ((IntPtr) (index + 3))]] << 5)) & this._hashMask;
            }
            uint num10 = this._hash[this.kFixHashSize + num6];
            if (this.HASH_ARRAY)
            {
                uint num11 = this._hash[num7];
                uint num12 = this._hash[(int) ((IntPtr) (0x400 + num8))];
                this._hash[num7] = base._pos;
                this._hash[(int) ((IntPtr) (0x400 + num8))] = base._pos;
                if ((num11 > num3) && (base._bufferBase[base._bufferOffset + num11] == base._bufferBase[index]))
                {
                    distances[num2++] = num5 = 2;
                    distances[num2++] = (base._pos - num11) - 1;
                }
                if ((num12 > num3) && (base._bufferBase[base._bufferOffset + num12] == base._bufferBase[index]))
                {
                    if (num12 == num11)
                    {
                        num2 -= 2;
                    }
                    distances[num2++] = num5 = 3;
                    distances[num2++] = (base._pos - num12) - 1;
                    num11 = num12;
                }
                if ((num2 != 0) && (num11 == num10))
                {
                    num2 -= 2;
                    num5 = 1;
                }
            }
            this._hash[this.kFixHashSize + num6] = base._pos;
            uint num13 = (this._cyclicBufferPos << 1) + 1;
            uint num14 = this._cyclicBufferPos << 1;
            uint num15 = num16 = this.kNumHashDirectBytes;
            if ((this.kNumHashDirectBytes != 0) && ((num10 > num3) && (base._bufferBase[(base._bufferOffset + num10) + this.kNumHashDirectBytes] != base._bufferBase[index + this.kNumHashDirectBytes])))
            {
                distances[num2++] = num5 = this.kNumHashDirectBytes;
                distances[num2++] = (base._pos - num10) - 1;
            }
            uint num17 = this._cutValue;
            while (true)
            {
                if ((num10 <= num3) || (num17-- == 0))
                {
                    this._son[num13] = this._son[num14] = 0;
                }
                else
                {
                    uint num19 = base._pos - num10;
                    uint num20 = ((num19 > this._cyclicBufferPos) ? ((this._cyclicBufferPos - num19) + this._cyclicBufferSize) : (this._cyclicBufferPos - num19)) << 1;
                    uint num21 = base._bufferOffset + num10;
                    uint num22 = Math.Min(num15, num16);
                    if (base._bufferBase[num21 + num22] == base._bufferBase[index + num22])
                    {
                        while (true)
                        {
                            if ((++num22 != num) && (base._bufferBase[num21 + num22] == base._bufferBase[index + num22]))
                            {
                                continue;
                            }
                            if (num5 < num22)
                            {
                                distances[num2++] = num5 = num22;
                                distances[num2++] = num19 - 1;
                                if (num22 == num)
                                {
                                    this._son[num14] = this._son[num20];
                                    this._son[num13] = this._son[(int) ((IntPtr) (num20 + 1))];
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    if (base._bufferBase[num21 + num22] < base._bufferBase[index + num22])
                    {
                        this._son[num14] = num10;
                        num14 = num20 + 1;
                        num10 = this._son[num14];
                        num16 = num22;
                    }
                    else
                    {
                        this._son[num13] = num10;
                        num13 = num20;
                        num10 = this._son[num13];
                        num15 = num22;
                    }
                    continue;
                }
                break;
            }
            this.MovePos();
            return num2;
        }

        public uint GetMatchLen(int index, uint distance, uint limit) => 
            base.GetMatchLen(index, distance, limit);

        public uint GetNumAvailableBytes() => 
            base.GetNumAvailableBytes();

        public void Init()
        {
            base.Init();
            for (uint i = 0; i < this._hashSizeSum; i++)
            {
                this._hash[i] = 0;
            }
            this._cyclicBufferPos = 0;
            base.ReduceOffsets(-1);
        }

        public void MovePos()
        {
            if (++this._cyclicBufferPos >= this._cyclicBufferSize)
            {
                this._cyclicBufferPos = 0;
            }
            base.MovePos();
            if (base._pos == 0x7fffffff)
            {
                this.Normalize();
            }
        }

        private void Normalize()
        {
            uint subValue = base._pos - this._cyclicBufferSize;
            this.NormalizeLinks(this._son, this._cyclicBufferSize * 2, subValue);
            this.NormalizeLinks(this._hash, this._hashSizeSum, subValue);
            base.ReduceOffsets((int) subValue);
        }

        private void NormalizeLinks(uint[] items, uint numItems, uint subValue)
        {
            for (uint i = 0; i < numItems; i++)
            {
                uint num2 = items[i];
                items[i] = (num2 > subValue) ? (num2 - subValue) : 0;
            }
        }

        public void ReleaseStream()
        {
            base.ReleaseStream();
        }

        public void SetCutValue(uint cutValue)
        {
            this._cutValue = cutValue;
        }

        public void SetStream(Stream stream)
        {
            base.SetStream(stream);
        }

        public void SetType(int numHashBytes)
        {
            this.HASH_ARRAY = numHashBytes > 2;
            if (this.HASH_ARRAY)
            {
                this.kNumHashDirectBytes = 0;
                this.kMinMatchCheck = 4;
                this.kFixHashSize = 0x10400;
            }
            else
            {
                this.kNumHashDirectBytes = 2;
                this.kMinMatchCheck = 3;
                this.kFixHashSize = 0;
            }
        }

        public void Skip(uint num)
        {
            uint num2;
            uint num5;
            uint num13;
        TR_0019:
            while (true)
            {
                if ((base._pos + this._matchMaxLen) <= base._streamPos)
                {
                    num2 = this._matchMaxLen;
                    break;
                }
                if ((base._streamPos - base._pos) >= this.kMinMatchCheck)
                {
                    break;
                }
                this.MovePos();
                goto TR_0001;
            }
            uint num3 = (base._pos <= this._cyclicBufferSize) ? 0 : (base._pos - this._cyclicBufferSize);
            uint index = base._bufferOffset + base._pos;
            if (!this.HASH_ARRAY)
            {
                num5 = (uint) (base._bufferBase[index] ^ (base._bufferBase[(int) ((IntPtr) (index + 1))] << 8));
            }
            else
            {
                uint num6 = CRC.Table[base._bufferBase[index]] ^ base._bufferBase[(int) ((IntPtr) (index + 1))];
                uint num7 = num6 & 0x3ff;
                this._hash[num7] = base._pos;
                num6 ^= (uint) (base._bufferBase[(int) ((IntPtr) (index + 2))] << 8);
                uint num8 = num6 & 0xffff;
                this._hash[(int) ((IntPtr) (0x400 + num8))] = base._pos;
                num5 = (num6 ^ (CRC.Table[base._bufferBase[(int) ((IntPtr) (index + 3))]] << 5)) & this._hashMask;
            }
            uint num9 = this._hash[this.kFixHashSize + num5];
            this._hash[this.kFixHashSize + num5] = base._pos;
            uint num10 = (this._cyclicBufferPos << 1) + 1;
            uint num11 = this._cyclicBufferPos << 1;
            uint num12 = num13 = this.kNumHashDirectBytes;
            uint num14 = this._cutValue;
            while (true)
            {
                if ((num9 <= num3) || (num14-- == 0))
                {
                    this._son[num10] = this._son[num11] = 0;
                }
                else
                {
                    uint num16 = base._pos - num9;
                    uint num17 = ((num16 > this._cyclicBufferPos) ? ((this._cyclicBufferPos - num16) + this._cyclicBufferSize) : (this._cyclicBufferPos - num16)) << 1;
                    uint num18 = base._bufferOffset + num9;
                    uint num19 = Math.Min(num12, num13);
                    if (base._bufferBase[num18 + num19] == base._bufferBase[index + num19])
                    {
                        while (true)
                        {
                            if ((++num19 != num2) && (base._bufferBase[num18 + num19] == base._bufferBase[index + num19]))
                            {
                                continue;
                            }
                            if (num19 == num2)
                            {
                                this._son[num11] = this._son[num17];
                                this._son[num10] = this._son[(int) ((IntPtr) (num17 + 1))];
                            }
                            break;
                        }
                    }
                    if (base._bufferBase[num18 + num19] < base._bufferBase[index + num19])
                    {
                        this._son[num11] = num9;
                        num11 = num17 + 1;
                        num9 = this._son[num11];
                        num13 = num19;
                    }
                    else
                    {
                        this._son[num10] = num9;
                        num10 = num17;
                        num9 = this._son[num10];
                        num12 = num19;
                    }
                    continue;
                }
                break;
            }
            this.MovePos();
        TR_0001:
            if (--num == 0)
            {
                return;
            }
            goto TR_0019;
        }
    }
}

