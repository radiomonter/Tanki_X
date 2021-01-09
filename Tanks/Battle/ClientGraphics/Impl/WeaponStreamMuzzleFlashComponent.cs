namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class WeaponStreamMuzzleFlashComponent : BehaviourComponent
    {
        [SerializeField]
        private GameObject effectPrefab;

        public GameObject EffectPrefab
        {
            get => 
                this.effectPrefab;
            set => 
                this.effectPrefab = value;
        }

        public ParticleSystem EffectInstance { get; set; }

        public Light LightInstance { get; set; }
    }
}

