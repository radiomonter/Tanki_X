namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d531404acc9c83L)]
    public class InviteUserToChatEvent : Event
    {
        public string UserUid { get; set; }
    }
}

