namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientBattleSelect.Impl;

    [SerialVersionUID(0x16054779afeL)]
    public interface XCrystalBattleRewardTemplate : BattleResultRewardTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        XCrystalRewardItemsConfigComponent itemsConfig();
        [AutoAdded, PersistentConfig("", false)]
        XCrystalRewardTextConfigComponent xCrystalRewardTextConfig();
    }
}

