namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CameraTargetComponent : Component
    {
        public CameraTargetComponent()
        {
        }

        public CameraTargetComponent(GameObject targetObject)
        {
            this.TargetObject = targetObject;
        }

        public GameObject TargetObject { get; set; }
    }
}

