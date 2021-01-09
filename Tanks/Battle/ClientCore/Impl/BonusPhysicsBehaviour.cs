namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using UnityEngine;

    public class BonusPhysicsBehaviour : TriggerBehaviour<TriggerEnterEvent>
    {
        private void OnTriggerEnter(Collider other)
        {
            base.SendEventByCollision(other);
        }
    }
}

