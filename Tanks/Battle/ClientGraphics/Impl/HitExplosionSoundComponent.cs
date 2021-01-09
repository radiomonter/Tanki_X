namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class HitExplosionSoundComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private GameObject soundPrefab;
        [SerializeField]
        private float duration = 2f;

        public float Duration
        {
            get => 
                this.duration;
            set => 
                this.duration = value;
        }

        public GameObject SoundPrefab
        {
            get => 
                this.soundPrefab;
            set => 
                this.soundPrefab = value;
        }
    }
}

