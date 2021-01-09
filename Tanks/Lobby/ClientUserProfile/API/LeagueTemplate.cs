namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x15de0a51a2eL)]
    public interface LeagueTemplate : Template
    {
        [AutoAdded]
        LeagueComponent league();
        [AutoAdded, PersistentConfig("", false)]
        LeagueEnergyConfigComponent leagueEnergyConfig();
        [AutoAdded, PersistentConfig("", false)]
        LeagueEnterNotificationTextsComponent leagueEnterNotificationTexts();
        [AutoAdded, PersistentConfig("", false)]
        LeagueIconComponent leagueIcon();
        [AutoAdded, PersistentConfig("", false)]
        LeagueNameComponent leagueName();
        TopLeagueComponent topLeague();
    }
}

