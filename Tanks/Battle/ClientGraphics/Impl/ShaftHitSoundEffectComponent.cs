namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public abstract class ShaftHitSoundEffectComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private GameObject asset;
        [SerializeField]
        private float duration;

        protected ShaftHitSoundEffectComponent()
        {
        }

        public GameObject Asset
        {
            get => 
                this.asset;
            set => 
                this.asset = value;
        }

        public float Duration
        {
            get => 
                this.duration;
            set => 
                this.duration = value;
        }
    }
}

