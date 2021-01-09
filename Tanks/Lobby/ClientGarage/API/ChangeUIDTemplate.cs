namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientPayment.API;

    [SerialVersionUID(0x1574709bf34L)]
    public interface ChangeUIDTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        BuyButtonConfirmWithPriceLocalizedTextComponent buttonWithPriceLocalizedText();
        [AutoAdded]
        ChangeUIDComponent changeUid();
        [AutoAdded, PersistentConfig("", false)]
        GoodsXPriceComponent goodsXPrice();
    }
}

