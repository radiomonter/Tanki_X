namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.API;

    [SerialVersionUID(0x221aa1186c6c880eL)]
    public interface RicochetBulletTemplate : Template
    {
        BulletTargetingComponent barrelTargeting();
        BulletComponent bullet();
        [PersistentConfig("", false)]
        BulletConfigComponent bulletConfig();
        RicochetBulletComponent ricochetBullet();
        TargetCollectorComponent targetCollector();
    }
}

