namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public abstract class BaseStreamHitWeaponSoundEffect : BehaviourComponent
    {
        [SerializeField]
        private GameObject effectPrefab;
        private Tanks.Battle.ClientGraphics.Impl.SoundController soundController;

        protected BaseStreamHitWeaponSoundEffect()
        {
        }

        public Tanks.Battle.ClientGraphics.Impl.SoundController SoundController
        {
            get => 
                this.soundController;
            set => 
                this.soundController = value;
        }

        public GameObject EffectPrefab =>
            this.effectPrefab;
    }
}

