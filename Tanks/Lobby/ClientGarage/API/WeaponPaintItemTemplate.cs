namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d48bcb5fecaff8L)]
    public interface WeaponPaintItemTemplate : PaintItemTemplate, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate, Template
    {
        [AutoAdded]
        WeaponPaintItemComponent weaponPaintItem();
    }
}

