namespace SharpCompress.Compressor.Filters
{
    using System;
    using System.IO;

    public class BCJFilter : Filter
    {
        private static readonly bool[] MASK_TO_ALLOWED_STATUS = new bool[] { true, true, true, false, true, false, false, false };
        private static readonly int[] MASK_TO_BIT_NUMBER = new int[] { 0, 1, 2, 2, 3, 3, 3, 3 };
        private int pos;
        private int prevMask;

        public BCJFilter(bool isEncoder, Stream baseStream) : base(isEncoder, baseStream, 5)
        {
            this.pos = 5;
        }

        private static bool test86MSByte(byte b) => 
            (b == 0) || (b == 0xff);

        protected override int Transform(byte[] buffer, int offset, int count)
        {
            int num = offset - 1;
            int num2 = (offset + count) - 5;
            int index = offset;
            while (true)
            {
                while (true)
                {
                    if (index > num2)
                    {
                        num = index - num;
                        this.prevMask = ((num & -4) == 0) ? (this.prevMask << ((num - 1) & 0x1f)) : 0;
                        index -= offset;
                        this.pos += index;
                        return index;
                    }
                    if ((buffer[index] & 0xfe) == 0xe8)
                    {
                        num = index - num;
                        if ((num & -4) != 0)
                        {
                            this.prevMask = 0;
                        }
                        else
                        {
                            this.prevMask = (this.prevMask << ((num - 1) & 0x1f)) & 7;
                            if ((this.prevMask != 0) && (!MASK_TO_ALLOWED_STATUS[this.prevMask] || test86MSByte(buffer[(index + 4) - MASK_TO_BIT_NUMBER[this.prevMask]])))
                            {
                                num = index;
                                this.prevMask = (this.prevMask << 1) | 1;
                                break;
                            }
                        }
                        num = index;
                        if (!test86MSByte(buffer[index + 4]))
                        {
                            this.prevMask = (this.prevMask << 1) | 1;
                        }
                        else
                        {
                            int num4 = ((buffer[index + 1] | (buffer[index + 2] << 8)) | (buffer[index + 3] << 0x10)) | (buffer[index + 4] << 0x18);
                            while (true)
                            {
                                int num5 = !base.isEncoder ? (num4 - ((this.pos + index) - offset)) : (num4 + ((this.pos + index) - offset));
                                if (this.prevMask != 0)
                                {
                                    int num6 = MASK_TO_BIT_NUMBER[this.prevMask] * 8;
                                    if (test86MSByte((byte) (num5 >> ((0x18 - num6) & 0x1f))))
                                    {
                                        num4 = num5 ^ ((1 << ((0x20 - num6) & 0x1f)) - 1);
                                        continue;
                                    }
                                }
                                buffer[index + 1] = (byte) num5;
                                buffer[index + 2] = (byte) (num5 >> 8);
                                buffer[index + 3] = (byte) (num5 >> 0x10);
                                buffer[index + 4] = (byte) ~(((num5 >> 0x18) & 1) - 1);
                                index += 4;
                                break;
                            }
                        }
                    }
                    break;
                }
                index++;
            }
        }
    }
}

