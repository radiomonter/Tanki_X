namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Reflection;

    public class OptionalTypeCodec : NotOptionalCodec
    {
        private readonly Type type;
        private readonly CodecInfoWithAttributes elementCodecInfo;
        private readonly MethodInfo emptyMethod;
        private readonly MethodInfo ofMethod;
        private readonly MethodInfo isPresentMethod;
        private readonly MethodInfo getMethod;
        private Codec elementCodec;

        public OptionalTypeCodec(Type type, CodecInfoWithAttributes elementCodecInfo)
        {
            this.type = type;
            this.elementCodecInfo = elementCodecInfo;
            this.emptyMethod = type.GetMethod("empty");
            this.ofMethod = type.GetMethod("of");
            this.isPresentMethod = type.GetMethod("IsPresent");
            this.getMethod = type.GetMethod("Get");
        }

        public override object Decode(ProtocolBuffer protocolBuffer)
        {
            if (protocolBuffer.OptionalMap.Get())
            {
                return this.emptyMethod.Invoke(null, null);
            }
            object obj2 = this.elementCodec.Decode(protocolBuffer);
            object[] parameters = new object[] { obj2 };
            return this.ofMethod.Invoke(null, parameters);
        }

        public override void Encode(ProtocolBuffer protocolBuffer, object data)
        {
            if (!((data != null) && ((bool) this.isPresentMethod.Invoke(data, null))))
            {
                protocolBuffer.OptionalMap.Add(true);
            }
            else
            {
                protocolBuffer.OptionalMap.Add(false);
                this.elementCodec.Encode(protocolBuffer, this.getMethod.Invoke(data, null));
            }
        }

        public override void Init(Protocol protocol)
        {
            this.elementCodec = protocol.GetCodec(this.elementCodecInfo);
        }
    }
}

