namespace Platform.Library.ClientProtocol.Impl
{
    using System;
    using System.IO;

    public class LengthCodecHelper
    {
        public static int DecodeLength(BinaryReader buf)
        {
            byte num = buf.ReadByte();
            if ((num & 0x80) == 0)
            {
                return num;
            }
            byte num2 = buf.ReadByte();
            if ((num & 0x40) == 0)
            {
                return (((num & 0x3f) << 8) + (num2 & 0xff));
            }
            byte num3 = buf.ReadByte();
            return ((((num & 0x3f) << 0x10) + ((num2 & 0xff) << 8)) + (num3 & 0xff));
        }

        public static void EncodeLength(Stream buf, int length)
        {
            if (length < 0)
            {
                throw new IndexOutOfRangeException("length=" + length);
            }
            if (length < 0x80)
            {
                buf.WriteByte((byte) (length & 0x7f));
            }
            else if (length < 0x4000)
            {
                long num = (length & 0x3fff) + 0x8000;
                buf.WriteByte((byte) ((num & 0xff00L) >> 8));
                buf.WriteByte((byte) (num & 0xffL));
            }
            else
            {
                if (length >= 0x400000)
                {
                    throw new IndexOutOfRangeException("length=" + length);
                }
                long num2 = (length & 0x3fffff) + 0xc00000;
                buf.WriteByte((byte) ((num2 & 0xff0000L) >> 0x10));
                buf.WriteByte((byte) ((num2 & 0xff00L) >> 8));
                buf.WriteByte((byte) (num2 & 0xffL));
            }
        }
    }
}

