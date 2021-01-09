namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class SupplyAnimationPlayer
    {
        private Animator animator;
        private int paramID;

        public SupplyAnimationPlayer(Animator animator, string animationParameterName)
        {
            this.animator = animator;
            this.paramID = Animator.StringToHash(animationParameterName);
        }

        public void StartAnimation()
        {
            this.animator.SetBool(this.paramID, true);
            this.animator.Update(0f);
        }

        public void StopAnimation()
        {
            this.animator.SetBool(this.paramID, false);
        }
    }
}

