namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientBattleSelect.Impl;
    using Tanks.Lobby.ClientNavigation.API;

    [SerialVersionUID(0x8d2a7177d0399adL)]
    public interface BattleSelectScreenTemplate : ScreenTemplate, Template
    {
        BattleSelectLoadedComponent battleSelectLoaded();
        BattleSelectScreenComponent battleSelectScreen();
        [AutoAdded, PersistentConfig("", false)]
        BattleSelectScreenHeaderTextComponent battleSelectScreenHeaderText();
        [PersistentConfig("", false)]
        BattleSelectScreenLocalizationComponent battleSelectScreenLocalization();
        [AutoAdded, PersistentConfig("", false)]
        InviteFriendsConfigComponent inviteFriendsConfig();
        [PersistentConfig("", false)]
        InviteFriendsPanelLocalizationComponent inviteFriendsPanelLocalization();
        [AutoAdded, PersistentConfig("", false)]
        ScoreTableEmptyRowTextComponent scoreTableEmptyRowText();
        [AutoAdded]
        VisibleItemsRangeComponent visibleItemsRange();
    }
}

