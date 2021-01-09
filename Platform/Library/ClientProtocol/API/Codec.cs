namespace Platform.Library.ClientProtocol.API
{
    using System;

    public interface Codec
    {
        object Decode(ProtocolBuffer protocolBuffer);
        void DecodeToInstance(ProtocolBuffer protocolBuffer, object instance);
        void Encode(ProtocolBuffer protocolBuffer, object data);
        void Init(Protocol protocol);
    }
}

