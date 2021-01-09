namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientDataStructures.Impl.Cache;
    using Platform.Library.ClientProtocol.API;
    using Platform.System.Data.Exchange.ClientNetwork.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ClientNetworkInstancesCacheImpl : ClientNetworkInstancesCache
    {
        private readonly Cache<CommandPacket> commandPacketCache = new CacheImpl<CommandPacket>();
        private readonly Cache<List<Command>> commandsCollectionCache;
        [CompilerGenerated]
        private static Action<List<Command>> <>f__am$cache0;

        public ClientNetworkInstancesCacheImpl()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = new Action<List<Command>>(ClientNetworkInstancesCacheImpl.<ClientNetworkInstancesCacheImpl>m__0);
            }
            this.commandsCollectionCache = new CacheImpl<List<Command>>(<>f__am$cache0);
        }

        [CompilerGenerated]
        private static void <ClientNetworkInstancesCacheImpl>m__0(List<Command> a)
        {
            a.Clear();
        }

        public List<Command> GetCommandCollection() => 
            this.commandsCollectionCache.GetInstance();

        public CommandPacket GetCommandPacketInstance(List<Command> commands)
        {
            CommandPacket instance = this.commandPacketCache.GetInstance();
            instance.Commands = commands;
            return instance;
        }

        public void ReleaseCommandCollection(List<Command> commands)
        {
            this.commandsCollectionCache.Free(commands);
        }

        public void ReleaseCommandPacketWithCommandsCollection(CommandPacket commandPacket)
        {
            this.ReleaseCommandCollection(commandPacket.Commands);
            this.commandPacketCache.Free(commandPacket);
        }

        [Inject]
        public static Platform.Library.ClientProtocol.API.ClientProtocolInstancesCache ClientProtocolInstancesCache { get; set; }
    }
}

