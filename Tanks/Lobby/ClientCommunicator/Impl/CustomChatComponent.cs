namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [SerialVersionUID(0x8d5168e968c2fdeL)]
    public class CustomChatComponent : Component
    {
        public Tanks.Lobby.ClientCommunicator.Impl.ChatType ChatType =>
            Tanks.Lobby.ClientCommunicator.Impl.ChatType.CUSTOMGROUP;
    }
}

