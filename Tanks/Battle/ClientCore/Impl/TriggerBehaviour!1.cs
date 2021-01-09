namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public abstract class TriggerBehaviour<T> : ECSBehaviour where T: Event, new()
    {
        private GameObject collisionGameObject;

        protected TriggerBehaviour()
        {
        }

        private void SendEvent()
        {
            TargetBehaviour componentInParent = this.collisionGameObject.GetComponentInParent<TargetBehaviour>();
            if (componentInParent)
            {
                base.NewEvent<T>().Attach(this.TriggerEntity).Attach(componentInParent.TargetEntity).Schedule();
            }
        }

        protected void SendEventByCollision(Collider other)
        {
            this.collisionGameObject = other.gameObject;
            this.SendEvent();
        }

        public Entity TriggerEntity { get; set; }
    }
}

