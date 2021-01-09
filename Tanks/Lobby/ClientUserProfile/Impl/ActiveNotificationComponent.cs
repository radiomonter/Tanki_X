namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class ActiveNotificationComponent : BehaviourComponent
    {
        [SerializeField]
        private UnityEngine.Animator animator;
        [SerializeField]
        private string showState = "Show";
        [SerializeField]
        private string hideState = "Hide";
        [SerializeField]
        private UnityEngine.UI.Text text;
        private bool visible;
        private Platform.Kernel.ECS.ClientEntitySystem.API.Entity entity;
        [CompilerGenerated]
        private static Func<AnimatorControllerParameter, bool> <>f__am$cache0;

        public ActiveNotificationComponent()
        {
            this.ShowState = UnityEngine.Animator.StringToHash(this.showState);
            this.HideState = UnityEngine.Animator.StringToHash(this.hideState);
        }

        public void Hide()
        {
            this.visible = false;
            if (this.Animator != null)
            {
                this.Animator.Play(this.HideState);
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = p => p.name.Equals("HideFlag");
                }
                if (this.Animator.parameters.Any<AnimatorControllerParameter>(<>f__am$cache0))
                {
                    this.Animator.SetBool("HideFlag", true);
                }
            }
        }

        public void OnHidden()
        {
            ECSBehaviour.EngineService.Engine.ScheduleEvent<NotificationShownEvent>(this.entity);
        }

        public void Show()
        {
            this.visible = true;
            if (this.Animator != null)
            {
                this.Animator.Play(this.ShowState);
            }
        }

        public Platform.Kernel.ECS.ClientEntitySystem.API.Entity Entity
        {
            get => 
                this.entity;
            set => 
                this.entity = value;
        }

        public UnityEngine.Animator Animator =>
            this.animator;

        public int ShowState { get; private set; }

        public int HideState { get; private set; }

        public UnityEngine.UI.Text Text =>
            this.text;

        public bool Visible =>
            this.visible;
    }
}

