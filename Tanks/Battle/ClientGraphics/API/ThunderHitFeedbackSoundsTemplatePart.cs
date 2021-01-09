namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Tanks.Battle.ClientCore;
    using Tanks.Battle.ClientCore.API;

    [TemplatePart]
    public interface ThunderHitFeedbackSoundsTemplatePart : ThunderBattleItemTemplate, DiscreteWeaponEnergyTemplate, DiscreteWeaponTemplate, WeaponTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        HitFeedbackSoundsPlayingSettingsComponent hitFeedbackSoundsPlayingSettings();
    }
}

