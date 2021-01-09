namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    [RequireComponent(typeof(Animator))]
    public class DoubleDamageEffectComponent : MonoBehaviour, Component
    {
        private bool prepared;
        public Animator animator;
        public LightContainer light;
        public Renderer renderer;
        public DoubleDamageSoundEffectComponent soundEffect;
        public Color emissionColor;
        public int burningTimeInMs = 500;
        private SupplyAnimationPlayer animationPlayer;
        private Material ddDetailsMaterial;
        private Material mainMaterial;
        [HideInInspector]
        public Color usualEmissionColor;
        private SmoothHeater smoothHeater;

        private void Awake()
        {
            base.enabled = false;
        }

        public void InitEffect(SupplyEffectSettingsComponent settings)
        {
            this.animationPlayer = new SupplyAnimationPlayer(this.animator, AnimationParameters.DAMAGE_ACTIVE);
            this.mainMaterial = TankMaterialsUtil.GetMainMaterial(this.renderer);
            this.ddDetailsMaterial = TankMaterialsUtil.GetMaterialForDoubleDamageDetails(this.renderer);
            this.soundEffect.Init(base.transform);
            this.usualEmissionColor = this.mainMaterial.GetColor("_EmissionColor");
            this.smoothHeater = !settings.LightIsEnabled ? new SmoothHeater(this.burningTimeInMs, this.ddDetailsMaterial, this) : new SmoothHeaterLighting(this.burningTimeInMs, this.ddDetailsMaterial, this, this.light);
            this.prepared = true;
        }

        private void OnDamageStart()
        {
            this.soundEffect.BeginEffect();
            this.mainMaterial.SetColor("_EmissionColor", this.emissionColor);
        }

        private void OnDamageStarted()
        {
            this.smoothHeater.Heat();
        }

        private void OnDamageStop()
        {
            this.smoothHeater.Cool();
            this.soundEffect.StopEffect();
            this.mainMaterial.SetColor("_EmissionColor", this.usualEmissionColor);
        }

        public void Play()
        {
            this.animationPlayer.StartAnimation();
        }

        public void Reset()
        {
            this.mainMaterial.SetColor("_EmissionColor", this.usualEmissionColor);
        }

        public void Stop()
        {
            this.animationPlayer.StopAnimation();
        }

        private void Update()
        {
            this.smoothHeater.Update();
        }

        public bool Prepared =>
            this.prepared;
    }
}

