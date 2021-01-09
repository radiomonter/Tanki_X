namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientBattleSelect.Impl;
    using Tanks.Lobby.ClientGarage.API;

    [SerialVersionUID(0x16053dda20fL)]
    public interface BattleResultRewardTemplate : Template
    {
        BattleRewardGroupComponent battleRewardGroup();
        [AutoAdded, PersistentConfig("", false)]
        DescriptionItemComponent descriptionItem();
    }
}

