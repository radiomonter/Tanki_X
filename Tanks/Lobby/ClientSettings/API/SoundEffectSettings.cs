namespace Tanks.Lobby.ClientSettings.API
{
    using UnityEngine;

    [RequireComponent(typeof(AudioSource))]
    public class SoundEffectSettings : MonoBehaviour
    {
        [SerializeField]
        private AudioSource source;

        public AudioSource Source =>
            this.source;
    }
}

