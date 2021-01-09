namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class RPMVolumeUpdaterFinishBehaviour : MonoBehaviour
    {
        private const float SOUND_PAUSE_LATENCY_SEC = 2f;
        [SerializeField]
        private AudioSource source;
        [SerializeField]
        private float soundPauseTimer;

        private void Awake()
        {
            base.enabled = false;
        }

        public void Build(AudioSource source)
        {
            this.source = source;
        }

        private void OnEnable()
        {
            this.soundPauseTimer = 2f;
        }

        private void Update()
        {
            this.soundPauseTimer -= Time.deltaTime;
            if (this.soundPauseTimer <= 0f)
            {
                this.source.Pause();
                base.enabled = false;
            }
        }
    }
}

