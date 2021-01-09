namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class TargetCollectorComponent : Component
    {
        public TargetCollectorComponent(Tanks.Battle.ClientCore.Impl.TargetCollector targetCollector, Tanks.Battle.ClientCore.Impl.TargetValidator targetValidator)
        {
            this.TargetCollector = targetCollector;
            this.TargetValidator = targetValidator;
        }

        public void Collect(TargetingData targetingData, int layerMask = 0)
        {
            this.TargetCollector.Collect(this.TargetValidator, targetingData, layerMask);
        }

        public void Collect(float fullDistance, DirectionData direction, int layerMask = 0)
        {
            this.TargetCollector.Collect(this.TargetValidator, fullDistance, direction, layerMask);
        }

        public DirectionData Collect(Vector3 origin, Vector3 dir, float fullDistance, int layerMask = 0) => 
            this.TargetCollector.Collect(this.TargetValidator, fullDistance, origin, dir, layerMask);

        public Tanks.Battle.ClientCore.Impl.TargetCollector TargetCollector { get; protected set; }

        public Tanks.Battle.ClientCore.Impl.TargetValidator TargetValidator { get; protected set; }
    }
}

