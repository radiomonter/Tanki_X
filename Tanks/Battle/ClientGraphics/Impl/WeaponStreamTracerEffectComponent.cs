namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class WeaponStreamTracerEffectComponent : BehaviourComponent
    {
        [SerializeField]
        private float tracerMaxLength = 222.22f;
        [SerializeField]
        private float startTracerOffset = 0.5f;
        [SerializeField]
        private GameObject tracer;

        public float StartTracerOffset
        {
            get => 
                this.startTracerOffset;
            set => 
                this.startTracerOffset = value;
        }

        public float TracerMaxLength
        {
            get => 
                this.tracerMaxLength;
            set => 
                this.tracerMaxLength = value;
        }

        public GameObject Tracer
        {
            get => 
                this.tracer;
            set => 
                this.tracer = value;
        }
    }
}

