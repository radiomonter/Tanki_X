namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public abstract class AbstractIsisSoundEffectComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private GameObject asset;

        protected AbstractIsisSoundEffectComponent()
        {
        }

        public GameObject Asset
        {
            get => 
                this.asset;
            set => 
                this.asset = value;
        }

        public Tanks.Battle.ClientGraphics.Impl.SoundController SoundController { get; set; }
    }
}

