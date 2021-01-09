namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientBattleSelect.Impl;

    [SerialVersionUID(0x16069bfa1c5L)]
    public interface PersonalBattleRewardTemplate : Template
    {
        BattleRewardGroupComponent battleRewardGroup();
        [AutoAdded]
        PersonalBattleRewardComponent personalBattleReward();
    }
}

