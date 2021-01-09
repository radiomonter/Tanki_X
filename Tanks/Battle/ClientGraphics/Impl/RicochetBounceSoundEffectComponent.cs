namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class RicochetBounceSoundEffectComponent : BaseRicochetSoundEffectComponent
    {
        private const float PLAY_INTERVAL_TIME = 0.5f;
        [SerializeField]
        private AudioClip[] avaibleClips;
        private int clipIndex;
        private int clipsCount;
        private float playInterval;

        private void Awake()
        {
            this.playInterval = -1f;
            base.enabled = false;
            this.clipIndex = 0;
            this.clipsCount = this.avaibleClips.Length;
        }

        public override void Play(AudioSource sourceInstance)
        {
            AudioClip clip = this.avaibleClips[this.clipIndex];
            sourceInstance.clip = clip;
            this.clipIndex++;
            if (this.clipIndex == this.clipsCount)
            {
                this.clipIndex = 0;
            }
            sourceInstance.Play();
        }

        public override void PlayEffect(Vector3 position)
        {
            if (this.playInterval <= 0f)
            {
                base.PlayEffect(position);
                this.playInterval = 0.5f;
                base.enabled = true;
            }
        }

        private void Update()
        {
            this.playInterval -= Time.deltaTime;
            if (this.playInterval <= 0f)
            {
                this.playInterval = -1f;
                base.enabled = false;
            }
        }
    }
}

