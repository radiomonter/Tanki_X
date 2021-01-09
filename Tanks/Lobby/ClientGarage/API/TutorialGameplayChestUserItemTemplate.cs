namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4fe89dd5fbf5fL)]
    public interface TutorialGameplayChestUserItemTemplate : GameplayChestUserItemTemplate, ContainerItemTemplate, UserItemTemplate, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate, Template
    {
        [AutoAdded]
        HiddenInGarageItemComponent hiddenInGarageItem();
    }
}

