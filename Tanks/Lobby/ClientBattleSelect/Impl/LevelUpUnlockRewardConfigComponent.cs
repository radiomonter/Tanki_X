namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d54bb607955f28L)]
    public class LevelUpUnlockRewardConfigComponent : Component
    {
        public string ActiveSlotSpriteUid { get; set; }

        public string PassiveSlotSpriteUid { get; set; }

        public string ActiveSlotWeaponText { get; set; }

        public string ActiveSlotHullText { get; set; }

        public string PassiveSlotWeaponText { get; set; }

        public string PassiveSlotHullText { get; set; }
    }
}

