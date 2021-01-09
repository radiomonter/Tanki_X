namespace Tanks.Lobby.ClientPayment.Impl
{
    using Lobby.ClientPayment.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d41ddb13be1d90L)]
    public interface StarterPackSpecialOfferTemplate : BaseStarterPackSpecialOfferTemplate, SpecialOfferBaseTemplate, GoodsTemplate, Template
    {
        [PersistentConfig("", false), AutoAdded]
        StarterPackMainElementComponent starterPackMainElement();
        [AutoAdded, PersistentConfig("", false)]
        StarterPackVisualConfigComponent starterPackVisualConfig();
    }
}

