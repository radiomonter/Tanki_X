namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class TankCollisionComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private bool hasCollision;

        private void OnCollisionEnter(UnityEngine.Collision collision)
        {
            this.hasCollision = true;
            this.Collision = collision;
        }

        private void OnCollisionExit(UnityEngine.Collision collision)
        {
            this.hasCollision = false;
            this.Collision = null;
        }

        public bool HasCollision =>
            this.hasCollision;

        public UnityEngine.Collision Collision { get; private set; }
    }
}

