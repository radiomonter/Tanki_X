namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class TankEngineSoundEffectComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private GameObject enginePrefab;

        public GameObject EnginePrefab
        {
            get => 
                this.enginePrefab;
            set => 
                this.enginePrefab = value;
        }

        public HullSoundEngineController SoundEngineController { get; set; }
    }
}

