namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [SerialVersionUID(0x8d532756220c93cL)]
    public class PersonalChatComponent : Component
    {
        public Tanks.Lobby.ClientCommunicator.Impl.ChatType ChatType =>
            Tanks.Lobby.ClientCommunicator.Impl.ChatType.PERSONAL;
    }
}

