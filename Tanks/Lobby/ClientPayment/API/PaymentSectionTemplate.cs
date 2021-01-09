namespace Tanks.Lobby.ClientPayment.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientPaymentGUI.Impl;

    [SerialVersionUID(0x152cb04dda6L)]
    public interface PaymentSectionTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ExchangeRateComponent exchangeRate();
        [AutoAdded, PersistentConfig("", false)]
        PacksImagesComponent packsImages();
    }
}

