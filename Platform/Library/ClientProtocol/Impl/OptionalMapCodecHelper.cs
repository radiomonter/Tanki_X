namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.IO;

    public class OptionalMapCodecHelper
    {
        private const byte INPLACE_MASK_1_BYTES = 0x20;
        private const byte INPLACE_MASK_2_BYTES = 0x40;
        private const byte INPLACE_MASK_3_BYTES = 0x60;
        private const byte MASK_LENGTH_1_BYTE = 0x80;
        private const int MASK_LENGTH_3_BYTE = 0xc00000;
        private const byte INPLACE_MASK_FLAG = 0x80;
        private const byte MASK_LENGTH_2_BYTES_FLAG = 0x40;

        public static void EncodeOptionalMap(IOptionalMap optionalMap, Stream dest)
        {
            int size = optionalMap.GetSize();
            byte[] map = ((OptionalMap) optionalMap).GetMap();
            if (size <= 5)
            {
                dest.WriteByte((byte) (map[0] >> 3));
            }
            else if (size <= 13)
            {
                dest.WriteByte((byte) ((map[0] >> 3) + 0x20));
                dest.WriteByte((byte) ((map[1] >> 3) + (map[0] << 5)));
            }
            else if (size <= 0x15)
            {
                dest.WriteByte((byte) ((map[0] >> 3) + 0x40));
                dest.WriteByte((byte) ((map[1] >> 3) + (map[0] << 5)));
                dest.WriteByte((byte) ((map[2] >> 3) + (map[1] << 5)));
            }
            else if (size <= 0x1d)
            {
                dest.WriteByte((byte) ((map[0] >> 3) + 0x60));
                dest.WriteByte((byte) ((map[1] >> 3) + (map[0] << 5)));
                dest.WriteByte((byte) ((map[2] >> 3) + (map[1] << 5)));
                dest.WriteByte((byte) ((map[3] >> 3) + (map[2] << 5)));
            }
            else if (size <= 0x1f8)
            {
                int length = (size >> 3) + (((size & 7) != 0) ? 1 : 0);
                byte[] destinationArray = new byte[] { (byte) ((length & 0xff) + 0x80) };
                Array.Copy(map, 0, destinationArray, 1, length);
                dest.Write(destinationArray, 0, destinationArray.Length);
            }
            else
            {
                if (size > 0x2000000)
                {
                    throw new IndexOutOfRangeException("NullMap overflow");
                }
                int length = (size >> 3) + (((size & 7) != 0) ? 1 : 0);
                int num5 = length + 0xc00000;
                byte[] destinationArray = new byte[] { (byte) ((num5 & 0xff0000) >> 0x10), (byte) ((num5 & 0xff00) >> 8), (byte) (num5 & 0xff) };
                Array.Copy(map, 0, destinationArray, 3, length);
                dest.Write(destinationArray, 0, destinationArray.Length);
            }
        }

        public static void UnwrapOptionalMap(Stream packet, ProtocolBuffer dest)
        {
            BinaryReader reader = new BinaryReader(packet);
            byte num = reader.ReadByte();
            OptionalMap optionalMap = (OptionalMap) dest.OptionalMap;
            byte[] map = optionalMap.GetMap();
            if ((num & 0x80) != 0)
            {
                int num3;
                byte num2 = (byte) (num & 0x3f);
                if ((num & 0x40) == 0)
                {
                    num3 = num2;
                }
                else
                {
                    byte num4 = reader.ReadByte();
                    byte num5 = reader.ReadByte();
                    num3 = ((num2 << 0x10) + ((num4 & 0xff) << 8)) + (num5 & 0xff);
                }
                reader.Read(map, 0, num3);
                optionalMap.SetSize(num3 << 3);
            }
            else
            {
                byte num9;
                byte num10;
                byte num7 = (byte) (num << 3);
                switch (((num & 0x60) >> 5))
                {
                    case 0:
                        map[0] = num7;
                        optionalMap.SetSize(5);
                        break;

                    case 1:
                        num9 = reader.ReadByte();
                        map[0] = (byte) (num7 + (num9 >> 5));
                        map[1] = (byte) (num9 << 3);
                        optionalMap.SetSize(13);
                        break;

                    case 2:
                        num9 = reader.ReadByte();
                        num10 = reader.ReadByte();
                        map[0] = (byte) (num7 + (num9 >> 5));
                        map[1] = (byte) ((num9 << 3) + (num10 >> 5));
                        map[2] = (byte) (num10 << 3);
                        optionalMap.SetSize(0x15);
                        break;

                    case 3:
                    {
                        num9 = reader.ReadByte();
                        num10 = reader.ReadByte();
                        byte num11 = reader.ReadByte();
                        map[0] = (byte) (num7 + (num9 >> 5));
                        map[1] = (byte) ((num9 << 3) + (num10 >> 5));
                        map[2] = (byte) ((num10 << 3) + (num11 >> 5));
                        map[3] = (byte) (num11 << 3);
                        optionalMap.SetSize(0x1d);
                        break;
                    }
                    default:
                        break;
                }
            }
        }
    }
}

