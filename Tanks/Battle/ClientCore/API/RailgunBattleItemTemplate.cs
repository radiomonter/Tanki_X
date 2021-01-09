namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.Impl;

    [SerialVersionUID(-6419489500262573655L)]
    public interface RailgunBattleItemTemplate : DiscreteWeaponEnergyTemplate, DiscreteWeaponTemplate, WeaponTemplate, Template
    {
        RailgunChargingWeaponComponent chargingWeapon();
        RailgunComponent railgun();
        [AutoAdded]
        RailgunEnergyBarComponent railgunEnergyBar();
        ReadyRailgunChargingWeaponComponent readyRailgunChargingWeapon();
        [AutoAdded, PersistentConfig("reticle", false)]
        ReticleTemplateComponent reticleTemplate();
        [PersistentConfig("", false)]
        VerticalSectorsTargetingComponent verticalSectorsTargeting();
    }
}

