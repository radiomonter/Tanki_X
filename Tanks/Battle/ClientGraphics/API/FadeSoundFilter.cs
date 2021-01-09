namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public abstract class FadeSoundFilter : MonoBehaviour
    {
        private const float MIN_FADE_TIME_SEC = 0.01f;
        [SerializeField]
        protected AudioSource source;
        [SerializeField]
        private float fadeInTimeSec = 1f;
        [SerializeField]
        private float fadeOutTimeSec = 1f;
        private volatile float fadeInSpeed;
        private volatile float fadeOutSpeed;
        private volatile float fadeSpeed;
        private double prevAudioTime;
        private float maxVolume;
        private volatile bool needToKill;
        private volatile bool needToDisable;
        private volatile bool isFading;
        private volatile bool firstAudioFilterIteration;

        protected FadeSoundFilter()
        {
        }

        private void ApplySourceVolume()
        {
            this.source.volume = this.FilterVolume * this.maxVolume;
        }

        protected virtual void Awake()
        {
            this.fadeInSpeed = 1f / this.fadeInTimeSec;
            this.fadeOutSpeed = -1f / this.fadeOutTimeSec;
            this.maxVolume = this.source.volume;
            this.ResetFilter();
        }

        protected abstract bool CheckSoundIsPlaying();
        public void Play(float delay = -1f)
        {
            float num = 0f;
            if (this.fadeInTimeSec > 0.01f)
            {
                this.StartFilter(this.fadeInSpeed);
            }
            else
            {
                num = 1f;
                this.ResetFilter();
            }
            if (!this.source.isPlaying)
            {
                this.FilterVolume = num;
                this.ApplySourceVolume();
            }
            this.PlaySound(delay);
        }

        private void PlaySound(float delay)
        {
            if (!this.source.isPlaying)
            {
                if (delay <= 0f)
                {
                    this.source.Play();
                }
                else
                {
                    this.source.PlayScheduled(AudioSettings.dspTime + delay);
                }
            }
        }

        protected void ResetFilter()
        {
            base.enabled = false;
            this.needToKill = false;
            this.needToDisable = false;
            this.isFading = false;
            this.firstAudioFilterIteration = false;
        }

        private void StartFilter(float speed)
        {
            this.fadeSpeed = speed;
            this.firstAudioFilterIteration = true;
            this.fadeSpeed = speed;
            this.needToKill = false;
            this.needToDisable = false;
            this.isFading = true;
            base.enabled = true;
        }

        public void Stop()
        {
            if (!this.CheckSoundIsPlaying())
            {
                this.StopAndDestroy();
            }
            else if (this.fadeOutTimeSec > 0.01f)
            {
                this.StartFilter(this.fadeOutSpeed);
            }
            else
            {
                this.StopAndDestroy();
            }
        }

        protected abstract void StopAndDestroy();
        private void Update()
        {
            this.UpdateSoundWithinMainThread();
            this.ApplySourceVolume();
            if (this.needToKill)
            {
                this.StopAndDestroy();
            }
            else if (this.needToDisable)
            {
                this.ResetFilter();
            }
        }

        private void UpdateSoundWithinMainThread()
        {
            float fadeSpeed = this.fadeSpeed;
            float num4 = this.FilterVolume + (fadeSpeed * Time.deltaTime);
            float num5 = Mathf.Clamp(num4, 0f, 1f);
            if (!this.isFading)
            {
                this.FilterVolume = num5;
            }
            else
            {
                if ((num4 <= 0f) && (fadeSpeed < 0f))
                {
                    this.needToKill = true;
                }
                if ((num4 >= 1f) && (fadeSpeed > 0f))
                {
                    this.needToDisable = true;
                }
                this.FilterVolume = num5;
            }
        }

        public AudioSource Source =>
            this.source;

        protected abstract float FilterVolume { get; set; }
    }
}

