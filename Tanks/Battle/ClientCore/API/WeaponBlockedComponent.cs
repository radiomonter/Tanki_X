namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class WeaponBlockedComponent : Component
    {
        public Vector3 BlockPoint { get; set; }

        public Vector3 BlockNormal { get; set; }

        public GameObject BlockGameObject { get; set; }
    }
}

