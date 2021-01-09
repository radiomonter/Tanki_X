namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.API;

    [TemplatePart, SerialVersionUID(0x8d47b383aebfab1L)]
    public interface DiscreteWeaponCameraShakerTemplatePart : DiscreteWeaponTemplate, WeaponTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ImpactCameraShakerConfigComponent impactCameraShakerConfig();
        [AutoAdded, PersistentConfig("", false)]
        KickbackCameraShakerConfigComponent kickbackCameraShakerConfig();
    }
}

