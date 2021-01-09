namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public abstract class AbstractVulcanSoundEffectComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private GameObject effectPrefab;
        [SerializeField]
        private float startTimePerSec;
        [SerializeField]
        private float delayPerSec;
        private AudioSource soundSource;

        protected AbstractVulcanSoundEffectComponent()
        {
        }

        public float DelayPerSec
        {
            get => 
                this.delayPerSec;
            set => 
                this.delayPerSec = value;
        }

        public AudioSource SoundSource
        {
            get => 
                this.soundSource;
            set => 
                this.soundSource = value;
        }

        public GameObject EffectPrefab
        {
            get => 
                this.effectPrefab;
            set => 
                this.effectPrefab = value;
        }

        public float StartTimePerSec
        {
            get => 
                this.startTimePerSec;
            set => 
                this.startTimePerSec = value;
        }
    }
}

