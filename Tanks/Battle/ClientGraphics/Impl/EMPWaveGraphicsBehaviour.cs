namespace Tanks.Battle.ClientGraphics.Impl
{
    using UnityEngine;

    [RequireComponent(typeof(ParticleSystem))]
    public class EMPWaveGraphicsBehaviour : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem waveParticleSystem;
        [SerializeField]
        private AudioSource waveSound;

        public ParticleSystem WaveParticleSystem =>
            this.waveParticleSystem;

        public AudioSource WaveSound =>
            this.waveSound;
    }
}

