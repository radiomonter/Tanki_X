namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class DroneOwnerComponent : Component
    {
        private Entity incarnation;
        private UnityEngine.Rigidbody rigidbody;

        public Entity Incarnation
        {
            get => 
                this.incarnation;
            set => 
                this.incarnation = value;
        }

        public UnityEngine.Rigidbody Rigidbody
        {
            get => 
                this.rigidbody;
            set => 
                this.rigidbody = value;
        }
    }
}

