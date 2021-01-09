namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class TankFrictionSoundEffectComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private float minValuableFrictionPower;
        [SerializeField]
        private float maxValuableFrictionPower = 1f;
        [SerializeField]
        private SoundController metallFrictionSourcePrefab;
        [SerializeField]
        private SoundController stoneFrictionSourcePrefab;
        [SerializeField]
        private SoundController frictionContactSourcePrefab;

        public SoundController MetallFrictionSourcePrefab
        {
            get => 
                this.metallFrictionSourcePrefab;
            set => 
                this.metallFrictionSourcePrefab = value;
        }

        public SoundController StoneFrictionSourcePrefab
        {
            get => 
                this.stoneFrictionSourcePrefab;
            set => 
                this.stoneFrictionSourcePrefab = value;
        }

        public SoundController FrictionContactSourcePrefab
        {
            get => 
                this.frictionContactSourcePrefab;
            set => 
                this.frictionContactSourcePrefab = value;
        }

        public float MinValuableFrictionPower
        {
            get => 
                this.minValuableFrictionPower;
            set => 
                this.minValuableFrictionPower = value;
        }

        public float MaxValuableFrictionPower
        {
            get => 
                this.maxValuableFrictionPower;
            set => 
                this.maxValuableFrictionPower = value;
        }

        public SoundController MetallFrictionSourceInstance { get; set; }

        public SoundController StoneFrictionSourceInstance { get; set; }

        public SoundController FrictionContactSourceInstance { get; set; }
    }
}

