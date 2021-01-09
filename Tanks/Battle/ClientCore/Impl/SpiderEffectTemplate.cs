namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x159d50469cfL)]
    public interface SpiderEffectTemplate : EffectBaseTemplate, Template
    {
        [AutoAdded]
        DirectionEvaluatorComponent directionEvaluator();
        [AutoAdded]
        EffectInstanceRemovableComponent effectInstanceRemovable();
        [AutoAdded, PersistentConfig("", false)]
        PreloadingMineKeyComponent preloadingMineKey();
        [AutoAdded]
        SpiderMineEffectComponent spiderMineEffect();
    }
}

