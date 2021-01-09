namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(AudioSource))]
    public class SoundController : MonoBehaviour
    {
        [SerializeField]
        private float playingDelaySec;
        [SerializeField]
        private float playingOffsetSec;
        [SerializeField]
        private float fadeOutTimeSec = 0.5f;
        [SerializeField]
        private float fadeInTimeSec;
        [SerializeField, Range(0f, 1f)]
        private float minVolume;
        [SerializeField, Range(0f, 1f)]
        private float maxVolume = 1f;
        [SerializeField]
        private AudioSource source;
        private float currentVolume;
        private float fadeOutSpeed;
        private float fadeInSpeed;
        private float currentFadeSpeed;
        private float playingDelayTimer;
        private SoundControllerStates state;

        private void Awake()
        {
            this.source.time = this.playingOffsetSec;
            this.fadeInSpeed = this.CalculateFadingSpeed(this.fadeInTimeSec);
            this.fadeOutSpeed = -this.CalculateFadingSpeed(this.fadeOutTimeSec);
            this.State = SoundControllerStates.INACTIVE;
        }

        private float CalculateFadingSpeed(float fadingTime) => 
            (fadingTime <= 0f) ? 0f : (1f / fadingTime);

        public void FadeIn()
        {
            this.State = SoundControllerStates.FADE_IN;
        }

        public void FadeOut()
        {
            this.State = SoundControllerStates.FADE_OUT;
        }

        private void SetActiveParams()
        {
            this.CurrentVolume = this.maxVolume;
            base.enabled = false;
            this.StartSound();
        }

        private void SetInactiveParams()
        {
            this.CurrentVolume = this.minVolume;
            base.enabled = false;
            this.StopSound();
        }

        public void SetSoundActive()
        {
            this.State = SoundControllerStates.ACTIVE;
        }

        private void StartFadingPhase()
        {
            float fadeInTimeSec;
            float fadeInSpeed;
            SoundControllerStates aCTIVE;
            bool flag;
            SoundControllerStates state = this.state;
            if (state == SoundControllerStates.FADE_IN)
            {
                fadeInTimeSec = this.fadeInTimeSec;
                fadeInSpeed = this.fadeInSpeed;
                aCTIVE = SoundControllerStates.ACTIVE;
                flag = this.currentVolume >= this.maxVolume;
                this.StartSound();
            }
            else
            {
                if (state != SoundControllerStates.FADE_OUT)
                {
                    throw new ArgumentException("Fading phase doesn't exist");
                }
                fadeInTimeSec = this.fadeOutTimeSec;
                fadeInSpeed = this.fadeOutSpeed;
                aCTIVE = SoundControllerStates.INACTIVE;
                flag = this.currentVolume <= this.minVolume;
            }
            if (flag | (fadeInTimeSec == 0f))
            {
                this.State = aCTIVE;
            }
            else
            {
                this.currentFadeSpeed = fadeInSpeed;
                base.enabled = true;
            }
        }

        private void StartSound()
        {
            if (!this.source.isPlaying)
            {
                this.source.time = this.playingOffsetSec;
                this.source.PlayScheduled(AudioSettings.dspTime + this.PlayingDelaySec);
                this.playingDelayTimer = this.PlayingDelaySec;
                base.enabled = true;
            }
        }

        public void StopImmediately()
        {
            this.State = SoundControllerStates.INACTIVE;
        }

        private void StopSound()
        {
            this.source.Stop();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            if (this.playingDelayTimer > 0f)
            {
                this.playingDelayTimer -= deltaTime;
                if (this.playingDelayTimer > 0f)
                {
                    return;
                }
            }
            if (this.State == SoundControllerStates.INACTIVE)
            {
                base.enabled = false;
            }
            else if (this.State == SoundControllerStates.ACTIVE)
            {
                base.enabled = false;
            }
            else
            {
                this.CurrentVolume += this.currentFadeSpeed * deltaTime;
                if (this.CurrentVolume <= this.minVolume)
                {
                    this.State = SoundControllerStates.INACTIVE;
                }
                else if (this.CurrentVolume >= this.maxVolume)
                {
                    this.State = SoundControllerStates.ACTIVE;
                }
            }
        }

        private SoundControllerStates State
        {
            get => 
                this.state;
            set
            {
                this.playingDelayTimer = 0f;
                SoundControllerStates state = this.state;
                bool flag = state != value;
                this.state = value;
                switch (value)
                {
                    case SoundControllerStates.ACTIVE:
                        this.SetActiveParams();
                        break;

                    case SoundControllerStates.INACTIVE:
                        this.SetInactiveParams();
                        break;

                    case SoundControllerStates.FADE_IN:
                        if (flag)
                        {
                            if (state != SoundControllerStates.ACTIVE)
                            {
                                this.StartFadingPhase();
                            }
                            else
                            {
                                this.State = SoundControllerStates.ACTIVE;
                            }
                        }
                        break;

                    case SoundControllerStates.FADE_OUT:
                        if (flag)
                        {
                            if (state != SoundControllerStates.INACTIVE)
                            {
                                this.StartFadingPhase();
                            }
                            else
                            {
                                this.State = SoundControllerStates.INACTIVE;
                            }
                        }
                        break;

                    default:
                        throw new ArgumentException("Invalid sound Controller state");
                }
            }
        }

        public AudioSource Source =>
            this.source;

        private float CurrentVolume
        {
            get => 
                this.currentVolume;
            set
            {
                this.currentVolume = value;
                this.currentVolume = Mathf.Clamp(value, this.minVolume, this.maxVolume);
                this.source.volume = this.currentVolume;
            }
        }

        public float FadeInTimeSec
        {
            get => 
                this.fadeInTimeSec;
            set
            {
                this.fadeInTimeSec = value;
                this.fadeInSpeed = this.CalculateFadingSpeed(this.fadeInTimeSec);
            }
        }

        public float FadeOutTimeSec
        {
            get => 
                this.fadeOutTimeSec;
            set
            {
                this.fadeOutTimeSec = value;
                this.fadeOutSpeed = -this.CalculateFadingSpeed(this.fadeOutTimeSec);
            }
        }

        public float PlayingDelaySec
        {
            get => 
                this.playingDelaySec;
            set => 
                this.playingDelaySec = value;
        }

        public float PlayingOffsetSec
        {
            get => 
                this.playingOffsetSec;
            set => 
                this.playingOffsetSec = value;
        }

        public float MinVolume
        {
            get => 
                this.minVolume;
            set
            {
                this.minVolume = value;
                this.source.volume = Mathf.Clamp(this.currentVolume, this.minVolume, this.maxVolume);
            }
        }

        public float MaxVolume
        {
            get => 
                this.maxVolume;
            set
            {
                this.maxVolume = value;
                this.source.volume = Mathf.Clamp(this.currentVolume, this.minVolume, this.maxVolume);
            }
        }

        private enum SoundControllerStates
        {
            INITIAL,
            ACTIVE,
            INACTIVE,
            FADE_IN,
            FADE_OUT
        }
    }
}

