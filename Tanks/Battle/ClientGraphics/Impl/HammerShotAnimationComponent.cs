namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [RequireComponent(typeof(Animator))]
    public class HammerShotAnimationComponent : AnimationTriggerComponent
    {
        [SerializeField]
        private string shotTriggerName = "shot";
        [SerializeField]
        private string isReloadingName = "isReloading";
        [SerializeField]
        private string resetTriggerName = "reset";
        [SerializeField]
        private string reloadingSpeedName = "reloadSpeedCoeff";
        [SerializeField]
        private string cooldownSpeedName = "cooldownSpeedCoeff";
        [SerializeField]
        private float idleTimeAfterCooldown = 0.5f;
        [SerializeField]
        private AnimationClip reloadClip;
        [SerializeField]
        private AnimationClip shotClip;
        private int shotTriggerID;
        private int isReloadingID;
        private int resetTriggerID;
        private int reloadingSpeedID;
        private int cooldownSpeedID;
        private Animator hammerAnimator;

        public void InitHammerShotAnimation(Entity entity, Animator animator, float reloadingTimeSec, float shotCooldownLogicTime)
        {
            this.shotTriggerID = Animator.StringToHash(this.shotTriggerName);
            this.isReloadingID = Animator.StringToHash(this.isReloadingName);
            this.resetTriggerID = Animator.StringToHash(this.resetTriggerName);
            this.reloadingSpeedID = Animator.StringToHash(this.reloadingSpeedName);
            this.cooldownSpeedID = Animator.StringToHash(this.cooldownSpeedName);
            float length = this.reloadClip.length;
            float num3 = shotCooldownLogicTime - this.idleTimeAfterCooldown;
            float num4 = reloadingTimeSec - num3;
            this.RequiredReloadingTime = num4;
            float num5 = length / num4;
            float num6 = this.shotClip.length / num3;
            animator.SetFloat(this.reloadingSpeedID, num5);
            animator.SetFloat(this.cooldownSpeedID, num6);
            this.hammerAnimator = animator;
            base.Entity = entity;
            base.enabled = true;
        }

        private void OnBlowOff()
        {
            base.ProvideEvent<HammerBlowOffEvent>();
        }

        private void OnBounce()
        {
            base.ProvideEvent<HammerBounceEvent>();
        }

        private void OnCartridgeClick()
        {
            base.ProvideEvent<HammerCartridgeClickEvent>();
        }

        private void OnChargeLastCartridge()
        {
            base.ProvideEvent<HammerChargeLastCartridgeEvent>();
        }

        private void OnCooldown()
        {
            base.ProvideEvent<HammerCooldownEvent>();
        }

        private void OnMagazineShot()
        {
            base.ProvideEvent<HammerMagazineShotEvent>();
        }

        private void OnOffset()
        {
            base.ProvideEvent<HammerOffsetEvent>();
        }

        private void OnRoll()
        {
            base.ProvideEvent<HammerRollEvent>();
        }

        private void Play(bool needReload)
        {
            this.hammerAnimator.ResetTrigger(this.resetTriggerID);
            this.hammerAnimator.SetTrigger(this.shotTriggerID);
            this.hammerAnimator.SetBool(this.isReloadingID, needReload);
        }

        public void PlayShot()
        {
            this.Play(false);
        }

        public void PlayShotAndReload()
        {
            this.Play(true);
        }

        public void Reset()
        {
            this.hammerAnimator.ResetTrigger(this.shotTriggerID);
            this.hammerAnimator.SetTrigger(this.resetTriggerID);
        }

        public AnimationClip ReloadClip
        {
            get => 
                this.reloadClip;
            set => 
                this.reloadClip = value;
        }

        public float RequiredReloadingTime { get; set; }
    }
}

