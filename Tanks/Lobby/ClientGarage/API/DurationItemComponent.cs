namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x16054abec1aL)]
    public class DurationItemComponent : Component
    {
        public TimeSpan Duration { get; set; }
    }
}

