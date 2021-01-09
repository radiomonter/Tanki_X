namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class RicochetBulletSystem : AbstractBulletSystem
    {
        [OnEventFire]
        public void Build(BulletBuildEvent e, WeaponNode weaponNode, [JoinByTank] TankNode tankNode)
        {
            Entity entity = base.CreateEntity<RicochetBulletTemplate>("battle/weapon/ricochet/bullet");
            BulletComponent bullet = new BulletComponent();
            WeaponBulletShotComponent weaponBulletShot = weaponNode.weaponBulletShot;
            float bulletRadius = weaponBulletShot.BulletRadius;
            bullet.Radius = bulletRadius;
            bullet.Speed = weaponBulletShot.BulletSpeed;
            MuzzleVisualAccessor accessor = new MuzzleVisualAccessor(weaponNode.muzzlePoint);
            BulletTargetingComponent component4 = entity.AddComponentAndGetInstance<BulletTargetingComponent>();
            component4.RadialRaysCount = entity.AddComponentAndGetInstance<BulletConfigComponent>().RadialRaysCount;
            component4.Radius = bulletRadius;
            Rigidbody tankRigidbody = tankNode.rigidbody.Rigidbody;
            bullet.ShotId = weaponNode.shotId.ShotId;
            base.InitBullet(bullet, accessor.GetWorldPosition(), e.Direction, bullet.Radius, tankRigidbody);
            entity.AddComponent(bullet);
            entity.AddComponent(new TankGroupComponent(weaponNode.tankGroup.Key));
            entity.AddComponent<RicochetBulletComponent>();
            entity.AddComponent(new TargetCollectorComponent(new TargetCollector(tankNode.Entity), new TargetValidator(tankNode.Entity)));
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
                Vector3 position = data.StaticHit.Position;
                base.ScheduleEvent(new RicochetBulletBounceEvent(position), bulletNode);
                bullet.Distance += (bullet.Position - data.StaticHit.Position).magnitude;
                this.ProcessRicochet(bullet, data.StaticHit);
            }
            else
            {
                if (base.DestroyOnAnyTargetHit(bulletNode.Entity, bullet, bulletConfig, e.TargetingData))
                {
                    return;
                }
                base.MoveBullet(bulletNode.Entity, bullet);
            }
            if (bullet.Distance > bulletConfig.FullDistance)
            {
                base.DestroyBullet(bulletNode.Entity);
            }
        }

        private void ProcessRicochet(BulletComponent bullet, StaticHit staticHit)
        {
            bullet.Position = staticHit.Position - (bullet.Direction * bullet.Radius);
            Vector3 direction = bullet.Direction;
            bullet.Direction = (direction - ((2f * Vector3.Dot(direction, staticHit.Normal)) * staticHit.Normal)).normalized;
        }

        public class BulletNode : Node
        {
            public TankGroupComponent tankGroup;
            public BulletComponent bullet;
            public ReadyBulletComponent readyBullet;
            public BulletConfigComponent bulletConfig;
            public RicochetBulletComponent ricochetBullet;
            public TargetCollectorComponent targetCollector;
        }

        public class TankNode : Node
        {
            public AssembledTankComponent assembledTank;
            public RigidbodyComponent rigidbody;
        }

        public class WeaponNode : Node
        {
            public TankGroupComponent tankGroup;
            public RicochetComponent ricochet;
            public MuzzlePointComponent muzzlePoint;
            public WeaponBulletShotComponent weaponBulletShot;
            public ShotIdComponent shotId;
        }
    }
}

