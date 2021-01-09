namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(AudioSource))]
    public class VulcanFadeSoundBehaviour : MonoBehaviour
    {
        private AudioSource source;
        private float fadeSpeed;
        public float fadeDuration;
        public float maxVolume;

        private void FixedUpdate()
        {
            this.source.volume += this.fadeSpeed * Time.fixedDeltaTime;
            if (!this.source.isPlaying)
            {
                base.enabled = false;
            }
        }

        private void OnEnable()
        {
            this.source = base.gameObject.GetComponent<AudioSource>();
            this.source.volume = this.maxVolume;
            this.fadeSpeed = -this.maxVolume / this.fadeDuration;
        }
    }
}

