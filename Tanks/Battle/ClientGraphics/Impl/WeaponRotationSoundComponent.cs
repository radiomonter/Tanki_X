namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [SerialVersionUID(0x8d2e6e120aaca9aL)]
    public class WeaponRotationSoundComponent : MonoBehaviour, Component
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

        public AudioSource StartAudioSource { get; set; }

        public AudioSource LoopAudioSource { get; set; }

        public AudioSource StopAudioSource { get; set; }

        public bool IsActive { get; set; }
    }
}

