namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x15a26c44ffcL)]
    public interface MineEffectTemplate : EffectBaseTemplate, Template
    {
        [AutoAdded]
        EffectInstanceRemovableComponent effectInstanceRemovable();
        [AutoAdded]
        MineEffectComponent mineEffect();
        [AutoAdded, PersistentConfig("", false)]
        PreloadingMineKeyComponent preloadingMineKey();
    }
}

