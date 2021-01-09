namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Text;

    public class StringCodec : NotOptionalCodec
    {
        private static readonly System.Text.Encoding Encoding = System.Text.Encoding.UTF8;

        public override object Decode(ProtocolBuffer protocolBuffer)
        {
            int count = LengthCodecHelper.DecodeLength(protocolBuffer.Reader);
            byte[] bytes = protocolBuffer.Reader.ReadBytes(count);
            return Encoding.GetString(bytes);
        }

        public override void Encode(ProtocolBuffer protocolBuffer, object data)
        {
            base.Encode(protocolBuffer, data);
            string s = (string) data;
            int byteCount = Encoding.GetByteCount(s);
            LengthCodecHelper.EncodeLength(protocolBuffer.Data.Stream, byteCount);
            protocolBuffer.Writer.Write(Encoding.GetBytes(s), 0, byteCount);
        }

        public override void Init(Protocol protocol)
        {
        }
    }
}

