namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using System;
    using UnityEngine;

    public class WeaponDetachColliderComponent : WarmableResourceBehaviour, Component
    {
        [SerializeField]
        private MeshCollider collider;
        [SerializeField]
        private UnityEngine.Rigidbody rigidbody;

        private void Awake()
        {
            this.rigidbody.detectCollisions = false;
        }

        public override void WarmUp()
        {
            this.collider.enabled = true;
        }

        public MeshCollider Collider =>
            this.collider;

        public UnityEngine.Rigidbody Rigidbody =>
            this.rigidbody;
    }
}

