namespace Platform.Library.ClientProtocol.API
{
    using Platform.Library.ClientProtocol.Impl;
    using System;

    public interface ClientProtocolInstancesCache
    {
        MemoryStreamData GetMemoryStreamDataInstance();
        ProtocolBuffer GetProtocolBufferInstance();
        void ReleaseMemoryStreamData(MemoryStreamData memoryStreamData);
        void ReleaseProtocolBufferInstance(ProtocolBuffer protocolBuffer);
    }
}

