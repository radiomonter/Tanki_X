namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;

    public class LongCodec : NotOptionalCodec
    {
        public override object Decode(ProtocolBuffer protocolBuffer) => 
            protocolBuffer.Reader.ReadInt64();

        public override void Encode(ProtocolBuffer protocolBuffer, object data)
        {
            base.Encode(protocolBuffer, data);
            protocolBuffer.Writer.Write((long) data);
        }

        public override void Init(Protocol protocol)
        {
        }
    }
}

