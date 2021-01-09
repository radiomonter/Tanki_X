namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class UpdateRankEffectFadeInOutSound : MonoBehaviour
    {
        public float MaxVolume = 1f;
        public float StartDelay;
        public float FadeInSpeed;
        public float FadeOutDelay;
        public float FadeOutSpeed;
        public bool FadeOutAfterCollision;
        public bool UseHideStatus;
        private AudioSource audioSource;
        private float oldVolume;
        private float currentVolume;
        private bool canStart;
        private bool canStartFadeOut;
        private bool fadeInComplited;
        private bool fadeOutComplited;
        private bool isCollisionEnter;
        private bool allComplited;
        private bool isStartDelay;
        private bool isIn;
        private bool isOut;
        private UpdateRankEffectSettings effectSettings;
        private bool isInitialized;

        private void FadeIn()
        {
            this.currentVolume = this.oldVolume + ((Time.deltaTime / this.FadeInSpeed) * this.MaxVolume);
            if (this.currentVolume >= this.MaxVolume)
            {
                this.fadeInComplited = true;
                if (!this.isOut)
                {
                    this.allComplited = true;
                }
                this.currentVolume = this.MaxVolume;
                base.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
            }
            this.audioSource.volume = this.currentVolume;
            this.oldVolume = this.currentVolume;
        }

        private void FadeOut()
        {
            this.currentVolume = this.oldVolume - ((Time.deltaTime / this.FadeOutSpeed) * this.MaxVolume);
            if (this.currentVolume <= 0f)
            {
                this.currentVolume = 0f;
                this.fadeOutComplited = true;
                this.allComplited = true;
            }
            this.audioSource.volume = this.currentVolume;
            this.oldVolume = this.currentVolume;
        }

        private void GetEffectSettingsComponent(Transform tr)
        {
            Transform parent = tr.parent;
            if (parent != null)
            {
                this.effectSettings = parent.GetComponentInChildren<UpdateRankEffectSettings>();
                if (this.effectSettings == null)
                {
                    this.GetEffectSettingsComponent(parent.transform);
                }
            }
        }

        private void InitDefaultVariables()
        {
            this.fadeInComplited = false;
            this.fadeOutComplited = false;
            this.allComplited = false;
            this.canStartFadeOut = false;
            this.isCollisionEnter = false;
            this.oldVolume = 0f;
            this.currentVolume = this.MaxVolume;
            if (this.isIn)
            {
                this.currentVolume = 0f;
            }
            this.audioSource.volume = this.currentVolume;
            if (this.isStartDelay)
            {
                base.Invoke("SetupStartDelay", this.StartDelay);
            }
            else
            {
                this.canStart = true;
            }
            if (!this.isIn)
            {
                if (!this.FadeOutAfterCollision)
                {
                    base.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
                }
                this.oldVolume = this.MaxVolume;
            }
        }

        private void InitSource()
        {
            if (!this.isInitialized)
            {
                this.audioSource = base.GetComponent<AudioSource>();
                if (this.audioSource != null)
                {
                    this.isStartDelay = this.StartDelay > 0.001f;
                    this.isIn = this.FadeInSpeed > 0.001f;
                    this.isOut = this.FadeOutSpeed > 0.001f;
                    this.InitDefaultVariables();
                    this.isInitialized = true;
                }
            }
        }

        private void OnEnable()
        {
            if (this.isInitialized)
            {
                this.InitDefaultVariables();
            }
        }

        private void prefabSettings_CollisionEnter(object sender, UpdateRankCollisionInfo e)
        {
            this.isCollisionEnter = true;
            if (!this.isIn && this.FadeOutAfterCollision)
            {
                base.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
            }
        }

        private void SetupFadeOutDelay()
        {
            this.canStartFadeOut = true;
        }

        private void SetupStartDelay()
        {
            this.canStart = true;
        }

        private void Start()
        {
            this.GetEffectSettingsComponent(base.transform);
            if (this.effectSettings != null)
            {
                this.effectSettings.CollisionEnter += new EventHandler<UpdateRankCollisionInfo>(this.prefabSettings_CollisionEnter);
            }
            this.InitSource();
        }

        private void Update()
        {
            if (this.canStart && (this.audioSource != null))
            {
                if ((this.effectSettings != null) && (this.UseHideStatus && (this.allComplited && this.effectSettings.IsVisible)))
                {
                    this.allComplited = false;
                    this.fadeInComplited = false;
                    this.fadeOutComplited = false;
                    this.InitDefaultVariables();
                }
                if (this.isIn && !this.fadeInComplited)
                {
                    if (this.effectSettings == null)
                    {
                        this.FadeIn();
                    }
                    else if ((this.UseHideStatus && this.effectSettings.IsVisible) || !this.UseHideStatus)
                    {
                        this.FadeIn();
                    }
                }
                if (this.isOut && (!this.fadeOutComplited && this.canStartFadeOut))
                {
                    if ((this.effectSettings == null) || (!this.UseHideStatus && !this.FadeOutAfterCollision))
                    {
                        this.FadeOut();
                    }
                    else if ((this.UseHideStatus && !this.effectSettings.IsVisible) || this.isCollisionEnter)
                    {
                        this.FadeOut();
                    }
                }
            }
        }
    }
}

