namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x159bb45b957L)]
    public interface ModuleMarketItemTemplate : ModuleItemTemplate, MarketItemTemplate, GarageItemTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ModuleUpgradePropertiesInfoComponent moduleUpgradePropertiesInfo();
    }
}

