namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ShowBattleEvent : Event
    {
        public ShowBattleEvent()
        {
        }

        public ShowBattleEvent(long battleId)
        {
            this.BattleId = battleId;
        }

        public long BattleId { get; set; }
    }
}

