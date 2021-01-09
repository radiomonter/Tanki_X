namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class RemoteTankSmootherComponent : Component
    {
        public float smoothingCoeff = 20f;
        public Vector3 prevVisualPosition = Vector3.zero;
        public Quaternion prevVisualRotation = Quaternion.identity;
    }
}

