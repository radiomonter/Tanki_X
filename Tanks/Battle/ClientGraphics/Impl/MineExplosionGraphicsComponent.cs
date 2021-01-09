namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class MineExplosionGraphicsComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private GameObject effectPrefab;
        [SerializeField]
        private float explosionLifeTime = 2f;
        [SerializeField]
        private Vector3 origin = Vector3.up;

        public GameObject EffectPrefab
        {
            get => 
                this.effectPrefab;
            set => 
                this.effectPrefab = value;
        }

        public float ExplosionLifeTime =>
            this.explosionLifeTime;

        public Vector3 Origin =>
            this.origin;
    }
}

