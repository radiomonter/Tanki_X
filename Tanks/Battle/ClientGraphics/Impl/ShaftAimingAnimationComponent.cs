namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    [RequireComponent(typeof(Animator))]
    public class ShaftAimingAnimationComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private string aimingPropertyName = "isAiming";
        private int aimingPropertyID;
        private Animator shaftAimingAnimator;

        public void InitShaftAimingAnimation(Animator animator)
        {
            this.shaftAimingAnimator = animator;
            this.aimingPropertyID = Animator.StringToHash(this.aimingPropertyName);
        }

        public void StartAiming()
        {
            this.shaftAimingAnimator.SetBool(this.aimingPropertyID, true);
        }

        public void StopAiming()
        {
            this.shaftAimingAnimator.SetBool(this.aimingPropertyID, false);
        }
    }
}

