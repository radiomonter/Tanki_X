namespace SharpCompress.Compressor.Deflate
{
    using System;

    internal sealed class InflateBlocks
    {
        private const int MANY = 0x5a0;
        internal static readonly int[] border = new int[] { 
            0x10, 0x11, 0x12, 0, 8, 7, 9, 6, 10, 5, 11, 4, 12, 3, 13, 2,
            14, 1, 15
        };
        internal ZlibCodec _codec;
        internal int[] bb = new int[1];
        internal int bitb;
        internal int bitk;
        internal int[] blens;
        internal uint check;
        internal object checkfn;
        internal InflateCodes codes = new InflateCodes();
        internal int end;
        internal int[] hufts;
        internal int index;
        internal InfTree inftree = new InfTree();
        internal int last;
        internal int left;
        private InflateBlockMode mode;
        internal int readAt;
        internal int table;
        internal int[] tb = new int[1];
        internal byte[] window;
        internal int writeAt;

        internal InflateBlocks(ZlibCodec codec, object checkfn, int w)
        {
            this._codec = codec;
            this.hufts = new int[0x10e0];
            this.window = new byte[w];
            this.end = w;
            this.checkfn = checkfn;
            this.mode = InflateBlockMode.TYPE;
            this.Reset();
        }

        internal int Flush(int r)
        {
            for (int i = 0; i < 2; i++)
            {
                int len = (i != 0) ? (this.writeAt - this.readAt) : (((this.readAt > this.writeAt) ? this.end : this.writeAt) - this.readAt);
                if (len == 0)
                {
                    if (r == -5)
                    {
                        r = 0;
                    }
                    return r;
                }
                if (len > this._codec.AvailableBytesOut)
                {
                    len = this._codec.AvailableBytesOut;
                }
                if ((len != 0) && (r == -5))
                {
                    r = 0;
                }
                this._codec.AvailableBytesOut -= len;
                this._codec.TotalBytesOut += len;
                if (this.checkfn != null)
                {
                    this._codec._Adler32 = this.check = Adler.Adler32(this.check, this.window, this.readAt, len);
                }
                Array.Copy(this.window, this.readAt, this._codec.OutputBuffer, this._codec.NextOut, len);
                this._codec.NextOut += len;
                this.readAt += len;
                if ((this.readAt != this.end) || (i != 0))
                {
                    i++;
                }
                else
                {
                    this.readAt = 0;
                    if (this.writeAt == this.end)
                    {
                        this.writeAt = 0;
                    }
                }
            }
            return r;
        }

        internal void Free()
        {
            this.Reset();
            this.window = null;
            this.hufts = null;
        }

        internal int Process(int r)
        {
            int table;
            int num9;
            int nextIn = this._codec.NextIn;
            int availableBytesIn = this._codec.AvailableBytesIn;
            int bitb = this.bitb;
            int bitk = this.bitk;
            int writeAt = this.writeAt;
            int num7 = (writeAt >= this.readAt) ? (this.end - writeAt) : ((this.readAt - writeAt) - 1);
            goto TR_0075;
        TR_0031:
            r = 1;
            this.bitb = bitb;
            this.bitk = bitk;
            this._codec.AvailableBytesIn = availableBytesIn;
            this._codec.TotalBytesIn += nextIn - this._codec.NextIn;
            this._codec.NextIn = nextIn;
            this.writeAt = writeAt;
            return this.Flush(r);
        TR_0033:
            this.writeAt = writeAt;
            r = this.Flush(r);
            writeAt = this.writeAt;
            num7 = (writeAt >= this.readAt) ? (this.end - writeAt) : ((this.readAt - writeAt) - 1);
            if (this.readAt != this.writeAt)
            {
                this.bitb = bitb;
                this.bitk = bitk;
                this._codec.AvailableBytesIn = availableBytesIn;
                this._codec.TotalBytesIn += nextIn - this._codec.NextIn;
                this._codec.NextIn = nextIn;
                this.writeAt = writeAt;
                return this.Flush(r);
            }
            this.mode = InflateBlockMode.DONE;
            goto TR_0031;
        TR_0038:
            this.bitb = bitb;
            this.bitk = bitk;
            this._codec.AvailableBytesIn = availableBytesIn;
            this._codec.TotalBytesIn += nextIn - this._codec.NextIn;
            this._codec.NextIn = nextIn;
            this.writeAt = writeAt;
            r = this.codes.Process(this, r);
            if (r != 1)
            {
                return this.Flush(r);
            }
            r = 0;
            nextIn = this._codec.NextIn;
            availableBytesIn = this._codec.AvailableBytesIn;
            bitb = this.bitb;
            bitk = this.bitk;
            writeAt = this.writeAt;
            num7 = (writeAt >= this.readAt) ? (this.end - writeAt) : ((this.readAt - writeAt) - 1);
            if (this.last != 0)
            {
                this.mode = InflateBlockMode.DRY;
                goto TR_0033;
            }
            else
            {
                this.mode = InflateBlockMode.TYPE;
            }
            goto TR_0075;
        TR_0056:
            while (true)
            {
                table = this.table;
                if (this.index >= ((0x102 + (table & 0x1f)) + ((table >> 5) & 0x1f)))
                {
                    this.tb[0] = -1;
                    int[] bl = new int[] { 9 };
                    int[] bd = new int[] { 6 };
                    int[] tl = new int[1];
                    int[] td = new int[1];
                    table = this.table;
                    table = this.inftree.inflate_trees_dynamic(0x101 + (table & 0x1f), 1 + ((table >> 5) & 0x1f), this.blens, bl, bd, tl, td, this.hufts, this._codec);
                    if (table == 0)
                    {
                        this.codes.Init(bl[0], bd[0], this.hufts, tl[0], this.hufts, td[0]);
                        this.mode = InflateBlockMode.CODES;
                        break;
                    }
                    if (table == -3)
                    {
                        this.blens = null;
                        this.mode = InflateBlockMode.BAD;
                    }
                    r = table;
                    this.bitb = bitb;
                    this.bitk = bitk;
                    this._codec.AvailableBytesIn = availableBytesIn;
                    this._codec.TotalBytesIn += nextIn - this._codec.NextIn;
                    this._codec.NextIn = nextIn;
                    this.writeAt = writeAt;
                    return this.Flush(r);
                }
                table = this.bb[0];
                while (true)
                {
                    if (bitk >= table)
                    {
                        table = this.hufts[((this.tb[0] + (bitb & InternalInflateConstants.InflateMask[table])) * 3) + 1];
                        int num12 = this.hufts[((this.tb[0] + (bitb & InternalInflateConstants.InflateMask[table])) * 3) + 2];
                        if (num12 < 0x10)
                        {
                            bitb = bitb >> (table & 0x1f);
                            bitk -= table;
                            this.index = (num9 = this.index) + 1;
                            this.blens[num9] = num12;
                        }
                        else
                        {
                            int index = (num12 != 0x12) ? (num12 - 14) : 7;
                            int num11 = (num12 != 0x12) ? 3 : 11;
                            while (true)
                            {
                                if (bitk >= (table + index))
                                {
                                    bitb = bitb >> (table & 0x1f);
                                    num11 += bitb & InternalInflateConstants.InflateMask[index];
                                    bitb = bitb >> (index & 0x1f);
                                    bitk = (bitk - table) - index;
                                    index = this.index;
                                    table = this.table;
                                    if (((index + num11) <= ((0x102 + (table & 0x1f)) + ((table >> 5) & 0x1f))) && ((num12 != 0x10) || (index >= 1)))
                                    {
                                        num12 = (num12 != 0x10) ? 0 : this.blens[index - 1];
                                        while (true)
                                        {
                                            this.blens[index++] = num12;
                                            if (--num11 == 0)
                                            {
                                                this.index = index;
                                                break;
                                            }
                                        }
                                        break;
                                    }
                                    this.blens = null;
                                    this.mode = InflateBlockMode.BAD;
                                    this._codec.Message = "invalid bit length repeat";
                                    r = -3;
                                    this.bitb = bitb;
                                    this.bitk = bitk;
                                    this._codec.AvailableBytesIn = availableBytesIn;
                                    this._codec.TotalBytesIn += nextIn - this._codec.NextIn;
                                    this._codec.NextIn = nextIn;
                                    this.writeAt = writeAt;
                                    return this.Flush(r);
                                }
                                if (availableBytesIn == 0)
                                {
                                    this.bitb = bitb;
                                    this.bitk = bitk;
                                    this._codec.AvailableBytesIn = availableBytesIn;
                                    this._codec.TotalBytesIn += nextIn - this._codec.NextIn;
                                    this._codec.NextIn = nextIn;
                                    this.writeAt = writeAt;
                                    return this.Flush(r);
                                }
                                r = 0;
                                availableBytesIn--;
                                bitb |= (this._codec.InputBuffer[nextIn++] & 0xff) << (bitk & 0x1f);
                                bitk += 8;
                            }
                        }
                        break;
                    }
                    if (availableBytesIn == 0)
                    {
                        this.bitb = bitb;
                        this.bitk = bitk;
                        this._codec.AvailableBytesIn = availableBytesIn;
                        this._codec.TotalBytesIn += nextIn - this._codec.NextIn;
                        this._codec.NextIn = nextIn;
                        this.writeAt = writeAt;
                        return this.Flush(r);
                    }
                    r = 0;
                    availableBytesIn--;
                    bitb |= (this._codec.InputBuffer[nextIn++] & 0xff) << (bitk & 0x1f);
                    bitk += 8;
                }
            }
            goto TR_0038;
        TR_0075:
            while (true)
            {
                InflateBlockMode mode = this.mode;
                switch (mode)
                {
                    case InflateBlockMode.TYPE:
                    {
                        while (true)
                        {
                            if (bitk >= 3)
                            {
                                table = bitb & 7;
                                this.last = table & 1;
                                switch ((table >> 1))
                                {
                                    case 0:
                                        bitk -= 3;
                                        table = bitk & 7;
                                        bitb = (bitb >> 3) >> (table & 0x1f);
                                        bitk -= table;
                                        this.mode = InflateBlockMode.LENS;
                                        break;

                                    case 1:
                                    {
                                        int[] bl = new int[1];
                                        int[] bd = new int[1];
                                        int[][] tl = new int[1][];
                                        int[][] td = new int[1][];
                                        InfTree.inflate_trees_fixed(bl, bd, tl, td, this._codec);
                                        this.codes.Init(bl[0], bd[0], tl[0], 0, td[0], 0);
                                        bitb = bitb >> 3;
                                        bitk -= 3;
                                        this.mode = InflateBlockMode.CODES;
                                        break;
                                    }
                                    case 2:
                                        bitb = bitb >> 3;
                                        bitk -= 3;
                                        this.mode = InflateBlockMode.TABLE;
                                        break;

                                    case 3:
                                        bitb = bitb >> 3;
                                        bitk -= 3;
                                        this.mode = InflateBlockMode.BAD;
                                        this._codec.Message = "invalid block type";
                                        r = -3;
                                        this.bitb = bitb;
                                        this.bitk = bitk;
                                        this._codec.AvailableBytesIn = availableBytesIn;
                                        this._codec.TotalBytesIn += nextIn - this._codec.NextIn;
                                        this._codec.NextIn = nextIn;
                                        this.writeAt = writeAt;
                                        return this.Flush(r);

                                    default:
                                        break;
                                }
                                break;
                            }
                            if (availableBytesIn == 0)
                            {
                                this.bitb = bitb;
                                this.bitk = bitk;
                                this._codec.AvailableBytesIn = availableBytesIn;
                                this._codec.TotalBytesIn += nextIn - this._codec.NextIn;
                                this._codec.NextIn = nextIn;
                                this.writeAt = writeAt;
                                return this.Flush(r);
                            }
                            r = 0;
                            availableBytesIn--;
                            bitb |= (this._codec.InputBuffer[nextIn++] & 0xff) << (bitk & 0x1f);
                            bitk += 8;
                        }
                        continue;
                    }
                    case InflateBlockMode.LENS:
                    {
                        while (true)
                        {
                            if (bitk >= 0x20)
                            {
                                if (((~bitb >> 0x10) & 0xffff) == (bitb & 0xffff))
                                {
                                    this.left = bitb & 0xffff;
                                    bitb = bitk = 0;
                                    this.mode = (this.left == 0) ? ((this.last == 0) ? InflateBlockMode.TYPE : InflateBlockMode.DRY) : InflateBlockMode.STORED;
                                    break;
                                }
                                this.mode = InflateBlockMode.BAD;
                                this._codec.Message = "invalid stored block lengths";
                                r = -3;
                                this.bitb = bitb;
                                this.bitk = bitk;
                                this._codec.AvailableBytesIn = availableBytesIn;
                                this._codec.TotalBytesIn += nextIn - this._codec.NextIn;
                                this._codec.NextIn = nextIn;
                                this.writeAt = writeAt;
                                return this.Flush(r);
                            }
                            if (availableBytesIn == 0)
                            {
                                this.bitb = bitb;
                                this.bitk = bitk;
                                this._codec.AvailableBytesIn = availableBytesIn;
                                this._codec.TotalBytesIn += nextIn - this._codec.NextIn;
                                this._codec.NextIn = nextIn;
                                this.writeAt = writeAt;
                                return this.Flush(r);
                            }
                            r = 0;
                            availableBytesIn--;
                            bitb |= (this._codec.InputBuffer[nextIn++] & 0xff) << (bitk & 0x1f);
                            bitk += 8;
                        }
                        continue;
                    }
                    case InflateBlockMode.STORED:
                    {
                        if (availableBytesIn == 0)
                        {
                            this.bitb = bitb;
                            this.bitk = bitk;
                            this._codec.AvailableBytesIn = availableBytesIn;
                            this._codec.TotalBytesIn += nextIn - this._codec.NextIn;
                            this._codec.NextIn = nextIn;
                            this.writeAt = writeAt;
                            return this.Flush(r);
                        }
                        if (num7 == 0)
                        {
                            if ((writeAt == this.end) && (this.readAt != 0))
                            {
                                writeAt = 0;
                                num7 = (writeAt >= this.readAt) ? (this.end - writeAt) : ((this.readAt - writeAt) - 1);
                            }
                            if (num7 == 0)
                            {
                                this.writeAt = writeAt;
                                r = this.Flush(r);
                                writeAt = this.writeAt;
                                num7 = (writeAt >= this.readAt) ? (this.end - writeAt) : ((this.readAt - writeAt) - 1);
                                if ((writeAt == this.end) && (this.readAt != 0))
                                {
                                    writeAt = 0;
                                    num7 = (writeAt >= this.readAt) ? (this.end - writeAt) : ((this.readAt - writeAt) - 1);
                                }
                                if (num7 == 0)
                                {
                                    this.bitb = bitb;
                                    this.bitk = bitk;
                                    this._codec.AvailableBytesIn = availableBytesIn;
                                    this._codec.TotalBytesIn += nextIn - this._codec.NextIn;
                                    this._codec.NextIn = nextIn;
                                    this.writeAt = writeAt;
                                    return this.Flush(r);
                                }
                            }
                        }
                        r = 0;
                        table = this.left;
                        if (table > availableBytesIn)
                        {
                            table = availableBytesIn;
                        }
                        if (table > num7)
                        {
                            table = num7;
                        }
                        Array.Copy(this._codec.InputBuffer, nextIn, this.window, writeAt, table);
                        nextIn += table;
                        availableBytesIn -= table;
                        writeAt += table;
                        num7 -= table;
                        this.left = num9 = this.left - table;
                        if (num9 == 0)
                        {
                            this.mode = (this.last == 0) ? InflateBlockMode.TYPE : InflateBlockMode.DRY;
                        }
                        continue;
                    }
                    case InflateBlockMode.TABLE:
                        while (true)
                        {
                            if (bitk >= 14)
                            {
                                this.table = table = bitb & 0x3fff;
                                if (((table & 0x1f) > 0x1d) || (((table >> 5) & 0x1f) > 0x1d))
                                {
                                    this.mode = InflateBlockMode.BAD;
                                    this._codec.Message = "too many length or distance symbols";
                                    r = -3;
                                    this.bitb = bitb;
                                    this.bitk = bitk;
                                    this._codec.AvailableBytesIn = availableBytesIn;
                                    this._codec.TotalBytesIn += nextIn - this._codec.NextIn;
                                    this._codec.NextIn = nextIn;
                                    this.writeAt = writeAt;
                                    return this.Flush(r);
                                }
                                table = (0x102 + (table & 0x1f)) + ((table >> 5) & 0x1f);
                                if ((this.blens != null) && (this.blens.Length >= table))
                                {
                                    Array.Clear(this.blens, 0, table);
                                }
                                else
                                {
                                    this.blens = new int[table];
                                }
                                bitb = bitb >> 14;
                                bitk -= 14;
                                this.index = 0;
                                this.mode = InflateBlockMode.BTREE;
                                break;
                            }
                            if (availableBytesIn == 0)
                            {
                                this.bitb = bitb;
                                this.bitk = bitk;
                                this._codec.AvailableBytesIn = availableBytesIn;
                                this._codec.TotalBytesIn += nextIn - this._codec.NextIn;
                                this._codec.NextIn = nextIn;
                                this.writeAt = writeAt;
                                return this.Flush(r);
                            }
                            r = 0;
                            availableBytesIn--;
                            bitb |= (this._codec.InputBuffer[nextIn++] & 0xff) << (bitk & 0x1f);
                            bitk += 8;
                        }
                        break;

                    case InflateBlockMode.BTREE:
                        break;

                    case InflateBlockMode.DTREE:
                        goto TR_0056;

                    case InflateBlockMode.CODES:
                        goto TR_0038;

                    case InflateBlockMode.DRY:
                        goto TR_0033;

                    case InflateBlockMode.DONE:
                        goto TR_0031;

                    case InflateBlockMode.BAD:
                        r = -3;
                        this.bitb = bitb;
                        this.bitk = bitk;
                        this._codec.AvailableBytesIn = availableBytesIn;
                        this._codec.TotalBytesIn += nextIn - this._codec.NextIn;
                        this._codec.NextIn = nextIn;
                        this.writeAt = writeAt;
                        return this.Flush(r);

                    default:
                        r = -2;
                        this.bitb = bitb;
                        this.bitk = bitk;
                        this._codec.AvailableBytesIn = availableBytesIn;
                        this._codec.TotalBytesIn += nextIn - this._codec.NextIn;
                        this._codec.NextIn = nextIn;
                        this.writeAt = writeAt;
                        return this.Flush(r);
                }
                while (true)
                {
                    if (this.index >= (4 + (this.table >> 10)))
                    {
                        while (true)
                        {
                            if (this.index >= 0x13)
                            {
                                this.bb[0] = 7;
                                table = this.inftree.inflate_trees_bits(this.blens, this.bb, this.tb, this.hufts, this._codec);
                                if (table == 0)
                                {
                                    this.index = 0;
                                    this.mode = InflateBlockMode.DTREE;
                                    break;
                                }
                                r = table;
                                if (r == -3)
                                {
                                    this.blens = null;
                                    this.mode = InflateBlockMode.BAD;
                                }
                                this.bitb = bitb;
                                this.bitk = bitk;
                                this._codec.AvailableBytesIn = availableBytesIn;
                                this._codec.TotalBytesIn += nextIn - this._codec.NextIn;
                                this._codec.NextIn = nextIn;
                                this.writeAt = writeAt;
                                return this.Flush(r);
                            }
                            this.index = (num9 = this.index) + 1;
                            this.blens[border[num9]] = 0;
                        }
                        break;
                    }
                    while (true)
                    {
                        if (bitk >= 3)
                        {
                            this.index = (num9 = this.index) + 1;
                            this.blens[border[num9]] = bitb & 7;
                            bitb = bitb >> 3;
                            bitk -= 3;
                            break;
                        }
                        if (availableBytesIn == 0)
                        {
                            this.bitb = bitb;
                            this.bitk = bitk;
                            this._codec.AvailableBytesIn = availableBytesIn;
                            this._codec.TotalBytesIn += nextIn - this._codec.NextIn;
                            this._codec.NextIn = nextIn;
                            this.writeAt = writeAt;
                            return this.Flush(r);
                        }
                        r = 0;
                        availableBytesIn--;
                        bitb |= (this._codec.InputBuffer[nextIn++] & 0xff) << (bitk & 0x1f);
                        bitk += 8;
                    }
                }
                break;
            }
            goto TR_0056;
        }

        internal uint Reset()
        {
            uint check = this.check;
            this.mode = InflateBlockMode.TYPE;
            this.bitk = 0;
            this.bitb = 0;
            this.readAt = this.writeAt = 0;
            if (this.checkfn != null)
            {
                this._codec._Adler32 = this.check = Adler.Adler32(0, null, 0, 0);
            }
            return check;
        }

        internal void SetDictionary(byte[] d, int start, int n)
        {
            Array.Copy(d, start, this.window, 0, n);
            this.readAt = this.writeAt = n;
        }

        internal int SyncPoint() => 
            (this.mode != InflateBlockMode.LENS) ? 0 : 1;

        private enum InflateBlockMode
        {
            TYPE,
            LENS,
            STORED,
            TABLE,
            BTREE,
            DTREE,
            CODES,
            DRY,
            DONE,
            BAD
        }
    }
}

