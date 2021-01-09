namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8191c52152253c3L)]
    public interface TwinsBattleItemTemplate : DiscreteWeaponEnergyTemplate, DiscreteWeaponTemplate, WeaponTemplate, Template
    {
        [AutoAdded]
        EnergyBarComponent energyBar();
        [AutoAdded, PersistentConfig("reticle", false)]
        ReticleTemplateComponent reticleTemplate();
        TwinsComponent twins();
        [PersistentConfig("", false)]
        VerticalTargetingComponent verticalTargeting();
    }
}

