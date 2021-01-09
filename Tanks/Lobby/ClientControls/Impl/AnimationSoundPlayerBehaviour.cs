namespace Tanks.Lobby.ClientControls.Impl
{
    using System;
    using UnityEngine;

    public class AnimationSoundPlayerBehaviour : MonoBehaviour
    {
        public AudioSource audio;

        public void PlaySound()
        {
            this.audio.Play();
        }
    }
}

