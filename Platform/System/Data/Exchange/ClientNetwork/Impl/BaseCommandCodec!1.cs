namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;

    public abstract class BaseCommandCodec<T> : CommandCodec, Codec where T: Command, new()
    {
        protected BaseCommandCodec()
        {
        }

        public virtual object Decode(ProtocolBuffer protocolBuffer) => 
            Activator.CreateInstance<T>();

        public virtual void DecodeToInstance(ProtocolBuffer protocolBuffer, object instance)
        {
        }

        public abstract void Encode(ProtocolBuffer protocolBuffer, object data);
        public abstract void Init(Protocol protocol);
    }
}

