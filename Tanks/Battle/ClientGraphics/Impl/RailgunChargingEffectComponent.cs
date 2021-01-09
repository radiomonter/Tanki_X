namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class RailgunChargingEffectComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private GameObject prefab;

        public GameObject Prefab
        {
            get => 
                this.prefab;
            set => 
                this.prefab = value;
        }
    }
}

