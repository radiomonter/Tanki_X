namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class TankFallingSoundEffectComponent : BehaviourComponent
    {
        [SerializeField]
        private AudioSource fallingSourceAsset;
        [SerializeField]
        private AudioClip[] fallingClips;
        [SerializeField]
        private AudioSource collisionSourceAsset;
        [SerializeField]
        private float minPower = 1f;
        [SerializeField]
        private float maxPower = 64f;

        private void Awake()
        {
            this.FallingClipIndex = 0;
        }

        public int FallingClipIndex { get; set; }

        public AudioClip[] FallingClips
        {
            get => 
                this.fallingClips;
            set => 
                this.fallingClips = value;
        }

        public AudioSource FallingSourceAsset
        {
            get => 
                this.fallingSourceAsset;
            set => 
                this.fallingSourceAsset = value;
        }

        public AudioSource CollisionSourceAsset
        {
            get => 
                this.collisionSourceAsset;
            set => 
                this.collisionSourceAsset = value;
        }

        public float MinPower
        {
            get => 
                this.minPower;
            set => 
                this.minPower = value;
        }

        public float MaxPower
        {
            get => 
                this.maxPower;
            set => 
                this.maxPower = value;
        }
    }
}

