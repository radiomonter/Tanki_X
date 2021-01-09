namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;

    public class OptionalCodec : Codec
    {
        private readonly Codec codec;

        public OptionalCodec(Codec codec)
        {
            this.codec = codec;
        }

        public object Decode(ProtocolBuffer protocolBuffer) => 
            !protocolBuffer.OptionalMap.Get() ? this.codec.Decode(protocolBuffer) : null;

        public virtual void DecodeToInstance(ProtocolBuffer protocolBuffer, object instance)
        {
            throw new NotImplementedException();
        }

        public void Encode(ProtocolBuffer protocolBuffer, object data)
        {
            if (data == null)
            {
                protocolBuffer.OptionalMap.Add(true);
            }
            else
            {
                protocolBuffer.OptionalMap.Add(false);
                this.codec.Encode(protocolBuffer, data);
            }
        }

        public void Init(Protocol protocol)
        {
            this.codec.Init(protocol);
        }
    }
}

