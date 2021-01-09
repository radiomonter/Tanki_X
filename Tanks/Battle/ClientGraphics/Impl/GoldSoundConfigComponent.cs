namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class GoldSoundConfigComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private AudioSource goldNotificationSound;

        public AudioSource GoldNotificationSound
        {
            get => 
                this.goldNotificationSound;
            set => 
                this.goldNotificationSound = value;
        }
    }
}

