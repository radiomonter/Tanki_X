namespace SharpCompress.Compressor.LZMA
{
    using SharpCompress.Compressor.LZMA.LZ;
    using SharpCompress.Compressor.LZMA.RangeCoder;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    internal class Encoder : ICoder, ISetCoderProperties, IWriteCoderProperties
    {
        private const uint kIfinityPrice = 0xfffffff;
        private static byte[] g_FastPos = new byte[0x800];
        private Base.State _state;
        private byte _previousByte;
        private uint[] _repDistances;
        private const int kDefaultDictionaryLogSize = 0x16;
        private const uint kNumFastBytesDefault = 0x20;
        private const uint kNumLenSpecSymbols = 0x10;
        private const uint kNumOpts = 0x1000;
        private Optimal[] _optimum;
        private BinTree _matchFinder;
        private Encoder _rangeEncoder;
        private BitEncoder[] _isMatch;
        private BitEncoder[] _isRep;
        private BitEncoder[] _isRepG0;
        private BitEncoder[] _isRepG1;
        private BitEncoder[] _isRepG2;
        private BitEncoder[] _isRep0Long;
        private BitTreeEncoder[] _posSlotEncoder;
        private BitEncoder[] _posEncoders;
        private BitTreeEncoder _posAlignEncoder;
        private LenPriceTableEncoder _lenEncoder;
        private LenPriceTableEncoder _repMatchLenEncoder;
        private LiteralEncoder _literalEncoder;
        private uint[] _matchDistances;
        private uint _numFastBytes;
        private uint _longestMatchLength;
        private uint _numDistancePairs;
        private uint _additionalOffset;
        private uint _optimumEndIndex;
        private uint _optimumCurrentIndex;
        private bool _longestMatchWasFound;
        private uint[] _posSlotPrices;
        private uint[] _distancesPrices;
        private uint[] _alignPrices;
        private uint _alignPriceCount;
        private uint _distTableSize;
        private int _posStateBits;
        private uint _posStateMask;
        private int _numLiteralPosStateBits;
        private int _numLiteralContextBits;
        private uint _dictionarySize;
        private uint _dictionarySizePrev;
        private uint _numFastBytesPrev;
        private long nowPos64;
        private bool _finished;
        private Stream _inStream;
        private EMatchFinderType _matchFinderType;
        private bool _writeEndMark;
        private bool _needReleaseMFStream;
        private bool _processingMode;
        private uint[] reps;
        private uint[] repLens;
        private const int kPropSize = 5;
        private byte[] properties;
        private uint[] tempPrices;
        private uint _matchPriceCount;
        private static string[] kMatchFinderIDs = new string[] { "BT2", "BT4" };
        private uint _trainSize;

        static Encoder()
        {
            int index = 2;
            g_FastPos[0] = 0;
            g_FastPos[1] = 1;
            byte num2 = 2;
            while (num2 < 0x16)
            {
                uint num3 = (uint) (1 << (((num2 >> 1) - 1) & 0x1f));
                uint num4 = 0;
                while (true)
                {
                    if (num4 >= num3)
                    {
                        num2 = (byte) (num2 + 1);
                        break;
                    }
                    g_FastPos[index] = num2;
                    num4++;
                    index++;
                }
            }
        }

        public Encoder()
        {
            Base.State state = new Base.State();
            this._state = state;
            this._repDistances = new uint[4];
            this._optimum = new Optimal[0x1000];
            this._rangeEncoder = new Encoder();
            this._isMatch = new BitEncoder[0xc0];
            this._isRep = new BitEncoder[12];
            this._isRepG0 = new BitEncoder[12];
            this._isRepG1 = new BitEncoder[12];
            this._isRepG2 = new BitEncoder[12];
            this._isRep0Long = new BitEncoder[0xc0];
            this._posSlotEncoder = new BitTreeEncoder[4];
            this._posEncoders = new BitEncoder[0x72];
            this._posAlignEncoder = new BitTreeEncoder(4);
            this._lenEncoder = new LenPriceTableEncoder();
            this._repMatchLenEncoder = new LenPriceTableEncoder();
            this._literalEncoder = new LiteralEncoder();
            this._matchDistances = new uint[0x224];
            this._numFastBytes = 0x20;
            this._posSlotPrices = new uint[0x100];
            this._distancesPrices = new uint[0x200];
            this._alignPrices = new uint[0x10];
            this._distTableSize = 0x2c;
            this._posStateBits = 2;
            this._posStateMask = 3;
            this._numLiteralContextBits = 3;
            this._dictionarySize = 0x400000;
            this._dictionarySizePrev = uint.MaxValue;
            this._numFastBytesPrev = uint.MaxValue;
            this._matchFinderType = EMatchFinderType.BT4;
            this.reps = new uint[4];
            this.repLens = new uint[4];
            this.properties = new byte[5];
            this.tempPrices = new uint[0x80];
            for (int i = 0; i < 0x1000L; i++)
            {
                this._optimum[i] = new Optimal();
            }
            for (int j = 0; j < 4L; j++)
            {
                this._posSlotEncoder[j] = new BitTreeEncoder(6);
            }
        }

        private uint Backward(out uint backRes, uint cur)
        {
            this._optimumEndIndex = cur;
            uint posPrev = this._optimum[cur].PosPrev;
            uint backPrev = this._optimum[cur].BackPrev;
            while (true)
            {
                if (this._optimum[cur].Prev1IsChar)
                {
                    this._optimum[posPrev].MakeAsChar();
                    this._optimum[posPrev].PosPrev = posPrev - 1;
                    if (this._optimum[cur].Prev2)
                    {
                        this._optimum[(int) ((IntPtr) (posPrev - 1))].Prev1IsChar = false;
                        this._optimum[(int) ((IntPtr) (posPrev - 1))].PosPrev = this._optimum[cur].PosPrev2;
                        this._optimum[(int) ((IntPtr) (posPrev - 1))].BackPrev = this._optimum[cur].BackPrev2;
                    }
                }
                uint index = posPrev;
                uint num4 = backPrev;
                backPrev = this._optimum[index].BackPrev;
                posPrev = this._optimum[index].PosPrev;
                this._optimum[index].BackPrev = num4;
                this._optimum[index].PosPrev = cur;
                cur = index;
                if (cur <= 0)
                {
                    backRes = this._optimum[0].BackPrev;
                    this._optimumCurrentIndex = this._optimum[0].PosPrev;
                    return this._optimumCurrentIndex;
                }
            }
        }

        private void BaseInit()
        {
            this._state.Init();
            this._previousByte = 0;
            for (uint i = 0; i < 4; i++)
            {
                this._repDistances[i] = 0;
            }
        }

        private bool ChangePair(uint smallDist, uint bigDist) => 
            (smallDist < 0x2000000) && (bigDist >= (smallDist << 7));

        public long Code(Stream inStream, bool final)
        {
            this._matchFinder.SetStream(inStream);
            this._processingMode = !final;
            while (true)
            {
                try
                {
                    while (true)
                    {
                        long num;
                        long num2;
                        bool flag;
                        this.CodeOneBlock(out num, out num2, out flag);
                        if (!flag)
                        {
                            break;
                        }
                        return num;
                    }
                }
                finally
                {
                    this._matchFinder.ReleaseStream();
                    if (final)
                    {
                        this.ReleaseStreams();
                    }
                }
            }
        }

        public void Code(Stream inStream, Stream outStream, long inSize, long outSize, ICodeProgress progress)
        {
            this._needReleaseMFStream = false;
            this._processingMode = false;
            try
            {
                this.SetStreams(inStream, outStream, inSize, outSize);
                while (true)
                {
                    long num;
                    long num2;
                    bool flag;
                    this.CodeOneBlock(out num, out num2, out flag);
                    if (flag)
                    {
                        break;
                    }
                    if (progress != null)
                    {
                        progress.SetProgress(num, num2);
                    }
                }
            }
            finally
            {
                this.ReleaseStreams();
            }
        }

        public void CodeOneBlock(out long inSize, out long outSize, out bool finished)
        {
            inSize = 0L;
            outSize = 0L;
            finished = true;
            if (this._inStream != null)
            {
                this._matchFinder.SetStream(this._inStream);
                this._needReleaseMFStream = true;
                this._inStream = null;
            }
            if (!this._finished)
            {
                this._finished = true;
                long num = this.nowPos64;
                if (this.nowPos64 == 0L)
                {
                    uint num2;
                    uint num3;
                    if (this._trainSize > 0)
                    {
                        while (true)
                        {
                            if ((this._trainSize <= 0) || (this._processingMode && this._matchFinder.IsDataStarved))
                            {
                                if (this._trainSize == 0)
                                {
                                    this._previousByte = this._matchFinder.GetIndexByte(-1);
                                }
                                break;
                            }
                            this._matchFinder.Skip(1);
                            this._trainSize--;
                        }
                    }
                    if (this._processingMode && this._matchFinder.IsDataStarved)
                    {
                        this._finished = false;
                        return;
                    }
                    if (this._matchFinder.GetNumAvailableBytes() == 0)
                    {
                        this.Flush((uint) this.nowPos64);
                        return;
                    }
                    this.ReadMatchDistances(out num2, out num3);
                    uint num4 = ((uint) this.nowPos64) & this._posStateMask;
                    this._isMatch[(this._state.Index << 4) + num4].Encode(this._rangeEncoder, 0);
                    this._state.UpdateChar();
                    byte indexByte = this._matchFinder.GetIndexByte((int) -this._additionalOffset);
                    this._literalEncoder.GetSubCoder((uint) this.nowPos64, this._previousByte).Encode(this._rangeEncoder, indexByte);
                    this._previousByte = indexByte;
                    this._additionalOffset--;
                    this.nowPos64 += 1L;
                }
                if (this._processingMode && this._matchFinder.IsDataStarved)
                {
                    this._finished = false;
                }
                else if (this._matchFinder.GetNumAvailableBytes() == 0)
                {
                    this.Flush((uint) this.nowPos64);
                }
                else
                {
                    while (!this._processingMode || !this._matchFinder.IsDataStarved)
                    {
                        uint num6;
                        uint optimum = this.GetOptimum((uint) this.nowPos64, out num6);
                        uint posState = ((uint) this.nowPos64) & this._posStateMask;
                        uint index = (this._state.Index << 4) + posState;
                        if ((optimum == 1) && (num6 == uint.MaxValue))
                        {
                            this._isMatch[index].Encode(this._rangeEncoder, 0);
                            byte indexByte = this._matchFinder.GetIndexByte((int) -this._additionalOffset);
                            LiteralEncoder.Encoder2 subCoder = this._literalEncoder.GetSubCoder((uint) this.nowPos64, this._previousByte);
                            if (this._state.IsCharState())
                            {
                                subCoder.Encode(this._rangeEncoder, indexByte);
                            }
                            else
                            {
                                byte matchByte = this._matchFinder.GetIndexByte((int) ((-this._repDistances[0] - 1) - this._additionalOffset));
                                subCoder.EncodeMatched(this._rangeEncoder, matchByte, indexByte);
                            }
                            this._previousByte = indexByte;
                            this._state.UpdateChar();
                        }
                        else
                        {
                            this._isMatch[index].Encode(this._rangeEncoder, 1);
                            if (num6 >= 4)
                            {
                                this._isRep[this._state.Index].Encode(this._rangeEncoder, 0);
                                this._state.UpdateMatch();
                                this._lenEncoder.Encode(this._rangeEncoder, optimum - 2, posState);
                                num6 -= 4;
                                uint posSlot = GetPosSlot(num6);
                                uint lenToPosState = Base.GetLenToPosState(optimum);
                                this._posSlotEncoder[lenToPosState].Encode(this._rangeEncoder, posSlot);
                                if (posSlot >= 4)
                                {
                                    int numBitLevels = ((int) (posSlot >> 1)) - 1;
                                    uint num17 = (uint) ((2 | (posSlot & 1)) << (numBitLevels & 0x1f));
                                    uint symbol = num6 - num17;
                                    if (posSlot < 14)
                                    {
                                        BitTreeEncoder.ReverseEncode(this._posEncoders, (num17 - posSlot) - 1, this._rangeEncoder, numBitLevels, symbol);
                                    }
                                    else
                                    {
                                        this._rangeEncoder.EncodeDirectBits(symbol >> 4, numBitLevels - 4);
                                        this._posAlignEncoder.ReverseEncode(this._rangeEncoder, symbol & ((uint) 15));
                                        this._alignPriceCount++;
                                    }
                                }
                                uint num19 = num6;
                                uint num20 = 3;
                                while (true)
                                {
                                    if (num20 < 1)
                                    {
                                        this._repDistances[0] = num19;
                                        this._matchPriceCount++;
                                        break;
                                    }
                                    this._repDistances[num20] = this._repDistances[(int) ((IntPtr) (num20 - 1))];
                                    num20--;
                                }
                            }
                            else
                            {
                                this._isRep[this._state.Index].Encode(this._rangeEncoder, 1);
                                if (num6 == 0)
                                {
                                    this._isRepG0[this._state.Index].Encode(this._rangeEncoder, 0);
                                    if (optimum == 1)
                                    {
                                        this._isRep0Long[index].Encode(this._rangeEncoder, 0);
                                    }
                                    else
                                    {
                                        this._isRep0Long[index].Encode(this._rangeEncoder, 1);
                                    }
                                }
                                else
                                {
                                    this._isRepG0[this._state.Index].Encode(this._rangeEncoder, 1);
                                    if (num6 == 1)
                                    {
                                        this._isRepG1[this._state.Index].Encode(this._rangeEncoder, 0);
                                    }
                                    else
                                    {
                                        this._isRepG1[this._state.Index].Encode(this._rangeEncoder, 1);
                                        this._isRepG2[this._state.Index].Encode(this._rangeEncoder, num6 - 2);
                                    }
                                }
                                if (optimum == 1)
                                {
                                    this._state.UpdateShortRep();
                                }
                                else
                                {
                                    this._repMatchLenEncoder.Encode(this._rangeEncoder, optimum - 2, posState);
                                    this._state.UpdateRep();
                                }
                                uint num12 = this._repDistances[num6];
                                if (num6 != 0)
                                {
                                    uint num13 = num6;
                                    while (true)
                                    {
                                        if (num13 < 1)
                                        {
                                            this._repDistances[0] = num12;
                                            break;
                                        }
                                        this._repDistances[num13] = this._repDistances[(int) ((IntPtr) (num13 - 1))];
                                        num13--;
                                    }
                                }
                            }
                            this._previousByte = this._matchFinder.GetIndexByte((int) ((optimum - 1) - this._additionalOffset));
                        }
                        this._additionalOffset -= optimum;
                        this.nowPos64 += optimum;
                        if (this._additionalOffset == 0)
                        {
                            if (this._matchPriceCount >= 0x80)
                            {
                                this.FillDistancesPrices();
                            }
                            if (this._alignPriceCount >= 0x10)
                            {
                                this.FillAlignPrices();
                            }
                            inSize = this.nowPos64;
                            outSize = this._rangeEncoder.GetProcessedSizeAdd();
                            if (this._processingMode && this._matchFinder.IsDataStarved)
                            {
                                this._finished = false;
                                return;
                            }
                            if (this._matchFinder.GetNumAvailableBytes() == 0)
                            {
                                this.Flush((uint) this.nowPos64);
                                return;
                            }
                            if ((this.nowPos64 - num) >= 0x1000L)
                            {
                                this._finished = false;
                                finished = false;
                                return;
                            }
                        }
                    }
                    this._finished = false;
                }
            }
        }

        private void Create()
        {
            if (this._matchFinder == null)
            {
                BinTree tree = new BinTree();
                int numHashBytes = 4;
                if (this._matchFinderType == EMatchFinderType.BT2)
                {
                    numHashBytes = 2;
                }
                tree.SetType(numHashBytes);
                this._matchFinder = tree;
            }
            this._literalEncoder.Create(this._numLiteralPosStateBits, this._numLiteralContextBits);
            if ((this._dictionarySize != this._dictionarySizePrev) || (this._numFastBytesPrev != this._numFastBytes))
            {
                this._matchFinder.Create(this._dictionarySize, 0x1000, this._numFastBytes, 0x1112);
                this._dictionarySizePrev = this._dictionarySize;
                this._numFastBytesPrev = this._numFastBytes;
            }
        }

        private void FillAlignPrices()
        {
            for (uint i = 0; i < 0x10; i++)
            {
                this._alignPrices[i] = this._posAlignEncoder.ReverseGetPrice(i);
            }
            this._alignPriceCount = 0;
        }

        private unsafe void FillDistancesPrices()
        {
            for (uint i = 4; i < 0x80; i++)
            {
                uint posSlot = GetPosSlot(i);
                int numBitLevels = ((int) (posSlot >> 1)) - 1;
                uint num4 = (uint) ((2 | (posSlot & 1)) << (numBitLevels & 0x1f));
                this.tempPrices[i] = BitTreeEncoder.ReverseGetPrice(this._posEncoders, (num4 - posSlot) - 1, numBitLevels, i - num4);
            }
            uint index = 0;
            while (index < 4)
            {
                BitTreeEncoder encoder = this._posSlotEncoder[index];
                uint num7 = index << 6;
                uint symbol = 0;
                while (true)
                {
                    if (symbol >= this._distTableSize)
                    {
                        symbol = 14;
                        while (true)
                        {
                            if (symbol >= this._distTableSize)
                            {
                                uint num8 = index * 0x80;
                                uint pos = 0;
                                while (true)
                                {
                                    if (pos >= 4)
                                    {
                                        while (true)
                                        {
                                            if (pos >= 0x80)
                                            {
                                                index++;
                                                break;
                                            }
                                            this._distancesPrices[num8 + pos] = this._posSlotPrices[num7 + GetPosSlot(pos)] + this.tempPrices[pos];
                                            pos++;
                                        }
                                        break;
                                    }
                                    this._distancesPrices[num8 + pos] = this._posSlotPrices[num7 + pos];
                                    pos++;
                                }
                                break;
                            }
                            uint* numPtr1 = &(this._posSlotPrices[num7 + symbol]);
                            numPtr1[0] += (uint) ((((symbol >> 1) - 1) - 4) << 6);
                            symbol++;
                        }
                        break;
                    }
                    this._posSlotPrices[num7 + symbol] = encoder.GetPrice(symbol);
                    symbol++;
                }
            }
            this._matchPriceCount = 0;
        }

        private static int FindMatchFinder(string s)
        {
            for (int i = 0; i < kMatchFinderIDs.Length; i++)
            {
                if (s == kMatchFinderIDs[i])
                {
                    return i;
                }
            }
            return -1;
        }

        private void Flush(uint nowPos)
        {
            this.ReleaseMFStream();
            this.WriteEndMarker(nowPos & this._posStateMask);
            this._rangeEncoder.FlushData();
            this._rangeEncoder.FlushStream();
        }

        private uint GetOptimum(uint position, out uint backRes)
        {
            uint num3;
            uint num4;
            byte indexByte;
            byte indexByte;
            uint num11;
            uint num12;
            uint num13;
            uint num15;
            uint num20;
            uint num24;
            Base.State state;
            uint num31;
            uint num51;
            uint num52;
            if (this._optimumEndIndex != this._optimumCurrentIndex)
            {
                uint num = this._optimum[this._optimumCurrentIndex].PosPrev - this._optimumCurrentIndex;
                backRes = this._optimum[this._optimumCurrentIndex].BackPrev;
                this._optimumCurrentIndex = this._optimum[this._optimumCurrentIndex].PosPrev;
                return num;
            }
            this._optimumCurrentIndex = this._optimumEndIndex = 0;
            if (!this._longestMatchWasFound)
            {
                this.ReadMatchDistances(out num3, out num4);
            }
            else
            {
                num3 = this._longestMatchLength;
                num4 = this._numDistancePairs;
                this._longestMatchWasFound = false;
            }
            uint limit = this._matchFinder.GetNumAvailableBytes() + 1;
            if (limit < 2)
            {
                backRes = uint.MaxValue;
                return 1;
            }
            if (limit > 0x111)
            {
                limit = 0x111;
            }
            uint index = 0;
            uint num7 = 0;
            while (true)
            {
                if (num7 >= 4)
                {
                    if (this.repLens[index] >= this._numFastBytes)
                    {
                        backRes = index;
                        uint num8 = this.repLens[index];
                        this.MovePos(num8 - 1);
                        return num8;
                    }
                    if (num3 >= this._numFastBytes)
                    {
                        backRes = this._matchDistances[(int) ((IntPtr) (num4 - 1))] + 4;
                        this.MovePos(num3 - 1);
                        return num3;
                    }
                    indexByte = this._matchFinder.GetIndexByte(-1);
                    indexByte = this._matchFinder.GetIndexByte((((int) -this._repDistances[0]) - 1) - 1);
                    if ((num3 < 2) && ((indexByte != indexByte) && (this.repLens[index] < 2)))
                    {
                        backRes = uint.MaxValue;
                        return 1;
                    }
                    this._optimum[0].State = this._state;
                    num11 = position & this._posStateMask;
                    this._optimum[1].Price = this._isMatch[(this._state.Index << 4) + num11].GetPrice0() + this._literalEncoder.GetSubCoder(position, this._previousByte).GetPrice(!this._state.IsCharState(), indexByte, indexByte);
                    this._optimum[1].MakeAsChar();
                    num12 = this._isMatch[(this._state.Index << 4) + num11].GetPrice1();
                    num13 = num12 + this._isRep[this._state.Index].GetPrice1();
                    if (indexByte == indexByte)
                    {
                        uint num14 = num13 + this.GetRepLen1Price(this._state, num11);
                        if (num14 < this._optimum[1].Price)
                        {
                            this._optimum[1].Price = num14;
                            this._optimum[1].MakeAsShortRep();
                        }
                    }
                    num15 = (num3 < this.repLens[index]) ? this.repLens[index] : num3;
                    if (num15 < 2)
                    {
                        backRes = this._optimum[1].BackPrev;
                        return 1;
                    }
                    this._optimum[1].PosPrev = 0;
                    this._optimum[0].Backs0 = this.reps[0];
                    this._optimum[0].Backs1 = this.reps[1];
                    this._optimum[0].Backs2 = this.reps[2];
                    this._optimum[0].Backs3 = this.reps[3];
                    uint len = num15;
                    while (true)
                    {
                        this._optimum[len--].Price = 0xfffffff;
                        if (len < 2)
                        {
                            num7 = 0;
                            while (true)
                            {
                                if (num7 >= 4)
                                {
                                    num20 = num12 + this._isRep[this._state.Index].GetPrice0();
                                    len = (this.repLens[0] < 2) ? 2 : (this.repLens[0] + 1);
                                    if (len <= num3)
                                    {
                                        uint num21 = 0;
                                        while (true)
                                        {
                                            if (len <= this._matchDistances[num21])
                                            {
                                                while (true)
                                                {
                                                    uint pos = this._matchDistances[(int) ((IntPtr) (num21 + 1))];
                                                    uint num23 = num20 + this.GetPosLenPrice(pos, len, num11);
                                                    Optimal optimal2 = this._optimum[len];
                                                    if (num23 < optimal2.Price)
                                                    {
                                                        optimal2.Price = num23;
                                                        optimal2.PosPrev = 0;
                                                        optimal2.BackPrev = pos + 4;
                                                        optimal2.Prev1IsChar = false;
                                                    }
                                                    if (len == this._matchDistances[num21])
                                                    {
                                                        num21 += 2;
                                                        if (num21 == num4)
                                                        {
                                                            break;
                                                        }
                                                    }
                                                    len++;
                                                }
                                                break;
                                            }
                                            num21 += 2;
                                        }
                                    }
                                    num24 = 0;
                                    break;
                                }
                                uint num17 = this.repLens[num7];
                                if (num17 >= 2)
                                {
                                    uint num18 = num13 + this.GetPureRepPrice(num7, this._state, num11);
                                    do
                                    {
                                        uint num19 = num18 + this._repMatchLenEncoder.GetPrice(num17 - 2, num11);
                                        Optimal optimal = this._optimum[num17];
                                        if (num19 < optimal.Price)
                                        {
                                            optimal.Price = num19;
                                            optimal.PosPrev = 0;
                                            optimal.BackPrev = num7;
                                            optimal.Prev1IsChar = false;
                                        }
                                    }
                                    while (--num17 >= 2);
                                }
                                num7++;
                            }
                            break;
                        }
                    }
                    break;
                }
                this.reps[num7] = this._repDistances[num7];
                this.repLens[num7] = this._matchFinder.GetMatchLen(-1, this.reps[num7], 0x111);
                if (this.repLens[num7] > this.repLens[index])
                {
                    index = num7;
                }
                num7++;
            }
            goto TR_007E;
        TR_0008:
            num52++;
        TR_0019:
            while (true)
            {
                uint pos = this._matchDistances[(int) ((IntPtr) (num51 + 1))];
                uint num54 = num20 + this.GetPosLenPrice(pos, num52, num11);
                Optimal optimal8 = this._optimum[num24 + num52];
                if (num54 < optimal8.Price)
                {
                    optimal8.Price = num54;
                    optimal8.PosPrev = num24;
                    optimal8.BackPrev = pos + 4;
                    optimal8.Prev1IsChar = false;
                }
                if (num52 != this._matchDistances[num51])
                {
                    goto TR_0008;
                }
                else
                {
                    if (num52 < num31)
                    {
                        uint len = this._matchFinder.GetMatchLen((int) num52, pos, Math.Min((num31 - 1) - num52, this._numFastBytes));
                        if (len >= 2)
                        {
                            Base.State state4 = state;
                            state4.UpdateMatch();
                            uint posState = (position + num52) & this._posStateMask;
                            state4.UpdateChar();
                            posState = ((position + num52) + 1) & this._posStateMask;
                            uint num60 = (((num54 + this._isMatch[(state4.Index << 4) + posState].GetPrice0()) + this._literalEncoder.GetSubCoder(position + num52, this._matchFinder.GetIndexByte((((int) num52) - 1) - 1)).GetPrice(true, this._matchFinder.GetIndexByte(((int) (num52 - (pos + 1))) - 1), this._matchFinder.GetIndexByte(((int) num52) - 1))) + this._isMatch[(state4.Index << 4) + posState].GetPrice1()) + this._isRep[state4.Index].GetPrice1();
                            uint num61 = (num52 + 1) + len;
                            while (true)
                            {
                                if (num15 >= (num24 + num61))
                                {
                                    num54 = num60 + this.GetRepPrice(0, len, state4, posState);
                                    optimal8 = this._optimum[num24 + num61];
                                    if (num54 < optimal8.Price)
                                    {
                                        optimal8.Price = num54;
                                        optimal8.PosPrev = (num24 + num52) + 1;
                                        optimal8.BackPrev = 0;
                                        optimal8.Prev1IsChar = true;
                                        optimal8.Prev2 = true;
                                        optimal8.PosPrev2 = num24;
                                        optimal8.BackPrev2 = pos + 4;
                                    }
                                    break;
                                }
                                this._optimum[++num15].Price = 0xfffffff;
                            }
                        }
                    }
                    num51 += 2;
                    if (num51 != num4)
                    {
                        goto TR_0008;
                    }
                }
                break;
            }
        TR_007E:
            while (true)
            {
                uint num25;
                num24++;
                if (num24 == num15)
                {
                    return this.Backward(out backRes, num24);
                }
                this.ReadMatchDistances(out num25, out num4);
                if (num25 >= this._numFastBytes)
                {
                    this._numDistancePairs = num4;
                    this._longestMatchLength = num25;
                    this._longestMatchWasFound = true;
                    return this.Backward(out backRes, num24);
                }
                position++;
                uint posPrev = this._optimum[num24].PosPrev;
                if (!this._optimum[num24].Prev1IsChar)
                {
                    state = this._optimum[posPrev].State;
                }
                else
                {
                    posPrev--;
                    if (!this._optimum[num24].Prev2)
                    {
                        state = this._optimum[posPrev].State;
                    }
                    else
                    {
                        state = this._optimum[this._optimum[num24].PosPrev2].State;
                        if (this._optimum[num24].BackPrev2 < 4)
                        {
                            state.UpdateRep();
                        }
                        else
                        {
                            state.UpdateMatch();
                        }
                    }
                    state.UpdateChar();
                }
                if (posPrev == (num24 - 1))
                {
                    if (this._optimum[num24].IsShortRep())
                    {
                        state.UpdateShortRep();
                    }
                    else
                    {
                        state.UpdateChar();
                    }
                }
                else
                {
                    uint backPrev;
                    if (this._optimum[num24].Prev1IsChar && this._optimum[num24].Prev2)
                    {
                        posPrev = this._optimum[num24].PosPrev2;
                        backPrev = this._optimum[num24].BackPrev2;
                        state.UpdateRep();
                    }
                    else
                    {
                        backPrev = this._optimum[num24].BackPrev;
                        if (backPrev < 4)
                        {
                            state.UpdateRep();
                        }
                        else
                        {
                            state.UpdateMatch();
                        }
                    }
                    Optimal optimal3 = this._optimum[posPrev];
                    if (backPrev >= 4)
                    {
                        this.reps[0] = backPrev - 4;
                        this.reps[1] = optimal3.Backs0;
                        this.reps[2] = optimal3.Backs1;
                        this.reps[3] = optimal3.Backs2;
                    }
                    else if (backPrev == 0)
                    {
                        this.reps[0] = optimal3.Backs0;
                        this.reps[1] = optimal3.Backs1;
                        this.reps[2] = optimal3.Backs2;
                        this.reps[3] = optimal3.Backs3;
                    }
                    else if (backPrev == 1)
                    {
                        this.reps[0] = optimal3.Backs1;
                        this.reps[1] = optimal3.Backs0;
                        this.reps[2] = optimal3.Backs2;
                        this.reps[3] = optimal3.Backs3;
                    }
                    else if (backPrev == 2)
                    {
                        this.reps[0] = optimal3.Backs2;
                        this.reps[1] = optimal3.Backs0;
                        this.reps[2] = optimal3.Backs1;
                        this.reps[3] = optimal3.Backs3;
                    }
                    else
                    {
                        this.reps[0] = optimal3.Backs3;
                        this.reps[1] = optimal3.Backs0;
                        this.reps[2] = optimal3.Backs1;
                        this.reps[3] = optimal3.Backs2;
                    }
                }
                this._optimum[num24].State = state;
                this._optimum[num24].Backs0 = this.reps[0];
                this._optimum[num24].Backs1 = this.reps[1];
                this._optimum[num24].Backs2 = this.reps[2];
                this._optimum[num24].Backs3 = this.reps[3];
                uint price = this._optimum[num24].Price;
                indexByte = this._matchFinder.GetIndexByte(-1);
                indexByte = this._matchFinder.GetIndexByte((((int) -this.reps[0]) - 1) - 1);
                num11 = position & this._posStateMask;
                uint num29 = (price + this._isMatch[(state.Index << 4) + num11].GetPrice0()) + this._literalEncoder.GetSubCoder(position, this._matchFinder.GetIndexByte(-2)).GetPrice(!state.IsCharState(), indexByte, indexByte);
                Optimal optimal4 = this._optimum[(int) ((IntPtr) (num24 + 1))];
                bool flag = false;
                if (num29 < optimal4.Price)
                {
                    optimal4.Price = num29;
                    optimal4.PosPrev = num24;
                    optimal4.MakeAsChar();
                    flag = true;
                }
                num12 = price + this._isMatch[(state.Index << 4) + num11].GetPrice1();
                num13 = num12 + this._isRep[state.Index].GetPrice1();
                if ((indexByte == indexByte) && ((optimal4.PosPrev >= num24) || (optimal4.BackPrev != 0)))
                {
                    uint num30 = num13 + this.GetRepLen1Price(state, num11);
                    if (num30 <= optimal4.Price)
                    {
                        optimal4.Price = num30;
                        optimal4.PosPrev = num24;
                        optimal4.MakeAsShortRep();
                        flag = true;
                    }
                }
                num31 = this._matchFinder.GetNumAvailableBytes() + 1;
                num31 = Math.Min(0xfff - num24, num31);
                limit = num31;
                if (limit >= 2)
                {
                    if (limit > this._numFastBytes)
                    {
                        limit = this._numFastBytes;
                    }
                    if (!flag && (indexByte != indexByte))
                    {
                        uint num32 = Math.Min(num31 - 1, this._numFastBytes);
                        uint len = this._matchFinder.GetMatchLen(0, this.reps[0], num32);
                        if (len >= 2)
                        {
                            Base.State state2 = state;
                            state2.UpdateChar();
                            uint posState = (position + 1) & this._posStateMask;
                            uint num35 = (num29 + this._isMatch[(state2.Index << 4) + posState].GetPrice1()) + this._isRep[state2.Index].GetPrice1();
                            uint num36 = (num24 + 1) + len;
                            while (true)
                            {
                                if (num15 >= num36)
                                {
                                    uint num37 = num35 + this.GetRepPrice(0, len, state2, posState);
                                    Optimal optimal5 = this._optimum[num36];
                                    if (num37 < optimal5.Price)
                                    {
                                        optimal5.Price = num37;
                                        optimal5.PosPrev = num24 + 1;
                                        optimal5.BackPrev = 0;
                                        optimal5.Prev1IsChar = true;
                                        optimal5.Prev2 = false;
                                    }
                                    break;
                                }
                                this._optimum[++num15].Price = 0xfffffff;
                            }
                        }
                    }
                    uint num38 = 2;
                    uint num39 = 0;
                    while (true)
                    {
                        if (num39 < 4)
                        {
                            uint len = this._matchFinder.GetMatchLen(-1, this.reps[num39], limit);
                            if (len >= 2)
                            {
                                uint num41 = len;
                                while (true)
                                {
                                    if (num15 < (num24 + len))
                                    {
                                        this._optimum[++num15].Price = 0xfffffff;
                                        continue;
                                    }
                                    uint num42 = num13 + this.GetRepPrice(num39, len, state, num11);
                                    Optimal optimal6 = this._optimum[num24 + len];
                                    if (num42 < optimal6.Price)
                                    {
                                        optimal6.Price = num42;
                                        optimal6.PosPrev = num24;
                                        optimal6.BackPrev = num39;
                                        optimal6.Prev1IsChar = false;
                                    }
                                    if (--len < 2)
                                    {
                                        len = num41;
                                        if (num39 == 0)
                                        {
                                            num38 = len + 1;
                                        }
                                        if (len < num31)
                                        {
                                            uint num43 = Math.Min((num31 - 1) - len, this._numFastBytes);
                                            uint num44 = this._matchFinder.GetMatchLen((int) len, this.reps[num39], num43);
                                            if (num44 >= 2)
                                            {
                                                Base.State state3 = state;
                                                state3.UpdateRep();
                                                uint posState = (position + len) & this._posStateMask;
                                                state3.UpdateChar();
                                                posState = ((position + len) + 1) & this._posStateMask;
                                                uint num48 = ((((num13 + this.GetRepPrice(num39, len, state, num11)) + this._isMatch[(state3.Index << 4) + posState].GetPrice0()) + this._literalEncoder.GetSubCoder(position + len, this._matchFinder.GetIndexByte((((int) len) - 1) - 1)).GetPrice(true, this._matchFinder.GetIndexByte((int) ((len - 1) - (this.reps[num39] + 1))), this._matchFinder.GetIndexByte(((int) len) - 1))) + this._isMatch[(state3.Index << 4) + posState].GetPrice1()) + this._isRep[state3.Index].GetPrice1();
                                                uint num49 = (len + 1) + num44;
                                                while (true)
                                                {
                                                    if (num15 >= (num24 + num49))
                                                    {
                                                        uint num50 = num48 + this.GetRepPrice(0, num44, state3, posState);
                                                        Optimal optimal7 = this._optimum[num24 + num49];
                                                        if (num50 < optimal7.Price)
                                                        {
                                                            optimal7.Price = num50;
                                                            optimal7.PosPrev = (num24 + len) + 1;
                                                            optimal7.BackPrev = 0;
                                                            optimal7.Prev1IsChar = true;
                                                            optimal7.Prev2 = true;
                                                            optimal7.PosPrev2 = num24;
                                                            optimal7.BackPrev2 = num39;
                                                        }
                                                        break;
                                                    }
                                                    this._optimum[++num15].Price = 0xfffffff;
                                                }
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                            num39++;
                            continue;
                        }
                        if (num25 > limit)
                        {
                            num25 = limit;
                            num4 = 0;
                            while (true)
                            {
                                if (num25 <= this._matchDistances[num4])
                                {
                                    this._matchDistances[num4] = num25;
                                    num4 += 2;
                                    break;
                                }
                                num4 += 2;
                            }
                        }
                        if (num25 < num38)
                        {
                            continue;
                        }
                        else
                        {
                            num20 = num12 + this._isRep[state.Index].GetPrice0();
                            while (true)
                            {
                                if (num15 >= (num24 + num25))
                                {
                                    num51 = 0;
                                    while (true)
                                    {
                                        if (num38 <= this._matchDistances[num51])
                                        {
                                            num52 = num38;
                                            break;
                                        }
                                        num51 += 2;
                                    }
                                    break;
                                }
                                this._optimum[++num15].Price = 0xfffffff;
                            }
                        }
                        break;
                    }
                    break;
                }
            }
            goto TR_0019;
        }

        private uint GetPosLenPrice(uint pos, uint len, uint posState)
        {
            uint lenToPosState = Base.GetLenToPosState(len);
            uint num = (pos >= 0x80) ? (this._posSlotPrices[(lenToPosState << 6) + GetPosSlot2(pos)] + this._alignPrices[(int) ((IntPtr) (pos & 15))]) : this._distancesPrices[(int) ((IntPtr) ((lenToPosState * 0x80) + pos))];
            return (num + this._lenEncoder.GetPrice(len - 2, posState));
        }

        private static uint GetPosSlot(uint pos) => 
            (pos >= 0x800) ? ((pos >= 0x200000) ? ((uint) (g_FastPos[pos >> 20] + 40)) : ((uint) (g_FastPos[pos >> 10] + 20))) : ((uint) g_FastPos[pos]);

        private static uint GetPosSlot2(uint pos) => 
            (pos >= 0x20000) ? ((pos >= 0x8000000) ? ((uint) (g_FastPos[pos >> 0x1a] + 0x34)) : ((uint) (g_FastPos[pos >> 0x10] + 0x20))) : ((uint) (g_FastPos[pos >> 6] + 12));

        private uint GetPureRepPrice(uint repIndex, Base.State state, uint posState)
        {
            uint num;
            if (repIndex == 0)
            {
                num = this._isRepG0[state.Index].GetPrice0() + this._isRep0Long[(state.Index << 4) + posState].GetPrice1();
            }
            else
            {
                num = this._isRepG0[state.Index].GetPrice1();
                num = (repIndex != 1) ? ((num + this._isRepG1[state.Index].GetPrice1()) + this._isRepG2[state.Index].GetPrice(repIndex - 2)) : (num + this._isRepG1[state.Index].GetPrice0());
            }
            return num;
        }

        private uint GetRepLen1Price(Base.State state, uint posState) => 
            this._isRepG0[state.Index].GetPrice0() + this._isRep0Long[(state.Index << 4) + posState].GetPrice0();

        private uint GetRepPrice(uint repIndex, uint len, Base.State state, uint posState) => 
            this._repMatchLenEncoder.GetPrice(len - 2, posState) + this.GetPureRepPrice(repIndex, state, posState);

        private void Init()
        {
            this.BaseInit();
            this._rangeEncoder.Init();
            uint index = 0;
            while (index < 12)
            {
                uint num2 = 0;
                while (true)
                {
                    if (num2 > this._posStateMask)
                    {
                        this._isRep[index].Init();
                        this._isRepG0[index].Init();
                        this._isRepG1[index].Init();
                        this._isRepG2[index].Init();
                        index++;
                        break;
                    }
                    uint num3 = (index << 4) + num2;
                    this._isMatch[num3].Init();
                    this._isRep0Long[num3].Init();
                    num2++;
                }
            }
            this._literalEncoder.Init();
            for (index = 0; index < 4; index++)
            {
                this._posSlotEncoder[index].Init();
            }
            for (index = 0; index < 0x72; index++)
            {
                this._posEncoders[index].Init();
            }
            this._lenEncoder.Init((uint) (1 << (this._posStateBits & 0x1f)));
            this._repMatchLenEncoder.Init((uint) (1 << (this._posStateBits & 0x1f)));
            this._posAlignEncoder.Init();
            this._longestMatchWasFound = false;
            this._optimumEndIndex = 0;
            this._optimumCurrentIndex = 0;
            this._additionalOffset = 0;
        }

        private void MovePos(uint num)
        {
            if (num > 0)
            {
                this._matchFinder.Skip(num);
                this._additionalOffset += num;
            }
        }

        private void ReadMatchDistances(out uint lenRes, out uint numDistancePairs)
        {
            lenRes = 0;
            numDistancePairs = this._matchFinder.GetMatches(this._matchDistances);
            if (numDistancePairs > 0)
            {
                lenRes = this._matchDistances[(int) ((IntPtr) (numDistancePairs - 2))];
                if (lenRes == this._numFastBytes)
                {
                    lenRes += this._matchFinder.GetMatchLen(((int) lenRes) - 1, this._matchDistances[(int) ((IntPtr) (numDistancePairs - 1))], 0x111 - lenRes);
                }
            }
            this._additionalOffset++;
        }

        private void ReleaseMFStream()
        {
            if ((this._matchFinder != null) && this._needReleaseMFStream)
            {
                this._matchFinder.ReleaseStream();
                this._needReleaseMFStream = false;
            }
        }

        private void ReleaseOutStream()
        {
            this._rangeEncoder.ReleaseStream();
        }

        private void ReleaseStreams()
        {
            this.ReleaseMFStream();
            this.ReleaseOutStream();
        }

        public void SetCoderProperties(CoderPropID[] propIDs, object[] properties)
        {
            for (uint i = 0; i < properties.Length; i++)
            {
                object obj2 = properties[i];
                CoderPropID pid = propIDs[i];
                switch (pid)
                {
                    case CoderPropID.PosStateBits:
                    {
                        if (!(obj2 is int))
                        {
                            throw new InvalidParamException();
                        }
                        int num6 = (int) obj2;
                        if ((num6 < 0) || (num6 > 4L))
                        {
                            throw new InvalidParamException();
                        }
                        this._posStateBits = num6;
                        this._posStateMask = (uint) ((1 << (this._posStateBits & 0x1f)) - 1);
                        break;
                    }
                    case CoderPropID.LitContextBits:
                    {
                        if (!(obj2 is int))
                        {
                            throw new InvalidParamException();
                        }
                        int num8 = (int) obj2;
                        if ((num8 < 0) || (num8 > 8L))
                        {
                            throw new InvalidParamException();
                        }
                        this._numLiteralContextBits = num8;
                        break;
                    }
                    case CoderPropID.LitPosBits:
                    {
                        if (!(obj2 is int))
                        {
                            throw new InvalidParamException();
                        }
                        int num7 = (int) obj2;
                        if ((num7 < 0) || (num7 > 4L))
                        {
                            throw new InvalidParamException();
                        }
                        this._numLiteralPosStateBits = num7;
                        break;
                    }
                    case CoderPropID.NumFastBytes:
                    {
                        if (!(obj2 is int))
                        {
                            throw new InvalidParamException();
                        }
                        int num2 = (int) obj2;
                        if ((num2 < 5) || (num2 > 0x111L))
                        {
                            throw new InvalidParamException();
                        }
                        this._numFastBytes = (uint) num2;
                        break;
                    }
                    case CoderPropID.MatchFinder:
                    {
                        if (!(obj2 is string))
                        {
                            throw new InvalidParamException();
                        }
                        EMatchFinderType type = this._matchFinderType;
                        int num3 = FindMatchFinder(((string) obj2).ToUpper());
                        if (num3 < 0)
                        {
                            throw new InvalidParamException();
                        }
                        this._matchFinderType = (EMatchFinderType) num3;
                        if ((this._matchFinder != null) && (type != this._matchFinderType))
                        {
                            this._dictionarySizePrev = uint.MaxValue;
                            this._matchFinder = null;
                        }
                        break;
                    }
                    case CoderPropID.Algorithm:
                        break;

                    case CoderPropID.EndMarker:
                        if (!(obj2 as bool))
                        {
                            throw new InvalidParamException();
                        }
                        this.SetWriteEndMarkerMode((bool) obj2);
                        break;

                    default:
                    {
                        if (pid != CoderPropID.DictionarySize)
                        {
                            throw new InvalidParamException();
                        }
                        if (!(obj2 is int))
                        {
                            throw new InvalidParamException();
                        }
                        int num4 = (int) obj2;
                        if ((num4 < 1L) || (num4 > 0x40000000L))
                        {
                            throw new InvalidParamException();
                        }
                        this._dictionarySize = (uint) num4;
                        int num5 = 0;
                        while (true)
                        {
                            if ((num5 >= 30) || (num4 <= ((ulong) (1 << (num5 & 0x1f)))))
                            {
                                this._distTableSize = (uint) (num5 * 2);
                                break;
                            }
                            num5++;
                        }
                        break;
                    }
                }
            }
        }

        private void SetOutStream(Stream outStream)
        {
            this._rangeEncoder.SetStream(outStream);
        }

        public void SetStreams(Stream inStream, Stream outStream, long inSize, long outSize)
        {
            this._inStream = inStream;
            this._finished = false;
            this.Create();
            this.SetOutStream(outStream);
            this.Init();
            this._matchFinder.Init();
            this.FillDistancesPrices();
            this.FillAlignPrices();
            this._lenEncoder.SetTableSize((this._numFastBytes + 1) - 2);
            this._lenEncoder.UpdateTables((uint) (1 << (this._posStateBits & 0x1f)));
            this._repMatchLenEncoder.SetTableSize((this._numFastBytes + 1) - 2);
            this._repMatchLenEncoder.UpdateTables((uint) (1 << (this._posStateBits & 0x1f)));
            this.nowPos64 = 0L;
        }

        public void SetTrainSize(uint trainSize)
        {
            this._trainSize = trainSize;
        }

        private void SetWriteEndMarkerMode(bool writeEndMarker)
        {
            this._writeEndMark = writeEndMarker;
        }

        public void Train(Stream trainStream)
        {
            if (this.nowPos64 > 0L)
            {
                throw new InvalidOperationException();
            }
            this._trainSize = (uint) trainStream.Length;
            if (this._trainSize > 0)
            {
                this._matchFinder.SetStream(trainStream);
                while (true)
                {
                    if ((this._trainSize <= 0) || this._matchFinder.IsDataStarved)
                    {
                        if (this._trainSize == 0)
                        {
                            this._previousByte = this._matchFinder.GetIndexByte(-1);
                        }
                        this._matchFinder.ReleaseStream();
                        break;
                    }
                    this._matchFinder.Skip(1);
                    this._trainSize--;
                }
            }
        }

        public void WriteCoderProperties(Stream outStream)
        {
            this.properties[0] = (byte) ((((this._posStateBits * 5) + this._numLiteralPosStateBits) * 9) + this._numLiteralContextBits);
            for (int i = 0; i < 4; i++)
            {
                this.properties[1 + i] = (byte) ((this._dictionarySize >> ((8 * i) & 0x1f)) & 0xff);
            }
            outStream.Write(this.properties, 0, 5);
        }

        private void WriteEndMarker(uint posState)
        {
            if (this._writeEndMark)
            {
                this._isMatch[(this._state.Index << 4) + posState].Encode(this._rangeEncoder, 1);
                this._isRep[this._state.Index].Encode(this._rangeEncoder, 0);
                this._state.UpdateMatch();
                uint len = 2;
                this._lenEncoder.Encode(this._rangeEncoder, len - 2, posState);
                uint symbol = 0x3f;
                uint lenToPosState = Base.GetLenToPosState(len);
                this._posSlotEncoder[lenToPosState].Encode(this._rangeEncoder, symbol);
                int num4 = 30;
                uint num5 = (uint) ((1 << (num4 & 0x1f)) - 1);
                this._rangeEncoder.EncodeDirectBits(num5 >> 4, num4 - 4);
                this._posAlignEncoder.ReverseEncode(this._rangeEncoder, num5 & ((uint) 15));
            }
        }

        private enum EMatchFinderType
        {
            BT2,
            BT4
        }

        private class LenEncoder
        {
            private BitEncoder _choice;
            private BitEncoder _choice2;
            private BitTreeEncoder[] _lowCoder;
            private BitTreeEncoder[] _midCoder;
            private BitTreeEncoder _highCoder;

            public LenEncoder()
            {
                BitEncoder encoder = new BitEncoder();
                this._choice = encoder;
                BitEncoder encoder2 = new BitEncoder();
                this._choice2 = encoder2;
                this._lowCoder = new BitTreeEncoder[0x10];
                this._midCoder = new BitTreeEncoder[0x10];
                this._highCoder = new BitTreeEncoder(8);
                for (uint i = 0; i < 0x10; i++)
                {
                    this._lowCoder[i] = new BitTreeEncoder(3);
                    this._midCoder[i] = new BitTreeEncoder(3);
                }
            }

            public void Encode(Encoder rangeEncoder, uint symbol, uint posState)
            {
                if (symbol < 8)
                {
                    this._choice.Encode(rangeEncoder, 0);
                    this._lowCoder[posState].Encode(rangeEncoder, symbol);
                }
                else
                {
                    symbol -= 8;
                    this._choice.Encode(rangeEncoder, 1);
                    if (symbol < 8)
                    {
                        this._choice2.Encode(rangeEncoder, 0);
                        this._midCoder[posState].Encode(rangeEncoder, symbol);
                    }
                    else
                    {
                        this._choice2.Encode(rangeEncoder, 1);
                        this._highCoder.Encode(rangeEncoder, symbol - 8);
                    }
                }
            }

            public void Init(uint numPosStates)
            {
                this._choice.Init();
                this._choice2.Init();
                for (uint i = 0; i < numPosStates; i++)
                {
                    this._lowCoder[i].Init();
                    this._midCoder[i].Init();
                }
                this._highCoder.Init();
            }

            public void SetPrices(uint posState, uint numSymbols, uint[] prices, uint st)
            {
                uint num = this._choice.GetPrice0();
                uint num2 = this._choice.GetPrice1();
                uint num3 = num2 + this._choice2.GetPrice0();
                uint num4 = num2 + this._choice2.GetPrice1();
                uint symbol = 0;
                symbol = 0;
                while (symbol < 8)
                {
                    if (symbol >= numSymbols)
                    {
                        return;
                    }
                    prices[st + symbol] = num + this._lowCoder[posState].GetPrice(symbol);
                    symbol++;
                }
                while (symbol < 0x10)
                {
                    if (symbol >= numSymbols)
                    {
                        return;
                    }
                    prices[st + symbol] = num3 + this._midCoder[posState].GetPrice(symbol - 8);
                    symbol++;
                }
                while (symbol < numSymbols)
                {
                    prices[st + symbol] = num4 + this._highCoder.GetPrice((symbol - 8) - 8);
                    symbol++;
                }
            }
        }

        private class LenPriceTableEncoder : Encoder.LenEncoder
        {
            private uint[] _prices = new uint[0x1100];
            private uint _tableSize;
            private uint[] _counters = new uint[0x10];

            public unsafe void Encode(Encoder rangeEncoder, uint symbol, uint posState)
            {
                base.Encode(rangeEncoder, symbol, posState);
                uint* numPtr1 = &(this._counters[posState]);
                if (--numPtr1[0] == 0)
                {
                    this.UpdateTable(posState);
                }
            }

            public uint GetPrice(uint symbol, uint posState) => 
                this._prices[(int) ((IntPtr) ((posState * 0x110) + symbol))];

            public void SetTableSize(uint tableSize)
            {
                this._tableSize = tableSize;
            }

            private void UpdateTable(uint posState)
            {
                base.SetPrices(posState, this._tableSize, this._prices, posState * 0x110);
                this._counters[posState] = this._tableSize;
            }

            public void UpdateTables(uint numPosStates)
            {
                for (uint i = 0; i < numPosStates; i++)
                {
                    this.UpdateTable(i);
                }
            }
        }

        private class LiteralEncoder
        {
            private Encoder2[] m_Coders;
            private int m_NumPrevBits;
            private int m_NumPosBits;
            private uint m_PosMask;

            public void Create(int numPosBits, int numPrevBits)
            {
                if ((this.m_Coders == null) || ((this.m_NumPrevBits != numPrevBits) || (this.m_NumPosBits != numPosBits)))
                {
                    this.m_NumPosBits = numPosBits;
                    this.m_PosMask = (uint) ((1 << (numPosBits & 0x1f)) - 1);
                    this.m_NumPrevBits = numPrevBits;
                    uint num = (uint) (1 << ((this.m_NumPrevBits + this.m_NumPosBits) & 0x1f));
                    this.m_Coders = new Encoder2[num];
                    for (uint i = 0; i < num; i++)
                    {
                        this.m_Coders[i].Create();
                    }
                }
            }

            public Encoder2 GetSubCoder(uint pos, byte prevByte) => 
                this.m_Coders[(int) ((IntPtr) (((pos & this.m_PosMask) << (this.m_NumPrevBits & 0x1f)) + (prevByte >> ((8 - this.m_NumPrevBits) & 0x1f))))];

            public void Init()
            {
                uint num = (uint) (1 << ((this.m_NumPrevBits + this.m_NumPosBits) & 0x1f));
                for (uint i = 0; i < num; i++)
                {
                    this.m_Coders[i].Init();
                }
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct Encoder2
            {
                private BitEncoder[] m_Encoders;
                public void Create()
                {
                    this.m_Encoders = new BitEncoder[0x300];
                }

                public void Init()
                {
                    for (int i = 0; i < 0x300; i++)
                    {
                        this.m_Encoders[i].Init();
                    }
                }

                public void Encode(Encoder rangeEncoder, byte symbol)
                {
                    uint index = 1;
                    for (int i = 7; i >= 0; i--)
                    {
                        uint num3 = (uint) ((symbol >> (i & 0x1f)) & 1);
                        this.m_Encoders[index].Encode(rangeEncoder, num3);
                        index = (index << 1) | num3;
                    }
                }

                public void EncodeMatched(Encoder rangeEncoder, byte matchByte, byte symbol)
                {
                    uint num = 1;
                    bool flag = true;
                    for (int i = 7; i >= 0; i--)
                    {
                        uint num3 = (uint) ((symbol >> (i & 0x1f)) & 1);
                        uint index = num;
                        if (flag)
                        {
                            uint num5 = (uint) ((matchByte >> (i & 0x1f)) & 1);
                            index += (uint) ((1 + num5) << 8);
                            flag = num5 == num3;
                        }
                        this.m_Encoders[index].Encode(rangeEncoder, num3);
                        num = (num << 1) | num3;
                    }
                }

                public uint GetPrice(bool matchMode, byte matchByte, byte symbol)
                {
                    uint num = 0;
                    uint index = 1;
                    int num3 = 7;
                    if (matchMode)
                    {
                        while (num3 >= 0)
                        {
                            uint num4 = (uint) ((matchByte >> (num3 & 0x1f)) & 1);
                            uint num5 = (uint) ((symbol >> (num3 & 0x1f)) & 1);
                            num += this.m_Encoders[(int) ((IntPtr) (((1 + num4) << 8) + index))].GetPrice(num5);
                            index = (index << 1) | num5;
                            if (num4 != num5)
                            {
                                num3--;
                                break;
                            }
                            num3--;
                        }
                    }
                    while (num3 >= 0)
                    {
                        uint num6 = (uint) ((symbol >> (num3 & 0x1f)) & 1);
                        num += this.m_Encoders[index].GetPrice(num6);
                        index = (index << 1) | num6;
                        num3--;
                    }
                    return num;
                }
            }
        }

        private class Optimal
        {
            public SharpCompress.Compressor.LZMA.Base.State State;
            public bool Prev1IsChar;
            public bool Prev2;
            public uint PosPrev2;
            public uint BackPrev2;
            public uint Price;
            public uint PosPrev;
            public uint BackPrev;
            public uint Backs0;
            public uint Backs1;
            public uint Backs2;
            public uint Backs3;

            public bool IsShortRep() => 
                this.BackPrev == 0;

            public void MakeAsChar()
            {
                this.BackPrev = uint.MaxValue;
                this.Prev1IsChar = false;
            }

            public void MakeAsShortRep()
            {
                this.BackPrev = 0;
                this.Prev1IsChar = false;
            }
        }
    }
}

