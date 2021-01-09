namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4c84fe116bd8bL)]
    public interface ModuleUpgradeAcceleratedGearsEffectInfoTemplate : ModuleUpgradeInfoTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ModuleAcceleratedGearsEffectHullRotationSpeedPropertyComponent moduleAcceleratedGearsEffectHullRotationSpeedProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleAcceleratedGearsEffectTurretAccelerationPropertyComponent moduleAcceleratedGearsEffectTurretAccelerationProperty();
        [AutoAdded, PersistentConfig("", false)]
        ModuleAcceleratedGearsEffectTurretSpeedPropertyComponent moduleAcceleratedGearsEffectTurretSpeedProperty();
    }
}

