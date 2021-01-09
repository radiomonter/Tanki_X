namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.Impl;

    [SerialVersionUID(0x8d2fbc28573bde6L)]
    public interface GarageItemsTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        GarageItemsScreenTextComponent garageItemsScreenText();
    }
}

