namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class FlagColliderComponent : Component
    {
        public FlagColliderComponent()
        {
        }

        public FlagColliderComponent(BoxCollider boxCollider)
        {
            this.boxCollider = boxCollider;
        }

        public BoxCollider boxCollider { get; set; }
    }
}

