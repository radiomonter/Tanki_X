namespace Platform.Library.ClientProtocol.API
{
    using Platform.Library.ClientProtocol.Impl;
    using System;

    public interface Protocol
    {
        void FreeProtocolBuffer(ProtocolBuffer protocolBuffer);
        Codec GetCodec(CodecInfoWithAttributes infoWithAttributes);
        Codec GetCodec(long uid);
        Codec GetCodec(Type type);
        Type GetTypeByUid(long uid);
        long GetUidByType(Type cl);
        ProtocolBuffer NewProtocolBuffer();
        void RegisterCodecForType<T>(Codec codec);
        void RegisterInheritanceCodecForType<T>(Codec codec);
        void RegisterTypeWithSerialUid(Type type);
        bool UnwrapPacket(StreamData source, ProtocolBuffer dest);
        void WrapPacket(ProtocolBuffer source, StreamData dest);

        int ServerProtocolVersion { get; set; }
    }
}

