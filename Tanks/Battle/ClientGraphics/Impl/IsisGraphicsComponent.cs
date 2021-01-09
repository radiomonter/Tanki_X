namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class IsisGraphicsComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private GameObject rayPrefab;

        public GameObject RayPrefab
        {
            get => 
                this.rayPrefab;
            set => 
                this.rayPrefab = value;
        }

        public IsisRayEffectBehaviour Ray { get; set; }
    }
}

