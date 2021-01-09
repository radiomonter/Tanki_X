namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d52691576d6ed3L)]
    public interface DetailItemTemplate : GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        DetailItemComponent detailItem();
    }
}

