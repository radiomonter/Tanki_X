namespace tanks.ClientResources.Content.Maps.AcidLakeHalloween.CustomScripts
{
    using System;
    using UnityEngine;

    public class SoundOnHit : MonoBehaviour
    {
        public AudioSource Sound;
        public bool OnTrigger;

        private void OnCollisionEnter(Collision other)
        {
            if (!this.OnTrigger && !this.Sound.isPlaying)
            {
                this.Sound.Play();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (this.OnTrigger && !this.Sound.isPlaying)
            {
                this.Sound.Play();
            }
        }
    }
}

