namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d5314193547c6fL)]
    public class CreatePrivateChatEvent : Event
    {
        public string UserUid { get; set; }
    }
}

