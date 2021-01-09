namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x1608d80757eL)]
    public class LevelUpUnlockPersonalRewardComponent : Component
    {
        public List<Entity> Unlocked { get; set; }
    }
}

