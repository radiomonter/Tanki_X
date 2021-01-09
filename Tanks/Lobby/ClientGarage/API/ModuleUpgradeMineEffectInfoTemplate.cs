namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.Impl;

    [SerialVersionUID(0x8d4d9a51b42d4a4L)]
    public interface ModuleUpgradeMineEffectInfoTemplate : ModuleUpgradeCommonMineEffectInfoTemplate, ModuleUpgradeInfoTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ModuleMineEffectExplosionDelayMSPropertyComponent moduleMineEffectExplosionDelayMSProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleMineEffectTriggeringAreaPropertyComponent moduleMineEffectTriggeringAreaProperty();
    }
}

