namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x15a464d0d27L)]
    public interface TargetFocusEffectTemplate : EffectBaseTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        TargetFocusConicTargetingComponent targetFocusConicTargeting();
        [AutoAdded]
        TargetFocusEffectComponent targetFocusEffect();
        [AutoAdded, PersistentConfig("", false)]
        TargetFocusPelletConeComponent targetFocusPelletCone();
        [AutoAdded, PersistentConfig("", false)]
        TargetFocusVerticalSectorTargetingComponent targetFocusVerticalSectorTargeting();
        [AutoAdded, PersistentConfig("", false)]
        TargetFocusVerticalTargetingComponent targetFocusVerticalTargeting();
    }
}

