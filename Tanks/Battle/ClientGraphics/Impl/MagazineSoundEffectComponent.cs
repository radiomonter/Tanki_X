namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class MagazineSoundEffectComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private GameObject asset;

        public GameObject Asset
        {
            get => 
                this.asset;
            set => 
                this.asset = value;
        }

        public UnityEngine.AudioSource AudioSource { get; set; }
    }
}

