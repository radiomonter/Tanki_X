namespace Platform.System.Data.Exchange.ClientNetwork.API
{
    using Platform.System.Data.Exchange.ClientNetwork.Impl;
    using System;
    using System.Collections.Generic;

    public interface ClientNetworkInstancesCache
    {
        List<Command> GetCommandCollection();
        CommandPacket GetCommandPacketInstance(List<Command> commands);
        void ReleaseCommandCollection(List<Command> commands);
        void ReleaseCommandPacketWithCommandsCollection(CommandPacket commandPacket);
    }
}

