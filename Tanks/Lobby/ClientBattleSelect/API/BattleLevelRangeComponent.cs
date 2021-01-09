namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;

    [Shared, SerialVersionUID(0x1546b03a9e9L)]
    public class BattleLevelRangeComponent : Component
    {
        public IndexRange Range { get; set; }
    }
}

