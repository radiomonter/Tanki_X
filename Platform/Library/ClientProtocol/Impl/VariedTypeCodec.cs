namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;

    public class VariedTypeCodec : NotOptionalCodec
    {
        private Protocol protocol;

        public override object Decode(ProtocolBuffer protocolBuffer)
        {
            long uid = protocolBuffer.Reader.ReadInt64();
            return this.protocol.GetTypeByUid(uid);
        }

        public override void Encode(ProtocolBuffer protocolBuffer, object data)
        {
            Type cl = (Type) data;
            long uidByType = this.protocol.GetUidByType(cl);
            protocolBuffer.Writer.Write(uidByType);
        }

        public override void Init(Protocol protocol)
        {
            this.protocol = protocol;
        }
    }
}

