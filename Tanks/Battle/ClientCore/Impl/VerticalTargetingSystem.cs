namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    public class VerticalTargetingSystem : ECSSystem
    {
        [OnEventFire]
        public void PrepareTargeting(TargetingEvent evt, TargetingNode verticalTargeting)
        {
            TargetingData targetingData = evt.TargetingData;
            VerticalTargetingComponent component = verticalTargeting.verticalTargeting;
            MuzzleLogicAccessor accessor = new MuzzleLogicAccessor(verticalTargeting.muzzlePoint, verticalTargeting.weaponInstance);
            targetingData.Origin = accessor.GetWorldPosition();
            targetingData.Dir = accessor.GetFireDirectionWorld();
            targetingData.FullDistance = component.WorkDistance;
            targetingData.MaxAngle = (component.AngleUp <= component.AngleDown) ? component.AngleDown : component.AngleUp;
            base.ScheduleEvent(BattleCache.collectDirectionsEvent.GetInstance().Init(targetingData), verticalTargeting);
            base.ScheduleEvent(BattleCache.collectTargetsEvent.GetInstance().Init(targetingData), verticalTargeting);
            base.ScheduleEvent(BattleCache.targetEvaluateEvent.GetInstance().Init(targetingData), verticalTargeting);
        }

        [Inject]
        public static BattleFlowInstancesCache BattleCache { get; set; }

        public class TargetingNode : Node
        {
            public VerticalTargetingComponent verticalTargeting;
            public MuzzlePointComponent muzzlePoint;
            public WeaponInstanceComponent weaponInstance;
        }
    }
}

