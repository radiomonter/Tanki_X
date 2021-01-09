namespace Tanks.Lobby.ClientPayment.API
{
    using Lobby.ClientPayment.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientPayment.Impl;

    [SerialVersionUID(0x16069a1fd4eL)]
    public interface LeagueFirstEntranceSpecialOfferTemplate : SpecialOfferBaseTemplate, GoodsTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        LeagueFirstEntranceSpecialOfferComponent leagueFirstEntranceSpecialOffer();
    }
}

