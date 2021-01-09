namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4c8638251d1d8L)]
    public interface ModuleUpgradeLifestealEffectInfoTemplate : ModuleUpgradeInfoTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ModuleLifestealEffectAdditiveHPFactorPropertyComponent moduleLifestealEffectAdditiveHPFactorProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleLifestealEffectFixedHPPropertyComponent moduleLifestealEffectFixedHPProperty();
    }
}

