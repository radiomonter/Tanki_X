namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using System;
    using System.Linq;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class RicochetBulletBounceGraphicsSystem : ECSSystem
    {
        [OnEventFire]
        public void SpawnExplosionOnBounce(UpdateBulletEvent e, BulletNode bulletNode, [JoinByTank] WeaponNode weaponNode)
        {
            BulletEffectComponent bulletEffect = weaponNode.bulletEffect;
            DirectionData data = e.TargetingData.Directions.First<DirectionData>();
            if (data.StaticHit != null)
            {
                GetInstanceFromPoolEvent eventInstance = new GetInstanceFromPoolEvent {
                    Prefab = bulletEffect.ExplosionPrefab,
                    AutoRecycleTime = bulletEffect.ExplosionTime
                };
                base.ScheduleEvent(eventInstance, new EntityStub());
                Transform instance = eventInstance.Instance;
                instance.position = data.StaticHit.Position + (data.StaticHit.Normal * bulletEffect.ExplosionOffset);
                instance.rotation = Quaternion.LookRotation(data.StaticHit.Normal);
                instance.gameObject.SetActive(true);
            }
        }

        public class BulletNode : Node
        {
            public TankGroupComponent tankGroup;
            public RicochetBulletComponent ricochetBullet;
        }

        public class WeaponNode : Node
        {
            public TankGroupComponent tankGroup;
            public RicochetComponent ricochet;
            public BulletEffectComponent bulletEffect;
        }
    }
}

