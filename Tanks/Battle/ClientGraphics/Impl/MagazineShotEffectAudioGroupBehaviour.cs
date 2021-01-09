namespace Tanks.Battle.ClientGraphics.Impl
{
    using UnityEngine;
    using UnityEngine.Audio;

    public class MagazineShotEffectAudioGroupBehaviour : MonoBehaviour
    {
        [SerializeField]
        private AudioMixerGroup selfShotGroup;
        [SerializeField]
        private AudioMixerGroup remoteShotGroup;

        public AudioMixerGroup SelfShotGroup =>
            this.selfShotGroup;

        public AudioMixerGroup RemoteShotGroup =>
            this.remoteShotGroup;
    }
}

