namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class BonusSoundConfigComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private AudioSource bonusTakingSound;

        public AudioSource BonusTakingSound
        {
            get => 
                this.bonusTakingSound;
            set => 
                this.bonusTakingSound = value;
        }
    }
}

