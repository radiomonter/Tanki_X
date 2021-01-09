namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x150dc551f85L)]
    public interface BuyButtonWithPriceTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        BuyButtonConfirmWithPriceLocalizedTextComponent buttonWithPriceLocalizedText();
    }
}

