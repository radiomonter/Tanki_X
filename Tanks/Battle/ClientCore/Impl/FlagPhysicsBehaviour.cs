namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using UnityEngine;

    public class FlagPhysicsBehaviour : TriggerBehaviour<TankFlagCollisionEvent>
    {
        private void OnTriggerEnter(Collider other)
        {
            base.SendEventByCollision(other);
        }

        private void OnTriggerExit(Collider other)
        {
            base.SendEventByCollision(other);
        }
    }
}

