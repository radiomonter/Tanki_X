namespace Tanks.Lobby.ClientSettings.API
{
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;
    using UnityEngine.Audio;

    public class SoundListenerResourcesBehaviour : MonoBehaviour
    {
        [SerializeField]
        private AudioMixer sfxMixer;
        [SerializeField]
        private AudioMixer musicMixer;
        [SerializeField]
        private AudioMixer uiMixer;
        [SerializeField]
        private AudioMixerSnapshot[] sfxMixerSnapshots;
        [SerializeField]
        private AudioMixerSnapshot[] musicMixerSnapshots;
        [SerializeField]
        private AudioSource sfxSourcePreview;
        [SerializeField]
        private GameObject moduleActivation;
        [SerializeField]
        private GameObject moduleUpgrade;
        [SerializeField]
        private DailyBonusSoundsBehaviour dailyBonusSounds;

        public AudioMixer SfxMixer =>
            this.sfxMixer;

        public AudioMixer MusicMixer =>
            this.musicMixer;

        public AudioMixer UIMixer =>
            this.uiMixer;

        public AudioMixerSnapshot[] SfxMixerSnapshots =>
            this.sfxMixerSnapshots;

        public AudioSource SfxSourcePreview =>
            this.sfxSourcePreview;

        public AudioMixerSnapshot[] MusicMixerSnapshots =>
            this.musicMixerSnapshots;

        public GameObject ModuleActivation =>
            this.moduleActivation;

        public GameObject ModuleUpgrade =>
            this.moduleUpgrade;

        public DailyBonusSoundsBehaviour DailyBonusSounds =>
            this.dailyBonusSounds;
    }
}

