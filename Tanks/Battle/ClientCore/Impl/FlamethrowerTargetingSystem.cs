namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class FlamethrowerTargetingSystem : ECSSystem
    {
        [OnEventFire]
        public void SetMaskForFlamethrowerTargeting(NodeAddedEvent evt, FlamethrowerTargetCollectorNode weapon)
        {
            weapon.targetCollector.TargetValidator.LayerMask = LayerMasks.GUN_TARGETING_WITHOUT_DEAD_UNITS;
        }

        public class FlamethrowerTargetCollectorNode : Node
        {
            public FlamethrowerComponent flamethrower;
            public TargetCollectorComponent targetCollector;
        }
    }
}

