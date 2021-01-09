namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ShaftShotAnimationComponent : AbstractShotAnimationComponent
    {
        [SerializeField]
        private string shotTriggerName = "shot";
        [SerializeField]
        private string stopTriggerName = "stop";
        [SerializeField]
        private string reloadSpeedName = "reloadCoeff";
        [SerializeField]
        private AnimationClip reloadShaftClip;
        [SerializeField]
        private AnimationClip shaftShotClip;
        [SerializeField]
        private float minReloadingSpeedCoeff = 0.5f;
        private int shotTriggerID;
        private int stopTriggerID;
        private int reloadSpeedID;
        private float decelerationCoeff;
        private Animator shaftAnimator;
        private float maxReloadingSpeedCoeff;
        private float reloadSpeedCoeff;

        public void Init(Animator animator, float cooldownTimeSec, float eShot, float energyReloadingSpeed, float unloadEnergyPerAimingShot)
        {
            this.stopTriggerID = Animator.StringToHash(this.stopTriggerName);
            this.shotTriggerID = Animator.StringToHash(this.shotTriggerName);
            this.reloadSpeedID = Animator.StringToHash(this.reloadSpeedName);
            float num3 = Mathf.Clamp01(Mathf.Clamp01(1f - unloadEnergyPerAimingShot) + (energyReloadingSpeed * cooldownTimeSec));
            float num5 = Mathf.Clamp01(eShot - num3) / energyReloadingSpeed;
            float num7 = Mathf.Min(cooldownTimeSec + num5, base.CalculateOptimalAnimationTime(energyReloadingSpeed, cooldownTimeSec, eShot));
            this.CooldownAnimationTime = num7 - this.shaftShotClip.length;
            float length = this.reloadShaftClip.length;
            this.maxReloadingSpeedCoeff = ((2f * length) / this.CooldownAnimationTime) - this.minReloadingSpeedCoeff;
            this.decelerationCoeff = (this.maxReloadingSpeedCoeff - this.minReloadingSpeedCoeff) / this.CooldownAnimationTime;
            this.shaftAnimator = animator;
        }

        public void PlayShot()
        {
            this.ReloadSpeedCoeff = this.maxReloadingSpeedCoeff;
            this.shaftAnimator.SetTrigger(this.shotTriggerID);
        }

        public void Stop()
        {
            this.shaftAnimator.SetTrigger(this.stopTriggerID);
        }

        public void UpdateShotCooldownAnimation(float dt)
        {
            this.ReloadSpeedCoeff -= this.decelerationCoeff * dt;
        }

        private float ReloadSpeedCoeff
        {
            get => 
                this.reloadSpeedCoeff;
            set
            {
                this.reloadSpeedCoeff = value;
                this.shaftAnimator.SetFloat(this.reloadSpeedID, this.reloadSpeedCoeff);
            }
        }

        public float CooldownAnimationTime { get; set; }
    }
}

