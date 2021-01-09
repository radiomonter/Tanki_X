namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    [RequireComponent(typeof(Animator))]
    public abstract class AbstractShotAnimationComponent : MonoBehaviour, Component
    {
        protected AbstractShotAnimationComponent()
        {
        }

        private float CalculateEnergyAfterShotsWithCooldown(float eCool, float eShot) => 
            Mathf.Clamp01((1f + eCool) - eShot);

        protected float CalculateOptimalAnimationTime(float energyReloadingSpeed, float cooldownTimeSec, float eShot)
        {
            float num = 0f;
            float eCool = energyReloadingSpeed * cooldownTimeSec;
            if (eCool >= eShot)
            {
                num = cooldownTimeSec;
            }
            else
            {
                float currentEnergy = this.CalculateEnergyAfterShotsWithCooldown(eCool, eShot);
                float num4 = this.CalculateRequiredEnergyForNextShot(currentEnergy, eShot);
                num = (num4 != 0f) ? (cooldownTimeSec + (num4 / energyReloadingSpeed)) : cooldownTimeSec;
            }
            return num;
        }

        private float CalculateRequiredEnergyForNextShot(float currentEnergy, float eShot) => 
            Mathf.Clamp01(eShot - currentEnergy);

        protected float CalculateShotSpeedCoeff(AnimationClip shotAnimationClip, float optimalAnimationTime, bool canPlaySlower)
        {
            float length = shotAnimationClip.length;
            float num2 = 1f;
            if (length > optimalAnimationTime)
            {
                num2 = length / optimalAnimationTime;
            }
            else if ((length < optimalAnimationTime) && canPlaySlower)
            {
                num2 = length / optimalAnimationTime;
            }
            return num2;
        }
    }
}

