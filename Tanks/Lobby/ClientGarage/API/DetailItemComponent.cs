namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d5268ff6355f42L)]
    public class DetailItemComponent : Component
    {
        public long TargetMarketItemId { get; set; }

        public long RequiredCount { get; set; }
    }
}

