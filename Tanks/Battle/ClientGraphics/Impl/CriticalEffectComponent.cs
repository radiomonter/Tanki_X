namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class CriticalEffectComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private GameObject effectAsset;

        public GameObject EffectAsset
        {
            get => 
                this.effectAsset;
            set => 
                this.effectAsset = value;
        }
    }
}

