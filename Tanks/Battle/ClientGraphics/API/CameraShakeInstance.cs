namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using UnityEngine;

    public class CameraShakeInstance
    {
        public float magnitude;
        public float roughness;
        public Vector3 positionInfluence;
        public Vector3 rotationInfluence;
        public bool deleteOnInactive = true;
        private float roughMod = 1f;
        private float magnMod = 1f;
        private float fadeOutDuration;
        private float fadeInDuration;
        private bool sustain;
        private float currentFadeTime;
        private float tick;
        private Vector3 amt;

        public CameraShakeInstance()
        {
            this.ResetFields();
        }

        public CameraShakeInstance Init(float magnitude, float roughness)
        {
            this.ResetFields();
            this.magnitude = magnitude;
            this.roughness = roughness;
            this.sustain = true;
            this.tick = Random.Range(-100, 100);
            return this;
        }

        public CameraShakeInstance Init(float magnitude, float roughness, float fadeInTime, float fadeOutTime)
        {
            this.ResetFields();
            this.magnitude = magnitude;
            this.fadeOutDuration = fadeOutTime;
            this.fadeInDuration = fadeInTime;
            this.roughness = roughness;
            if (fadeInTime > 0f)
            {
                this.sustain = true;
                this.currentFadeTime = 0f;
            }
            else
            {
                this.sustain = false;
                this.currentFadeTime = 1f;
            }
            this.tick = Random.Range(-100, 100);
            return this;
        }

        private void ResetFields()
        {
            this.magnitude = 0f;
            this.roughness = 0f;
            this.positionInfluence = Vector3.zero;
            this.rotationInfluence = Vector3.zero;
            this.deleteOnInactive = true;
            this.roughMod = 1f;
            this.magnMod = 1f;
            this.fadeInDuration = 0f;
            this.fadeOutDuration = 0f;
            this.sustain = false;
            this.currentFadeTime = 0f;
            this.tick = 0f;
            this.amt = Vector3.zero;
        }

        public void StartFadeIn(float fadeInTime)
        {
            if (fadeInTime == 0f)
            {
                this.currentFadeTime = 1f;
            }
            this.fadeInDuration = fadeInTime;
            this.fadeOutDuration = 0f;
            this.sustain = true;
        }

        public void StartFadeOut(float fadeOutTime)
        {
            if (fadeOutTime == 0f)
            {
                this.currentFadeTime = 0f;
            }
            this.fadeOutDuration = fadeOutTime;
            this.fadeInDuration = 0f;
            this.sustain = false;
        }

        public Vector3 UpdateShake()
        {
            this.amt.x = Mathf.PerlinNoise(this.tick, 0f) - 0.5f;
            this.amt.y = Mathf.PerlinNoise(0f, this.tick) - 0.5f;
            this.amt.z = Mathf.PerlinNoise(this.tick, this.tick) - 0.5f;
            if ((this.fadeInDuration > 0f) && this.sustain)
            {
                if (this.currentFadeTime < 1f)
                {
                    this.currentFadeTime += Time.deltaTime / this.fadeInDuration;
                }
                else if (this.fadeOutDuration > 0f)
                {
                    this.sustain = false;
                }
            }
            if (!this.sustain)
            {
                this.currentFadeTime -= Time.deltaTime / this.fadeOutDuration;
            }
            this.tick = !this.sustain ? (this.tick + (((Time.deltaTime * this.roughness) * this.roughMod) * this.currentFadeTime)) : (this.tick + ((Time.deltaTime * this.roughness) * this.roughMod));
            return (((this.amt * this.magnitude) * this.magnMod) * this.currentFadeTime);
        }

        public float ScaleRoughness
        {
            get => 
                this.roughMod;
            set => 
                this.roughMod = value;
        }

        public float ScaleMagnitude
        {
            get => 
                this.magnMod;
            set => 
                this.magnMod = value;
        }

        public float NormalizedFadeTime =>
            this.currentFadeTime;

        private bool IsShaking =>
            (this.currentFadeTime > 0f) || this.sustain;

        private bool IsFadingOut =>
            !this.sustain && (this.currentFadeTime > 0f);

        private bool IsFadingIn =>
            ((this.currentFadeTime < 1f) && this.sustain) && (this.fadeInDuration > 0f);

        public CameraShakeState CurrentState =>
            !this.IsFadingIn ? (!this.IsFadingOut ? (!this.IsShaking ? CameraShakeState.Inactive : CameraShakeState.Sustained) : CameraShakeState.FadingOut) : CameraShakeState.FadingIn;
    }
}

