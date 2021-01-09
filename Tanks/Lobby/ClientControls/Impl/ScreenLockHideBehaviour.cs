namespace Tanks.Lobby.ClientControls.Impl
{
    using System;
    using UnityEngine;

    public class ScreenLockHideBehaviour : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.gameObject.SetActive(false);
        }
    }
}

