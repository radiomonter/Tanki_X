namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class ImpactEvent : Event
    {
        private Vector3 localHitPoint;
        private Vector3 force;
        private float weakeningCoeff;

        public static bool ValidateImpactData(Vector3 force) => 
            PhysicsUtil.IsValidVector3(force);

        public Vector3 LocalHitPoint
        {
            get => 
                this.localHitPoint;
            set => 
                this.localHitPoint = !ValidateImpactData(value) ? Vector3.zero : value;
        }

        public Vector3 Force
        {
            get => 
                this.force;
            set => 
                this.force = !ValidateImpactData(value) ? Vector3.zero : value;
        }

        public float WeakeningCoeff
        {
            get => 
                this.weakeningCoeff;
            set => 
                this.weakeningCoeff = !PhysicsUtil.IsValidFloat(value) ? 0f : value;
        }
    }
}

