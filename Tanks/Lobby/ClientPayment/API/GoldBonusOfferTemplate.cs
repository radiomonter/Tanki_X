namespace Tanks.Lobby.ClientPayment.API
{
    using Lobby.ClientPayment.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientPayment.Impl;

    [SerialVersionUID(-3834761627489275062L)]
    public interface GoldBonusOfferTemplate : SpecialOfferBaseTemplate, GoodsTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        GoldBonusOfferComponent goldBonusOffer();
    }
}

