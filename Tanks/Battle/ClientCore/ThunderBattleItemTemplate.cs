namespace Tanks.Battle.ClientCore
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.API;

    [SerialVersionUID(-8770103861152493981L)]
    public interface ThunderBattleItemTemplate : DiscreteWeaponEnergyTemplate, DiscreteWeaponTemplate, WeaponTemplate, Template
    {
        [AutoAdded]
        EnergyBarComponent energyBar();
        [AutoAdded, PersistentConfig("reticle", false)]
        ReticleTemplateComponent reticleTemplate();
        [PersistentConfig("", false)]
        SplashWeaponComponent splashWeapon();
        ThunderComponent thunder();
        [PersistentConfig("", false)]
        VerticalSectorsTargetingComponent verticalSectorsTargeting();
    }
}

