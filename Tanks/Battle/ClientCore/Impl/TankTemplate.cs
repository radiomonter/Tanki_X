namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.API;

    [SerialVersionUID(0x1bedcc8e3c80d9caL)]
    public interface TankTemplate : Template
    {
        AssembledTankComponent assembledTank();
        [PersistentConfig("", false)]
        ChassisConfigComponent chassisConfig();
        [PersistentConfig("", false)]
        HealthConfigComponent healthConfig();
        [PersistentConfig("", false)]
        TankCommonPrefabComponent tankCommonPrefab();
        TankPartComponent tankPart();
    }
}

