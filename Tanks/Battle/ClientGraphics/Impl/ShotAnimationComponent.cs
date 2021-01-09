namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class ShotAnimationComponent : AbstractShotAnimationComponent
    {
        [SerializeField]
        private AnimationClip shotAnimationClip;
        [SerializeField]
        private bool canPlaySlower = true;
        private Animator weaponAnimator;
        private int shotTriggerID;
        private int shotSpeedCoeffID;

        public void Init(Animator animator, float cooldownTimeSec, float eShot, float energyReloadingSpeed)
        {
            this.weaponAnimator = animator;
            this.shotTriggerID = Animator.StringToHash(AnimationParameters.SHOT_TRIGGER);
            this.shotSpeedCoeffID = Animator.StringToHash(AnimationParameters.SHOT_SPEED_COEFF);
            float optimalAnimationTime = base.CalculateOptimalAnimationTime(energyReloadingSpeed, cooldownTimeSec, eShot);
            float num2 = base.CalculateShotSpeedCoeff(this.shotAnimationClip, optimalAnimationTime, this.canPlaySlower);
            this.weaponAnimator.SetFloat(this.shotSpeedCoeffID, num2);
        }

        public void Play()
        {
            this.weaponAnimator.SetTrigger(this.shotTriggerID);
        }
    }
}

