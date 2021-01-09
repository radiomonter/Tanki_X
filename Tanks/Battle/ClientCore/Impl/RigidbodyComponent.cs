namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [SerialVersionUID(0x8d2e6e143ac6e4aL)]
    public class RigidbodyComponent : Component
    {
        public RigidbodyComponent()
        {
        }

        public RigidbodyComponent(UnityEngine.Rigidbody rigidbody)
        {
            this.Rigidbody = rigidbody;
        }

        public UnityEngine.Rigidbody Rigidbody { get; set; }

        public Transform RigidbodyTransform =>
            this.Rigidbody.transform;
    }
}

