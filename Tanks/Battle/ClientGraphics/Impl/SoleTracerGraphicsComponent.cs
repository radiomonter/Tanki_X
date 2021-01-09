namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class SoleTracerGraphicsComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private ParticleSystem tracer;
        [SerializeField]
        private float maxTime = 0.2f;

        public ParticleSystem Tracer
        {
            get => 
                this.tracer;
            set => 
                this.tracer = value;
        }

        public float MaxTime
        {
            get => 
                this.maxTime;
            set => 
                this.maxTime = value;
        }
    }
}

