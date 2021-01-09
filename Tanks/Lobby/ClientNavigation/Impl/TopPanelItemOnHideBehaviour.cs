namespace Tanks.Lobby.ClientNavigation.Impl
{
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class TopPanelItemOnHideBehaviour : StateMachineBehaviour
    {
        [CompilerGenerated]
        private static Func<AnimatorControllerParameter, bool> <>f__am$cache0;

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = p => p.name == "InactiveAfterHide";
            }
            bool flag = animator.parameters.Any<AnimatorControllerParameter>(<>f__am$cache0) && !animator.GetBool("InactiveAfterHide");
            animator.gameObject.SetActive(flag);
        }
    }
}

