namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    public class ConicTargetingSystem : ECSSystem
    {
        [OnEventFire]
        public void PrepareTargeting(TargetingEvent evt, TargetingNode conicTargeting)
        {
            TargetingData targetingData = evt.TargetingData;
            ConicTargetingComponent component = conicTargeting.conicTargeting;
            MuzzleLogicAccessor accessor = new MuzzleLogicAccessor(conicTargeting.muzzlePoint, conicTargeting.weaponInstance);
            targetingData.Origin = accessor.GetWorldPositionShiftDirectionBarrel(component.FireOriginOffsetCoeff);
            targetingData.Dir = accessor.GetFireDirectionWorld();
            targetingData.FullDistance = component.WorkDistance;
            targetingData.MaxAngle = component.HalfConeAngle;
            base.ScheduleEvent(BattleCache.collectDirectionsEvent.GetInstance().Init(targetingData), conicTargeting);
            base.ScheduleEvent(BattleCache.collectTargetsEvent.GetInstance().Init(targetingData), conicTargeting);
            base.ScheduleEvent(BattleCache.targetEvaluateEvent.GetInstance().Init(targetingData), conicTargeting);
        }

        [Inject]
        public static BattleFlowInstancesCache BattleCache { get; set; }

        public class TargetingNode : Node
        {
            public ConicTargetingComponent conicTargeting;
            public MuzzlePointComponent muzzlePoint;
            public WeaponInstanceComponent weaponInstance;
            public TankGroupComponent tankGroup;
        }
    }
}

