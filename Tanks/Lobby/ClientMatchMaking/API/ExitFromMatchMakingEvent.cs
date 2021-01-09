namespace Tanks.Lobby.ClientMatchMaking.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15c1f773caeL)]
    public class ExitFromMatchMakingEvent : Event
    {
        public bool InBattle { get; set; }
    }
}

