namespace Lobby.ClientPayment.API
{
    using Lobby.ClientPayment.Impl;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x152fe7fb511L)]
    public interface GoodsTemplate : Template
    {
        [AutoAdded]
        GoodsComponent Goods();
        GoodsPriceComponent GoodsPrice();
    }
}

