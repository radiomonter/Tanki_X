namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class NameplatePositionComponent : Component
    {
        public float sqrDistance;
        public Vector3 previousPosition;
    }
}

