namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class TwinsBulletSystem : AbstractBulletSystem
    {
        [OnEventFire]
        public void Build(BulletBuildEvent e, WeaponNode weaponNode, [JoinByTank] TankNode tankNode)
        {
            Entity entity = base.CreateEntity<TwinsBulletTemplate>("battle/weapon/twins/bullet");
            BulletComponent bullet = new BulletComponent();
            WeaponBulletShotComponent weaponBulletShot = weaponNode.weaponBulletShot;
            bullet.Speed = weaponBulletShot.BulletSpeed;
            bullet.Radius = weaponBulletShot.BulletRadius;
            MuzzleVisualAccessor accessor = new MuzzleVisualAccessor(weaponNode.muzzlePoint);
            BulletTargetingComponent component = new BulletTargetingComponent {
                RadialRaysCount = entity.AddComponentAndGetInstance<BulletConfigComponent>().RadialRaysCount,
                Radius = bullet.Radius
            };
            Rigidbody tankRigidbody = tankNode.rigidbody.Rigidbody;
            bullet.ShotId = weaponNode.shotId.ShotId;
            entity.AddComponent(component);
            entity.AddComponent(new TargetCollectorComponent(new TargetCollector(tankNode.Entity), new TargetValidator(tankNode.Entity)));
            base.InitBullet(bullet, accessor.GetWorldPosition(), e.Direction, bullet.Radius, tankRigidbody);
            entity.AddComponent(bullet);
            entity.AddComponent(new TankGroupComponent(weaponNode.tankGroup.Key));
            entity.AddComponent<TwinsBulletComponent>();
            entity.AddComponent<ReadyBulletComponent>();
        }

        [OnEventComplete]
        public void HandleFrame(UpdateBulletEvent e, BulletNode bulletNode)
        {
            BulletComponent bullet = bulletNode.bullet;
            BulletConfigComponent bulletConfig = bulletNode.bulletConfig;
            DirectionData data = e.TargetingData.Directions[0];
            if (data.StaticHit != null)
            {
                bullet.Distance += (bullet.Position - data.StaticHit.Position).magnitude;
                base.SetPositionNearHitPoint(bullet, data.StaticHit.Position);
                base.SendBulletStaticHitEvent(bulletNode.Entity, bullet);
                base.DestroyBullet(bulletNode.Entity);
            }
            else if (!base.DestroyOnAnyTargetHit(bulletNode.Entity, bullet, bulletConfig, e.TargetingData))
            {
                base.MoveBullet(bulletNode.Entity, bullet);
                if (bullet.Distance > bulletConfig.FullDistance)
                {
                    base.DestroyBullet(bulletNode.Entity);
                }
            }
        }

        public class BulletNode : Node
        {
            public TankGroupComponent tankGroup;
            public BulletComponent bullet;
            public ReadyBulletComponent readyBullet;
            public BulletConfigComponent bulletConfig;
            public TwinsBulletComponent twinsBullet;
        }

        public class TankNode : Node
        {
            public AssembledTankComponent assembledTank;
            public RigidbodyComponent rigidbody;
        }

        public class WeaponNode : Node
        {
            public TankGroupComponent tankGroup;
            public TwinsComponent twins;
            public MuzzlePointComponent muzzlePoint;
            public WeaponBulletShotComponent weaponBulletShot;
            public ShotIdComponent shotId;
        }
    }
}

