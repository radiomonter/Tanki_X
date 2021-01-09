namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;

    public class ByteCodec : NotOptionalCodec
    {
        public override object Decode(ProtocolBuffer protocolBuffer) => 
            protocolBuffer.Reader.ReadByte();

        public override void Encode(ProtocolBuffer protocolBuffer, object data)
        {
            base.Encode(protocolBuffer, data);
            protocolBuffer.Writer.Write((byte) data);
        }

        public override void Init(Protocol protocol)
        {
        }
    }
}

