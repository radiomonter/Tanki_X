namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;

    public class PacketHelper
    {
        private const int PACKET_HELPER_BUFFER_SIZE = 0x200;
        private static OptionalMap packetHelperOptionalMap = new OptionalMap(null, 0);
        private static byte[] packetHelperBuffer = new byte[0x200];

        private static int GetSizeInBytes(int size) => 
            (int) Math.Ceiling((double) (((double) size) / 8.0));

        public static bool UnwrapPacket(StreamData source, ProtocolBuffer dest)
        {
            BigEndianBinaryReader reader = source.Reader;
            long num = source.Length - source.Position;
            long position = source.Position;
            if (num < 10)
            {
                return false;
            }
            if (reader.ReadByte() != 0xff)
            {
                throw new CorruptBufferException();
            }
            if (reader.ReadByte() != 0)
            {
                throw new CorruptBufferException();
            }
            int size = reader.ReadInt32();
            int num6 = reader.ReadInt32();
            int sizeInBytes = GetSizeInBytes(size);
            if (num < ((sizeInBytes + num6) + 10))
            {
                source.Position = position;
                return false;
            }
            UpdatePackeHelperBuffer(sizeInBytes);
            source.Read(packetHelperBuffer, 0, sizeInBytes);
            packetHelperOptionalMap.Fill(packetHelperBuffer, size);
            dest.OptionalMap.Concat(packetHelperOptionalMap);
            UpdatePackeHelperBuffer(num6);
            source.Read(packetHelperBuffer, 0, num6);
            dest.Data.Write(packetHelperBuffer, 0, num6);
            dest.Flip();
            return true;
        }

        private static void UpdatePackeHelperBuffer(int size)
        {
            packetHelperBuffer = BufferUtils.GetBufferWithValidSize(packetHelperBuffer, size);
        }

        public static void WrapPacket(ProtocolBuffer source, StreamData dest)
        {
            BigEndianBinaryWriter writer = dest.Writer;
            writer.Write((byte) 0xff);
            writer.Write((byte) 0);
            OptionalMap optionalMap = (OptionalMap) source.OptionalMap;
            writer.Write(optionalMap.GetSize());
            writer.Write((int) source.Data.Length);
            byte[] map = optionalMap.GetMap();
            int sizeInBytes = GetSizeInBytes(optionalMap.GetSize());
            for (int i = 0; i < sizeInBytes; i++)
            {
                writer.Write(map[i]);
            }
            source.Data.CastedStream.WriteTo(dest.Stream);
        }
    }
}

