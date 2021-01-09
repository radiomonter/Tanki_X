namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15deb09daafL)]
    public class PresetEquipmentComponent : Component
    {
        public long WeaponId { get; set; }

        public long HullId { get; set; }
    }
}

