namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using UnityEngine;

    public class DailyBonusSoundsBehaviour : MonoBehaviour
    {
        [SerializeField]
        private AudioSource upgrade;
        [SerializeField]
        private AudioSource click;
        [SerializeField]
        private AudioSource hover;
        [SerializeField]
        private AudioSource take;

        private void Play(AudioSource source)
        {
            source.Stop();
            source.Play();
        }

        public void PlayClick()
        {
            this.Play(this.click);
        }

        public void PlayHover()
        {
            this.Play(this.hover);
        }

        public void PlayTake()
        {
            this.Play(this.take);
        }

        public void PlayUpgrade()
        {
            this.Play(this.upgrade);
        }
    }
}

