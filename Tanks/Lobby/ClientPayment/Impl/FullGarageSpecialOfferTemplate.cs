namespace Tanks.Lobby.ClientPayment.Impl
{
    using Lobby.ClientPayment.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d547be325ff912L)]
    public interface FullGarageSpecialOfferTemplate : BaseStarterPackSpecialOfferTemplate, SpecialOfferBaseTemplate, GoodsTemplate, Template
    {
    }
}

