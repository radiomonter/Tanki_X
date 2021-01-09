namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class IsisAnimationWorkingStateBehaviour : StateMachineBehaviour
    {
        [SerializeField]
        private string startLoopName = "startWorkingLoop";
        [SerializeField]
        private string stopLoopName = "stopWorkingLoop";
        private int startLoopID;
        private int stopLoopID;

        private void Awake()
        {
            this.startLoopID = Animator.StringToHash(this.startLoopName);
            this.stopLoopID = Animator.StringToHash(this.stopLoopName);
        }

        private void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            animator.SetTrigger(this.startLoopID);
        }

        private void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            animator.SetTrigger(this.stopLoopID);
        }
    }
}

