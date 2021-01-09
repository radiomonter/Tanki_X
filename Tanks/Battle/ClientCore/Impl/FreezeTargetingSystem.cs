﻿namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class FreezeTargetingSystem : ECSSystem
    {
        [OnEventFire]
        public void SetMaskForFreezeTargeting(NodeAddedEvent evt, FreezeTargetCollectorNode weapon)
        {
            weapon.targetCollector.TargetValidator.LayerMask = LayerMasks.GUN_TARGETING_WITHOUT_DEAD_UNITS;
        }

        public class FreezeTargetCollectorNode : Node
        {
            public FreezeComponent freeze;
            public TargetCollectorComponent targetCollector;
        }
    }
}

