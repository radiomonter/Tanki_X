namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;

    [TemplatePart]
    public interface SmokyHitFeedbackSoundsTemplatePart : SmokyBattleItemTemplate, DiscreteWeaponEnergyTemplate, DiscreteWeaponTemplate, WeaponTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        HitFeedbackSoundsPlayingSettingsComponent hitFeedbackSoundsPlayingSettings();
    }
}

