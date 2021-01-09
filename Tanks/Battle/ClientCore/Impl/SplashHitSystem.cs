namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class SplashHitSystem : ECSSystem
    {
        private const float FORWARD_SPLASH_OFFSET = 0.25f;
        private const float STATIC_HIT_SPLASH_CENTER_OFFSET = 0.01f;

        [OnEventFire]
        public void CalculateSplashCenterByStatic(CalculateSplashCenterByStaticHitEvent evt, SplashWeaponNode weapon)
        {
            SplashHitData splashHit = evt.SplashHit;
            StaticHit staticHit = splashHit.StaticHit;
            splashHit.SplashCenter = staticHit.Position + (staticHit.Normal * 0.01f);
        }

        [OnEventFire]
        public void CalculateSplashCenterByTarget(CalculateSplashCenterByDirectTargetEvent evt, TankNode tank)
        {
            SplashHitData splashHit = evt.SplashHit;
            splashHit.SplashCenter = MathUtil.LocalPositionToWorldPosition(evt.DirectTargetLocalHitPoint, tank.rigidbody.Rigidbody.gameObject);
            HashSet<Entity> set = new HashSet<Entity> {
                tank.Entity
            };
            splashHit.ExcludedEntityForSplashHit = set;
            splashHit.ExclusionGameObjectForSplashRaycast.AddRange(tank.tankColliders.TargetingColliders);
        }

        [OnEventFire]
        public void CalculateSplashTargetsList(CalculateSplashTargetsWithCenterEvent evt, SplashWeaponNode weapon, [JoinByBattle] ICollection<ActiveTankNode> activeTanks)
        {
            SplashHitData splashHit = evt.SplashHit;
            HashSet<Entity> excludedEntityForSplashHit = splashHit.ExcludedEntityForSplashHit;
            foreach (ActiveTankNode node in activeTanks)
            {
                if ((excludedEntityForSplashHit == null) || !excludedEntityForSplashHit.Contains(node.Entity))
                {
                    ValidateSplashHitPointsEvent eventInstance = new ValidateSplashHitPointsEvent(splashHit, splashHit.ExclusionGameObjectForSplashRaycast);
                    base.NewEvent(eventInstance).Attach(weapon).Attach(node).Schedule();
                }
            }
        }

        [OnEventComplete]
        public void FinalizeCalculationSplashCenter(CalculateSplashCenterEvent evt, Node node)
        {
            SplashHitData splashHit = evt.SplashHit;
            if (splashHit.SplashCenterInitialized)
            {
                base.ScheduleEvent(new CalculateSplashTargetsWithCenterEvent(splashHit), splashHit.WeaponHitEntity);
            }
        }

        private bool IsPointOccluded(ActiveTankNode activeTank, Vector3 splashCenter, Vector3 tankPosition)
        {
            RaycastHit hit;
            Vector3 vector = tankPosition - splashCenter;
            Vector3 normalized = vector.normalized;
            if (!Physics.Raycast(splashCenter, normalized, out hit, vector.magnitude, LayerMasks.GUN_TARGETING_WITHOUT_DEAD_UNITS))
            {
                return false;
            }
            TargetBehaviour componentInParent = hit.transform.gameObject.GetComponentInParent<TargetBehaviour>();
            return (this.IsValidTarget(componentInParent) ? !ReferenceEquals(componentInParent.TargetEntity, activeTank.Entity) : true);
        }

        private bool IsValidSplashPoint(ActiveTankNode activeTank, Vector3 splashPositionForValidation, Vector3 splashCenter, ValidateSplashHitPointsEvent e, float radius)
        {
            if (!PhysicsUtil.ValidateVector3(splashCenter))
            {
                return false;
            }
            if (!PhysicsUtil.ValidateVector3(splashPositionForValidation))
            {
                return false;
            }
            using (new RaycastExclude(e.excludeObjects))
            {
                return (((splashPositionForValidation - splashCenter).magnitude <= radius) ? !this.IsPointOccluded(activeTank, splashCenter, splashPositionForValidation) : false);
            }
        }

        private bool IsValidTarget(TargetBehaviour targetBehaviour) => 
            (targetBehaviour != null) && targetBehaviour.TargetEntity.HasComponent<TankActiveStateComponent>();

        [OnEventFire]
        public void PrepareHit(CollectSplashTargetsEvent evt, SplashWeaponNode weapon)
        {
            SplashHitData splashHit = evt.SplashHit;
            List<HitTarget> directTargets = splashHit.DirectTargets;
            if (directTargets.Count <= 0)
            {
                base.ScheduleEvent(new CalculateSplashCenterByStaticHitEvent(splashHit), weapon.Entity);
            }
            else
            {
                HitTarget target = directTargets.First<HitTarget>();
                Entity entity = target.Entity;
                base.ScheduleEvent(new CalculateSplashCenterByDirectTargetEvent(splashHit, target.LocalHitPoint), entity);
            }
        }

        [OnEventFire]
        public void PrepareHitByUnblockedWeapon(SendHitToServerIfNeedEvent evt, UnblockedWeaponNode weapon)
        {
            DirectionData bestDirection = evt.TargetingData.BestDirection;
            if (bestDirection.HasAnyHit())
            {
                SplashHitData splashHit = SplashHitData.CreateSplashHitData(HitTargetAdapter.Adapt(bestDirection.Targets), bestDirection.StaticHit, weapon.Entity);
                base.ScheduleEvent(new CollectSplashTargetsEvent(splashHit), weapon);
            }
        }

        [OnEventComplete]
        public void PrepareSplashTargetsWhenBlockedWeapon(ShotPrepareEvent evt, BlockedWeaponNode weapon)
        {
            WeaponBlockedComponent weaponBlocked = weapon.weaponBlocked;
            StaticHit staticHit = new StaticHit {
                Position = weaponBlocked.BlockPoint,
                Normal = weaponBlocked.BlockNormal
            };
            SplashHitData splashHit = SplashHitData.CreateSplashHitData(new List<HitTarget>(), staticHit, weapon.Entity);
            base.ScheduleEvent(new CollectSplashTargetsEvent(splashHit), weapon);
        }

        [OnEventComplete]
        public void PrepareTargetsByUnblockedWeapon(ShotPrepareEvent evt, UnblockedWeaponNode weaponNode)
        {
            TargetingData targetingData = BattleCache.targetingData.GetInstance().Init();
            base.ScheduleEvent(new TargetingEvent(targetingData), weaponNode);
            base.ScheduleEvent(new SendShotToServerEvent(targetingData), weaponNode);
            base.ScheduleEvent(new SendHitToServerIfNeedEvent(targetingData), weaponNode);
        }

        [OnEventComplete]
        public void SendHitEvent(CollectSplashTargetsEvent evt, SplashWeaponNode weapon)
        {
            SplashHitData splashHit = evt.SplashHit;
            SelfSplashHitEvent eventInstance = new SelfSplashHitEvent(splashHit.DirectTargets, splashHit.StaticHit, splashHit.SplashTargets);
            base.ScheduleEvent(eventInstance, weapon);
        }

        [OnEventFire]
        public void ValidateSplashHitTargetByWeaponPoint(ValidateSplashHitPointsEvent evt, SplashWeaponNode weaponHit, ActiveTankNode targetTank, [JoinByTank] SingleNode<TankIncarnationComponent> targetIncarnation, [JoinByTank] WeaponBoundsNode weaponBoundsTarget)
        {
            TankCollidersComponent tankColliders = targetTank.tankColliders;
            BoxCollider boundsCollider = tankColliders.BoundsCollider;
            float num = weaponBoundsTarget.weaponBounds.WeaponBounds.size.y * 0.5f;
            Vector3 position = targetTank.mountPoint.MountPoint.position;
            float radiusOfMinSplashDamage = weaponHit.splashWeapon.RadiusOfMinSplashDamage;
            SplashHitData splashHit = evt.SplashHit;
            Vector3 splashCenter = splashHit.SplashCenter;
            List<HitTarget> splashTargets = splashHit.SplashTargets;
            List<GameObject> exclusionGameObjectForSplashRaycast = splashHit.ExclusionGameObjectForSplashRaycast;
            List<GameObject> targetingColliders = tankColliders.TargetingColliders;
            Vector3 vector5 = targetTank.rigidbody.Rigidbody.position;
            Vector3 vector6 = vector5 - splashCenter;
            float num3 = boundsCollider.size.z * 0.25f;
            Vector3 vector8 = boundsCollider.transform.forward * num3;
            Vector3 center = boundsCollider.bounds.center;
            List<Vector3> list5 = new List<Vector3> {
                position + (boundsCollider.transform.up * num),
                center,
                center - vector8,
                center + vector8,
                position
            };
            foreach (Vector3 vector12 in list5)
            {
                if (this.IsValidSplashPoint(targetTank, vector12, splashCenter, evt, radiusOfMinSplashDamage))
                {
                    HitTarget item = new HitTarget {
                        Entity = targetTank.Entity,
                        IncarnationEntity = targetIncarnation.Entity,
                        LocalHitPoint = Vector3.zero,
                        TargetPosition = vector5,
                        HitDirection = vector6.normalized,
                        HitDistance = vector6.magnitude
                    };
                    splashTargets.Add(item);
                    exclusionGameObjectForSplashRaycast.AddRange(targetingColliders);
                    break;
                }
            }
        }

        [Inject]
        public static BattleFlowInstancesCache BattleCache { get; set; }

        public class ActiveTankNode : SplashHitSystem.TankNode
        {
            public TankActiveStateComponent tankActiveState;
        }

        public class BlockedWeaponNode : SplashHitSystem.SplashWeaponNode
        {
            public WeaponBlockedComponent weaponBlocked;
        }

        public class SplashWeaponNode : Node
        {
            public SplashWeaponComponent splashWeapon;
            public BattleGroupComponent battleGroup;
        }

        public class TankNode : Node
        {
            public TankCollidersComponent tankColliders;
            public RigidbodyComponent rigidbody;
            public BattleGroupComponent battleGroup;
            public MountPointComponent mountPoint;
            public TankGroupComponent tankGroup;
        }

        public class UnblockedWeaponNode : SplashHitSystem.SplashWeaponNode
        {
            public WeaponUnblockedComponent weaponUnblocked;
        }

        public class WeaponBoundsNode : Node
        {
            public WeaponComponent weapon;
            public WeaponBoundsComponent weaponBounds;
            public TankGroupComponent tankGroup;
        }
    }
}

