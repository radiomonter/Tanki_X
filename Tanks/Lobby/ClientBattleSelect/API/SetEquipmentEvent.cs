namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15c868a2b58L)]
    public class SetEquipmentEvent : Event
    {
        public long WeaponId { get; set; }

        public long HullId { get; set; }
    }
}

