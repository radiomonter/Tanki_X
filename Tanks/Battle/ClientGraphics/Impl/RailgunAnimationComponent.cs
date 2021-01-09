namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    [RequireComponent(typeof(Animator))]
    public class RailgunAnimationComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private string railgunChargingTriggerName = "railgunCharging";
        [SerializeField]
        private string railgunStartReloadingName = "startReloading";
        [SerializeField]
        private string railgunStopReloadingName = "stopReloading";
        [SerializeField]
        private string railgunStopAnimationName = "stopAnyActions";
        [SerializeField]
        private string railgunReloadingSpeedCoeffName = "reloadingSpeedCoeff";
        [SerializeField]
        private string railgunChargeSpeedCoeffName = "chargeSpeedCoeff";
        [SerializeField]
        private int railgunReloadingCyclesCount = 2;
        [SerializeField]
        private AnimationClip reloadClip;
        [SerializeField]
        private AnimationClip chargeClip;
        private int railgunChargingTriggerID;
        private int railgunReloadingSpeedCoeffID;
        private int railgunChargeSpeedCoeffID;
        private int railgunStartReloadingID;
        private int railgunStopReloadingID;
        private int railgunStopAnimationID;
        private Animator railgunAnimator;

        public void InitRailgunAnimation(Animator animator, float reloadingSpeed, float chargingTime)
        {
            this.railgunChargingTriggerID = Animator.StringToHash(this.railgunChargingTriggerName);
            this.railgunStartReloadingID = Animator.StringToHash(this.railgunStartReloadingName);
            this.railgunStopReloadingID = Animator.StringToHash(this.railgunStopReloadingName);
            this.railgunStopAnimationID = Animator.StringToHash(this.railgunStopAnimationName);
            this.railgunReloadingSpeedCoeffID = Animator.StringToHash(this.railgunReloadingSpeedCoeffName);
            this.railgunChargeSpeedCoeffID = Animator.StringToHash(this.railgunChargeSpeedCoeffName);
            this.railgunAnimator = animator;
            float length = this.reloadClip.length;
            float num2 = (reloadingSpeed * this.railgunReloadingCyclesCount) * length;
            float num4 = this.chargeClip.length / chargingTime;
            animator.SetFloat(this.railgunReloadingSpeedCoeffID, num2);
            animator.SetFloat(this.railgunChargeSpeedCoeffID, num4);
        }

        public void StartChargingAnimation()
        {
            this.railgunAnimator.ResetTrigger(this.railgunStopReloadingID);
            this.railgunAnimator.SetTrigger(this.railgunChargingTriggerID);
            this.railgunAnimator.Update(0f);
        }

        public void StartReloading()
        {
            this.railgunAnimator.ResetTrigger(this.railgunStopReloadingID);
            this.railgunAnimator.SetTrigger(this.railgunStartReloadingID);
        }

        public void StopAnyRailgunAnimation()
        {
            this.railgunAnimator.SetTrigger(this.railgunStopAnimationID);
        }

        public void StopReloading()
        {
            this.railgunAnimator.SetTrigger(this.railgunStopReloadingID);
        }
    }
}

