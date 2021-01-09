namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientDataStructures.Impl.Cache;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ClientProtocolInstancesCacheImpl : ClientProtocolInstancesCache
    {
        private readonly Cache<ProtocolBuffer> protocolBufferCache;
        private readonly Cache<MemoryStreamData> memoryStreamDataCache;
        [CompilerGenerated]
        private static Action<ProtocolBuffer> <>f__am$cache0;
        [CompilerGenerated]
        private static Action<MemoryStreamData> <>f__am$cache1;

        public ClientProtocolInstancesCacheImpl()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = new Action<ProtocolBuffer>(ClientProtocolInstancesCacheImpl.<ClientProtocolInstancesCacheImpl>m__0);
            }
            this.protocolBufferCache = new CacheImpl<ProtocolBuffer>(<>f__am$cache0);
            <>f__am$cache1 ??= new Action<MemoryStreamData>(ClientProtocolInstancesCacheImpl.<ClientProtocolInstancesCacheImpl>m__1);
            this.memoryStreamDataCache = new CacheImpl<MemoryStreamData>(<>f__am$cache1);
        }

        [CompilerGenerated]
        private static void <ClientProtocolInstancesCacheImpl>m__0(ProtocolBuffer a)
        {
            a.Clear();
        }

        [CompilerGenerated]
        private static void <ClientProtocolInstancesCacheImpl>m__1(MemoryStreamData a)
        {
            a.Clear();
        }

        public MemoryStreamData GetMemoryStreamDataInstance() => 
            this.memoryStreamDataCache.GetInstance();

        public ProtocolBuffer GetProtocolBufferInstance() => 
            this.protocolBufferCache.GetInstance();

        public void ReleaseMemoryStreamData(MemoryStreamData memoryStreamData)
        {
            this.memoryStreamDataCache.Free(memoryStreamData);
        }

        public void ReleaseProtocolBufferInstance(ProtocolBuffer protocolBuffer)
        {
            this.protocolBufferCache.Free(protocolBuffer);
        }
    }
}

