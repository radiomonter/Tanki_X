namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CameraRootTransformComponent : Component
    {
        public CameraRootTransformComponent(Transform root)
        {
            this.Root = root;
        }

        public Transform Root { get; set; }
    }
}

