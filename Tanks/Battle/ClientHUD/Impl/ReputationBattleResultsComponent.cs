namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ReputationBattleResultsComponent : Component
    {
        public int Delta { get; set; }

        public bool UnfairMatching { get; set; }
    }
}

