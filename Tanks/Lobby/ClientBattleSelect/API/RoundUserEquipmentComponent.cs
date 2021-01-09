namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class RoundUserEquipmentComponent : Component
    {
        public long WeaponId { get; set; }

        public long HullId { get; set; }
    }
}

