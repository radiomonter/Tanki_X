namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.API;

    [SerialVersionUID(0x2d0b367a1212894aL)]
    public interface TwinsBulletTemplate : Template
    {
        BulletTargetingComponent barrelTargeting();
        BulletComponent bullet();
        [PersistentConfig("", false)]
        BulletConfigComponent bulletConfig();
        TargetCollectorComponent targetCollector();
        TwinsBulletComponent twinsBullet();
    }
}

