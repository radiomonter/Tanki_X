namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class VulcanImpactEvent : Event
    {
        private Vector3 localHitPoint;
        private Vector3 force;
        private float weakeningCoeff;

        public Vector3 LocalHitPoint
        {
            get => 
                this.localHitPoint;
            set => 
                this.localHitPoint = !ImpactEvent.ValidateImpactData(value) ? Vector3.zero : value;
        }

        public Vector3 Force
        {
            get => 
                this.force;
            set => 
                this.force = !ImpactEvent.ValidateImpactData(value) ? Vector3.zero : value;
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

