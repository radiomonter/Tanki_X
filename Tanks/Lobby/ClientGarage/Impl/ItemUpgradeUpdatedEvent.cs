namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x14e7225b826L), Shared]
    public class ItemUpgradeUpdatedEvent : Event
    {
        public int Level { get; set; }
    }
}

