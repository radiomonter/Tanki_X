namespace Tanks.Battle.ClientHUD.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientEntrance.API;

    [SerialVersionUID(0x8d3f110cfe1d270L)]
    public interface UserNotificatorRankNamesTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        RanksNamesComponent ranksNames();
        [AutoAdded]
        UserNotificatorRankNamesComponent userNotificatorRankNames();
    }
}

