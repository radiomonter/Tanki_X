namespace SharpCompress.Compressor.BZip2
{
    using System;
    using System.IO;

    internal class CBZip2InputStream : Stream
    {
        private int last;
        private int origPtr;
        private int blockSize100k;
        private bool blockRandomised;
        private int bsBuff;
        private int bsLive;
        private CRC mCrc = new CRC();
        private bool[] inUse = new bool[0x100];
        private int nInUse;
        private char[] seqToUnseq = new char[0x100];
        private char[] unseqToSeq = new char[0x100];
        private char[] selector = new char[0x4652];
        private char[] selectorMtf = new char[0x4652];
        private int[] tt;
        private char[] ll8;
        private int[] unzftab = new int[0x100];
        private int[][] limit = InitIntArray(6, 0x102);
        private int[][] basev = InitIntArray(6, 0x102);
        private int[][] perm = InitIntArray(6, 0x102);
        private int[] minLens = new int[6];
        private Stream bsStream;
        private bool leaveOpen;
        private bool streamEnd;
        private int currentChar = -1;
        private const int START_BLOCK_STATE = 1;
        private const int RAND_PART_A_STATE = 2;
        private const int RAND_PART_B_STATE = 3;
        private const int RAND_PART_C_STATE = 4;
        private const int NO_RAND_PART_A_STATE = 5;
        private const int NO_RAND_PART_B_STATE = 6;
        private const int NO_RAND_PART_C_STATE = 7;
        private int currentState = 1;
        private int storedBlockCRC;
        private int storedCombinedCRC;
        private int computedBlockCRC;
        private int computedCombinedCRC;
        private bool decompressConcatenated;
        private int i2;
        private int count;
        private int chPrev;
        private int ch2;
        private int i;
        private int tPos;
        private int rNToGo;
        private int rTPos;
        private int j2;
        private char z;

        public CBZip2InputStream(Stream zStream, bool decompressConcatenated, bool leaveOpen)
        {
            this.decompressConcatenated = decompressConcatenated;
            this.ll8 = null;
            this.tt = null;
            this.BsSetStream(zStream, leaveOpen);
            this.Initialize(true);
            this.InitBlock();
            this.SetupBlock();
        }

        private static void BadBGLengths()
        {
            Cadvise();
        }

        private static void BadBlockHeader()
        {
            Cadvise();
        }

        private static void BitStreamEOF()
        {
            Cadvise();
        }

        private static void BlockOverrun()
        {
            Cadvise();
        }

        private void BsFinishedWithStream()
        {
            try
            {
                if (this.bsStream != null)
                {
                    if (!this.leaveOpen)
                    {
                        this.bsStream.Dispose();
                    }
                    this.bsStream = null;
                }
            }
            catch
            {
            }
        }

        private int BsGetint() => 
            (((((((0 << 8) | this.BsR(8)) << 8) | this.BsR(8)) << 8) | this.BsR(8)) << 8) | this.BsR(8);

        private int BsGetInt32() => 
            this.BsGetint();

        private int BsGetIntVS(int numBits) => 
            this.BsR(numBits);

        private char BsGetUChar() => 
            (char) this.BsR(8);

        private int BsR(int n)
        {
            while (this.bsLive < n)
            {
                int num3 = 0;
                try
                {
                    num3 = (ushort) this.bsStream.ReadByte();
                }
                catch (IOException)
                {
                    CompressedStreamEOF();
                }
                if (num3 == 0xffff)
                {
                    CompressedStreamEOF();
                }
                int num2 = num3;
                this.bsBuff = (this.bsBuff << 8) | (num2 & 0xff);
                this.bsLive += 8;
            }
            int num = (this.bsBuff >> ((this.bsLive - n) & 0x1f)) & ((1 << (n & 0x1f)) - 1);
            this.bsLive -= n;
            return num;
        }

        private void BsSetStream(Stream f, bool leaveOpen)
        {
            this.bsStream = f;
            this.bsLive = 0;
            this.bsBuff = 0;
            this.leaveOpen = leaveOpen;
        }

        private static void Cadvise()
        {
        }

        private bool Complete()
        {
            this.storedCombinedCRC = this.BsGetInt32();
            if (this.storedCombinedCRC != this.computedCombinedCRC)
            {
                CrcError();
            }
            bool flag = !this.decompressConcatenated || !this.Initialize(false);
            if (flag)
            {
                this.BsFinishedWithStream();
                this.streamEnd = true;
            }
            return flag;
        }

        private static void CompressedStreamEOF()
        {
            Cadvise();
        }

        private static void CrcError()
        {
            Cadvise();
        }

        private void EndBlock()
        {
            this.computedBlockCRC = this.mCrc.GetFinalCRC();
            if (this.storedBlockCRC != this.computedBlockCRC)
            {
                CrcError();
            }
            this.computedCombinedCRC = (this.computedCombinedCRC << 1) | (this.computedCombinedCRC >> 0x1f);
            this.computedCombinedCRC ^= this.computedBlockCRC;
        }

        public override void Flush()
        {
        }

        private unsafe void GetAndMoveToFrontDecode()
        {
            int num3;
            char[] chArray = new char[0x100];
            int num4 = 0x186a0 * this.blockSize100k;
            this.origPtr = this.BsGetIntVS(0x18);
            this.RecvDecodingTables();
            int num5 = this.nInUse + 1;
            int index = -1;
            int num7 = 0;
            int num = 0;
            while (true)
            {
                if (num > 0xff)
                {
                    num = 0;
                    while (true)
                    {
                        if (num > 0xff)
                        {
                            this.last = -1;
                            if (num7 == 0)
                            {
                                index++;
                                num7 = 50;
                            }
                            num7--;
                            int num8 = this.selector[index];
                            int n = this.minLens[num8];
                            int num10 = this.BsR(n);
                            while (true)
                            {
                                if (num10 <= this.limit[num8][n])
                                {
                                    num3 = this.perm[num8][num10 - this.basev[num8][n]];
                                    break;
                                }
                                n++;
                                while (true)
                                {
                                    if (this.bsLive >= 1)
                                    {
                                        int num11 = (this.bsBuff >> ((this.bsLive - 1) & 0x1f)) & 1;
                                        this.bsLive--;
                                        num10 = (num10 << 1) | num11;
                                        break;
                                    }
                                    char ch = '\0';
                                    try
                                    {
                                        ch = (char) this.bsStream.ReadByte();
                                    }
                                    catch (IOException)
                                    {
                                        CompressedStreamEOF();
                                    }
                                    if (ch == 0xffff)
                                    {
                                        CompressedStreamEOF();
                                    }
                                    int num12 = ch;
                                    this.bsBuff = (this.bsBuff << 8) | (num12 & 0xff);
                                    this.bsLive += 8;
                                }
                            }
                            break;
                        }
                        chArray[num] = (char) num;
                        num++;
                    }
                    break;
                }
                this.unzftab[num] = 0;
                num++;
            }
            while (true)
            {
                int num13;
                int num14;
                while (true)
                {
                    if (num3 == num5)
                    {
                        return;
                    }
                    if ((num3 == 0) || (num3 == 1))
                    {
                        num13 = -1;
                        num14 = 1;
                        break;
                    }
                    this.last++;
                    if (this.last >= num4)
                    {
                        BlockOverrun();
                    }
                    char ch4 = chArray[num3 - 1];
                    int* numPtr2 = &(this.unzftab[this.seqToUnseq[ch4]]);
                    numPtr2[0]++;
                    this.ll8[this.last] = this.seqToUnseq[ch4];
                    int num2 = num3 - 1;
                    while (true)
                    {
                        if (num2 <= 3)
                        {
                            while (true)
                            {
                                if (num2 <= 0)
                                {
                                    chArray[0] = ch4;
                                    if (num7 == 0)
                                    {
                                        index++;
                                        num7 = 50;
                                    }
                                    num7--;
                                    int num20 = this.selector[index];
                                    int n = this.minLens[num20];
                                    int num22 = this.BsR(n);
                                    while (true)
                                    {
                                        if (num22 <= this.limit[num20][n])
                                        {
                                            num3 = this.perm[num20][num22 - this.basev[num20][n]];
                                            break;
                                        }
                                        n++;
                                        while (true)
                                        {
                                            if (this.bsLive >= 1)
                                            {
                                                int num23 = (this.bsBuff >> ((this.bsLive - 1) & 0x1f)) & 1;
                                                this.bsLive--;
                                                num22 = (num22 << 1) | num23;
                                                break;
                                            }
                                            char ch5 = '\0';
                                            try
                                            {
                                                ch5 = (char) this.bsStream.ReadByte();
                                            }
                                            catch (IOException)
                                            {
                                                CompressedStreamEOF();
                                            }
                                            int num24 = ch5;
                                            this.bsBuff = (this.bsBuff << 8) | (num24 & 0xff);
                                            this.bsLive += 8;
                                        }
                                    }
                                    break;
                                }
                                chArray[num2] = chArray[num2 - 1];
                                num2--;
                            }
                            break;
                        }
                        chArray[num2] = chArray[num2 - 1];
                        chArray[num2 - 1] = chArray[num2 - 2];
                        chArray[num2 - 2] = chArray[num2 - 3];
                        chArray[num2 - 3] = chArray[num2 - 4];
                        num2 -= 4;
                    }
                }
                while (true)
                {
                    if (num3 == 0)
                    {
                        num13 += num14;
                    }
                    else if (num3 == 1)
                    {
                        num13 += 2 * num14;
                    }
                    num14 *= 2;
                    if (num7 == 0)
                    {
                        index++;
                        num7 = 50;
                    }
                    num7--;
                    int num15 = this.selector[index];
                    int n = this.minLens[num15];
                    int num17 = this.BsR(n);
                    while (true)
                    {
                        if (num17 > this.limit[num15][n])
                        {
                            n++;
                            while (true)
                            {
                                if (this.bsLive >= 1)
                                {
                                    int num18 = (this.bsBuff >> ((this.bsLive - 1) & 0x1f)) & 1;
                                    this.bsLive--;
                                    num17 = (num17 << 1) | num18;
                                    break;
                                }
                                char ch3 = '\0';
                                try
                                {
                                    ch3 = (char) this.bsStream.ReadByte();
                                }
                                catch (IOException)
                                {
                                    CompressedStreamEOF();
                                }
                                if (ch3 == 0xffff)
                                {
                                    CompressedStreamEOF();
                                }
                                int num19 = ch3;
                                this.bsBuff = (this.bsBuff << 8) | (num19 & 0xff);
                                this.bsLive += 8;
                            }
                            continue;
                        }
                        num3 = this.perm[num15][num17 - this.basev[num15][n]];
                        if ((num3 == 0) || (num3 == 1))
                        {
                            continue;
                        }
                        else
                        {
                            num13++;
                            char ch2 = this.seqToUnseq[chArray[0]];
                            int* numPtr1 = &(this.unzftab[ch2]);
                            numPtr1[0] += num13;
                            while (true)
                            {
                                if (num13 <= 0)
                                {
                                    if (this.last >= num4)
                                    {
                                        BlockOverrun();
                                    }
                                    break;
                                }
                                this.last++;
                                this.ll8[this.last] = ch2;
                                num13--;
                            }
                        }
                        break;
                    }
                    break;
                }
            }
        }

        private unsafe void HbCreateDecodeTables(int[] limit, int[] basev, int[] perm, char[] length, int minLen, int maxLen, int alphaSize)
        {
            int index = 0;
            int num2 = minLen;
            while (num2 <= maxLen)
            {
                int num3 = 0;
                while (true)
                {
                    if (num3 >= alphaSize)
                    {
                        num2++;
                        break;
                    }
                    if (length[num3] == num2)
                    {
                        perm[index] = num3;
                        index++;
                    }
                    num3++;
                }
            }
            for (num2 = 0; num2 < 0x17; num2++)
            {
                basev[num2] = 0;
            }
            for (num2 = 0; num2 < alphaSize; num2++)
            {
                int* numPtr1 = &(basev[length[num2] + '\x0001']);
                numPtr1[0]++;
            }
            for (num2 = 1; num2 < 0x17; num2++)
            {
                int* numPtr2 = &(basev[num2]);
                numPtr2[0] += basev[num2 - 1];
            }
            for (num2 = 0; num2 < 0x17; num2++)
            {
                limit[num2] = 0;
            }
            int num4 = 0;
            for (num2 = minLen; num2 <= maxLen; num2++)
            {
                num4 += basev[num2 + 1] - basev[num2];
                limit[num2] = num4 - 1;
                num4 = num4 << 1;
            }
            for (num2 = minLen + 1; num2 <= maxLen; num2++)
            {
                basev[num2] = ((limit[num2 - 1] + 1) << 1) - basev[num2];
            }
        }

        private void InitBlock()
        {
            while (true)
            {
                char ch = this.BsGetUChar();
                char ch2 = this.BsGetUChar();
                char ch3 = this.BsGetUChar();
                char ch4 = this.BsGetUChar();
                char ch5 = this.BsGetUChar();
                char ch6 = this.BsGetUChar();
                if ((ch != '\x0017') || ((ch2 != 'r') || ((ch3 != 'E') || ((ch4 != '8') || ((ch5 != 'P') || (ch6 != '\x0090'))))))
                {
                    if ((ch != '1') || ((ch2 != 'A') || ((ch3 != 'Y') || ((ch4 != '&') || ((ch5 != 'S') || (ch6 != 'Y'))))))
                    {
                        BadBlockHeader();
                        this.streamEnd = true;
                        return;
                    }
                    this.storedBlockCRC = this.BsGetInt32();
                    this.blockRandomised = this.BsR(1) == 1;
                    this.GetAndMoveToFrontDecode();
                    this.mCrc.InitialiseCRC();
                    this.currentState = 1;
                    return;
                }
                if (this.Complete())
                {
                    return;
                }
            }
        }

        internal static char[][] InitCharArray(int n1, int n2)
        {
            char[][] chArray = new char[n1][];
            for (int i = 0; i < n1; i++)
            {
                chArray[i] = new char[n2];
            }
            return chArray;
        }

        private bool Initialize(bool isFirstStream)
        {
            int num = this.bsStream.ReadByte();
            int num2 = this.bsStream.ReadByte();
            int num3 = this.bsStream.ReadByte();
            if ((num == -1) && !isFirstStream)
            {
                return false;
            }
            if ((num != 0x42) || ((num2 != 90) || (num3 != 0x68)))
            {
                throw new IOException("Not a BZIP2 marked stream");
            }
            int num4 = this.bsStream.ReadByte();
            if ((num4 < 0x31) || (num4 > 0x39))
            {
                this.BsFinishedWithStream();
                this.streamEnd = true;
                return false;
            }
            this.SetDecompressStructureSizes(num4 - 0x30);
            this.bsLive = 0;
            this.computedCombinedCRC = 0;
            return true;
        }

        internal static int[][] InitIntArray(int n1, int n2)
        {
            int[][] numArray = new int[n1][];
            for (int i = 0; i < n1; i++)
            {
                numArray[i] = new int[n2];
            }
            return numArray;
        }

        private void MakeMaps()
        {
            this.nInUse = 0;
            for (int i = 0; i < 0x100; i++)
            {
                if (this.inUse[i])
                {
                    this.seqToUnseq[this.nInUse] = (char) i;
                    this.unseqToSeq[i] = (char) this.nInUse;
                    this.nInUse++;
                }
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int num = -1;
            int num2 = 0;
            while (true)
            {
                if (num2 < count)
                {
                    num = this.ReadByte();
                    if (num != -1)
                    {
                        buffer[num2 + offset] = (byte) num;
                        num2++;
                        continue;
                    }
                }
                return num2;
            }
        }

        public override int ReadByte()
        {
            if (this.streamEnd)
            {
                return -1;
            }
            int currentChar = this.currentChar;
            switch (this.currentState)
            {
                case 3:
                    this.SetupRandPartB();
                    break;

                case 4:
                    this.SetupRandPartC();
                    break;

                case 6:
                    this.SetupNoRandPartB();
                    break;

                case 7:
                    this.SetupNoRandPartC();
                    break;

                default:
                    break;
            }
            return currentChar;
        }

        private void RecvDecodingTables()
        {
            int num;
            int num2;
            char[][] chArray = InitCharArray(6, 0x102);
            bool[] flagArray = new bool[0x10];
            for (num = 0; num < 0x10; num++)
            {
                flagArray[num] = this.BsR(1) == 1;
            }
            for (num = 0; num < 0x100; num++)
            {
                this.inUse[num] = false;
            }
            for (num = 0; num < 0x10; num++)
            {
                if (flagArray[num])
                {
                    num2 = 0;
                    while (num2 < 0x10)
                    {
                        if (this.BsR(1) == 1)
                        {
                            this.inUse[(num * 0x10) + num2] = true;
                        }
                        num2++;
                    }
                }
            }
            this.MakeMaps();
            int alphaSize = this.nInUse + 2;
            int num4 = this.BsR(3);
            int num5 = this.BsR(15);
            num = 0;
            while (num < num5)
            {
                num2 = 0;
                while (true)
                {
                    if (this.BsR(1) != 1)
                    {
                        this.selectorMtf[num] = (char) num2;
                        num++;
                        break;
                    }
                    num2++;
                }
            }
            char[] chArray2 = new char[6];
            char index = '\0';
            while (index < num4)
            {
                chArray2[index] = index;
                index = (char) (index + '\x0001');
            }
            num = 0;
            while (num < num5)
            {
                index = this.selectorMtf[num];
                char ch = chArray2[index];
                while (true)
                {
                    if (index <= '\0')
                    {
                        chArray2[0] = ch;
                        this.selector[num] = ch;
                        num++;
                        break;
                    }
                    chArray2[index] = chArray2[index - '\x0001'];
                    index = (char) (index - '\x0001');
                }
            }
            int num3 = 0;
            while (num3 < num4)
            {
                int num9 = this.BsR(5);
                num = 0;
                while (true)
                {
                    if (num >= alphaSize)
                    {
                        num3++;
                        break;
                    }
                    while (true)
                    {
                        if (this.BsR(1) != 1)
                        {
                            chArray[num3][num] = (char) num9;
                            num++;
                            break;
                        }
                        num9 = (this.BsR(1) != 0) ? (num9 - 1) : (num9 + 1);
                    }
                }
            }
            num3 = 0;
            while (num3 < num4)
            {
                int minLen = 0x20;
                int maxLen = 0;
                num = 0;
                while (true)
                {
                    if (num >= alphaSize)
                    {
                        this.HbCreateDecodeTables(this.limit[num3], this.basev[num3], this.perm[num3], chArray[num3], minLen, maxLen, alphaSize);
                        this.minLens[num3] = minLen;
                        num3++;
                        break;
                    }
                    if (chArray[num3][num] > maxLen)
                    {
                        maxLen = chArray[num3][num];
                    }
                    if (chArray[num3][num] < minLen)
                    {
                        minLen = chArray[num3][num];
                    }
                    num++;
                }
            }
        }

        public override long Seek(long offset, SeekOrigin origin) => 
            0L;

        private void SetDecompressStructureSizes(int newSize100k)
        {
            if ((0 <= newSize100k) && ((newSize100k > 9) || ((0 <= this.blockSize100k) && (this.blockSize100k <= 9))))
            {
            }
            this.blockSize100k = newSize100k;
            if (newSize100k != 0)
            {
                int num = 0x186a0 * newSize100k;
                this.ll8 = new char[num];
                this.tt = new int[num];
            }
        }

        public override void SetLength(long value)
        {
        }

        private unsafe void SetupBlock()
        {
            int[] numArray = new int[] { 0 };
            this.i = 1;
            while (this.i <= 0x100)
            {
                numArray[this.i] = this.unzftab[this.i - 1];
                this.i++;
            }
            this.i = 1;
            while (this.i <= 0x100)
            {
                int* numPtr1 = &(numArray[this.i]);
                numPtr1[0] += numArray[this.i - 1];
                this.i++;
            }
            this.i = 0;
            while (this.i <= this.last)
            {
                char index = this.ll8[this.i];
                this.tt[numArray[index]] = this.i;
                int* numPtr2 = &(numArray[index]);
                numPtr2[0]++;
                this.i++;
            }
            numArray = null;
            this.tPos = this.tt[this.origPtr];
            this.count = 0;
            this.i2 = 0;
            this.ch2 = 0x100;
            if (!this.blockRandomised)
            {
                this.SetupNoRandPartA();
            }
            else
            {
                this.rNToGo = 0;
                this.rTPos = 0;
                this.SetupRandPartA();
            }
        }

        private void SetupNoRandPartA()
        {
            if (this.i2 > this.last)
            {
                this.EndBlock();
                this.InitBlock();
                this.SetupBlock();
            }
            else
            {
                this.chPrev = this.ch2;
                this.ch2 = this.ll8[this.tPos];
                this.tPos = this.tt[this.tPos];
                this.i2++;
                this.currentChar = this.ch2;
                this.currentState = 6;
                this.mCrc.UpdateCRC(this.ch2);
            }
        }

        private void SetupNoRandPartB()
        {
            if (this.ch2 != this.chPrev)
            {
                this.currentState = 5;
                this.count = 1;
                this.SetupNoRandPartA();
            }
            else
            {
                this.count++;
                if (this.count < 4)
                {
                    this.currentState = 5;
                    this.SetupNoRandPartA();
                }
                else
                {
                    this.z = this.ll8[this.tPos];
                    this.tPos = this.tt[this.tPos];
                    this.currentState = 7;
                    this.j2 = 0;
                    this.SetupNoRandPartC();
                }
            }
        }

        private void SetupNoRandPartC()
        {
            if (this.j2 < this.z)
            {
                this.currentChar = this.ch2;
                this.mCrc.UpdateCRC(this.ch2);
                this.j2++;
            }
            else
            {
                this.currentState = 5;
                this.i2++;
                this.count = 0;
                this.SetupNoRandPartA();
            }
        }

        private void SetupRandPartA()
        {
            if (this.i2 > this.last)
            {
                this.EndBlock();
                this.InitBlock();
                this.SetupBlock();
            }
            else
            {
                this.chPrev = this.ch2;
                this.ch2 = this.ll8[this.tPos];
                this.tPos = this.tt[this.tPos];
                if (this.rNToGo == 0)
                {
                    this.rNToGo = BZip2Constants.rNums[this.rTPos];
                    this.rTPos++;
                    if (this.rTPos == 0x200)
                    {
                        this.rTPos = 0;
                    }
                }
                this.rNToGo--;
                this.ch2 ^= (this.rNToGo != 1) ? 0 : 1;
                this.i2++;
                this.currentChar = this.ch2;
                this.currentState = 3;
                this.mCrc.UpdateCRC(this.ch2);
            }
        }

        private void SetupRandPartB()
        {
            if (this.ch2 != this.chPrev)
            {
                this.currentState = 2;
                this.count = 1;
                this.SetupRandPartA();
            }
            else
            {
                this.count++;
                if (this.count < 4)
                {
                    this.currentState = 2;
                    this.SetupRandPartA();
                }
                else
                {
                    this.z = this.ll8[this.tPos];
                    this.tPos = this.tt[this.tPos];
                    if (this.rNToGo == 0)
                    {
                        this.rNToGo = BZip2Constants.rNums[this.rTPos];
                        this.rTPos++;
                        if (this.rTPos == 0x200)
                        {
                            this.rTPos = 0;
                        }
                    }
                    this.rNToGo--;
                    this.z = (char) (this.z ^ ((this.rNToGo != 1) ? '\0' : '\x0001'));
                    this.j2 = 0;
                    this.currentState = 4;
                    this.SetupRandPartC();
                }
            }
        }

        private void SetupRandPartC()
        {
            if (this.j2 < this.z)
            {
                this.currentChar = this.ch2;
                this.mCrc.UpdateCRC(this.ch2);
                this.j2++;
            }
            else
            {
                this.currentState = 2;
                this.i2++;
                this.count = 0;
                this.SetupRandPartA();
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
        }

        public override bool CanRead =>
            true;

        public override bool CanSeek =>
            false;

        public override bool CanWrite =>
            false;

        public override long Length =>
            0L;

        public override long Position
        {
            get => 
                0L;
            set
            {
            }
        }
    }
}

