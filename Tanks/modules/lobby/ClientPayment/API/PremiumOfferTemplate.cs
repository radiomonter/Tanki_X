namespace tanks.modules.lobby.ClientPayment.API
{
    using Lobby.ClientPayment.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientPayment.API;
    using Tanks.Lobby.ClientPayment.Impl;

    [SerialVersionUID(0x1606e1945f9L)]
    public interface PremiumOfferTemplate : SpecialOfferBaseTemplate, GoodsTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        PremiumOfferComponent premiumOffer();
    }
}

