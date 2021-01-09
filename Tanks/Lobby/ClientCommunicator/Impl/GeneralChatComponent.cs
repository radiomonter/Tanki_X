namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [Shared, SerialVersionUID(0x8d5166fec3a3321L)]
    public class GeneralChatComponent : Component
    {
        [ProtocolTransient]
        public Tanks.Lobby.ClientCommunicator.Impl.ChatType ChatType =>
            Tanks.Lobby.ClientCommunicator.Impl.ChatType.GENERAL;
    }
}

