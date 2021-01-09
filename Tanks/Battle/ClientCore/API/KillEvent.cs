namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(-8835994525014820133L)]
    public class KillEvent : Event
    {
        public Entity Target { get; set; }

        public Entity KillerMarketItem { get; set; }
    }
}

