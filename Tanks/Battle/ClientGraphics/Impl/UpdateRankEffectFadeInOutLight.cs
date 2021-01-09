namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class UpdateRankEffectFadeInOutLight : MonoBehaviour
    {
        public float StartDelay;
        public float FadeInSpeed;
        public float FadeOutDelay;
        public float FadeOutSpeed;
        public bool FadeOutAfterCollision;
        public bool UseHideStatus;
        private Light goLight;
        private float oldIntensity;
        private float currentIntensity;
        private float startIntensity;
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
            this.currentIntensity = this.oldIntensity + ((Time.deltaTime / this.FadeInSpeed) * this.startIntensity);
            if (this.currentIntensity >= this.startIntensity)
            {
                this.fadeInComplited = true;
                if (!this.isOut)
                {
                    this.allComplited = true;
                }
                this.currentIntensity = this.startIntensity;
                base.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
            }
            this.goLight.intensity = this.currentIntensity;
            this.oldIntensity = this.currentIntensity;
        }

        private void FadeOut()
        {
            this.currentIntensity = this.oldIntensity - ((Time.deltaTime / this.FadeOutSpeed) * this.startIntensity);
            if (this.currentIntensity <= 0f)
            {
                this.currentIntensity = 0f;
                this.fadeOutComplited = true;
                this.allComplited = true;
            }
            this.goLight.intensity = this.currentIntensity;
            this.oldIntensity = this.currentIntensity;
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
            this.oldIntensity = 0f;
            this.currentIntensity = 0f;
            this.canStart = false;
            this.goLight.intensity = !this.isIn ? this.startIntensity : 0f;
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
                this.oldIntensity = this.startIntensity;
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
            this.goLight = base.GetComponent<Light>();
            this.startIntensity = this.goLight.intensity;
            this.isStartDelay = this.StartDelay > 0.001f;
            this.isIn = this.FadeInSpeed > 0.001f;
            this.isOut = this.FadeOutSpeed > 0.001f;
            this.InitDefaultVariables();
            this.isInitialized = true;
        }

        private void Update()
        {
            if (this.canStart)
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
                    else if ((this.UseHideStatus && !this.effectSettings.IsVisible) || (this.FadeOutAfterCollision && this.isCollisionEnter))
                    {
                        this.FadeOut();
                    }
                }
            }
        }
    }
}

