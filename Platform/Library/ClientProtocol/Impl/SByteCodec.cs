namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;

    public class SByteCodec : NotOptionalCodec
    {
        public override object Decode(ProtocolBuffer protocolBuffer) => 
            protocolBuffer.Reader.ReadSByte();

        public override void Encode(ProtocolBuffer protocolBuffer, object data)
        {
            base.Encode(protocolBuffer, data);
            protocolBuffer.Writer.Write((sbyte) data);
        }

        public override void Init(Protocol protocol)
        {
        }
    }
}

