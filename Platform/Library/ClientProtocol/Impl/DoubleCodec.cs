namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;

    public class DoubleCodec : NotOptionalCodec
    {
        public override object Decode(ProtocolBuffer protocolBuffer) => 
            protocolBuffer.Reader.ReadDouble();

        public override void Encode(ProtocolBuffer protocolBuffer, object data)
        {
            base.Encode(protocolBuffer, data);
            protocolBuffer.Writer.Write((double) data);
        }

        public override void Init(Protocol protocol)
        {
        }
    }
}

