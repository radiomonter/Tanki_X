namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    [RequireComponent(typeof(Animator))]
    public class NitroEffectComponent : MonoBehaviour, Component
    {
        private bool prepared;
        public Animator animator;
        public Renderer renderer;
        public LightContainer lightContainer;
        public GameObject effectPrefab;
        public Transform effectPoints;
        public SpeedSoundEffectComponent soundEffect;
        public int burningTimeInMs = 500;
        private SupplyAnimationPlayer animationPlayer;
        private ParticleSystem[] effectInstances;
        private SmoothHeater smoothHeater;
        private int effectInstancesCount;

        private void Awake()
        {
            base.enabled = false;
        }

        public void InitEffect(SupplyEffectSettingsComponent settings)
        {
            this.animationPlayer = new SupplyAnimationPlayer(this.animator, AnimationParameters.SPEED_ACTIVE);
            this.effectInstances = SupplyEffectUtil.InstantiateEffect(this.effectPrefab, this.effectPoints);
            this.soundEffect.Init(base.transform);
            Material materialForNitroDetails = TankMaterialsUtil.GetMaterialForNitroDetails(this.renderer);
            this.smoothHeater = !settings.LightIsEnabled ? new SmoothHeater(this.burningTimeInMs, materialForNitroDetails, this) : new SmoothHeaterLighting(this.burningTimeInMs, materialForNitroDetails, this, this.lightContainer);
            this.effectInstancesCount = this.effectInstances.Length;
            this.prepared = true;
        }

        private void OnSpeedStart()
        {
            this.soundEffect.BeginEffect();
        }

        private void OnSpeedStarted()
        {
            this.smoothHeater.Heat();
            for (int i = 0; i < this.effectInstancesCount; i++)
            {
                this.effectInstances[i].Play(true);
            }
        }

        private void OnSpeedStop()
        {
            this.smoothHeater.Cool();
            for (int i = 0; i < this.effectInstancesCount; i++)
            {
                this.effectInstances[i].Stop(true);
            }
            this.soundEffect.StopEffect();
        }

        public void Play()
        {
            this.animationPlayer.StartAnimation();
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

