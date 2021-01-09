namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [SerialVersionUID(0x8d536c31e71f74bL)]
    public class SquadChatComponent : Component
    {
        public Tanks.Lobby.ClientCommunicator.Impl.ChatType ChatType =>
            Tanks.Lobby.ClientCommunicator.Impl.ChatType.SQUAD;
    }
}

