namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class HammerHitSoundEffectComponent : BehaviourComponent
    {
        [SerializeField]
        private GameObject staticHitSoundAsset;
        [SerializeField]
        private GameObject targetHitSoundAsset;
        [SerializeField]
        private float staticHitSoundDuration;
        [SerializeField]
        private float targetHitSoundDuration;

        private void Awake()
        {
            this.DifferentTargetsByHit = new List<HitTarget>();
        }

        public GameObject StaticHitSoundAsset
        {
            get => 
                this.staticHitSoundAsset;
            set => 
                this.staticHitSoundAsset = value;
        }

        public GameObject TargetHitSoundAsset
        {
            get => 
                this.targetHitSoundAsset;
            set => 
                this.targetHitSoundAsset = value;
        }

        public float StaticHitSoundDuration
        {
            get => 
                this.staticHitSoundDuration;
            set => 
                this.staticHitSoundDuration = value;
        }

        public float TargetHitSoundDuration
        {
            get => 
                this.targetHitSoundDuration;
            set => 
                this.targetHitSoundDuration = value;
        }

        public List<HitTarget> DifferentTargetsByHit { get; set; }
    }
}

