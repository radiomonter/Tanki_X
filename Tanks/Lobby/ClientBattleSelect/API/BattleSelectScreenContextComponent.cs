namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class BattleSelectScreenContextComponent : Component
    {
        public BattleSelectScreenContextComponent()
        {
        }

        public BattleSelectScreenContextComponent(long? battleId)
        {
            this.BattleId = battleId;
        }

        public long? BattleId { get; set; }
    }
}

