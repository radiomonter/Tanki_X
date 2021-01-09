namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public abstract class CaseSoundEffectComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private GameObject caseSoundAsset;

        protected CaseSoundEffectComponent()
        {
        }

        public GameObject CaseSoundAsset
        {
            get => 
                this.caseSoundAsset;
            set => 
                this.caseSoundAsset = value;
        }

        public AudioSource Source { get; set; }
    }
}

