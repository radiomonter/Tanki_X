namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class BulletSystem : ECSSystem
    {
        [OnEventFire]
        public void DestroyBulletsOnRemoveWeapon(NodeRemoveEvent e, WeaponNode weapon, [JoinByTank, Combine] BulletNode bullet)
        {
            bullet.Entity.RemoveComponent<BulletComponent>();
            base.DeleteEntity(bullet.Entity);
        }

        protected void PrepareTargetData(TargetData targetData, BulletComponent bulletComponent)
        {
            targetData.HitDistance += bulletComponent.Distance;
            targetData.HitDirection = bulletComponent.Direction;
        }

        [OnEventFire]
        public void PrepareTargets(UpdateEvent e, BulletNode bulletNode, [JoinByTank] WeaponNode weaponNode)
        {
            BulletComponent bullet = bulletNode.bullet;
            float num = UnityTime.time - bullet.LastUpdateTime;
            TargetingData targetingData = BattleCache.targetingData.GetInstance().Init();
            targetingData.Origin = bullet.Position - (bullet.Direction * 0.1f);
            targetingData.Dir = bullet.Direction;
            targetingData.FullDistance = Math.Min(Math.Max((float) 0f, (float) (bulletNode.bulletConfig.FullDistance - bullet.Distance)), bullet.Speed * num);
            base.ScheduleEvent(BattleCache.targetingEvent.GetInstance().Init(targetingData), bulletNode);
            base.ScheduleEvent(BattleCache.updateBulletEvent.GetInstance().Init(targetingData), bulletNode);
        }

        [OnEventFire]
        public void PrepareTargetsAtFirstFrame(NodeAddedEvent e, BulletNode bulletNode, [JoinByTank] WeaponNode weaponNode)
        {
            MuzzleLogicAccessor accessor = new MuzzleLogicAccessor(weaponNode.muzzlePoint, weaponNode.weaponInstance);
            Vector3 barrelOriginWorld = accessor.GetBarrelOriginWorld();
            TargetingData targetingData = BattleCache.targetingData.GetInstance().Init();
            targetingData.Origin = barrelOriginWorld;
            targetingData.Dir = bulletNode.bullet.Direction;
            targetingData.FullDistance = (accessor.GetWorldPosition() - barrelOriginWorld).magnitude * 1.2f;
            base.ScheduleEvent(BattleCache.targetingEvent.GetInstance().Init(targetingData), bulletNode);
            base.ScheduleEvent(BattleCache.updateBulletEvent.GetInstance().Init(targetingData), bulletNode);
        }

        [OnEventComplete]
        public void SendHitEvent(TargetingEvent e, SingleNode<BulletComponent> bulletNode, [JoinByTank] UnblockedWeaponNode weaponNode, [JoinByTank] SingleNode<TankSyncComponent> tankNode)
        {
            if (!e.TargetingData.HasBaseStaticHit())
            {
                foreach (DirectionData data in e.TargetingData.Directions)
                {
                    if (data.HasTargetHit())
                    {
                        TargetData targetData = data.Targets.First<TargetData>();
                        if (targetData.TargetEntity.HasComponent<TankActiveStateComponent>())
                        {
                            this.PrepareTargetData(targetData, bulletNode.component);
                            SelfHitEvent event3 = new SelfHitEvent();
                            List<HitTarget> list = new List<HitTarget> {
                                HitTargetAdapter.Adapt(targetData)
                            };
                            event3.Targets = list;
                            event3.ShotId = bulletNode.component.ShotId;
                            SelfHitEvent eventInstance = event3;
                            base.ScheduleEvent(eventInstance, weaponNode.Entity);
                            break;
                        }
                    }
                }
            }
        }

        [Inject]
        public static BattleFlowInstancesCache BattleCache { get; set; }

        [Inject]
        public static Platform.Library.ClientUnityIntegration.API.UnityTime UnityTime { get; set; }

        public class BulletNode : Node
        {
            public TankGroupComponent tankGroup;
            public BulletComponent bullet;
            public ReadyBulletComponent readyBullet;
            public BulletConfigComponent bulletConfig;
        }

        public class UnblockedWeaponNode : Node
        {
            public TankGroupComponent tankGroup;
            public MuzzlePointComponent muzzlePoint;
        }

        public class WeaponNode : Node
        {
            public WeaponComponent weapon;
            public TankGroupComponent tankGroup;
            public MuzzlePointComponent muzzlePoint;
            public WeaponInstanceComponent weaponInstance;
        }
    }
}

