namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.Impl;

    [SerialVersionUID(0x14d03a9ccdbL)]
    public interface StreamWeaponTemplate : WeaponTemplate, Template
    {
        [PersistentConfig("", false)]
        ConicTargetingComponent conicTargeting();
        [AutoAdded]
        EnergyBarComponent energyBar();
        StreamWeaponControllerComponent streamWeaponController();
        StreamWeaponEnergyComponent streamWeaponEnergy();
    }
}

