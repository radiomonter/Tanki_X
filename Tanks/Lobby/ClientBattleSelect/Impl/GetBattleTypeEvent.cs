namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class GetBattleTypeEvent : Event
    {
        public bool WithCashback { get; set; }

        public BattleResultsAwardsScreenComponent.BattleTypes BattleType { get; set; }
    }
}

