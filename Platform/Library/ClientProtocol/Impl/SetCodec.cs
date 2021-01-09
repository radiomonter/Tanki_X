namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections;
    using System.Reflection;

    public class SetCodec : Codec
    {
        private readonly Type type;
        private readonly CodecInfoWithAttributes elementCodecInfo;
        private MethodInfo addMethod;
        private Codec elementCodec;
        private PropertyInfo countProperty;

        public SetCodec(Type type, CodecInfoWithAttributes elementCodecInfo)
        {
            this.type = type;
            this.elementCodecInfo = elementCodecInfo;
            this.addMethod = type.GetMethod("Add");
            this.countProperty = type.GetProperty("Count");
        }

        public object Decode(ProtocolBuffer protocolBuffer)
        {
            object obj2 = Activator.CreateInstance(this.type);
            int num = LengthCodecHelper.DecodeLength(protocolBuffer.Reader);
            if (num > 0)
            {
                for (int i = 0; i < num; i++)
                {
                    object obj3 = this.elementCodec.Decode(protocolBuffer);
                    object[] parameters = new object[] { obj3 };
                    this.addMethod.Invoke(obj2, parameters);
                }
            }
            return obj2;
        }

        public void DecodeToInstance(ProtocolBuffer protocolBuffer, object instance)
        {
            throw new NotImplementedException();
        }

        public void Encode(ProtocolBuffer protocolBuffer, object data)
        {
            int length = (int) this.countProperty.GetValue(data, BindingFlags.Default, null, null, null);
            LengthCodecHelper.EncodeLength(protocolBuffer.Data.Stream, length);
            if (length > 0)
            {
                IEnumerator enumerator = ((IEnumerable) data).GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        object current = enumerator.Current;
                        this.elementCodec.Encode(protocolBuffer, current);
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
            }
        }

        public void Init(Protocol protocol)
        {
            this.elementCodec = protocol.GetCodec(this.elementCodecInfo);
        }
    }
}

