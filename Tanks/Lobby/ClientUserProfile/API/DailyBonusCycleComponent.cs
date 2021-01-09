namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class DailyBonusCycleComponent : Component
    {
        public DailyBonusData[] DailyBonuses { get; set; }

        public int[] Zones { get; set; }
    }
}

