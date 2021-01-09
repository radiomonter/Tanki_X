namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class TankCollisionDetector : MonoBehaviour
    {
        public TankCollisionDetectionComponent tankCollisionComponent;

        private void CheckCollisionsWithOtherTanks(Collider other)
        {
            if (other.gameObject.layer == Layers.REMOTE_TANK_BOUNDS)
            {
                this.CanBeActivated = false;
            }
        }

        private void FixedUpdate()
        {
            if (this.UpdatesCout == 0)
            {
                this.CanBeActivated = true;
            }
            this.UpdatesCout++;
        }

        private void OnEnable()
        {
            this.UpdatesCout = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            this.CheckCollisionsWithOtherTanks(other);
        }

        private void OnTriggerStay(Collider other)
        {
            this.CheckCollisionsWithOtherTanks(other);
        }

        public int UpdatesCout { get; set; }

        public bool CanBeActivated { get; set; }
    }
}

