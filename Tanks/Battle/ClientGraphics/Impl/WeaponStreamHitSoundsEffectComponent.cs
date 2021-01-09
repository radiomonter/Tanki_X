namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class WeaponStreamHitSoundsEffectComponent : BaseStreamHitWeaponSoundEffect
    {
        [SerializeField]
        private AudioClip staticHitClip;
        [SerializeField]
        private AudioClip targetHitClip;
        private bool isStaticHit;

        public bool IsStaticHit
        {
            get => 
                this.isStaticHit;
            set => 
                this.isStaticHit = value;
        }

        public AudioClip StaticHitClip
        {
            get => 
                this.staticHitClip;
            set => 
                this.staticHitClip = value;
        }

        public AudioClip TargetHitClip
        {
            get => 
                this.targetHitClip;
            set => 
                this.targetHitClip = value;
        }
    }
}

