namespace SharpCompress.Compressor.Deflate
{
    using System;
    using System.Runtime.CompilerServices;

    internal sealed class DeflateManager
    {
        internal static readonly int[] ExtraLengthBits = new int[] { 
            0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2,
            3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 0
        };
        internal static readonly int[] ExtraDistanceBits = new int[] { 
            0, 0, 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6,
            7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12, 13, 13
        };
        private const int MEM_LEVEL_MAX = 9;
        private const int MEM_LEVEL_DEFAULT = 8;
        private CompressFunc DeflateFunction;
        private static readonly string[] _ErrorMessage;
        private const int PRESET_DICT = 0x20;
        private const int INIT_STATE = 0x2a;
        private const int BUSY_STATE = 0x71;
        private const int FINISH_STATE = 0x29a;
        private const int Z_DEFLATED = 8;
        private const int STORED_BLOCK = 0;
        private const int STATIC_TREES = 1;
        private const int DYN_TREES = 2;
        private const int Z_BINARY = 0;
        private const int Z_ASCII = 1;
        private const int Z_UNKNOWN = 2;
        private const int Buf_size = 0x10;
        private const int MIN_MATCH = 3;
        private const int MAX_MATCH = 0x102;
        private const int MIN_LOOKAHEAD = 0x106;
        private static readonly int HEAP_SIZE;
        private const int END_BLOCK = 0x100;
        internal ZlibCodec _codec;
        internal int status;
        internal byte[] pending;
        internal int nextPending;
        internal int pendingCount;
        internal sbyte data_type;
        internal int last_flush;
        internal int w_size;
        internal int w_bits;
        internal int w_mask;
        internal byte[] window;
        internal int window_size;
        internal short[] prev;
        private short[] head;
        private int ins_h;
        private int hash_size;
        private int hash_bits;
        private int hash_mask;
        private int hash_shift;
        private int blockStart;
        private Config config;
        private int match_length;
        private int prev_match;
        private int match_available;
        private int strstart;
        private int match_start;
        private int lookahead;
        private int prev_length;
        private CompressionLevel compressionLevel;
        private CompressionStrategy compressionStrategy;
        private short[] dyn_ltree = new short[HEAP_SIZE * 2];
        private short[] dyn_dtree = new short[((2 * InternalConstants.D_CODES) + 1) * 2];
        private short[] bl_tree = new short[((2 * InternalConstants.BL_CODES) + 1) * 2];
        private Tree treeLiterals = new Tree();
        private Tree treeDistances = new Tree();
        private Tree treeBitLengths = new Tree();
        private short[] bl_count = new short[InternalConstants.MAX_BITS + 1];
        private int[] heap = new int[(2 * InternalConstants.L_CODES) + 1];
        private int heap_len;
        private int heap_max;
        private sbyte[] depth = new sbyte[(2 * InternalConstants.L_CODES) + 1];
        private int _lengthOffset;
        internal int lit_bufsize;
        internal int last_lit;
        internal int _distanceOffset;
        internal int opt_len;
        internal int static_len;
        internal int matches;
        internal int last_eob_len;
        internal short bi_buf;
        internal int bi_valid;
        private bool Rfc1950BytesEmitted;
        private bool _WantRfc1950HeaderBytes = true;

        static DeflateManager()
        {
            string[] textArray1 = new string[10];
            textArray1[0] = "need dictionary";
            textArray1[1] = "stream end";
            textArray1[2] = string.Empty;
            textArray1[3] = "file error";
            textArray1[4] = "stream error";
            textArray1[5] = "data error";
            textArray1[6] = "insufficient memory";
            textArray1[7] = "buffer error";
            textArray1[8] = "incompatible version";
            textArray1[9] = string.Empty;
            _ErrorMessage = textArray1;
            HEAP_SIZE = (2 * InternalConstants.L_CODES) + 1;
        }

        internal DeflateManager()
        {
        }

        private void _fillWindow()
        {
            while (true)
            {
                int num;
                int size = (this.window_size - this.lookahead) - this.strstart;
                if ((size == 0) && ((this.strstart == 0) && (this.lookahead == 0)))
                {
                    size = this.w_size;
                }
                else if (size == -1)
                {
                    size--;
                }
                else if (this.strstart >= ((this.w_size + this.w_size) - 0x106))
                {
                    Array.Copy(this.window, this.w_size, this.window, 0, this.w_size);
                    this.match_start -= this.w_size;
                    this.strstart -= this.w_size;
                    this.blockStart -= this.w_size;
                    num = this.hash_size;
                    int index = num;
                    while (true)
                    {
                        int num2 = this.head[--index] & 0xffff;
                        this.head[index] = (num2 < this.w_size) ? ((short) 0) : ((short) (num2 - this.w_size));
                        if (--num == 0)
                        {
                            num = this.w_size;
                            index = num;
                            while (true)
                            {
                                num2 = this.prev[--index] & 0xffff;
                                this.prev[index] = (num2 < this.w_size) ? ((short) 0) : ((short) (num2 - this.w_size));
                                if (--num == 0)
                                {
                                    size += this.w_size;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                if (this._codec.AvailableBytesIn == 0)
                {
                    return;
                }
                num = this._codec.read_buf(this.window, this.strstart + this.lookahead, size);
                this.lookahead += num;
                if (this.lookahead >= 3)
                {
                    this.ins_h = this.window[this.strstart] & 0xff;
                    this.ins_h = ((this.ins_h << (this.hash_shift & 0x1f)) ^ (this.window[this.strstart + 1] & 0xff)) & this.hash_mask;
                }
                if ((this.lookahead >= 0x106) || (this._codec.AvailableBytesIn == 0))
                {
                    return;
                }
            }
        }

        internal void _InitializeBlocks()
        {
            for (int i = 0; i < InternalConstants.L_CODES; i++)
            {
                this.dyn_ltree[i * 2] = 0;
            }
            for (int j = 0; j < InternalConstants.D_CODES; j++)
            {
                this.dyn_dtree[j * 2] = 0;
            }
            for (int k = 0; k < InternalConstants.BL_CODES; k++)
            {
                this.bl_tree[k * 2] = 0;
            }
            this.dyn_ltree[0x200] = 1;
            this.opt_len = this.static_len = 0;
            this.last_lit = this.matches = 0;
        }

        private void _InitializeLazyMatch()
        {
            this.window_size = 2 * this.w_size;
            Array.Clear(this.head, 0, this.hash_size);
            this.config = Config.Lookup(this.compressionLevel);
            this.SetDeflater();
            this.strstart = 0;
            this.blockStart = 0;
            this.lookahead = 0;
            this.match_length = this.prev_length = 2;
            this.match_available = 0;
            this.ins_h = 0;
        }

        private void _InitializeTreeData()
        {
            this.treeLiterals.dyn_tree = this.dyn_ltree;
            this.treeLiterals.staticTree = StaticTree.Literals;
            this.treeDistances.dyn_tree = this.dyn_dtree;
            this.treeDistances.staticTree = StaticTree.Distances;
            this.treeBitLengths.dyn_tree = this.bl_tree;
            this.treeBitLengths.staticTree = StaticTree.BitLengths;
            this.bi_buf = 0;
            this.bi_valid = 0;
            this.last_eob_len = 8;
            this._InitializeBlocks();
        }

        internal void _tr_align()
        {
            this.send_bits(2, 3);
            this.send_code(0x100, StaticTree.lengthAndLiteralsTreeCodes);
            this.bi_flush();
            if ((((1 + this.last_eob_len) + 10) - this.bi_valid) < 9)
            {
                this.send_bits(2, 3);
                this.send_code(0x100, StaticTree.lengthAndLiteralsTreeCodes);
                this.bi_flush();
            }
            this.last_eob_len = 7;
        }

        internal void _tr_flush_block(int buf, int stored_len, bool eof)
        {
            int num;
            int num2;
            int num3 = 0;
            if (this.compressionLevel <= CompressionLevel.None)
            {
                num = num2 = stored_len + 5;
            }
            else
            {
                if (this.data_type == 2)
                {
                    this.set_data_type();
                }
                this.treeLiterals.build_tree(this);
                this.treeDistances.build_tree(this);
                num3 = this.BuildBlTree();
                num = ((this.opt_len + 3) + 7) >> 3;
                num2 = ((this.static_len + 3) + 7) >> 3;
                if (num2 <= num)
                {
                    num = num2;
                }
            }
            if (((stored_len + 4) <= num) && (buf != -1))
            {
                this._tr_stored_block(buf, stored_len, eof);
            }
            else if (num2 == num)
            {
                this.send_bits(2 + (!eof ? 0 : 1), 3);
                this.send_compressed_block(StaticTree.lengthAndLiteralsTreeCodes, StaticTree.distTreeCodes);
            }
            else
            {
                this.send_bits(4 + (!eof ? 0 : 1), 3);
                this.send_all_trees(this.treeLiterals.max_code + 1, this.treeDistances.max_code + 1, num3 + 1);
                this.send_compressed_block(this.dyn_ltree, this.dyn_dtree);
            }
            this._InitializeBlocks();
            if (eof)
            {
                this.bi_windup();
            }
        }

        internal void _tr_stored_block(int buf, int stored_len, bool eof)
        {
            this.send_bits(!eof ? 0 : 1, 3);
            this.copy_block(buf, stored_len, true);
        }

        internal unsafe bool _tr_tally(int dist, int lc)
        {
            this.pending[this._distanceOffset + (this.last_lit * 2)] = (byte) (dist >> 8);
            this.pending[(this._distanceOffset + (this.last_lit * 2)) + 1] = (byte) dist;
            this.pending[this._lengthOffset + this.last_lit] = (byte) lc;
            this.last_lit++;
            if (dist == 0)
            {
                short* numPtr1 = &(this.dyn_ltree[lc * 2]);
                numPtr1[0] = (short) (numPtr1[0] + 1);
            }
            else
            {
                this.matches++;
                dist--;
                short* numPtr2 = &(this.dyn_ltree[((Tree.LengthCode[lc] + InternalConstants.LITERALS) + 1) * 2]);
                numPtr2[0] = (short) (numPtr2[0] + 1);
                short* numPtr3 = &(this.dyn_dtree[Tree.DistanceCode(dist) * 2]);
                numPtr3[0] = (short) (numPtr3[0] + 1);
            }
            if (((this.last_lit & 0x1fff) == 0) && (this.compressionLevel > CompressionLevel.Level2))
            {
                int num = this.last_lit << 3;
                int num2 = this.strstart - this.blockStart;
                int index = 0;
                while (true)
                {
                    if (index >= InternalConstants.D_CODES)
                    {
                        num = num >> 3;
                        if ((this.matches >= (this.last_lit / 2)) || (num >= (num2 / 2)))
                        {
                            break;
                        }
                        return true;
                    }
                    num += (int) (this.dyn_dtree[index * 2] * (5L + ExtraDistanceBits[index]));
                    index++;
                }
            }
            return ((this.last_lit == (this.lit_bufsize - 1)) || (this.last_lit == this.lit_bufsize));
        }

        internal void bi_flush()
        {
            int num;
            if (this.bi_valid == 0x10)
            {
                this.pendingCount = (num = this.pendingCount) + 1;
                this.pending[num] = (byte) this.bi_buf;
                this.pendingCount = (num = this.pendingCount) + 1;
                this.pending[num] = (byte) (this.bi_buf >> 8);
                this.bi_buf = 0;
                this.bi_valid = 0;
            }
            else if (this.bi_valid >= 8)
            {
                this.pendingCount = (num = this.pendingCount) + 1;
                this.pending[num] = (byte) this.bi_buf;
                this.bi_buf = (short) (this.bi_buf >> 8);
                this.bi_valid -= 8;
            }
        }

        internal void bi_windup()
        {
            int num;
            if (this.bi_valid > 8)
            {
                this.pendingCount = (num = this.pendingCount) + 1;
                this.pending[num] = (byte) this.bi_buf;
                this.pendingCount = (num = this.pendingCount) + 1;
                this.pending[num] = (byte) (this.bi_buf >> 8);
            }
            else if (this.bi_valid > 0)
            {
                this.pendingCount = (num = this.pendingCount) + 1;
                this.pending[num] = (byte) this.bi_buf;
            }
            this.bi_buf = 0;
            this.bi_valid = 0;
        }

        internal int BuildBlTree()
        {
            this.ScanTree(this.dyn_ltree, this.treeLiterals.max_code);
            this.ScanTree(this.dyn_dtree, this.treeDistances.max_code);
            this.treeBitLengths.build_tree(this);
            int index = InternalConstants.BL_CODES - 1;
            while ((index >= 3) && (this.bl_tree[(Tree.bl_order[index] * 2) + 1] == 0))
            {
                index--;
            }
            this.opt_len += (((3 * (index + 1)) + 5) + 5) + 4;
            return index;
        }

        internal void copy_block(int buf, int len, bool header)
        {
            this.bi_windup();
            this.last_eob_len = 8;
            if (header)
            {
                int num;
                this.pendingCount = (num = this.pendingCount) + 1;
                this.pending[num] = (byte) len;
                this.pendingCount = (num = this.pendingCount) + 1;
                this.pending[num] = (byte) (len >> 8);
                this.pendingCount = (num = this.pendingCount) + 1;
                this.pending[num] = (byte) ~len;
                this.pendingCount = (num = this.pendingCount) + 1;
                this.pending[num] = (byte) (~len >> 8);
            }
            this.put_bytes(this.window, buf, len);
        }

        internal int Deflate(FlushType flush)
        {
            int num4;
            if (((this._codec.OutputBuffer == null) || ((this._codec.InputBuffer == null) && (this._codec.AvailableBytesIn != 0))) || ((this.status == 0x29a) && (flush != FlushType.Finish)))
            {
                this._codec.Message = _ErrorMessage[4];
                throw new ZlibException($"Something is fishy. [{this._codec.Message}]");
            }
            if (this._codec.AvailableBytesOut == 0)
            {
                this._codec.Message = _ErrorMessage[7];
                throw new ZlibException("OutputBuffer is full (AvailableBytesOut == 0)");
            }
            int num = this.last_flush;
            this.last_flush = (int) flush;
            if (this.status == 0x2a)
            {
                int num2 = (8 + ((this.w_bits - 8) << 4)) << 8;
                int num3 = ((int) ((this.compressionLevel - 1) & 0xff)) >> 1;
                if (num3 > 3)
                {
                    num3 = 3;
                }
                num2 |= num3 << 6;
                if (this.strstart != 0)
                {
                    num2 |= 0x20;
                }
                num2 += 0x1f - (num2 % 0x1f);
                this.status = 0x71;
                this.pendingCount = (num4 = this.pendingCount) + 1;
                this.pending[num4] = (byte) (num2 >> 8);
                this.pendingCount = (num4 = this.pendingCount) + 1;
                this.pending[num4] = (byte) num2;
                if (this.strstart != 0)
                {
                    this.pendingCount = (num4 = this.pendingCount) + 1;
                    this.pending[num4] = (byte) ((this._codec._Adler32 & -16777216) >> 0x18);
                    this.pendingCount = (num4 = this.pendingCount) + 1;
                    this.pending[num4] = (byte) ((this._codec._Adler32 & 0xff0000) >> 0x10);
                    this.pendingCount = (num4 = this.pendingCount) + 1;
                    this.pending[num4] = (byte) ((this._codec._Adler32 & 0xff00) >> 8);
                    this.pendingCount = (num4 = this.pendingCount) + 1;
                    this.pending[num4] = (byte) (this._codec._Adler32 & 0xff);
                }
                this._codec._Adler32 = Adler.Adler32(0, null, 0, 0);
            }
            if (this.pendingCount == 0)
            {
                if ((this._codec.AvailableBytesIn == 0) && ((flush <= num) && (flush != FlushType.Finish)))
                {
                    return 0;
                }
            }
            else
            {
                this._codec.flush_pending();
                if (this._codec.AvailableBytesOut == 0)
                {
                    this.last_flush = -1;
                    return 0;
                }
            }
            if ((this.status == 0x29a) && (this._codec.AvailableBytesIn != 0))
            {
                this._codec.Message = _ErrorMessage[7];
                throw new ZlibException("status == FINISH_STATE && _codec.AvailableBytesIn != 0");
            }
            if ((this._codec.AvailableBytesIn != 0) || ((this.lookahead != 0) || ((flush != FlushType.None) && (this.status != 0x29a))))
            {
                BlockState state = this.DeflateFunction(flush);
                if ((state == BlockState.FinishStarted) || (state == BlockState.FinishDone))
                {
                    this.status = 0x29a;
                }
                if ((state == BlockState.NeedMore) || (state == BlockState.FinishStarted))
                {
                    if (this._codec.AvailableBytesOut == 0)
                    {
                        this.last_flush = -1;
                    }
                    return 0;
                }
                if (state == BlockState.BlockDone)
                {
                    if (flush == FlushType.Partial)
                    {
                        this._tr_align();
                    }
                    else
                    {
                        this._tr_stored_block(0, 0, false);
                        if (flush == FlushType.Full)
                        {
                            for (int i = 0; i < this.hash_size; i++)
                            {
                                this.head[i] = 0;
                            }
                        }
                    }
                    this._codec.flush_pending();
                    if (this._codec.AvailableBytesOut == 0)
                    {
                        this.last_flush = -1;
                        return 0;
                    }
                }
            }
            if (flush != FlushType.Finish)
            {
                return 0;
            }
            if (!this.WantRfc1950HeaderBytes || this.Rfc1950BytesEmitted)
            {
                return 1;
            }
            this.pendingCount = (num4 = this.pendingCount) + 1;
            this.pending[num4] = (byte) ((this._codec._Adler32 & -16777216) >> 0x18);
            this.pendingCount = (num4 = this.pendingCount) + 1;
            this.pending[num4] = (byte) ((this._codec._Adler32 & 0xff0000) >> 0x10);
            this.pendingCount = (num4 = this.pendingCount) + 1;
            this.pending[num4] = (byte) ((this._codec._Adler32 & 0xff00) >> 8);
            this.pendingCount = (num4 = this.pendingCount) + 1;
            this.pending[num4] = (byte) (this._codec._Adler32 & 0xff);
            this._codec.flush_pending();
            this.Rfc1950BytesEmitted = true;
            return ((this.pendingCount == 0) ? 1 : 0);
        }

        internal BlockState DeflateFast(FlushType flush)
        {
            int num = 0;
            while (true)
            {
                bool flag;
                if (this.lookahead < 0x106)
                {
                    this._fillWindow();
                    if ((this.lookahead < 0x106) && (flush == FlushType.None))
                    {
                        return BlockState.NeedMore;
                    }
                    if (this.lookahead == 0)
                    {
                        this.flush_block_only(flush == FlushType.Finish);
                        return ((this._codec.AvailableBytesOut != 0) ? ((flush != FlushType.Finish) ? BlockState.BlockDone : BlockState.FinishDone) : ((flush != FlushType.Finish) ? BlockState.NeedMore : BlockState.FinishStarted));
                    }
                }
                if (this.lookahead >= 3)
                {
                    this.ins_h = ((this.ins_h << (this.hash_shift & 0x1f)) ^ (this.window[this.strstart + 2] & 0xff)) & this.hash_mask;
                    num = this.head[this.ins_h] & 0xffff;
                    this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
                    this.head[this.ins_h] = (short) this.strstart;
                }
                if ((num != 0L) && ((((this.strstart - num) & 0xffff) <= (this.w_size - 0x106)) && (this.compressionStrategy != CompressionStrategy.HuffmanOnly)))
                {
                    this.match_length = this.longest_match(num);
                }
                if (this.match_length < 3)
                {
                    flag = this._tr_tally(0, this.window[this.strstart] & 0xff);
                    this.lookahead--;
                    this.strstart++;
                }
                else
                {
                    flag = this._tr_tally(this.strstart - this.match_start, this.match_length - 3);
                    this.lookahead -= this.match_length;
                    if ((this.match_length > this.config.MaxLazy) || (this.lookahead < 3))
                    {
                        this.strstart += this.match_length;
                        this.match_length = 0;
                        this.ins_h = this.window[this.strstart] & 0xff;
                        this.ins_h = ((this.ins_h << (this.hash_shift & 0x1f)) ^ (this.window[this.strstart + 1] & 0xff)) & this.hash_mask;
                    }
                    else
                    {
                        this.match_length--;
                        while (true)
                        {
                            this.strstart++;
                            this.ins_h = ((this.ins_h << (this.hash_shift & 0x1f)) ^ (this.window[this.strstart + 2] & 0xff)) & this.hash_mask;
                            num = this.head[this.ins_h] & 0xffff;
                            this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
                            this.head[this.ins_h] = (short) this.strstart;
                            if (--this.match_length == 0)
                            {
                                this.strstart++;
                                break;
                            }
                        }
                    }
                }
                if (flag)
                {
                    this.flush_block_only(false);
                    if (this._codec.AvailableBytesOut == 0)
                    {
                        return BlockState.NeedMore;
                    }
                }
            }
        }

        internal BlockState DeflateNone(FlushType flush)
        {
            int num = 0xffff;
            if (num > (this.pending.Length - 5))
            {
                num = this.pending.Length - 5;
            }
            while (true)
            {
                if (this.lookahead <= 1)
                {
                    this._fillWindow();
                    if ((this.lookahead == 0) && (flush == FlushType.None))
                    {
                        return BlockState.NeedMore;
                    }
                    if (this.lookahead == 0)
                    {
                        this.flush_block_only(flush == FlushType.Finish);
                        return ((this._codec.AvailableBytesOut != 0) ? ((flush != FlushType.Finish) ? BlockState.BlockDone : BlockState.FinishDone) : ((flush != FlushType.Finish) ? BlockState.NeedMore : BlockState.FinishStarted));
                    }
                }
                this.strstart += this.lookahead;
                this.lookahead = 0;
                int num2 = this.blockStart + num;
                if ((this.strstart == 0) || (this.strstart >= num2))
                {
                    this.lookahead = this.strstart - num2;
                    this.strstart = num2;
                    this.flush_block_only(false);
                    if (this._codec.AvailableBytesOut == 0)
                    {
                        return BlockState.NeedMore;
                    }
                }
                if ((this.strstart - this.blockStart) >= (this.w_size - 0x106))
                {
                    this.flush_block_only(false);
                    if (this._codec.AvailableBytesOut == 0)
                    {
                        return BlockState.NeedMore;
                    }
                }
            }
        }

        internal BlockState DeflateSlow(FlushType flush)
        {
            int num = 0;
            while (true)
            {
                bool flag;
                if (this.lookahead < 0x106)
                {
                    this._fillWindow();
                    if ((this.lookahead < 0x106) && (flush == FlushType.None))
                    {
                        return BlockState.NeedMore;
                    }
                    if (this.lookahead == 0)
                    {
                        if (this.match_available != 0)
                        {
                            flag = this._tr_tally(0, this.window[this.strstart - 1] & 0xff);
                            this.match_available = 0;
                        }
                        this.flush_block_only(flush == FlushType.Finish);
                        return ((this._codec.AvailableBytesOut != 0) ? ((flush != FlushType.Finish) ? BlockState.BlockDone : BlockState.FinishDone) : ((flush != FlushType.Finish) ? BlockState.NeedMore : BlockState.FinishStarted));
                    }
                }
                if (this.lookahead >= 3)
                {
                    this.ins_h = ((this.ins_h << (this.hash_shift & 0x1f)) ^ (this.window[this.strstart + 2] & 0xff)) & this.hash_mask;
                    num = this.head[this.ins_h] & 0xffff;
                    this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
                    this.head[this.ins_h] = (short) this.strstart;
                }
                this.prev_length = this.match_length;
                this.prev_match = this.match_start;
                this.match_length = 2;
                if ((num != 0) && ((this.prev_length < this.config.MaxLazy) && (((this.strstart - num) & 0xffff) <= (this.w_size - 0x106))))
                {
                    if (this.compressionStrategy != CompressionStrategy.HuffmanOnly)
                    {
                        this.match_length = this.longest_match(num);
                    }
                    if ((this.match_length <= 5) && ((this.compressionStrategy == CompressionStrategy.Filtered) || ((this.match_length == 3) && ((this.strstart - this.match_start) > 0x1000))))
                    {
                        this.match_length = 2;
                    }
                }
                if ((this.prev_length < 3) || (this.match_length > this.prev_length))
                {
                    if (this.match_available == 0)
                    {
                        this.match_available = 1;
                        this.strstart++;
                        this.lookahead--;
                    }
                    else
                    {
                        if (this._tr_tally(0, this.window[this.strstart - 1] & 0xff))
                        {
                            this.flush_block_only(false);
                        }
                        this.strstart++;
                        this.lookahead--;
                        if (this._codec.AvailableBytesOut == 0)
                        {
                            return BlockState.NeedMore;
                        }
                    }
                }
                else
                {
                    int num2 = (this.strstart + this.lookahead) - 3;
                    flag = this._tr_tally((this.strstart - 1) - this.prev_match, this.prev_length - 3);
                    this.lookahead -= this.prev_length - 1;
                    this.prev_length -= 2;
                    while (true)
                    {
                        int num3;
                        this.strstart = num3 = this.strstart + 1;
                        if (num3 <= num2)
                        {
                            this.ins_h = ((this.ins_h << (this.hash_shift & 0x1f)) ^ (this.window[this.strstart + 2] & 0xff)) & this.hash_mask;
                            num = this.head[this.ins_h] & 0xffff;
                            this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
                            this.head[this.ins_h] = (short) this.strstart;
                        }
                        this.prev_length = num3 = this.prev_length - 1;
                        if (num3 == 0)
                        {
                            this.match_available = 0;
                            this.match_length = 2;
                            this.strstart++;
                            if (flag)
                            {
                                this.flush_block_only(false);
                                if (this._codec.AvailableBytesOut == 0)
                                {
                                    return BlockState.NeedMore;
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }

        internal int End()
        {
            if ((this.status != 0x2a) && ((this.status != 0x71) && (this.status != 0x29a)))
            {
                return -2;
            }
            this.pending = null;
            this.head = null;
            this.prev = null;
            this.window = null;
            return ((this.status != 0x71) ? 0 : -3);
        }

        internal void flush_block_only(bool eof)
        {
            this._tr_flush_block((this.blockStart < 0) ? -1 : this.blockStart, this.strstart - this.blockStart, eof);
            this.blockStart = this.strstart;
            this._codec.flush_pending();
        }

        internal int Initialize(ZlibCodec codec, CompressionLevel level) => 
            this.Initialize(codec, level, 15);

        internal int Initialize(ZlibCodec codec, CompressionLevel level, int bits) => 
            this.Initialize(codec, level, bits, 8, CompressionStrategy.Default);

        internal int Initialize(ZlibCodec codec, CompressionLevel level, int bits, CompressionStrategy compressionStrategy) => 
            this.Initialize(codec, level, bits, 8, compressionStrategy);

        internal int Initialize(ZlibCodec codec, CompressionLevel level, int windowBits, int memLevel, CompressionStrategy strategy)
        {
            this._codec = codec;
            this._codec.Message = null;
            if ((windowBits < 9) || (windowBits > 15))
            {
                throw new ZlibException("windowBits must be in the range 9..15.");
            }
            if ((memLevel < 1) || (memLevel > 9))
            {
                throw new ZlibException($"memLevel must be in the range 1.. {(int) 9}");
            }
            this._codec.dstate = this;
            this.w_bits = windowBits;
            this.w_size = 1 << (this.w_bits & 0x1f);
            this.w_mask = this.w_size - 1;
            this.hash_bits = memLevel + 7;
            this.hash_size = 1 << (this.hash_bits & 0x1f);
            this.hash_mask = this.hash_size - 1;
            this.hash_shift = ((this.hash_bits + 3) - 1) / 3;
            this.window = new byte[this.w_size * 2];
            this.prev = new short[this.w_size];
            this.head = new short[this.hash_size];
            this.lit_bufsize = 1 << ((memLevel + 6) & 0x1f);
            this.pending = new byte[this.lit_bufsize * 4];
            this._distanceOffset = this.lit_bufsize;
            this._lengthOffset = 3 * this.lit_bufsize;
            this.compressionLevel = level;
            this.compressionStrategy = strategy;
            this.Reset();
            return 0;
        }

        internal static bool IsSmaller(short[] tree, int n, int m, sbyte[] depth)
        {
            short num = tree[n * 2];
            short num2 = tree[m * 2];
            return ((num < num2) || ((num == num2) && (depth[n] <= depth[m])));
        }

        internal int longest_match(int cur_match)
        {
            int num1;
            int maxChainLength = this.config.MaxChainLength;
            int strstart = this.strstart;
            int num5 = this.prev_length;
            int num6 = (this.strstart <= (this.w_size - 0x106)) ? 0 : (this.strstart - (this.w_size - 0x106));
            int niceLength = this.config.NiceLength;
            int num8 = this.w_mask;
            int num9 = this.strstart + 0x102;
            byte num10 = this.window[(strstart + num5) - 1];
            byte num11 = this.window[strstart + num5];
            if (this.prev_length >= this.config.GoodLength)
            {
                maxChainLength = maxChainLength >> 2;
            }
            if (niceLength > this.lookahead)
            {
                niceLength = this.lookahead;
            }
            do
            {
                int index = cur_match;
                if ((this.window[index + num5] == num11) && ((this.window[(index + num5) - 1] == num10) && ((this.window[index] == this.window[strstart]) && (this.window[++index] == this.window[strstart + 1]))))
                {
                    strstart += 2;
                    index++;
                    while (true)
                    {
                        if ((this.window[++strstart] == this.window[++index]) && ((this.window[++strstart] == this.window[++index]) && ((this.window[++strstart] == this.window[++index]) && ((this.window[++strstart] == this.window[++index]) && ((this.window[++strstart] == this.window[++index]) && ((this.window[++strstart] == this.window[++index]) && ((this.window[++strstart] == this.window[++index]) && ((this.window[++strstart] == this.window[++index]) && (strstart < num9)))))))))
                        {
                            continue;
                        }
                        int num4 = 0x102 - (num9 - strstart);
                        strstart = num9 - 0x102;
                        if (num4 > num5)
                        {
                            this.match_start = cur_match;
                            num5 = num4;
                            if (num4 < niceLength)
                            {
                                num10 = this.window[(strstart + num5) - 1];
                                num11 = this.window[strstart + num5];
                            }
                            else
                            {
                                break;
                            }
                        }
                        break;
                    }
                }
                num1 = this.prev[cur_match & num8] & 0xffff;
            }
            while (((cur_match = num1) > num6) && (--maxChainLength != 0));
            return ((num5 > this.lookahead) ? this.lookahead : num5);
        }

        internal void pqdownheap(short[] tree, int k)
        {
            int n = this.heap[k];
            for (int i = k << 1; i <= this.heap_len; i = i << 1)
            {
                if ((i < this.heap_len) && IsSmaller(tree, this.heap[i + 1], this.heap[i], this.depth))
                {
                    i++;
                }
                if (IsSmaller(tree, n, this.heap[i], this.depth))
                {
                    break;
                }
                this.heap[k] = this.heap[i];
                k = i;
            }
            this.heap[k] = n;
        }

        private void put_bytes(byte[] p, int start, int len)
        {
            Array.Copy(p, start, this.pending, this.pendingCount, len);
            this.pendingCount += len;
        }

        internal void Reset()
        {
            this._codec.TotalBytesIn = this._codec.TotalBytesOut = 0L;
            this._codec.Message = null;
            this.pendingCount = 0;
            this.nextPending = 0;
            this.Rfc1950BytesEmitted = false;
            this.status = !this.WantRfc1950HeaderBytes ? 0x71 : 0x2a;
            this._codec._Adler32 = Adler.Adler32(0, null, 0, 0);
            this.last_flush = 0;
            this._InitializeTreeData();
            this._InitializeLazyMatch();
        }

        internal unsafe void ScanTree(short[] tree, int maxCode)
        {
            int num2 = -1;
            int num4 = tree[1];
            int num5 = 0;
            int num6 = 7;
            int num7 = 4;
            if (num4 == 0)
            {
                num6 = 0x8a;
                num7 = 3;
            }
            tree[((maxCode + 1) * 2) + 1] = 0x7fff;
            for (int i = 0; i <= maxCode; i++)
            {
                int num3 = num4;
                num4 = tree[((i + 1) * 2) + 1];
                if ((++num5 >= num6) || (num3 != num4))
                {
                    if (num5 < num7)
                    {
                        this.bl_tree[num3 * 2] = (short) (this.bl_tree[num3 * 2] + num5);
                    }
                    else if (num3 != 0)
                    {
                        if (num3 != num2)
                        {
                            short* numPtr1 = &(this.bl_tree[num3 * 2]);
                            numPtr1[0] = (short) (numPtr1[0] + 1);
                        }
                        short* numPtr2 = &(this.bl_tree[InternalConstants.REP_3_6 * 2]);
                        numPtr2[0] = (short) (numPtr2[0] + 1);
                    }
                    else if (num5 <= 10)
                    {
                        short* numPtr3 = &(this.bl_tree[InternalConstants.REPZ_3_10 * 2]);
                        numPtr3[0] = (short) (numPtr3[0] + 1);
                    }
                    else
                    {
                        short* numPtr4 = &(this.bl_tree[InternalConstants.REPZ_11_138 * 2]);
                        numPtr4[0] = (short) (numPtr4[0] + 1);
                    }
                    num5 = 0;
                    num2 = num3;
                    if (num4 == 0)
                    {
                        num6 = 0x8a;
                        num7 = 3;
                    }
                    else if (num3 == num4)
                    {
                        num6 = 6;
                        num7 = 3;
                    }
                    else
                    {
                        num6 = 7;
                        num7 = 4;
                    }
                }
            }
        }

        internal void send_all_trees(int lcodes, int dcodes, int blcodes)
        {
            this.send_bits(lcodes - 0x101, 5);
            this.send_bits(dcodes - 1, 5);
            this.send_bits(blcodes - 4, 4);
            for (int i = 0; i < blcodes; i++)
            {
                this.send_bits(this.bl_tree[(Tree.bl_order[i] * 2) + 1], 3);
            }
            this.send_tree(this.dyn_ltree, lcodes - 1);
            this.send_tree(this.dyn_dtree, dcodes - 1);
        }

        internal void send_bits(int value, int length)
        {
            int num = length;
            if (this.bi_valid <= (0x10 - num))
            {
                this.bi_buf = (short) (this.bi_buf | ((short) ((value << (this.bi_valid & 0x1f)) & 0xffff)));
                this.bi_valid += num;
            }
            else
            {
                int num2;
                this.bi_buf = (short) (this.bi_buf | ((short) ((value << (this.bi_valid & 0x1f)) & 0xffff)));
                this.pendingCount = (num2 = this.pendingCount) + 1;
                this.pending[num2] = (byte) this.bi_buf;
                this.pendingCount = (num2 = this.pendingCount) + 1;
                this.pending[num2] = (byte) (this.bi_buf >> 8);
                this.bi_buf = (short) (value >> ((0x10 - this.bi_valid) & 0x1f));
                this.bi_valid += num - 0x10;
            }
        }

        internal void send_code(int c, short[] tree)
        {
            int index = c * 2;
            this.send_bits(tree[index] & 0xffff, tree[index + 1] & 0xffff);
        }

        internal void send_compressed_block(short[] ltree, short[] dtree)
        {
            int num3 = 0;
            if (this.last_lit != 0)
            {
                do
                {
                    int index = this._distanceOffset + (num3 * 2);
                    int dist = ((this.pending[index] << 8) & 0xff00) | (this.pending[index + 1] & 0xff);
                    int c = this.pending[this._lengthOffset + num3] & 0xff;
                    num3++;
                    if (dist == 0)
                    {
                        this.send_code(c, ltree);
                    }
                    else
                    {
                        int num4 = Tree.LengthCode[c];
                        this.send_code((num4 + InternalConstants.LITERALS) + 1, ltree);
                        int length = ExtraLengthBits[num4];
                        if (length != 0)
                        {
                            c -= Tree.LengthBase[num4];
                            this.send_bits(c, length);
                        }
                        dist--;
                        num4 = Tree.DistanceCode(dist);
                        this.send_code(num4, dtree);
                        length = ExtraDistanceBits[num4];
                        if (length != 0)
                        {
                            dist -= Tree.DistanceBase[num4];
                            this.send_bits(dist, length);
                        }
                    }
                }
                while (num3 < this.last_lit);
            }
            this.send_code(0x100, ltree);
            this.last_eob_len = ltree[0x201];
        }

        internal void send_tree(short[] tree, int max_code)
        {
            int num2 = -1;
            int num4 = tree[1];
            int num5 = 0;
            int num6 = 7;
            int num7 = 4;
            if (num4 == 0)
            {
                num6 = 0x8a;
                num7 = 3;
            }
            for (int i = 0; i <= max_code; i++)
            {
                int c = num4;
                num4 = tree[((i + 1) * 2) + 1];
                if ((++num5 >= num6) || (c != num4))
                {
                    if (num5 < num7)
                    {
                        do
                        {
                            this.send_code(c, this.bl_tree);
                        }
                        while (--num5 != 0);
                    }
                    else if (c != 0)
                    {
                        if (c != num2)
                        {
                            this.send_code(c, this.bl_tree);
                            num5--;
                        }
                        this.send_code(InternalConstants.REP_3_6, this.bl_tree);
                        this.send_bits(num5 - 3, 2);
                    }
                    else if (num5 <= 10)
                    {
                        this.send_code(InternalConstants.REPZ_3_10, this.bl_tree);
                        this.send_bits(num5 - 3, 3);
                    }
                    else
                    {
                        this.send_code(InternalConstants.REPZ_11_138, this.bl_tree);
                        this.send_bits(num5 - 11, 7);
                    }
                    num5 = 0;
                    num2 = c;
                    if (num4 == 0)
                    {
                        num6 = 0x8a;
                        num7 = 3;
                    }
                    else if (c == num4)
                    {
                        num6 = 6;
                        num7 = 3;
                    }
                    else
                    {
                        num6 = 7;
                        num7 = 4;
                    }
                }
            }
        }

        internal void set_data_type()
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            while (num < 7)
            {
                num3 += this.dyn_ltree[num * 2];
                num++;
            }
            while (num < 0x80)
            {
                num2 += this.dyn_ltree[num * 2];
                num++;
            }
            while (num < InternalConstants.LITERALS)
            {
                num3 += this.dyn_ltree[num * 2];
                num++;
            }
            this.data_type = (num3 <= (num2 >> 2)) ? ((sbyte) 1) : ((sbyte) 0);
        }

        private void SetDeflater()
        {
            DeflateFlavor flavor = this.config.Flavor;
            if (flavor == DeflateFlavor.Store)
            {
                this.DeflateFunction = new CompressFunc(this.DeflateNone);
            }
            else if (flavor == DeflateFlavor.Fast)
            {
                this.DeflateFunction = new CompressFunc(this.DeflateFast);
            }
            else if (flavor == DeflateFlavor.Slow)
            {
                this.DeflateFunction = new CompressFunc(this.DeflateSlow);
            }
        }

        internal int SetDictionary(byte[] dictionary)
        {
            int length = dictionary.Length;
            int sourceIndex = 0;
            if ((dictionary == null) || (this.status != 0x2a))
            {
                throw new ZlibException("Stream error.");
            }
            this._codec._Adler32 = Adler.Adler32(this._codec._Adler32, dictionary, 0, dictionary.Length);
            if (length >= 3)
            {
                if (length > (this.w_size - 0x106))
                {
                    length = this.w_size - 0x106;
                    sourceIndex = dictionary.Length - length;
                }
                Array.Copy(dictionary, sourceIndex, this.window, 0, length);
                this.strstart = length;
                this.blockStart = length;
                this.ins_h = this.window[0] & 0xff;
                this.ins_h = ((this.ins_h << (this.hash_shift & 0x1f)) ^ (this.window[1] & 0xff)) & this.hash_mask;
                for (int i = 0; i <= (length - 3); i++)
                {
                    this.ins_h = ((this.ins_h << (this.hash_shift & 0x1f)) ^ (this.window[i + 2] & 0xff)) & this.hash_mask;
                    this.prev[i & this.w_mask] = this.head[this.ins_h];
                    this.head[this.ins_h] = (short) i;
                }
            }
            return 0;
        }

        internal int SetParams(CompressionLevel level, CompressionStrategy strategy)
        {
            int num = 0;
            if (this.compressionLevel != level)
            {
                Config config = Config.Lookup(level);
                if ((config.Flavor != this.config.Flavor) && (this._codec.TotalBytesIn != 0L))
                {
                    num = this._codec.Deflate(FlushType.Partial);
                }
                this.compressionLevel = level;
                this.config = config;
                this.SetDeflater();
            }
            this.compressionStrategy = strategy;
            return num;
        }

        internal bool WantRfc1950HeaderBytes
        {
            get => 
                this._WantRfc1950HeaderBytes;
            set => 
                this._WantRfc1950HeaderBytes = value;
        }

        internal enum BlockState
        {
            NeedMore,
            BlockDone,
            FinishStarted,
            FinishDone
        }

        internal delegate DeflateManager.BlockState CompressFunc(FlushType flush);

        internal class Config
        {
            internal int GoodLength;
            internal int MaxLazy;
            internal int NiceLength;
            internal int MaxChainLength;
            internal DeflateManager.DeflateFlavor Flavor;
            private static readonly DeflateManager.Config[] Table;

            static Config()
            {
                DeflateManager.Config[] configArray1 = new DeflateManager.Config[10];
                configArray1[0] = new DeflateManager.Config(0, 0, 0, 0, DeflateManager.DeflateFlavor.Store);
                configArray1[1] = new DeflateManager.Config(4, 4, 8, 4, DeflateManager.DeflateFlavor.Fast);
                configArray1[2] = new DeflateManager.Config(4, 5, 0x10, 8, DeflateManager.DeflateFlavor.Fast);
                configArray1[3] = new DeflateManager.Config(4, 6, 0x20, 0x20, DeflateManager.DeflateFlavor.Fast);
                configArray1[4] = new DeflateManager.Config(4, 4, 0x10, 0x10, DeflateManager.DeflateFlavor.Slow);
                configArray1[5] = new DeflateManager.Config(8, 0x10, 0x20, 0x20, DeflateManager.DeflateFlavor.Slow);
                configArray1[6] = new DeflateManager.Config(8, 0x10, 0x80, 0x80, DeflateManager.DeflateFlavor.Slow);
                configArray1[7] = new DeflateManager.Config(8, 0x20, 0x80, 0x100, DeflateManager.DeflateFlavor.Slow);
                configArray1[8] = new DeflateManager.Config(0x20, 0x80, 0x102, 0x400, DeflateManager.DeflateFlavor.Slow);
                configArray1[9] = new DeflateManager.Config(0x20, 0x102, 0x102, 0x1000, DeflateManager.DeflateFlavor.Slow);
                Table = configArray1;
            }

            private Config(int goodLength, int maxLazy, int niceLength, int maxChainLength, DeflateManager.DeflateFlavor flavor)
            {
                this.GoodLength = goodLength;
                this.MaxLazy = maxLazy;
                this.NiceLength = niceLength;
                this.MaxChainLength = maxChainLength;
                this.Flavor = flavor;
            }

            public static DeflateManager.Config Lookup(CompressionLevel level) => 
                Table[(int) level];
        }

        internal enum DeflateFlavor
        {
            Store,
            Fast,
            Slow
        }

        private sealed class Tree
        {
            internal const int Buf_size = 0x10;
            private static readonly int HEAP_SIZE = ((2 * InternalConstants.L_CODES) + 1);
            internal static readonly sbyte[] bl_order = new sbyte[] { 
                0x10, 0x11, 0x12, 0, 8, 7, 9, 6, 10, 5, 11, 4, 12, 3, 13, 2,
                14, 1, 15
            };
            private static readonly sbyte[] _dist_code = new sbyte[] { 
                0, 1, 2, 3, 4, 4, 5, 5, 6, 6, 6, 6, 7, 7, 7, 7,
                8, 8, 8, 8, 8, 8, 8, 8, 9, 9, 9, 9, 9, 9, 9, 9,
                10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10,
                11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11,
                12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12,
                12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12,
                13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13,
                13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13,
                14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
                14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
                14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
                14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
                15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
                15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
                15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
                15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
                0, 0, 0x10, 0x11, 0x12, 0x12, 0x13, 0x13, 20, 20, 20, 20, 0x15, 0x15, 0x15, 0x15,
                0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17,
                0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18,
                0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19,
                0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a,
                0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a,
                0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b,
                0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b,
                0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c,
                0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c,
                0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c,
                0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c, 0x1c,
                0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d,
                0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d,
                0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d,
                0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d, 0x1d
            };
            internal static readonly sbyte[] LengthCode = new sbyte[] { 
                0, 1, 2, 3, 4, 5, 6, 7, 8, 8, 9, 9, 10, 10, 11, 11,
                12, 12, 12, 12, 13, 13, 13, 13, 14, 14, 14, 14, 15, 15, 15, 15,
                0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11,
                0x12, 0x12, 0x12, 0x12, 0x12, 0x12, 0x12, 0x12, 0x13, 0x13, 0x13, 0x13, 0x13, 0x13, 0x13, 0x13,
                20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                0x15, 0x15, 0x15, 0x15, 0x15, 0x15, 0x15, 0x15, 0x15, 0x15, 0x15, 0x15, 0x15, 0x15, 0x15, 0x15,
                0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16,
                0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17,
                0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18,
                0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18,
                0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19,
                0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19,
                0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a,
                0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a, 0x1a,
                0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b,
                0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x1c
            };
            internal static readonly int[] LengthBase = new int[] { 
                0, 1, 2, 3, 4, 5, 6, 7, 8, 10, 12, 14, 0x10, 20, 0x18, 0x1c,
                0x20, 40, 0x30, 0x38, 0x40, 80, 0x60, 0x70, 0x80, 160, 0xc0, 0xe0, 0
            };
            internal static readonly int[] DistanceBase = new int[] { 
                0, 1, 2, 3, 4, 6, 8, 12, 0x10, 0x18, 0x20, 0x30, 0x40, 0x60, 0x80, 0xc0,
                0x100, 0x180, 0x200, 0x300, 0x400, 0x600, 0x800, 0xc00, 0x1000, 0x1800, 0x2000, 0x3000, 0x4000, 0x6000
            };
            internal short[] dyn_tree;
            internal int max_code;
            internal StaticTree staticTree;

            internal static int bi_reverse(int code, int len)
            {
                int num = 0;
                while (true)
                {
                    num |= code & 1;
                    code = code >> 1;
                    num = num << 1;
                    if (--len <= 0)
                    {
                        return (num >> 1);
                    }
                }
            }

            internal void build_tree(DeflateManager s)
            {
                int num2;
                int num5;
                int num6;
                short[] tree = this.dyn_tree;
                short[] treeCodes = this.staticTree.treeCodes;
                int elems = this.staticTree.elems;
                int num4 = -1;
                s.heap_len = 0;
                s.heap_max = HEAP_SIZE;
                for (num2 = 0; num2 < elems; num2++)
                {
                    if (tree[num2 * 2] == 0)
                    {
                        tree[(num2 * 2) + 1] = 0;
                    }
                    else
                    {
                        s.heap_len = num6 = s.heap_len + 1;
                        s.heap[num6] = num4 = num2;
                        s.depth[num2] = 0;
                    }
                }
                while (s.heap_len < 2)
                {
                    s.heap_len = num6 = s.heap_len + 1;
                    int index = num6;
                    s.heap[index] = num6 = (num4 >= 2) ? 0 : ++num4;
                    num5 = num6;
                    tree[num5 * 2] = 1;
                    s.depth[num5] = 0;
                    s.opt_len--;
                    if (treeCodes != null)
                    {
                        s.static_len -= treeCodes[(num5 * 2) + 1];
                    }
                }
                this.max_code = num4;
                num2 = s.heap_len / 2;
                while (num2 >= 1)
                {
                    s.pqdownheap(tree, num2);
                    num2--;
                }
                num5 = elems;
                while (true)
                {
                    num2 = s.heap[1];
                    s.heap_len = (num6 = s.heap_len) - 1;
                    s.heap[1] = s.heap[num6];
                    s.pqdownheap(tree, 1);
                    int index = s.heap[1];
                    s.heap_max = num6 = s.heap_max - 1;
                    s.heap[num6] = num2;
                    s.heap_max = num6 = s.heap_max - 1;
                    s.heap[num6] = index;
                    tree[num5 * 2] = (short) (tree[num2 * 2] + tree[index * 2]);
                    s.depth[num5] = (sbyte) (Math.Max((byte) s.depth[num2], (byte) s.depth[index]) + 1);
                    tree[(num2 * 2) + 1] = tree[(index * 2) + 1] = (short) num5;
                    s.heap[1] = num5++;
                    s.pqdownheap(tree, 1);
                    if (s.heap_len < 2)
                    {
                        s.heap_max = num6 = s.heap_max - 1;
                        s.heap[num6] = s.heap[1];
                        this.gen_bitlen(s);
                        gen_codes(tree, num4, s.bl_count);
                        return;
                    }
                }
            }

            internal static int DistanceCode(int dist) => 
                (dist >= 0x100) ? ((int) _dist_code[0x100 + SharedUtils.URShift(dist, 7)]) : ((int) _dist_code[dist]);

            internal unsafe void gen_bitlen(DeflateManager s)
            {
                int num4;
                short[] numArray = this.dyn_tree;
                short[] treeCodes = this.staticTree.treeCodes;
                int[] extraBits = this.staticTree.extraBits;
                int extraBase = this.staticTree.extraBase;
                int maxLength = this.staticTree.maxLength;
                int num9 = 0;
                int index = 0;
                while (index <= InternalConstants.MAX_BITS)
                {
                    s.bl_count[index] = 0;
                    index++;
                }
                numArray[(s.heap[s.heap_max] * 2) + 1] = 0;
                int num3 = s.heap_max + 1;
                while (num3 < HEAP_SIZE)
                {
                    num4 = s.heap[num3];
                    index = numArray[(numArray[(num4 * 2) + 1] * 2) + 1] + 1;
                    if (index > maxLength)
                    {
                        index = maxLength;
                        num9++;
                    }
                    numArray[(num4 * 2) + 1] = (short) index;
                    if (num4 <= this.max_code)
                    {
                        short* numPtr1 = &(s.bl_count[index]);
                        numPtr1[0] = (short) (numPtr1[0] + 1);
                        int num7 = 0;
                        if (num4 >= extraBase)
                        {
                            num7 = extraBits[num4 - extraBase];
                        }
                        short num8 = numArray[num4 * 2];
                        s.opt_len += num8 * (index + num7);
                        if (treeCodes != null)
                        {
                            s.static_len += num8 * (treeCodes[(num4 * 2) + 1] + num7);
                        }
                    }
                    num3++;
                }
                if (num9 != 0)
                {
                    while (true)
                    {
                        index = maxLength - 1;
                        while (true)
                        {
                            if (s.bl_count[index] != 0)
                            {
                                short* numPtr2 = &(s.bl_count[index]);
                                numPtr2[0] = (short) (numPtr2[0] - 1);
                                s.bl_count[index + 1] = (short) (s.bl_count[index + 1] + 2);
                                short* numPtr3 = &(s.bl_count[maxLength]);
                                numPtr3[0] = (short) (numPtr3[0] - 1);
                                if ((num9 - 2) > 0)
                                {
                                    break;
                                }
                                index = maxLength;
                                while (index != 0)
                                {
                                    num4 = s.bl_count[index];
                                    while (true)
                                    {
                                        if (num4 == 0)
                                        {
                                            index--;
                                            break;
                                        }
                                        int num5 = s.heap[--num3];
                                        if (num5 <= this.max_code)
                                        {
                                            if (numArray[(num5 * 2) + 1] != index)
                                            {
                                                s.opt_len += (index - numArray[(num5 * 2) + 1]) * numArray[num5 * 2];
                                                numArray[(num5 * 2) + 1] = (short) index;
                                            }
                                            num4--;
                                        }
                                    }
                                }
                                return;
                            }
                            index--;
                        }
                    }
                }
            }

            internal static unsafe void gen_codes(short[] tree, int max_code, short[] bl_count)
            {
                short[] numArray = new short[InternalConstants.MAX_BITS + 1];
                short num = 0;
                for (int i = 1; i <= InternalConstants.MAX_BITS; i++)
                {
                    short num1 = (short) ((num + bl_count[i - 1]) << 1);
                    numArray[i] = num = num1;
                }
                for (int j = 0; j <= max_code; j++)
                {
                    int index = tree[(j * 2) + 1];
                    if (index != 0)
                    {
                        short num5;
                        short* numPtr1 = &(numArray[index]);
                        numPtr1[0] = (short) ((num5 = numPtr1[0]) + 1);
                        tree[j * 2] = (short) bi_reverse(num5, index);
                    }
                }
            }
        }
    }
}

