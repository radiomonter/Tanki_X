namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x159d4e740b5L)]
    public interface DroneEffectTemplate : EffectBaseTemplate, Template
    {
        [AutoAdded]
        DirectionEvaluatorComponent directionEvaluator();
        [AutoAdded]
        EffectInstanceRemovableComponent effectInstanceRemovable();
        [AutoAdded]
        UnitTargetingComponent unitTargeting();
    }
}

