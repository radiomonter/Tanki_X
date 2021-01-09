namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4c908083c6406L)]
    public interface ModuleUpgradeBackhitModificatorEffectInfoTemplate : ModuleUpgradeInfoTemplate, Template
    {
        [AutoAdded]
        HiddenInGarageItemComponent hiddenInGarageItem();
        [AutoAdded, PersistentConfig("", false)]
        ModuleBackhitModificatorEffectPropertyComponent moduleBackhitModificatorEffectProperty();
    }
}

