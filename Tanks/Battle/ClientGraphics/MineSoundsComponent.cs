namespace Tanks.Battle.ClientGraphics
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class MineSoundsComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private AudioSource dropGroundSound;
        [SerializeField]
        private AudioSource dropNonGroundSound;
        [SerializeField]
        private AudioSource deactivationSound;
        [SerializeField]
        private AudioSource explosionSound;

        public AudioSource DropGroundSound
        {
            get => 
                this.dropGroundSound;
            set => 
                this.dropGroundSound = value;
        }

        public AudioSource DropNonGroundSound
        {
            get => 
                this.dropNonGroundSound;
            set => 
                this.dropNonGroundSound = value;
        }

        public AudioSource DeactivationSound
        {
            get => 
                this.deactivationSound;
            set => 
                this.deactivationSound = value;
        }

        public AudioSource ExplosionSound
        {
            get => 
                this.explosionSound;
            set => 
                this.explosionSound = value;
        }
    }
}

