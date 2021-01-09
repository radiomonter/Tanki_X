namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class ShaftHitSystem : ECSSystem
    {
        private void CompleteTargets(SelfShaftAimingHitEvent hitEvent, TargetingData targeting, float energy)
        {
            hitEvent.Targets = HitTargetAdapter.Adapt(targeting.BestDirection.Targets);
            hitEvent.StaticHit = targeting.BestDirection.StaticHit;
            hitEvent.HitPower = 1f - energy;
        }

        private void PrepareAimingTargets(Entity weapon, Vector3 workingDir)
        {
            TargetingData targetingData = BattleCache.targetingData.GetInstance().Init();
            ShaftAimingStraightTargetingEvent eventInstance = new ShaftAimingStraightTargetingEvent {
                TargetingData = targetingData,
                WorkingDirection = workingDir
            };
            base.ScheduleEvent(eventInstance, weapon);
            base.ScheduleEvent(new SendShotToServerEvent(targetingData), weapon);
            base.ScheduleEvent(new SendShaftAimingHitToServerEvent(targetingData), weapon);
        }

        [OnEventComplete]
        public void PrepareAimingTargets(ShaftAimingShotPrepareEvent evt, ShaftNode weapon)
        {
            this.PrepareAimingTargets(weapon.Entity, evt.WorkingDir);
        }

        private void PrepareQuickShotTargets(Entity weapon)
        {
            TargetingData targetingData = BattleCache.targetingData.GetInstance().Init();
            base.ScheduleEvent(new TargetingEvent(targetingData), weapon);
            base.ScheduleEvent(new SendShotToServerEvent(targetingData), weapon);
            base.ScheduleEvent(new SendHitToServerEvent(targetingData), weapon);
        }

        [OnEventComplete]
        public void PrepareQuickShotTargets(ShotPrepareEvent evt, BlockedShaftNode weapon)
        {
            this.PrepareQuickShotTargets(weapon.Entity);
        }

        [OnEventComplete]
        public void PrepareQuickShotTargets(ShotPrepareEvent evt, UnblockedShaftNode weapon)
        {
            this.PrepareQuickShotTargets(weapon.Entity);
        }

        [OnEventFire]
        public void SendHit(SendShaftAimingHitToServerEvent evt, ShaftNode weapon)
        {
            if (evt.TargetingData.BestDirection.HasAnyHit())
            {
                SelfShaftAimingHitEvent hitEvent = new SelfShaftAimingHitEvent();
                float energy = weapon.weaponEnergy.Energy;
                this.CompleteTargets(hitEvent, evt.TargetingData, energy);
                base.ScheduleEvent(hitEvent, weapon);
            }
        }

        [OnEventComplete]
        public void SendHitToServer(SendHitToServerEvent e, ShaftNode weapon)
        {
            if (e.TargetingData.BestDirection.HasAnyHit())
            {
                SelfShaftAimingHitEvent eventInstance = new SelfShaftAimingHitEvent(HitTargetAdapter.Adapt(e.TargetingData.BestDirection.Targets), e.TargetingData.BestDirection.StaticHit) {
                    HitPower = 0f
                };
                base.ScheduleEvent(eventInstance, weapon);
            }
        }

        [Inject]
        public static BattleFlowInstancesCache BattleCache { get; set; }

        public class BlockedShaftNode : ShaftHitSystem.ShaftNode
        {
            public WeaponBlockedComponent weaponBlocked;
        }

        public class ShaftNode : Node
        {
            public ShaftComponent shaft;
            public WeaponEnergyComponent weaponEnergy;
        }

        public class UnblockedShaftNode : ShaftHitSystem.ShaftNode
        {
            public WeaponUnblockedComponent weaponUnblocked;
        }
    }
}

