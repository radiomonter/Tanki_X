namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;

    public class VariedStructCodec : NotOptionalCodec
    {
        private Protocol protocol;

        public override object Decode(ProtocolBuffer protocolBuffer)
        {
            long uid = protocolBuffer.Reader.ReadInt64();
            return this.protocol.GetCodec(uid).Decode(protocolBuffer);
        }

        public override void Encode(ProtocolBuffer protocolBuffer, object data)
        {
            Type cl = data.GetType();
            long uidByType = this.protocol.GetUidByType(cl);
            protocolBuffer.Writer.Write(uidByType);
            ProtocolBuffer buffer = this.protocol.NewProtocolBuffer();
            this.protocol.GetCodec(cl).Encode(buffer, data);
            this.protocol.WrapPacket(buffer, protocolBuffer.Data);
            this.protocol.FreeProtocolBuffer(buffer);
        }

        public override void Init(Protocol protocol)
        {
            this.protocol = protocol;
        }
    }
}

