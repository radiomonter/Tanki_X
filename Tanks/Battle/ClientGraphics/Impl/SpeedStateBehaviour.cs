namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class SpeedStateBehaviour : StateMachineBehaviour
    {
        [SerializeField]
        private string startSpeedLoopName = "startSpeedLoop";
        [SerializeField]
        private string stopSpeedLoopName = "stopSpeedLoop";
        private int startSpeedLoopID;
        private int stopSpeedLoopID;

        private void Awake()
        {
            this.startSpeedLoopID = Animator.StringToHash(this.startSpeedLoopName);
            this.stopSpeedLoopID = Animator.StringToHash(this.stopSpeedLoopName);
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetTrigger(this.startSpeedLoopID);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetTrigger(this.stopSpeedLoopID);
        }
    }
}

