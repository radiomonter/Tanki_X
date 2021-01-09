namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.API;

    [TemplatePart, SerialVersionUID(0x8d47b3ddbc067e0L)]
    public interface VulcanCameraShakerTemplatePart : VulcanBattleItemTemplate, WeaponTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ImpactCameraShakerConfigComponent impactCameraShakerConfig();
        [AutoAdded, PersistentConfig("", false)]
        KickbackCameraShakerConfigComponent kickbackCameraShakerConfig();
    }
}

