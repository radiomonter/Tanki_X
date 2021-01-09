namespace Platform.Library.ClientProtocol.API
{
    using Platform.Library.ClientProtocol.Impl;
    using System;

    public abstract class NotOptionalCodec : Codec
    {
        protected NotOptionalCodec()
        {
        }

        public abstract object Decode(ProtocolBuffer protocolBuffer);
        public virtual void DecodeToInstance(ProtocolBuffer protocolBuffer, object instance)
        {
            throw new NotImplementedException();
        }

        public virtual void Encode(ProtocolBuffer protocolBuffer, object data)
        {
            if (data == null)
            {
                throw new OptionalAnnotationNotFoundForNullObjectException();
            }
        }

        public abstract void Init(Protocol protocol);
    }
}

