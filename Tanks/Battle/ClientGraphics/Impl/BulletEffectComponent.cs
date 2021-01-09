namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class BulletEffectComponent : BehaviourComponent
    {
        [SerializeField]
        private GameObject bulletPrefab;
        [SerializeField]
        private GameObject explosionPrefab;
        [SerializeField]
        private float explosionTime = 1f;
        [SerializeField]
        private float explosionOffset = 0.5f;

        public GameObject BulletPrefab
        {
            get => 
                this.bulletPrefab;
            set => 
                this.bulletPrefab = value;
        }

        public GameObject ExplosionPrefab
        {
            get => 
                this.explosionPrefab;
            set => 
                this.explosionPrefab = value;
        }

        public float ExplosionTime
        {
            get => 
                this.explosionTime;
            set => 
                this.explosionTime = value;
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

