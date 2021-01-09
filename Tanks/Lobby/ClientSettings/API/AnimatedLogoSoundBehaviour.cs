namespace Tanks.Lobby.ClientSettings.API
{
    using System;
    using UnityEngine;

    public class AnimatedLogoSoundBehaviour : MonoBehaviour
    {
        [SerializeField]
        private AudioSource source;

        private void Start()
        {
            this.source.Play();
        }
    }
}

