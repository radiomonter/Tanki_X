namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    public class StreamHitSystem : ECSSystem
    {
        private static readonly float NEAR_HIT_POSITION_EPSILON = 0.2f;

        private void AddStreamHit(Entity weapon, TargetingData targetingData, StreamHitConfigComponent config, StreamHitCheckingComponent checking)
        {
            if (!targetingData.HasAnyHit())
            {
                throw new Exception("No hit in StreamHit " + weapon);
            }
            if (!config.DetectStaticHit && !targetingData.HasTargetHit())
            {
                throw new Exception("No tank in StreamHit" + weapon);
            }
            StreamHitComponent hit = new StreamHitComponent();
            this.FillStreamHit(hit, targetingData);
            this.SaveHitSentToServer(checking, hit);
            weapon.AddComponent(hit);
        }

        [OnEventFire]
        public void Check(UpdateEvent e, CheckingNode weapon)
        {
            if (weapon.Entity.HasComponent<StreamHitComponent>() || ((weapon.streamHitChecking.LastCheckTime + weapon.streamHitConfig.LocalCheckPeriod) <= UnityTime.time))
            {
                TargetingData targetingData = BattleCache.targetingData.GetInstance().Init();
                base.ScheduleEvent(new TargetingEvent(targetingData), weapon.Entity);
                base.ScheduleEvent(new StreamHitResultEvent(targetingData), weapon.Entity);
            }
        }

        private void FillStreamHit(StreamHitComponent hit, TargetingData targetingData)
        {
            hit.TankHit = GetTankHit(targetingData);
            hit.StaticHit = targetingData.BestDirection.StaticHit;
        }

        private static HitTarget GetTankHit(TargetingData targetingData) => 
            !targetingData.BestDirection.HasTargetHit() ? null : HitTargetAdapter.Adapt(targetingData.BestDirection.Targets[0]);

        private bool HasHit(TargetingData targetingData, StreamHitConfigComponent streamHitConfigComponent) => 
            targetingData.BestDirection.HasTargetHit() || (streamHitConfigComponent.DetectStaticHit && targetingData.BestDirection.HasStaticHit());

        private bool IsAlmostEqual(HitTarget tankHit, HitTarget lastSentTankHit) => 
            (tankHit == null) || MathUtil.NearlyEqual(tankHit.LocalHitPoint, lastSentTankHit.LocalHitPoint, NEAR_HIT_POSITION_EPSILON);

        private static bool IsAlmostEqual(StaticHit staticHit, StaticHit lastSentStaticHit) => 
            ((staticHit != null) || (lastSentStaticHit != null)) ? ((staticHit != null) && ((lastSentStaticHit != null) && MathUtil.NearlyEqual(staticHit.Position, lastSentStaticHit.Position, NEAR_HIT_POSITION_EPSILON))) : true;

        private bool IsSameTank(HitTarget tankHit, HitTarget lastSentTankHit) => 
            ((tankHit != null) || (lastSentTankHit != null)) ? ((tankHit != null) && ((lastSentTankHit != null) && ReferenceEquals(tankHit.Entity, lastSentTankHit.Entity))) : true;

        [OnEventFire]
        public void RemoveStreamHit(RemoveStreamHitEvent e, SingleNode<StreamHitComponent> weapon)
        {
            weapon.Entity.RemoveComponent<StreamHitComponent>();
        }

        [OnEventFire]
        public void ResendRemoveStreamHit(NodeRemoveEvent e, SingleNode<StreamHitCheckingComponent> weapon)
        {
            base.ScheduleEvent<RemoveStreamHitEvent>(weapon);
        }

        private void SaveHitSentToServer(StreamHitCheckingComponent checking, StreamHitComponent streamHit)
        {
            checking.LastSendToServerTime = UnityTime.time;
            checking.LastSentTankHit = streamHit.TankHit;
            checking.LastSentStaticHit = streamHit.StaticHit;
        }

        [OnEventFire]
        public void UpdateChecking(StreamHitResultEvent e, CheckingNode weapon, [JoinSelf] SingleNode<ShootableComponent> node)
        {
            this.UpdateHitExistence(e.TargetingData, weapon);
        }

        [OnEventFire]
        public void UpdateDataBeforeHit(SendHitToServerEvent e, HitNode weapon, [JoinSelf] SingleNode<ShootableComponent> node)
        {
            if (this.HasHit(e.TargetingData, weapon.streamHitConfig))
            {
                this.UpdateHitData(weapon, e.TargetingData, true);
            }
        }

        [OnEventFire]
        public void UpdateExistenceBeforeHit(SendHitToServerEvent e, CheckingNode weapon, [JoinSelf] SingleNode<ShootableComponent> node)
        {
            this.UpdateHitExistence(e.TargetingData, weapon);
        }

        [OnEventFire]
        public void UpdateFromServer(RemoteUpdateStreamHitEvent e, SingleNode<StreamHitComponent> weapon)
        {
            weapon.component.TankHit = e.TankHit;
            weapon.component.StaticHit = e.StaticHit;
        }

        [OnEventComplete]
        public void UpdateHit(StreamHitResultEvent e, HitNode weapon, [JoinSelf] SingleNode<ShootableComponent> node)
        {
            this.UpdateHitData(weapon, e.TargetingData, false);
        }

        private void UpdateHitData(HitNode weapon, TargetingData targetingData, bool skipTimeoutCheck)
        {
            StreamHitConfigComponent streamHitConfig = weapon.streamHitConfig;
            StreamHitCheckingComponent streamHitChecking = weapon.streamHitChecking;
            StreamHitComponent streamHit = weapon.streamHit;
            HitTarget tankHit = GetTankHit(targetingData);
            DirectionData bestDirection = targetingData.BestDirection;
            weapon.streamHitChecking.LastCheckTime = UnityTime.time;
            streamHit.TankHit = tankHit;
            streamHit.StaticHit = bestDirection.StaticHit;
            StaticHit staticHit = !streamHitConfig.DetectStaticHit ? null : bestDirection.StaticHit;
            bool flag = false;
            bool flag2 = false;
            if (!this.IsSameTank(tankHit, streamHitChecking.LastSentTankHit))
            {
                flag = true;
            }
            else if (skipTimeoutCheck || ((streamHitChecking.LastSendToServerTime + streamHitConfig.SendToServerPeriod) < UnityTime.time))
            {
                if (!IsAlmostEqual(staticHit, streamHitChecking.LastSentStaticHit))
                {
                    flag2 = true;
                }
                else if (!this.IsAlmostEqual(tankHit, streamHitChecking.LastSentTankHit))
                {
                    flag2 = true;
                }
            }
            if (flag)
            {
                weapon.Entity.RemoveComponent<StreamHitComponent>();
                this.AddStreamHit(weapon.Entity, targetingData, streamHitConfig, streamHitChecking);
            }
            else if (flag2)
            {
                base.ScheduleEvent(new SelfUpdateStreamHitEvent(tankHit, staticHit), weapon);
                this.SaveHitSentToServer(streamHitChecking, streamHit);
            }
        }

        private void UpdateHitExistence(TargetingData targetingData, CheckingNode weapon)
        {
            bool flag = weapon.Entity.HasComponent<StreamHitComponent>();
            if (this.HasHit(targetingData, weapon.streamHitConfig))
            {
                if (!flag)
                {
                    this.AddStreamHit(weapon.Entity, targetingData, weapon.streamHitConfig, weapon.streamHitChecking);
                }
            }
            else if (flag)
            {
                weapon.Entity.RemoveComponent<StreamHitComponent>();
            }
        }

        [OnEventFire]
        public void ValidateHitEvent(SelfHitEvent e, HitNode weapon)
        {
            if ((e.Targets != null) && (e.Targets.Count > 0))
            {
                if (e.Targets.Count > 1)
                {
                    throw new Exception("Invalid stream hit. Targets.Count=" + e.Targets.Count);
                }
                Entity entity = e.Targets.Single<HitTarget>().Entity;
                Entity entity2 = weapon.streamHit.TankHit.Entity;
                if (!entity.Equals(entity2))
                {
                    object[] objArray1 = new object[] { "Invalid stream hit. targetTankInEvent=", entity, " targetTankInComponent=", entity2 };
                    throw new Exception(string.Concat(objArray1));
                }
            }
        }

        [Inject]
        public static BattleFlowInstancesCache BattleCache { get; set; }

        [Inject]
        public static Platform.Library.ClientUnityIntegration.API.UnityTime UnityTime { get; set; }

        public class CheckingNode : Node
        {
            public StreamHitConfigComponent streamHitConfig;
            public StreamHitCheckingComponent streamHitChecking;
        }

        public class HitNode : Node
        {
            public StreamHitConfigComponent streamHitConfig;
            public StreamHitCheckingComponent streamHitChecking;
            public StreamHitComponent streamHit;
            public WeaponInstanceComponent weaponInstance;
        }
    }
}

