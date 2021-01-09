namespace Tanks.Lobby.ClientNavigation.API
{
    using System;
    using UnityEngine;

    public class ScreenInvisibleBehaviour : ScreenBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.GetCanvasGroup(animator.gameObject).blocksRaycasts = false;
        }
    }
}

