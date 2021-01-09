namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.API;

    [SerialVersionUID(-2434344547754767853L)]
    public interface SmokyBattleItemTemplate : DiscreteWeaponEnergyTemplate, DiscreteWeaponTemplate, WeaponTemplate, Template
    {
        [AutoAdded]
        EnergyBarComponent energyBar();
        [AutoAdded, PersistentConfig("reticle", false)]
        ReticleTemplateComponent reticleTemplate();
        SmokyComponent smoky();
        [PersistentConfig("", false)]
        VerticalSectorsTargetingComponent verticalSectorsTargeting();
    }
}

