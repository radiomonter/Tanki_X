namespace Tanks.Battle.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class BattleResultsComponent : Component
    {
        public BattleResultForClient ResultForClient { get; set; }
    }
}

