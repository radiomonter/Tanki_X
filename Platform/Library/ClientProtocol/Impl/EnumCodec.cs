namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;

    public class EnumCodec : NotOptionalCodec
    {
        private readonly Type enumType;

        public EnumCodec(Type enumType)
        {
            this.enumType = enumType;
        }

        public override object Decode(ProtocolBuffer protocolBuffer) => 
            Enum.ToObject(this.enumType, protocolBuffer.Reader.ReadByte());

        public override void Encode(ProtocolBuffer protocolBuffer, object data)
        {
            base.Encode(protocolBuffer, data);
            Enum enum2 = (Enum) data;
            if (enum2.GetTypeCode() != TypeCode.Byte)
            {
                throw new UnsupportedEnumTypeCodeException(enum2.GetTypeCode());
            }
            protocolBuffer.Writer.Write((byte) data);
        }

        public override void Init(Protocol protocol)
        {
        }
    }
}

