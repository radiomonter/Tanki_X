namespace SharpCompress.Compressor.BZip2
{
    using System;
    using System.IO;

    internal class CBZip2OutputStream : Stream
    {
        protected const int SETMASK = 0x200000;
        protected const int CLEARMASK = -2097153;
        protected const int GREATER_ICOST = 15;
        protected const int LESSER_ICOST = 0;
        protected const int SMALL_THRESH = 20;
        protected const int DEPTH_THRESH = 10;
        protected const int QSORT_STACK_SIZE = 0x3e8;
        private bool finished;
        private int last;
        private int origPtr;
        private int blockSize100k;
        private bool blockRandomised;
        private int bytesOut;
        private int bsBuff;
        private int bsLive;
        private CRC mCrc;
        private bool[] inUse;
        private int nInUse;
        private char[] seqToUnseq;
        private char[] unseqToSeq;
        private char[] selector;
        private char[] selectorMtf;
        private char[] block;
        private int[] quadrant;
        private int[] zptr;
        private short[] szptr;
        private int[] ftab;
        private int nMTF;
        private int[] mtfFreq;
        private int workFactor;
        private int workDone;
        private int workLimit;
        private bool firstAttempt;
        private int nBlocksRandomised;
        private int currentChar;
        private int runLength;
        private bool disposed;
        private int blockCRC;
        private int combinedCRC;
        private int allowableBlockSize;
        private Stream bsStream;
        private bool leaveOpen;
        private int[] incs;

        public CBZip2OutputStream(Stream inStream) : this(inStream, 9, false)
        {
        }

        public CBZip2OutputStream(Stream inStream, bool leaveOpen) : this(inStream, 9, leaveOpen)
        {
        }

        public CBZip2OutputStream(Stream inStream, int inBlockSize, bool leaveOpen)
        {
            this.mCrc = new CRC();
            this.inUse = new bool[0x100];
            this.seqToUnseq = new char[0x100];
            this.unseqToSeq = new char[0x100];
            this.selector = new char[0x4652];
            this.selectorMtf = new char[0x4652];
            this.mtfFreq = new int[0x102];
            this.currentChar = -1;
            this.incs = new int[] { 1, 4, 13, 40, 0x79, 0x16c, 0x445, 0xcd0, 0x2671, 0x7354, 0x159fd, 0x40df8, 0xc29e9, 0x247dbc };
            this.block = null;
            this.quadrant = null;
            this.zptr = null;
            this.ftab = null;
            inStream.WriteByte(0x42);
            inStream.WriteByte(90);
            this.BsSetStream(inStream, leaveOpen);
            this.workFactor = 50;
            if (inBlockSize > 9)
            {
                inBlockSize = 9;
            }
            if (inBlockSize < 1)
            {
                inBlockSize = 1;
            }
            this.blockSize100k = inBlockSize;
            this.AllocateCompressStructures();
            this.Initialize();
            this.InitBlock();
        }

        private void AllocateCompressStructures()
        {
            int num = 0x186a0 * this.blockSize100k;
            this.block = new char[(num + 1) + 20];
            this.quadrant = new int[num + 20];
            this.zptr = new int[num];
            this.ftab = new int[0x10001];
            if ((this.block != null) && ((this.quadrant == null) || ((this.zptr != null) && (this.ftab != null))))
            {
            }
            this.szptr = new short[2 * num];
        }

        private void BsFinishedWithStream()
        {
            while (this.bsLive > 0)
            {
                int num = this.bsBuff >> 0x18;
                try
                {
                    this.bsStream.WriteByte((byte) num);
                }
                catch (IOException exception1)
                {
                    throw exception1;
                }
                this.bsBuff = this.bsBuff << 8;
                this.bsLive -= 8;
                this.bytesOut++;
            }
        }

        private void BsPutint(int u)
        {
            this.BsW(8, (u >> 0x18) & 0xff);
            this.BsW(8, (u >> 0x10) & 0xff);
            this.BsW(8, (u >> 8) & 0xff);
            this.BsW(8, u & 0xff);
        }

        private void BsPutIntVS(int numBits, int c)
        {
            this.BsW(numBits, c);
        }

        private void BsPutUChar(int c)
        {
            this.BsW(8, c);
        }

        private void BsSetStream(Stream f, bool leaveOpen)
        {
            this.bsStream = f;
            this.bsLive = 0;
            this.bsBuff = 0;
            this.bytesOut = 0;
            this.leaveOpen = leaveOpen;
        }

        private void BsW(int n, int v)
        {
            while (this.bsLive >= 8)
            {
                int num = this.bsBuff >> 0x18;
                try
                {
                    this.bsStream.WriteByte((byte) num);
                }
                catch (IOException exception1)
                {
                    throw exception1;
                }
                this.bsBuff = this.bsBuff << 8;
                this.bsLive -= 8;
                this.bytesOut++;
            }
            this.bsBuff |= v << (((0x20 - this.bsLive) - n) & 0x1f);
            this.bsLive += n;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !this.disposed)
            {
                this.Finish();
                this.disposed = true;
                base.Dispose();
                if (!this.leaveOpen)
                {
                    this.bsStream.Dispose();
                }
                this.bsStream = null;
            }
        }

        private void DoReversibleTransformation()
        {
            this.workLimit = this.workFactor * this.last;
            this.workDone = 0;
            this.blockRandomised = false;
            this.firstAttempt = true;
            this.MainSort();
            if ((this.workDone > this.workLimit) && this.firstAttempt)
            {
                this.RandomiseBlock();
                this.workLimit = this.workDone = 0;
                this.blockRandomised = true;
                this.firstAttempt = false;
                this.MainSort();
            }
            this.origPtr = -1;
            int index = 0;
            while (true)
            {
                if (index <= this.last)
                {
                    if (this.zptr[index] != 0)
                    {
                        index++;
                        continue;
                    }
                    this.origPtr = index;
                }
                if (this.origPtr == -1)
                {
                    Panic();
                }
                return;
            }
        }

        private void EndBlock()
        {
            this.blockCRC = this.mCrc.GetFinalCRC();
            this.combinedCRC = (this.combinedCRC << 1) | (this.combinedCRC >> 0x1f);
            this.combinedCRC ^= this.blockCRC;
            this.DoReversibleTransformation();
            this.BsPutUChar(0x31);
            this.BsPutUChar(0x41);
            this.BsPutUChar(0x59);
            this.BsPutUChar(0x26);
            this.BsPutUChar(0x53);
            this.BsPutUChar(0x59);
            this.BsPutint(this.blockCRC);
            if (!this.blockRandomised)
            {
                this.BsW(1, 0);
            }
            else
            {
                this.BsW(1, 1);
                this.nBlocksRandomised++;
            }
            this.MoveToFrontCodeAndSend();
        }

        private void EndCompression()
        {
            this.BsPutUChar(0x17);
            this.BsPutUChar(0x72);
            this.BsPutUChar(0x45);
            this.BsPutUChar(0x38);
            this.BsPutUChar(80);
            this.BsPutUChar(0x90);
            this.BsPutint(this.combinedCRC);
            this.BsFinishedWithStream();
        }

        public void Finish()
        {
            if (!this.finished)
            {
                if (this.runLength > 0)
                {
                    this.WriteRun();
                }
                this.currentChar = -1;
                this.EndBlock();
                this.EndCompression();
                this.finished = true;
                this.Flush();
            }
        }

        public override void Flush()
        {
            this.bsStream.Flush();
        }

        private bool FullGtU(int i1, int i2)
        {
            char ch = this.block[i1 + 1];
            char ch2 = this.block[i2 + 1];
            if (ch != ch2)
            {
                return (ch > ch2);
            }
            i1++;
            i2++;
            ch = this.block[i1 + 1];
            ch2 = this.block[i2 + 1];
            if (ch != ch2)
            {
                return (ch > ch2);
            }
            i1++;
            i2++;
            ch = this.block[i1 + 1];
            ch2 = this.block[i2 + 1];
            if (ch != ch2)
            {
                return (ch > ch2);
            }
            i1++;
            i2++;
            ch = this.block[i1 + 1];
            ch2 = this.block[i2 + 1];
            if (ch != ch2)
            {
                return (ch > ch2);
            }
            i1++;
            i2++;
            ch = this.block[i1 + 1];
            ch2 = this.block[i2 + 1];
            if (ch != ch2)
            {
                return (ch > ch2);
            }
            i1++;
            i2++;
            ch = this.block[i1 + 1];
            ch2 = this.block[i2 + 1];
            if (ch != ch2)
            {
                return (ch > ch2);
            }
            i1++;
            i2++;
            int num = this.last + 1;
            while (true)
            {
                ch = this.block[i1 + 1];
                ch2 = this.block[i2 + 1];
                if (ch != ch2)
                {
                    return (ch > ch2);
                }
                int num2 = this.quadrant[i1];
                int num3 = this.quadrant[i2];
                if (num2 != num3)
                {
                    return (num2 > num3);
                }
                i1++;
                i2++;
                ch = this.block[i1 + 1];
                ch2 = this.block[i2 + 1];
                if (ch != ch2)
                {
                    return (ch > ch2);
                }
                num2 = this.quadrant[i1];
                num3 = this.quadrant[i2];
                if (num2 != num3)
                {
                    return (num2 > num3);
                }
                i1++;
                i2++;
                ch = this.block[i1 + 1];
                ch2 = this.block[i2 + 1];
                if (ch != ch2)
                {
                    return (ch > ch2);
                }
                num2 = this.quadrant[i1];
                num3 = this.quadrant[i2];
                if (num2 != num3)
                {
                    return (num2 > num3);
                }
                i1++;
                i2++;
                ch = this.block[i1 + 1];
                ch2 = this.block[i2 + 1];
                if (ch != ch2)
                {
                    return (ch > ch2);
                }
                num2 = this.quadrant[i1];
                num3 = this.quadrant[i2];
                if (num2 != num3)
                {
                    return (num2 > num3);
                }
                i1++;
                i2++;
                if (i1 > this.last)
                {
                    i1 -= this.last;
                    i1--;
                }
                if (i2 > this.last)
                {
                    i2 -= this.last;
                    i2--;
                }
                num -= 4;
                this.workDone++;
                if (num < 0)
                {
                    return false;
                }
            }
        }

        private unsafe void GenerateMTFValues()
        {
            int num;
            char[] chArray = new char[0x100];
            this.MakeMaps();
            int index = this.nInUse + 1;
            for (num = 0; num <= index; num++)
            {
                this.mtfFreq[num] = 0;
            }
            int num4 = 0;
            int num3 = 0;
            for (num = 0; num < this.nInUse; num++)
            {
                chArray[num] = (char) num;
            }
            num = 0;
            while (num <= this.last)
            {
                char ch3 = this.unseqToSeq[this.block[this.zptr[num]]];
                int num2 = 0;
                char ch = chArray[num2];
                while (true)
                {
                    if (ch3 == ch)
                    {
                        chArray[0] = ch;
                        if (num2 == 0)
                        {
                            num3++;
                        }
                        else
                        {
                            if (num3 > 0)
                            {
                                num3--;
                                while (true)
                                {
                                    int num6 = num3 % 2;
                                    if (num6 == 0)
                                    {
                                        this.szptr[num4] = 0;
                                        num4++;
                                        int* mtfFreq = this.mtfFreq;
                                        mtfFreq[0]++;
                                    }
                                    else if (num6 == 1)
                                    {
                                        this.szptr[num4] = 1;
                                        num4++;
                                        int* numPtr2 = &(this.mtfFreq[1]);
                                        numPtr2[0]++;
                                    }
                                    if (num3 < 2)
                                    {
                                        num3 = 0;
                                        break;
                                    }
                                    num3 = (num3 - 2) / 2;
                                }
                            }
                            this.szptr[num4] = (short) (num2 + 1);
                            num4++;
                            int* numPtr3 = &(this.mtfFreq[num2 + 1]);
                            numPtr3[0]++;
                        }
                        num++;
                        break;
                    }
                    num2++;
                    char ch2 = ch;
                    ch = chArray[num2];
                    chArray[num2] = ch2;
                }
            }
            if (num3 > 0)
            {
                num3--;
                while (true)
                {
                    int num7 = num3 % 2;
                    if (num7 == 0)
                    {
                        this.szptr[num4] = 0;
                        num4++;
                        int* mtfFreq = this.mtfFreq;
                        mtfFreq[0]++;
                    }
                    else if (num7 == 1)
                    {
                        this.szptr[num4] = 1;
                        num4++;
                        int* numPtr5 = &(this.mtfFreq[1]);
                        numPtr5[0]++;
                    }
                    if (num3 < 2)
                    {
                        break;
                    }
                    num3 = (num3 - 2) / 2;
                }
            }
            this.szptr[num4] = (short) index;
            num4++;
            int* numPtr6 = &(this.mtfFreq[index]);
            numPtr6[0]++;
            this.nMTF = num4;
        }

        private void HbAssignCodes(int[] code, char[] length, int minLen, int maxLen, int alphaSize)
        {
            int num2 = 0;
            int num = minLen;
            while (num <= maxLen)
            {
                int index = 0;
                while (true)
                {
                    if (index >= alphaSize)
                    {
                        num2 = num2 << 1;
                        num++;
                        break;
                    }
                    if (length[index] == num)
                    {
                        code[index] = num2;
                        num2++;
                    }
                    index++;
                }
            }
        }

        protected static void HbMakeCodeLengths(char[] len, int[] freq, int alphaSize, int maxLen)
        {
            int[] numArray = new int[260];
            int[] numArray2 = new int[0x204];
            int[] numArray3 = new int[0x204];
            int index = 0;
            while (index < alphaSize)
            {
                numArray2[index + 1] = ((freq[index] != null) ? freq[index] : 1) << 8;
                index++;
            }
            while (true)
            {
                int num = alphaSize;
                int num2 = 0;
                numArray[0] = 0;
                numArray2[0] = 0;
                numArray3[0] = -2;
                index = 1;
                while (true)
                {
                    if (index > alphaSize)
                    {
                        if (num2 >= 260)
                        {
                            Panic();
                        }
                        while (true)
                        {
                            if (num2 <= 1)
                            {
                                if (num >= 0x204)
                                {
                                    Panic();
                                }
                                bool flag = false;
                                index = 1;
                                while (true)
                                {
                                    int num6;
                                    if (index > alphaSize)
                                    {
                                        if (!flag)
                                        {
                                            return;
                                        }
                                        index = 1;
                                        while (index < alphaSize)
                                        {
                                            num6 = 1 + ((numArray2[index] >> 8) / 2);
                                            numArray2[index] = num6 << 8;
                                            index++;
                                        }
                                        break;
                                    }
                                    num6 = 0;
                                    int num7 = index;
                                    while (true)
                                    {
                                        if (numArray3[num7] < 0)
                                        {
                                            len[index - 1] = (char) num6;
                                            if (num6 > maxLen)
                                            {
                                                flag = true;
                                            }
                                            index++;
                                            break;
                                        }
                                        num7 = numArray3[num7];
                                        num6++;
                                    }
                                }
                                break;
                            }
                            int num3 = numArray[1];
                            numArray[1] = numArray[num2];
                            num2--;
                            int num10 = 0;
                            int num11 = 0;
                            int num12 = 0;
                            num10 = 1;
                            num12 = numArray[num10];
                            while (true)
                            {
                                num11 = num10 << 1;
                                if (num11 > num2)
                                {
                                    break;
                                }
                                if ((num11 < num2) && (numArray2[numArray[num11 + 1]] < numArray2[numArray[num11]]))
                                {
                                    num11++;
                                }
                                if (numArray2[num12] < numArray2[numArray[num11]])
                                {
                                    break;
                                }
                                numArray[num10] = numArray[num11];
                                num10 = num11;
                            }
                            numArray[num10] = num12;
                            int num4 = numArray[1];
                            numArray[1] = numArray[num2];
                            num2--;
                            int num13 = 0;
                            int num14 = 0;
                            int num15 = 0;
                            num13 = 1;
                            num15 = numArray[num13];
                            while (true)
                            {
                                num14 = num13 << 1;
                                if (num14 > num2)
                                {
                                    break;
                                }
                                if ((num14 < num2) && (numArray2[numArray[num14 + 1]] < numArray2[numArray[num14]]))
                                {
                                    num14++;
                                }
                                if (numArray2[num15] < numArray2[numArray[num14]])
                                {
                                    break;
                                }
                                numArray[num13] = numArray[num14];
                                num13 = num14;
                            }
                            numArray[num13] = num15;
                            num++;
                            numArray3[num3] = numArray3[num4] = num;
                            numArray2[num] = ((int) ((uint) ((numArray2[num3] & 0xffffff00UL) + (numArray2[num4] & 0xffffff00UL)))) | (1 + (((numArray2[num3] & 0xff) <= (numArray2[num4] & 0xff)) ? (numArray2[num4] & 0xff) : (numArray2[num3] & 0xff)));
                            numArray3[num] = -1;
                            num2++;
                            numArray[num2] = num;
                            int num17 = 0;
                            int num18 = 0;
                            num17 = num2;
                            num18 = numArray[num17];
                            while (true)
                            {
                                if (numArray2[num18] >= numArray2[numArray[num17 >> 1]])
                                {
                                    numArray[num17] = num18;
                                    break;
                                }
                                numArray[num17] = numArray[num17 >> 1];
                                num17 = num17 >> 1;
                            }
                        }
                        break;
                    }
                    numArray3[index] = -1;
                    num2++;
                    numArray[num2] = index;
                    int num8 = num2;
                    int num9 = numArray[num8];
                    while (true)
                    {
                        if (numArray2[num9] >= numArray2[numArray[num8 >> 1]])
                        {
                            numArray[num8] = num9;
                            index++;
                            break;
                        }
                        numArray[num8] = numArray[num8 >> 1];
                        num8 = num8 >> 1;
                    }
                }
            }
        }

        private void InitBlock()
        {
            this.mCrc.InitialiseCRC();
            this.last = -1;
            for (int i = 0; i < 0x100; i++)
            {
                this.inUse[i] = false;
            }
            this.allowableBlockSize = (0x186a0 * this.blockSize100k) - 20;
        }

        private void Initialize()
        {
            this.bytesOut = 0;
            this.nBlocksRandomised = 0;
            this.BsPutUChar(0x68);
            this.BsPutUChar(0x30 + this.blockSize100k);
            this.combinedCRC = 0;
        }

        private unsafe void MainSort()
        {
            int[] numArray = new int[0x100];
            int[] numArray2 = new int[0x100];
            bool[] flagArray = new bool[0x100];
            int index = 0;
            while (true)
            {
                if (index < 20)
                {
                    this.block[(this.last + index) + 2] = this.block[(index % (this.last + 1)) + 1];
                    index++;
                    continue;
                }
                index = 0;
                while (true)
                {
                    if (index <= (this.last + 20))
                    {
                        this.quadrant[index] = 0;
                        index++;
                        continue;
                    }
                    this.block[0] = this.block[this.last + 1];
                    if (this.last >= 0xfa0)
                    {
                        int num7 = 0;
                        index = 0;
                        while (true)
                        {
                            if (index <= 0xff)
                            {
                                flagArray[index] = false;
                                index++;
                                continue;
                            }
                            index = 0;
                            while (true)
                            {
                                if (index <= 0x10000)
                                {
                                    this.ftab[index] = 0;
                                    index++;
                                    continue;
                                }
                                int num5 = this.block[0];
                                index = 0;
                                while (true)
                                {
                                    int num6;
                                    if (index <= this.last)
                                    {
                                        num6 = this.block[index + 1];
                                        int* numPtr1 = &(this.ftab[(num5 << 8) + num6]);
                                        numPtr1[0]++;
                                        num5 = num6;
                                        index++;
                                        continue;
                                    }
                                    index = 1;
                                    while (true)
                                    {
                                        if (index <= 0x10000)
                                        {
                                            int* numPtr2 = &(this.ftab[index]);
                                            numPtr2[0] += this.ftab[index - 1];
                                            index++;
                                            continue;
                                        }
                                        num5 = this.block[1];
                                        index = 0;
                                        while (true)
                                        {
                                            int num2;
                                            if (index < this.last)
                                            {
                                                num6 = this.block[index + 2];
                                                num2 = (num5 << 8) + num6;
                                                num5 = num6;
                                                int* numPtr3 = &(this.ftab[num2]);
                                                numPtr3[0]--;
                                                this.zptr[this.ftab[num2]] = index;
                                                index++;
                                                continue;
                                            }
                                            num2 = (this.block[this.last + 1] << 8) + this.block[1];
                                            int* numPtr4 = &(this.ftab[num2]);
                                            numPtr4[0]--;
                                            this.zptr[this.ftab[num2]] = this.last;
                                            index = 0;
                                            while (true)
                                            {
                                                if (index <= 0xff)
                                                {
                                                    numArray[index] = index;
                                                    index++;
                                                    continue;
                                                }
                                                int num10 = 1;
                                                while (true)
                                                {
                                                    num10 = (3 * num10) + 1;
                                                    if (num10 > 0x100)
                                                    {
                                                        while (true)
                                                        {
                                                            num10 /= 3;
                                                            index = num10;
                                                            while (true)
                                                            {
                                                                if (index <= 0xff)
                                                                {
                                                                    int num9 = numArray[index];
                                                                    num2 = index;
                                                                    while (true)
                                                                    {
                                                                        if ((this.ftab[(numArray[num2 - num10] + 1) << 8] - this.ftab[numArray[num2 - num10] << 8]) > (this.ftab[(num9 + 1) << 8] - this.ftab[num9 << 8]))
                                                                        {
                                                                            numArray[num2] = numArray[num2 - num10];
                                                                            num2 -= num10;
                                                                            if (num2 > (num10 - 1))
                                                                            {
                                                                                continue;
                                                                            }
                                                                        }
                                                                        numArray[num2] = num9;
                                                                        index++;
                                                                        break;
                                                                    }
                                                                    continue;
                                                                }
                                                                if (num10 == 1)
                                                                {
                                                                    index = 0;
                                                                    while (index <= 0xff)
                                                                    {
                                                                        int num3 = numArray[index];
                                                                        num2 = 0;
                                                                        while (true)
                                                                        {
                                                                            if (num2 > 0xff)
                                                                            {
                                                                                flagArray[num3] = true;
                                                                                if (index < 0xff)
                                                                                {
                                                                                    int num13 = this.ftab[num3 << 8] & -2097153;
                                                                                    int num14 = (this.ftab[(num3 + 1) << 8] & -2097153) - num13;
                                                                                    int num15 = 0;
                                                                                    while (true)
                                                                                    {
                                                                                        if ((num14 >> (num15 & 0x1f)) <= 0xfffe)
                                                                                        {
                                                                                            num2 = 0;
                                                                                            while (true)
                                                                                            {
                                                                                                if (num2 >= num14)
                                                                                                {
                                                                                                    if (((num14 - 1) >> (num15 & 0x1f)) > 0xffff)
                                                                                                    {
                                                                                                        Panic();
                                                                                                    }
                                                                                                    break;
                                                                                                }
                                                                                                int num16 = this.zptr[num13 + num2];
                                                                                                int num17 = num2 >> (num15 & 0x1f);
                                                                                                this.quadrant[num16] = num17;
                                                                                                if (num16 < 20)
                                                                                                {
                                                                                                    this.quadrant[(num16 + this.last) + 1] = num17;
                                                                                                }
                                                                                                num2++;
                                                                                            }
                                                                                            break;
                                                                                        }
                                                                                        num15++;
                                                                                    }
                                                                                }
                                                                                num2 = 0;
                                                                                while (true)
                                                                                {
                                                                                    if (num2 > 0xff)
                                                                                    {
                                                                                        num2 = this.ftab[num3 << 8] & -2097153;
                                                                                        while (true)
                                                                                        {
                                                                                            if (num2 >= (this.ftab[(num3 + 1) << 8] & -2097153))
                                                                                            {
                                                                                                num2 = 0;
                                                                                                while (true)
                                                                                                {
                                                                                                    if (num2 > 0xff)
                                                                                                    {
                                                                                                        index++;
                                                                                                        break;
                                                                                                    }
                                                                                                    int* numPtr7 = &(this.ftab[(num2 << 8) + num3]);
                                                                                                    numPtr7[0] |= 0x200000;
                                                                                                    num2++;
                                                                                                }
                                                                                                break;
                                                                                            }
                                                                                            num5 = this.block[this.zptr[num2]];
                                                                                            if (!flagArray[num5])
                                                                                            {
                                                                                                this.zptr[numArray2[num5]] = (this.zptr[num2] != 0) ? (this.zptr[num2] - 1) : this.last;
                                                                                                int* numPtr6 = &(numArray2[num5]);
                                                                                                numPtr6[0]++;
                                                                                            }
                                                                                            num2++;
                                                                                        }
                                                                                        break;
                                                                                    }
                                                                                    numArray2[num2] = this.ftab[(num2 << 8) + num3] & -2097153;
                                                                                    num2++;
                                                                                }
                                                                                break;
                                                                            }
                                                                            int num4 = (num3 << 8) + num2;
                                                                            if ((this.ftab[num4] & 0x200000) != 0x200000)
                                                                            {
                                                                                int loSt = this.ftab[num4] & -2097153;
                                                                                int hiSt = (this.ftab[num4 + 1] & -2097153) - 1;
                                                                                if (hiSt > loSt)
                                                                                {
                                                                                    this.QSort3(loSt, hiSt, 2);
                                                                                    num7 += (hiSt - loSt) + 1;
                                                                                    if ((this.workDone > this.workLimit) && this.firstAttempt)
                                                                                    {
                                                                                        return;
                                                                                    }
                                                                                }
                                                                                int* numPtr5 = &(this.ftab[num4]);
                                                                                numPtr5[0] |= 0x200000;
                                                                            }
                                                                            num2++;
                                                                        }
                                                                    }
                                                                }
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        index = 0;
                        while (true)
                        {
                            if (index > this.last)
                            {
                                this.firstAttempt = false;
                                this.workDone = this.workLimit = 0;
                                this.SimpleSort(0, this.last, 0);
                                break;
                            }
                            this.zptr[index] = index;
                            index++;
                        }
                    }
                    break;
                }
                break;
            }
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

        private char Med3(char a, char b, char c)
        {
            char ch;
            if (a > b)
            {
                ch = a;
                a = b;
                b = ch;
            }
            if (b > c)
            {
                ch = b;
                b = c;
                c = ch;
            }
            if (a > b)
            {
                b = a;
            }
            return b;
        }

        private void MoveToFrontCodeAndSend()
        {
            this.BsPutIntVS(0x18, this.origPtr);
            this.GenerateMTFValues();
            this.SendMTFValues();
        }

        private static void Panic()
        {
        }

        private void QSort3(int loSt, int hiSt, int dSt)
        {
            int num8;
            StackElem[] elemArray = new StackElem[0x3e8];
            int index = 0;
            while (true)
            {
                if (index >= 0x3e8)
                {
                    num8 = 0;
                    elemArray[num8].ll = loSt;
                    elemArray[num8].hh = hiSt;
                    elemArray[num8].dd = dSt;
                    num8++;
                    break;
                }
                elemArray[index] = new StackElem();
                index++;
            }
            while (true)
            {
                int num;
                int num2;
                int num3;
                int num4;
                int num5;
                int ll;
                int hh;
                int dd;
                while (true)
                {
                    if (num8 <= 0)
                    {
                        return;
                    }
                    if (num8 >= 0x3e8)
                    {
                        Panic();
                    }
                    num8--;
                    ll = elemArray[num8].ll;
                    hh = elemArray[num8].hh;
                    dd = elemArray[num8].dd;
                    if (((hh - ll) >= 20) && (dd <= 10))
                    {
                        num5 = this.Med3(this.block[(this.zptr[ll] + dd) + 1], this.block[(this.zptr[hh] + dd) + 1], this.block[(this.zptr[(ll + hh) >> 1] + dd) + 1]);
                        num = num3 = ll;
                        num2 = num4 = hh;
                        break;
                    }
                    this.SimpleSort(ll, hh, dd);
                    if ((this.workDone > this.workLimit) && this.firstAttempt)
                    {
                        return;
                    }
                }
                while (true)
                {
                    int num6;
                    if (num <= num2)
                    {
                        num6 = this.block[(this.zptr[num] + dd) + 1] - num5;
                        if (num6 == 0)
                        {
                            this.zptr[num] = this.zptr[num3];
                            this.zptr[num3] = this.zptr[num];
                            num3++;
                            num++;
                            continue;
                        }
                        if (num6 <= 0)
                        {
                            num++;
                            continue;
                        }
                    }
                    while (true)
                    {
                        if (num <= num2)
                        {
                            num6 = this.block[(this.zptr[num2] + dd) + 1] - num5;
                            if (num6 == 0)
                            {
                                this.zptr[num2] = this.zptr[num4];
                                this.zptr[num4] = this.zptr[num2];
                                num4--;
                                num2--;
                                continue;
                            }
                            if (num6 >= 0)
                            {
                                num2--;
                                continue;
                            }
                        }
                        if (num <= num2)
                        {
                            int num15 = this.zptr[num];
                            this.zptr[num] = this.zptr[num2];
                            this.zptr[num2] = num15;
                            num++;
                            num2--;
                            continue;
                        }
                        else if (num4 < num3)
                        {
                            elemArray[num8].ll = ll;
                            elemArray[num8].hh = hh;
                            elemArray[num8].dd = dd + 1;
                            num8++;
                        }
                        else
                        {
                            num6 = ((num3 - ll) >= (num - num3)) ? (num - num3) : (num3 - ll);
                            this.Vswap(ll, num - num6, num6);
                            int n = ((hh - num4) >= (num4 - num2)) ? (num4 - num2) : (hh - num4);
                            this.Vswap(num, (hh - n) + 1, n);
                            num6 = ((ll + num) - num3) - 1;
                            n = (hh - (num4 - num2)) + 1;
                            elemArray[num8].ll = ll;
                            elemArray[num8].hh = num6;
                            elemArray[num8].dd = dd;
                            num8++;
                            elemArray[num8].ll = num6 + 1;
                            elemArray[num8].hh = n - 1;
                            elemArray[num8].dd = dd + 1;
                            num8++;
                            elemArray[num8].ll = n;
                            elemArray[num8].hh = hh;
                            elemArray[num8].dd = dd;
                            num8++;
                        }
                        break;
                    }
                    break;
                }
            }
        }

        private unsafe void RandomiseBlock()
        {
            int num;
            int num2 = 0;
            int index = 0;
            for (num = 0; num < 0x100; num++)
            {
                this.inUse[num] = false;
            }
            for (num = 0; num <= this.last; num++)
            {
                if (num2 == 0)
                {
                    num2 = (ushort) BZip2Constants.rNums[index];
                    if ((index + 1) == 0x200)
                    {
                        index = 0;
                    }
                }
                num2--;
                char* chPtr1 = &(this.block[num + 1]);
                chPtr1[0] = (char) (chPtr1[0] ^ ((num2 != 1) ? '\0' : '\x0001'));
                char* chPtr2 = &(this.block[num + 1]);
                chPtr2[0] = (char) (chPtr2[0] & '\x00ff');
                this.inUse[this.block[num + 1]] = true;
            }
        }

        public override int Read(byte[] buffer, int offset, int count) => 
            0;

        public override long Seek(long offset, SeekOrigin origin) => 
            0L;

        private unsafe void SendMTFValues()
        {
            int num;
            int num3;
            int num4;
            int num6;
            char[][] chArray = CBZip2InputStream.InitCharArray(6, 0x102);
            int index = 0;
            int alphaSize = this.nInUse + 2;
            int num2 = 0;
            while (num2 < 6)
            {
                num = 0;
                while (true)
                {
                    if (num >= alphaSize)
                    {
                        num2++;
                        break;
                    }
                    chArray[num2][num] = '\x000f';
                    num++;
                }
            }
            if (this.nMTF <= 0)
            {
                Panic();
            }
            int v = (this.nMTF >= 200) ? ((this.nMTF >= 600) ? ((this.nMTF >= 0x4b0) ? ((this.nMTF >= 0x960) ? 6 : 5) : 4) : 3) : 2;
            int num17 = v;
            int nMTF = this.nMTF;
            int num5 = 0;
            while (num17 > 0)
            {
                int num19 = nMTF / num17;
                num6 = num5 - 1;
                int num20 = 0;
                while (true)
                {
                    if ((num20 >= num19) || (num6 >= (alphaSize - 1)))
                    {
                        if ((num6 > num5) && ((num17 != v) && ((num17 != 1) && (((v - num17) % 2) == 1))))
                        {
                            num20 -= this.mtfFreq[num6];
                            num6--;
                        }
                        num = 0;
                        while (true)
                        {
                            if (num >= alphaSize)
                            {
                                num17--;
                                num5 = num6 + 1;
                                nMTF -= num20;
                                break;
                            }
                            chArray[num17 - 1][num] = ((num < num5) || (num > num6)) ? '\x000f' : '\0';
                            num++;
                        }
                        break;
                    }
                    num6++;
                    num20 += this.mtfFreq[num6];
                }
            }
            int[][] numArray = CBZip2InputStream.InitIntArray(6, 0x102);
            int[] numArray2 = new int[6];
            short[] numArray3 = new short[6];
            int num10 = 0;
            while (num10 < 4)
            {
                num2 = 0;
                while (true)
                {
                    if (num2 >= v)
                    {
                        num2 = 0;
                        while (true)
                        {
                            if (num2 >= v)
                            {
                                index = 0;
                                int num7 = 0;
                                num5 = 0;
                                while (true)
                                {
                                    if (num5 >= this.nMTF)
                                    {
                                        num2 = 0;
                                        while (true)
                                        {
                                            if (num2 >= v)
                                            {
                                                num10++;
                                                break;
                                            }
                                            HbMakeCodeLengths(chArray[num2], numArray[num2], alphaSize, 20);
                                            num2++;
                                        }
                                        break;
                                    }
                                    if (((num5 + 50) - 1) >= this.nMTF)
                                    {
                                        num6 = this.nMTF - 1;
                                    }
                                    num2 = 0;
                                    while (true)
                                    {
                                        if (num2 >= v)
                                        {
                                            if (v != 6)
                                            {
                                                num3 = num5;
                                                while (num3 <= num6)
                                                {
                                                    short num28 = this.szptr[num3];
                                                    num2 = 0;
                                                    while (true)
                                                    {
                                                        if (num2 >= v)
                                                        {
                                                            num3++;
                                                            break;
                                                        }
                                                        short* numPtr1 = &(numArray3[num2]);
                                                        numPtr1[0] = (short) (numPtr1[0] + ((short) chArray[num2][num28]));
                                                        num2++;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                short num22;
                                                short num23;
                                                short num24;
                                                short num25;
                                                short num26;
                                                short num21 = num22 = num23 = num24 = num25 = num26 = 0;
                                                num3 = num5;
                                                while (true)
                                                {
                                                    if (num3 > num6)
                                                    {
                                                        numArray3[0] = num21;
                                                        numArray3[1] = num22;
                                                        numArray3[2] = num23;
                                                        numArray3[3] = num24;
                                                        numArray3[4] = num25;
                                                        numArray3[5] = num26;
                                                        break;
                                                    }
                                                    short num27 = this.szptr[num3];
                                                    num21 = (short) (num21 + ((short) chArray[0][num27]));
                                                    num22 = (short) (num22 + ((short) chArray[1][num27]));
                                                    num23 = (short) (num23 + ((short) chArray[2][num27]));
                                                    num24 = (short) (num24 + ((short) chArray[3][num27]));
                                                    num25 = (short) (num25 + ((short) chArray[4][num27]));
                                                    num26 = (short) (num26 + ((short) chArray[5][num27]));
                                                    num3++;
                                                }
                                            }
                                            int num9 = 0x3b9ac9ff;
                                            int num8 = -1;
                                            num2 = 0;
                                            while (true)
                                            {
                                                if (num2 >= v)
                                                {
                                                    num7 += num9;
                                                    int* numPtr2 = &(numArray2[num8]);
                                                    numPtr2[0]++;
                                                    this.selector[index] = (char) num8;
                                                    index++;
                                                    num3 = num5;
                                                    while (true)
                                                    {
                                                        if (num3 > num6)
                                                        {
                                                            num5 = num6 + 1;
                                                            break;
                                                        }
                                                        int* numPtr3 = &(numArray[num8][this.szptr[num3]]);
                                                        numPtr3[0]++;
                                                        num3++;
                                                    }
                                                    break;
                                                }
                                                if (numArray3[num2] < num9)
                                                {
                                                    num9 = numArray3[num2];
                                                    num8 = num2;
                                                }
                                                num2++;
                                            }
                                            break;
                                        }
                                        numArray3[num2] = 0;
                                        num2++;
                                    }
                                }
                                break;
                            }
                            num = 0;
                            while (true)
                            {
                                if (num >= alphaSize)
                                {
                                    num2++;
                                    break;
                                }
                                numArray[num2][num] = 0;
                                num++;
                            }
                        }
                        break;
                    }
                    numArray2[num2] = 0;
                    num2++;
                }
            }
            numArray = null;
            numArray2 = null;
            numArray3 = null;
            if (v >= 8)
            {
                Panic();
            }
            if ((index >= 0x8000) || (index > 0x4652))
            {
                Panic();
            }
            char[] chArray2 = new char[6];
            for (num3 = 0; num3 < v; num3++)
            {
                chArray2[num3] = (char) num3;
            }
            num3 = 0;
            while (num3 < index)
            {
                char ch = this.selector[num3];
                num4 = 0;
                char ch3 = chArray2[num4];
                while (true)
                {
                    if (ch == ch3)
                    {
                        chArray2[0] = ch3;
                        this.selectorMtf[num3] = (char) num4;
                        num3++;
                        break;
                    }
                    num4++;
                    char ch2 = ch3;
                    ch3 = chArray2[num4];
                    chArray2[num4] = ch2;
                }
            }
            int[][] numArray4 = CBZip2InputStream.InitIntArray(6, 0x102);
            num2 = 0;
            while (num2 < v)
            {
                int minLen = 0x20;
                int maxLen = 0;
                num3 = 0;
                while (true)
                {
                    if (num3 >= alphaSize)
                    {
                        if (maxLen > 20)
                        {
                            Panic();
                        }
                        if (minLen < 1)
                        {
                            Panic();
                        }
                        this.HbAssignCodes(numArray4[num2], chArray[num2], minLen, maxLen, alphaSize);
                        num2++;
                        break;
                    }
                    if (chArray[num2][num3] > maxLen)
                    {
                        maxLen = chArray[num2][num3];
                    }
                    if (chArray[num2][num3] < minLen)
                    {
                        minLen = chArray[num2][num3];
                    }
                    num3++;
                }
            }
            bool[] flagArray = new bool[0x10];
            num3 = 0;
            while (num3 < 0x10)
            {
                flagArray[num3] = false;
                num4 = 0;
                while (true)
                {
                    if (num4 >= 0x10)
                    {
                        num3++;
                        break;
                    }
                    if (this.inUse[(num3 * 0x10) + num4])
                    {
                        flagArray[num3] = true;
                    }
                    num4++;
                }
            }
            for (num3 = 0; num3 < 0x10; num3++)
            {
                if (flagArray[num3])
                {
                    this.BsW(1, 1);
                }
                else
                {
                    this.BsW(1, 0);
                }
            }
            for (num3 = 0; num3 < 0x10; num3++)
            {
                if (flagArray[num3])
                {
                    num4 = 0;
                    while (num4 < 0x10)
                    {
                        if (this.inUse[(num3 * 0x10) + num4])
                        {
                            this.BsW(1, 1);
                        }
                        else
                        {
                            this.BsW(1, 0);
                        }
                        num4++;
                    }
                }
            }
            this.BsW(3, v);
            this.BsW(15, index);
            num3 = 0;
            while (num3 < index)
            {
                num4 = 0;
                while (true)
                {
                    if (num4 >= this.selectorMtf[num3])
                    {
                        this.BsW(1, 0);
                        num3++;
                        break;
                    }
                    this.BsW(1, 1);
                    num4++;
                }
            }
            num2 = 0;
            while (num2 < v)
            {
                int num29 = chArray[num2][0];
                this.BsW(5, num29);
                num3 = 0;
                while (true)
                {
                    if (num3 >= alphaSize)
                    {
                        num2++;
                        break;
                    }
                    while (true)
                    {
                        if (num29 >= chArray[num2][num3])
                        {
                            while (true)
                            {
                                if (num29 <= chArray[num2][num3])
                                {
                                    this.BsW(1, 0);
                                    num3++;
                                    break;
                                }
                                this.BsW(2, 3);
                                num29--;
                            }
                            break;
                        }
                        this.BsW(2, 2);
                        num29++;
                    }
                }
            }
            int num15 = 0;
            num5 = 0;
            while (num5 < this.nMTF)
            {
                num6 = (num5 + 50) - 1;
                if (num6 >= this.nMTF)
                {
                    num6 = this.nMTF - 1;
                }
                num3 = num5;
                while (true)
                {
                    if (num3 > num6)
                    {
                        num5 = num6 + 1;
                        num15++;
                        break;
                    }
                    this.BsW(chArray[this.selector[num15]][this.szptr[num3]], numArray4[this.selector[num15]][this.szptr[num3]]);
                    num3++;
                }
            }
            if (num15 != index)
            {
                Panic();
            }
        }

        public override void SetLength(long value)
        {
        }

        private void SimpleSort(int lo, int hi, int d)
        {
            int num;
            int num3;
            int num4 = (hi - lo) + 1;
            if (num4 < 2)
            {
                return;
            }
            int index = 0;
            while (true)
            {
                if (this.incs[index] >= num4)
                {
                    index--;
                    break;
                }
                index++;
            }
            goto TR_001D;
        TR_0002:
            index--;
        TR_001D:
            while (true)
            {
                if (index < 0)
                {
                    return;
                }
                num3 = this.incs[index];
                num = lo + num3;
                break;
            }
            while (true)
            {
                while (true)
                {
                    if (num <= hi)
                    {
                        int num6 = this.zptr[num];
                        int num2 = num;
                        while (true)
                        {
                            if (this.FullGtU(this.zptr[num2 - num3] + d, num6 + d))
                            {
                                this.zptr[num2] = this.zptr[num2 - num3];
                                num2 -= num3;
                                if (num2 > ((lo + num3) - 1))
                                {
                                    continue;
                                }
                            }
                            this.zptr[num2] = num6;
                            num++;
                            if (num <= hi)
                            {
                                num6 = this.zptr[num];
                                num2 = num;
                                while (true)
                                {
                                    if (this.FullGtU(this.zptr[num2 - num3] + d, num6 + d))
                                    {
                                        this.zptr[num2] = this.zptr[num2 - num3];
                                        num2 -= num3;
                                        if (num2 > ((lo + num3) - 1))
                                        {
                                            continue;
                                        }
                                    }
                                    this.zptr[num2] = num6;
                                    num++;
                                    if (num > hi)
                                    {
                                        break;
                                    }
                                    num6 = this.zptr[num];
                                    num2 = num;
                                    while (true)
                                    {
                                        if (this.FullGtU(this.zptr[num2 - num3] + d, num6 + d))
                                        {
                                            this.zptr[num2] = this.zptr[num2 - num3];
                                            num2 -= num3;
                                            if (num2 > ((lo + num3) - 1))
                                            {
                                                continue;
                                            }
                                        }
                                        this.zptr[num2] = num6;
                                        num++;
                                        if ((this.workDone <= this.workLimit) || !this.firstAttempt)
                                        {
                                            break;
                                        }
                                        return;
                                    }
                                }
                            }
                            goto TR_0002;
                        }
                    }
                    else
                    {
                        goto TR_0002;
                    }
                }
            }
        }

        private void Vswap(int p1, int p2, int n)
        {
            int num = 0;
            while (n > 0)
            {
                num = this.zptr[p1];
                this.zptr[p1] = this.zptr[p2];
                this.zptr[p2] = num;
                p1++;
                p2++;
                n--;
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                this.WriteByte(buffer[i + offset]);
            }
        }

        public override void WriteByte(byte bv)
        {
            int num = (0x100 + bv) % 0x100;
            if (this.currentChar == -1)
            {
                this.currentChar = num;
                this.runLength++;
            }
            else if (this.currentChar != num)
            {
                this.WriteRun();
                this.runLength = 1;
                this.currentChar = num;
            }
            else
            {
                this.runLength++;
                if (this.runLength > 0xfe)
                {
                    this.WriteRun();
                    this.currentChar = -1;
                    this.runLength = 0;
                }
            }
        }

        private void WriteRun()
        {
            if (this.last >= this.allowableBlockSize)
            {
                this.EndBlock();
                this.InitBlock();
                this.WriteRun();
            }
            else
            {
                this.inUse[this.currentChar] = true;
                int num = 0;
                while (true)
                {
                    if (num >= this.runLength)
                    {
                        switch (this.runLength)
                        {
                            case 1:
                                this.last++;
                                this.block[this.last + 1] = (char) this.currentChar;
                                break;

                            case 2:
                                this.last++;
                                this.block[this.last + 1] = (char) this.currentChar;
                                this.last++;
                                this.block[this.last + 1] = (char) this.currentChar;
                                break;

                            case 3:
                                this.last++;
                                this.block[this.last + 1] = (char) this.currentChar;
                                this.last++;
                                this.block[this.last + 1] = (char) this.currentChar;
                                this.last++;
                                this.block[this.last + 1] = (char) this.currentChar;
                                break;

                            default:
                                this.inUse[this.runLength - 4] = true;
                                this.last++;
                                this.block[this.last + 1] = (char) this.currentChar;
                                this.last++;
                                this.block[this.last + 1] = (char) this.currentChar;
                                this.last++;
                                this.block[this.last + 1] = (char) this.currentChar;
                                this.last++;
                                this.block[this.last + 1] = (char) this.currentChar;
                                this.last++;
                                this.block[this.last + 1] = (char) (this.runLength - 4);
                                break;
                        }
                        break;
                    }
                    this.mCrc.UpdateCRC((ushort) this.currentChar);
                    num++;
                }
            }
        }

        public override bool CanRead =>
            false;

        public override bool CanSeek =>
            false;

        public override bool CanWrite =>
            true;

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

        internal class StackElem
        {
            internal int ll;
            internal int hh;
            internal int dd;
        }
    }
}

