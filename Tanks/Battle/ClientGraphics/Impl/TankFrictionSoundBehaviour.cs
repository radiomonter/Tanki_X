namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class TankFrictionSoundBehaviour : ECSBehaviour
    {
        private Entity tankEntity;

        private void Awake()
        {
            base.enabled = false;
        }

        private void DisableCollisionStay()
        {
            this.TriggerStay = false;
        }

        public void Init(Entity tankEntity)
        {
            this.tankEntity = tankEntity;
        }

        private void OnTriggerEnter(Collider other)
        {
            this.UpdateCollisionStay(other);
        }

        private void OnTriggerExit(Collider other)
        {
            this.DisableCollisionStay();
            this.SendTankFrictionExitEvent();
        }

        private void OnTriggerStay(Collider other)
        {
            this.UpdateCollisionStay(other);
        }

        private void SendTankFrictionExitEvent()
        {
            TankFrictionExitEvent eventInstance = new TankFrictionExitEvent();
            base.NewEvent(eventInstance).Attach(this.tankEntity).Schedule();
        }

        private void UpdateCollisionStay(Collider collider)
        {
            this.TriggerStay = true;
            this.FrictionCollider = collider;
        }

        public bool TriggerStay { get; set; }

        public Collider FrictionCollider { get; set; }
    }
}

