namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientBattleSelect.Impl;

    [SerialVersionUID(0x1606d88608cL)]
    public interface XCrystalBonusPersonalBattleRewardTemplate : PersonalBattleRewardTemplate, Template
    {
        XCrystalBonusPersonalRewardComponent xCrystalBonusPersonalReward();
    }
}

