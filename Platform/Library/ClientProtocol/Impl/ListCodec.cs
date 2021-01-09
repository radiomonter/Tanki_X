namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections;

    public class ListCodec : Codec
    {
        private readonly Type type;
        private readonly CodecInfoWithAttributes elementCodecInfo;
        private Codec elementCodec;

        public ListCodec(Type type, CodecInfoWithAttributes elementCodecInfo)
        {
            this.type = type;
            this.elementCodecInfo = elementCodecInfo;
        }

        public object Decode(ProtocolBuffer protocolBuffer)
        {
            IList list = (IList) Activator.CreateInstance(this.type);
            int num = LengthCodecHelper.DecodeLength(protocolBuffer.Reader);
            if (num > 0)
            {
                for (int i = 0; i < num; i++)
                {
                    object obj2 = this.elementCodec.Decode(protocolBuffer);
                    list.Add(obj2);
                }
            }
            return list;
        }

        public void DecodeToInstance(ProtocolBuffer protocolBuffer, object instance)
        {
            throw new NotImplementedException();
        }

        public void Encode(ProtocolBuffer protocolBuffer, object data)
        {
            IList list = (IList) data;
            LengthCodecHelper.EncodeLength(protocolBuffer.Data.Stream, list.Count);
            if (list.Count > 0)
            {
                IEnumerator enumerator = list.GetEnumerator();
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

