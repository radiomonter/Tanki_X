namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class PelletThrowingGraphicsComponent : BehaviourComponent
    {
        [SerializeField]
        private ParticleSystem trails;
        [SerializeField]
        private ParticleSystem hits;
        [SerializeField]
        private float sparklesMinLifetime = 0.25f;
        [SerializeField]
        private float sparklesMaxLifetime = 0.5f;
        [SerializeField]
        private float hitReflectVeolcity = 0.05f;

        public ParticleSystem Trails
        {
            get => 
                this.trails;
            set => 
                this.trails = value;
        }

        public ParticleSystem Hits
        {
            get => 
                this.hits;
            set => 
                this.hits = value;
        }

        public float SparklesMinLifetime
        {
            get => 
                this.sparklesMinLifetime;
            set => 
                this.sparklesMinLifetime = value;
        }

        public float SparklesMaxLifetime
        {
            get => 
                this.sparklesMaxLifetime;
            set => 
                this.sparklesMaxLifetime = value;
        }

        public float HitReflectVeolcity
        {
            get => 
                this.hitReflectVeolcity;
            set => 
                this.hitReflectVeolcity = value;
        }
    }
}

