namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class TargetCollectorSystem : ECSSystem
    {
        [OnEventFire]
        public void CollectTargetsOnDirections(CollectTargetsEvent evt, TargetCollectorNode targetCollectorNode)
        {
            TargetingData targetingData = evt.TargetingData;
            targetCollectorNode.targetCollector.Collect(targetingData, 0);
        }

        public class TargetCollectorNode : Node
        {
            public TargetCollectorComponent targetCollector;
        }
    }
}

