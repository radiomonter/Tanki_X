namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;

    public class DateTimeCodec : NotOptionalCodec
    {
        private static readonly DateTime UNIX_EPOCH = new DateTime(0x7b2, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private const int TICKS_IN_MILLISECOND = 0x2710;

        public override object Decode(ProtocolBuffer protocolBuffer)
        {
            long num = protocolBuffer.Reader.ReadInt64();
            return new DateTime(UNIX_EPOCH.Ticks + (num * 0x2710L), DateTimeKind.Utc);
        }

        public override void Encode(ProtocolBuffer protocolBuffer, object data)
        {
            base.Encode(protocolBuffer, data);
            DateTimeOffset offset = new DateTimeOffset((DateTime) data);
            long num = (offset.UtcTicks - UNIX_EPOCH.Ticks) / 0x2710L;
            protocolBuffer.Writer.Write(num);
        }

        public override void Init(Protocol protocol)
        {
        }
    }
}

