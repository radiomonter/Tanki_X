namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x448b748b93ce332bL)]
    public interface HammerBattleItemTemplate : DiscreteWeaponTemplate, WeaponTemplate, Template
    {
        HammerComponent hammer();
        [AutoAdded]
        HammerEnergyBarComponent hammerEnergyBarComponent();
        [PersistentConfig("", false)]
        HammerPelletConeComponent hammerPelletCone();
        [AutoAdded, PersistentConfig("reticle", false)]
        ReticleTemplateComponent reticleTemplate();
        WeaponShotComponent shot();
        [PersistentConfig("", false)]
        VerticalSectorsTargetingComponent verticalSectorsTargeting();
    }
}

