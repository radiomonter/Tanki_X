namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x159cabc24caL)]
    public interface ModuleItemTemplate : GarageItemTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ItemBigIconComponent itemBigIcon();
        [AutoAdded, PersistentConfig("", false)]
        ItemIconComponent itemIcon();
        [AutoAdded, PersistentConfig("", true)]
        ModuleEffectsComponent moduleEffects();
        [AutoAdded]
        ModuleItemComponent moduleItem();
        [AutoAdded, PersistentConfig("", false)]
        OrderItemComponent orderItem();
    }
}

