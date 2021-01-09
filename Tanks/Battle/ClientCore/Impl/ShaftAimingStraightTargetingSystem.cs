namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class ShaftAimingStraightTargetingSystem : ECSSystem
    {
        [OnEventFire]
        public void FillTargetingData(ShaftAimingStraightTargetingEvent evt, ShaftAimingStraightTargetingNode weapon)
        {
            TargetingData targetingData = evt.TargetingData;
            targetingData.Origin = new MuzzleLogicAccessor(weapon.muzzlePoint, weapon.weaponInstance).GetBarrelOriginWorld();
            targetingData.Dir = evt.WorkingDirection;
            targetingData.FullDistance = weapon.verticalSectorsTargeting.WorkDistance;
            base.ScheduleEvent(new ShaftAimingCollectDirectionEvent(targetingData), weapon);
            base.ScheduleEvent(new ShaftAimingCollectTargetsEvent(targetingData), weapon);
        }

        public class ShaftAimingStraightTargetingNode : Node
        {
            public VerticalSectorsTargetingComponent verticalSectorsTargeting;
            public MuzzlePointComponent muzzlePoint;
            public WeaponInstanceComponent weaponInstance;
        }
    }
}

