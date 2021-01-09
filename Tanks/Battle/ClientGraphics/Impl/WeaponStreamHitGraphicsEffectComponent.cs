namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class WeaponStreamHitGraphicsEffectComponent : BehaviourComponent
    {
        [SerializeField]
        private ParticleSystem hitStaticPrefab;
        [SerializeField]
        private ParticleSystem hitTargetPrefab;
        [SerializeField]
        private float hitOffset;

        public void Init(Transform parent)
        {
            this.HitStatic = Instantiate<ParticleSystem>(this.HitStaticPrefab);
            this.HitTarget = Instantiate<ParticleSystem>(this.HitTargetPrefab);
            this.HitStaticLight = this.HitStatic.GetComponent<Light>();
            this.HitTargetLight = this.HitTarget.GetComponent<Light>();
            this.HitStatic.transform.parent = parent;
            this.HitTarget.transform.parent = parent;
            this.HitStatic.Stop(true);
            this.HitTarget.Stop(true);
            this.HitStaticLight.enabled = false;
            this.HitTargetLight.enabled = false;
        }

        public ParticleSystem HitStatic { get; set; }

        public ParticleSystem HitTarget { get; set; }

        public Light HitStaticLight { get; set; }

        public Light HitTargetLight { get; set; }

        public float HitOffset
        {
            get => 
                this.hitOffset;
            set => 
                this.hitOffset = value;
        }

        public ParticleSystem HitStaticPrefab
        {
            get => 
                this.hitStaticPrefab;
            set => 
                this.hitStaticPrefab = value;
        }

        public ParticleSystem HitTargetPrefab
        {
            get => 
                this.hitTargetPrefab;
            set => 
                this.hitTargetPrefab = value;
        }
    }
}

