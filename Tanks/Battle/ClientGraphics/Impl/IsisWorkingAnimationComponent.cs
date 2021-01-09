namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    [RequireComponent(typeof(Animator))]
    public class IsisWorkingAnimationComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private string workingName = "isWorking";
        private int workingID;
        private Animator isisAnimator;

        public void InitIsisWorkingAnimation(Animator animator)
        {
            this.isisAnimator = animator;
            this.workingID = Animator.StringToHash(this.workingName);
        }

        public void StartWorkingAnimation()
        {
            this.isisAnimator.SetBool(this.workingID, true);
        }

        public void StopWorkingAnimation()
        {
            this.isisAnimator.SetBool(this.workingID, false);
        }
    }
}

