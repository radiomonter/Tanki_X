namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class HitExplosionGraphicsComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private GameObject explosionAsset;
        [SerializeField]
        private float explosionDuration;
        [SerializeField]
        private float explosionOffset;
        [SerializeField]
        private bool useForBlockedWeapon = true;

        public bool UseForBlockedWeapon
        {
            get => 
                this.useForBlockedWeapon;
            set => 
                this.useForBlockedWeapon = value;
        }

        public GameObject ExplosionAsset
        {
            get => 
                this.explosionAsset;
            set => 
                this.explosionAsset = value;
        }

        public float ExplosionDuration
        {
            get => 
                this.explosionDuration;
            set => 
                this.explosionDuration = value;
        }

        public float ExplosionOffset
        {
            get => 
                this.explosionOffset;
            set => 
                this.explosionOffset = value;
        }
    }
}

