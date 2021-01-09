namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    [RequireComponent(typeof(Animator))]
    public class FreezeMotionAnimationComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private float idleAnimationSpeedMultiplier = 0.125f;
        [SerializeField]
        private float workingAnimationSpeedMultiplier = 1f;
        [SerializeField]
        private string freezeAnimationSpeedCoeffName = "motionCoeff";
        [SerializeField]
        private float coefAcceleration = 0.6f;
        private int speedCoeffID;
        private Animator freezeAnimator;
        private float coeffChangeSpeed;
        private float currentSpeedMultiplier;
        private float freezeEnergyReloadingSpeed;
        private bool isWorking;

        private void Awake()
        {
            base.enabled = false;
        }

        public void Init(Animator animator, float energyReloadingSpeed)
        {
            this.freezeAnimator = animator;
            this.freezeEnergyReloadingSpeed = energyReloadingSpeed;
            this.speedCoeffID = Animator.StringToHash(this.freezeAnimationSpeedCoeffName);
            this.ResetMotion();
        }

        public void ResetMotion()
        {
            this.currentSpeedMultiplier = 0f;
            this.freezeAnimator.SetFloat(this.speedCoeffID, this.currentSpeedMultiplier);
            this.isWorking = false;
        }

        public void StartIdle(float currentEnergyLevel)
        {
            this.StartMode(currentEnergyLevel, false);
        }

        private void StartMode(float currentEnergyLevel, bool isWorking)
        {
            float num = 1f - currentEnergyLevel;
            this.coeffChangeSpeed = !isWorking ? ((num <= 0f) ? -this.coefAcceleration : -Mathf.Max((float) 0f, (float) (((this.currentSpeedMultiplier - this.idleAnimationSpeedMultiplier) / num) * this.freezeEnergyReloadingSpeed))) : this.coefAcceleration;
            base.enabled = true;
            this.isWorking = isWorking;
        }

        public void StartWorking(float currentEnergyLevel)
        {
            this.StartMode(currentEnergyLevel, true);
        }

        public void StopMotion()
        {
            this.freezeAnimator.SetFloat(this.speedCoeffID, 0f);
        }

        private void Update()
        {
            this.currentSpeedMultiplier += this.coeffChangeSpeed * Time.deltaTime;
            if (this.currentSpeedMultiplier >= this.workingAnimationSpeedMultiplier)
            {
                this.currentSpeedMultiplier = this.workingAnimationSpeedMultiplier;
                if (this.isWorking)
                {
                    base.enabled = false;
                }
            }
            if (this.currentSpeedMultiplier <= this.idleAnimationSpeedMultiplier)
            {
                this.currentSpeedMultiplier = this.idleAnimationSpeedMultiplier;
                if (!this.isWorking)
                {
                    base.enabled = false;
                }
            }
            this.freezeAnimator.SetFloat(this.speedCoeffID, this.currentSpeedMultiplier);
        }
    }
}

