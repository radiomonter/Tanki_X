namespace Tanks.Lobby.ClientMatchMaking.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15f5df473eaL)]
    public class ExitedFromMatchMakingEvent : Event
    {
        public bool SelfAction { get; set; }
    }
}

