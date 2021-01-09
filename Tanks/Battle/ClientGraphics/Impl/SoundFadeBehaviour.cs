namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class SoundFadeBehaviour : MonoBehaviour
    {
        [SerializeField]
        private AudioSource source;
        [SerializeField]
        private float fadeOutTime = 1.5f;
        private float fadeSpeed;

        private void Awake()
        {
            this.fadeSpeed = 1f / this.fadeOutTime;
        }

        private void Update()
        {
            this.source.volume -= this.fadeSpeed * Time.deltaTime;
        }
    }
}

