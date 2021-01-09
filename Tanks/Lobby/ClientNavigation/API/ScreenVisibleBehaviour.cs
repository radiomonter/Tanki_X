namespace Tanks.Lobby.ClientNavigation.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ScreenVisibleBehaviour : ScreenBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Entity entityStub = EngineService.EntityStub;
            EngineService.Engine.NewEvent<UnlockElementEvent>().Attach(entityStub).Schedule();
            base.GetCanvasGroup(animator.gameObject).blocksRaycasts = true;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Entity entityStub = EngineService.EntityStub;
            EngineService.Engine.NewEvent<LockElementEvent>().Attach(entityStub).Schedule();
            base.GetCanvasGroup(animator.gameObject).blocksRaycasts = false;
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }
    }
}

