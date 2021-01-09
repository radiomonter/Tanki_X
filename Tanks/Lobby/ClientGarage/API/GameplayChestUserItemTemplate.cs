namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x15a1e07899fL)]
    public interface GameplayChestUserItemTemplate : ContainerItemTemplate, UserItemTemplate, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate, Template
    {
        [AutoAdded]
        GameplayChestItemComponent gameplayChestItem();
        [AutoAdded, PersistentConfig("", false)]
        TargetTierComponent targetTier();
    }
}

