namespace Tanks.Lobby.ClientNavigation.API
{
    using System;
    using UnityEngine;

    public class ScreenInactiveBehaviour : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.gameObject.SetActive(false);
        }
    }
}

