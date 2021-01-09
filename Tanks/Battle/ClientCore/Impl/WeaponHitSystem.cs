namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    public class WeaponHitSystem : ECSSystem
    {
        [OnEventComplete]
        public void PrepareTargets(ShotPrepareEvent evt, UnblockedWeaponNode weaponNode)
        {
            TargetingData targetingData = BattleCache.targetingData.GetInstance().Init();
            base.ScheduleEvent(new TargetingEvent(targetingData), weaponNode);
            base.ScheduleEvent(new SendShotToServerEvent(targetingData), weaponNode);
            base.ScheduleEvent(new SendHitToServerIfNeedEvent(targetingData), weaponNode);
        }

        [OnEventComplete]
        public void SendHit(SendHitToServerIfNeedEvent evt, UnblockedWeaponNode weapon)
        {
            WeaponHitComponent weaponHit = weapon.weaponHit;
            StaticHit staticHit = null;
            List<HitTarget> targets = new List<HitTarget>(4);
            if (evt.TargetingData.HasTargetHit())
            {
                if (!weaponHit.RemoveDuplicateTargets)
                {
                    targets = HitTargetAdapter.Adapt(evt.TargetingData.BestDirection.Targets);
                }
                else
                {
                    HashSet<Entity> set = new HashSet<Entity>();
                    int num = 0;
                    while (num < evt.TargetingData.Directions.Count)
                    {
                        DirectionData data = evt.TargetingData.Directions[num];
                        int num2 = 0;
                        while (true)
                        {
                            if (num2 >= data.Targets.Count)
                            {
                                num++;
                                break;
                            }
                            TargetData targetData = data.Targets[num2];
                            if (set.Add(targetData.TargetEntity))
                            {
                                targets.Add(HitTargetAdapter.Adapt(targetData));
                            }
                            num2++;
                        }
                    }
                }
            }
            if (weaponHit.SendStaticHit && evt.TargetingData.HasStaticHit())
            {
                staticHit = evt.TargetingData.BestDirection.StaticHit;
            }
            if ((staticHit == null) && (targets.Count == 0))
            {
                base.ScheduleEvent<SelfHitSkipEvent>(weapon);
            }
            else
            {
                base.ScheduleEvent(new SendHitToServerEvent(evt.TargetingData, targets, staticHit), weapon);
            }
        }

        [OnEventComplete]
        public void SendHitToServer(SendHitToServerEvent e, UnblockedWeaponNode weapon)
        {
            SelfHitEvent eventInstance = new SelfHitEvent(e.Targets, e.StaticHit) {
                ShotId = weapon.shotId.ShotId
            };
            base.ScheduleEvent(eventInstance, weapon);
        }

        [Inject]
        public static BattleFlowInstancesCache BattleCache { get; set; }

        public class UnblockedWeaponNode : Node
        {
            public WeaponHitComponent weaponHit;
            public MuzzlePointComponent muzzlePoint;
            public WeaponUnblockedComponent weaponUnblocked;
            public ShotIdComponent shotId;
        }
    }
}

