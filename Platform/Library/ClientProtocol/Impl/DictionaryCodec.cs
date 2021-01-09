namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections;

    public class DictionaryCodec : Codec
    {
        private readonly Type type;
        private readonly CodecInfoWithAttributes keyRequest;
        private readonly CodecInfoWithAttributes valueRequest;
        private Codec keyCodec;
        private Codec valueCodec;

        public DictionaryCodec(Type type, CodecInfoWithAttributes keyRequest, CodecInfoWithAttributes valueRequest)
        {
            this.type = type;
            this.keyRequest = keyRequest;
            this.valueRequest = valueRequest;
        }

        public object Decode(ProtocolBuffer protocolBuffer)
        {
            int num = LengthCodecHelper.DecodeLength(protocolBuffer.Reader);
            IDictionary dictionary = (IDictionary) Activator.CreateInstance(this.type, (object[]) null);
            if (num > 0)
            {
                for (int i = 0; i < num; i++)
                {
                    object key = this.keyCodec.Decode(protocolBuffer);
                    object obj3 = this.valueCodec.Decode(protocolBuffer);
                    dictionary.Add(key, obj3);
                }
            }
            return dictionary;
        }

        public void DecodeToInstance(ProtocolBuffer protocolBuffer, object instance)
        {
            throw new NotImplementedException();
        }

        public void Encode(ProtocolBuffer protocolBuffer, object data)
        {
            IDictionary dictionary = (IDictionary) data;
            LengthCodecHelper.EncodeLength(protocolBuffer.Data.Stream, dictionary.Count);
            if (dictionary.Count > 0)
            {
                IEnumerator enumerator = dictionary.Keys.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        object current = enumerator.Current;
                        this.keyCodec.Encode(protocolBuffer, current);
                        this.valueCodec.Encode(protocolBuffer, dictionary[current]);
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
            this.keyCodec = protocol.GetCodec(this.keyRequest);
            this.valueCodec = protocol.GetCodec(this.valueRequest);
        }
    }
}

