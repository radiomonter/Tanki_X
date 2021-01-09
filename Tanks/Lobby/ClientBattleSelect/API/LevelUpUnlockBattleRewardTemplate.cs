namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientBattleSelect.Impl;

    [SerialVersionUID(0x1608d21b50eL)]
    public interface LevelUpUnlockBattleRewardTemplate : BattleResultRewardTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        LevelUpUnlockRewardConfigComponent levelUpUnlock();
    }
}

