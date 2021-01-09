namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    public class VerticalSectorTargetingSystem : ECSSystem
    {
        [OnEventFire]
        public void PrepareTargeting(TargetingEvent evt, WeaponNode weapon, [JoinByTank] TankNode tank)
        {
            TargetingData targetingData = evt.TargetingData;
            VerticalSectorsTargetingComponent verticalSectorsTargeting = weapon.verticalSectorsTargeting;
            MuzzleLogicAccessor accessor = new MuzzleLogicAccessor(weapon.muzzlePoint, weapon.weaponInstance);
            targetingData.Origin = accessor.GetBarrelOriginWorld();
            targetingData.Dir = accessor.GetFireDirectionWorld();
            targetingData.FullDistance = verticalSectorsTargeting.WorkDistance;
            targetingData.MaxAngle = (verticalSectorsTargeting.VAngleUp <= verticalSectorsTargeting.VAngleDown) ? verticalSectorsTargeting.VAngleDown : verticalSectorsTargeting.VAngleUp;
            LinkedList<TargetSector> instance = BattleCache.targetSectors.GetInstance();
            instance.Clear();
            CollectTargetSectorsEvent eventInstance = BattleCache.collectTargetSectorsEvent.GetInstance().Init();
            eventInstance.TargetSectors = instance;
            TargetingCone cone = new TargetingCone {
                VAngleUp = verticalSectorsTargeting.VAngleUp,
                VAngleDown = verticalSectorsTargeting.VAngleDown,
                HAngle = verticalSectorsTargeting.HAngle,
                Distance = verticalSectorsTargeting.WorkDistance
            };
            eventInstance.TargetingCone = cone;
            base.ScheduleEvent(eventInstance, weapon);
            CollectSectorDirectionsEvent event3 = BattleCache.collectSectorDirectionsEvent.GetInstance().Init();
            event3.TargetSectors = instance;
            event3.TargetingData = targetingData;
            base.ScheduleEvent(event3, weapon);
            base.ScheduleEvent(BattleCache.collectTargetsEvent.GetInstance().Init(targetingData), weapon);
            base.ScheduleEvent(BattleCache.targetEvaluateEvent.GetInstance().Init(targetingData), weapon);
        }

        [Inject]
        public static BattleFlowInstancesCache BattleCache { get; set; }

        public class TankNode : Node
        {
            public TankComponent tank;
        }

        public class WeaponNode : Node
        {
            public MuzzlePointComponent muzzlePoint;
            public WeaponInstanceComponent weaponInstance;
            public VerticalSectorsTargetingComponent verticalSectorsTargeting;
        }
    }
}

