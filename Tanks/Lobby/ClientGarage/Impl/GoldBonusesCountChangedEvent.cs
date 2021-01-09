namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x164d1167018L)]
    public class GoldBonusesCountChangedEvent : Event
    {
        public long NewCount { get; set; }
    }
}

