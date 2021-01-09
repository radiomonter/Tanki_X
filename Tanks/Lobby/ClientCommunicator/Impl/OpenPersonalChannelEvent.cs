namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class OpenPersonalChannelEvent : Event
    {
        public string UserUid { get; set; }
    }
}

