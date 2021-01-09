namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class MapModeRestrictionComponent : Component
    {
        public List<BattleMode> AvailableModes { get; set; }
    }
}

