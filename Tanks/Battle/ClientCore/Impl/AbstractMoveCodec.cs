namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections;
    using UnityEngine;

    public abstract class AbstractMoveCodec : NotOptionalCodec
    {
        protected AbstractMoveCodec()
        {
        }

        protected void CopyBits(byte[] buffer, BitArray bits)
        {
            int index = 0;
            while (index < buffer.Length)
            {
                int num2 = 0;
                while (true)
                {
                    if (num2 >= 8)
                    {
                        index++;
                        break;
                    }
                    int num3 = (index * 8) + num2;
                    bool flag = (buffer[index] & (1 << (num2 & 0x1f))) != 0;
                    bits.Set(num3, flag);
                    num2++;
                }
            }
        }

        private static int PrepareValue(float val, int offset, float factor)
        {
            int num = (int) (val / factor);
            int num2 = 0;
            if (num < -offset)
            {
                Debug.LogWarning($"Value too small {val} offset={offset} factor={factor}");
            }
            else
            {
                num2 = num - offset;
            }
            if (num2 >= offset)
            {
                Debug.LogWarning($"Value too big {val} offset={offset} factor={factor}");
                num2 = offset;
            }
            return num2;
        }

        private static int Read(BitArray bits, ref int position, int bitsCount)
        {
            if (bitsCount > 0x20)
            {
                throw new Exception("Cannot read more that 32 bit at once (requested " + bitsCount + ")");
            }
            if ((position + bitsCount) > bits.Length)
            {
                object[] objArray1 = new object[] { "BitArea is out of data: requesed ", bitsCount, " bits, avaliable:", bits.Length - position };
                throw new Exception(string.Concat(objArray1));
            }
            int num = 0;
            for (int i = bitsCount - 1; i >= 0; i--)
            {
                if (bits.Get(position))
                {
                    num += 1 << (i & 0x1f);
                }
                position++;
            }
            return num;
        }

        protected static float ReadFloat(BitArray bits, ref int position, int size, float factor)
        {
            float val = (Read(bits, ref position, size) - (1 << ((size - 1) & 0x1f))) * factor;
            if (PhysicsUtil.IsValidFloat(val))
            {
                return val;
            }
            Debug.LogError("AbstractMoveCodec.ReadFloat: invalid float: " + val);
            return 0f;
        }

        private static void Write(BitArray bits, ref int position, int bitsCount, int value)
        {
            if (bitsCount > 0x20)
            {
                throw new Exception("Cannot write more that 32 bit at once (requested " + bitsCount + ")");
            }
            if ((position + bitsCount) > bits.Length)
            {
                object[] objArray1 = new object[] { "BitArea overflow attempt to write ", bitsCount, " bits, space avaliable:", bits.Length - position };
                throw new Exception(string.Concat(objArray1));
            }
            for (int i = bitsCount - 1; i >= 0; i--)
            {
                bool flag = (value & (1 << (i & 0x1f))) != 0;
                bits.Set(position, flag);
                position++;
            }
        }

        protected static void WriteFloat(BitArray bits, ref int position, float value, int size, float factor)
        {
            int offset = 1 << ((size - 1) & 0x1f);
            Write(bits, ref position, size, PrepareValue(value, offset, factor));
        }
    }
}

