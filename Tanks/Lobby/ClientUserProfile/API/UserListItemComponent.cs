namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientUserProfile.Impl;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;

    public class UserListItemComponent : UIBehaviour, Component
    {
        public long userId;
        [SerializeField]
        private GameObject userLabelPrefab;
        [SerializeField]
        private RectTransform userLabelRoot;
        private GameObject userLabelObject;
        public bool isVisible;
        public RectTransform viewportRect;
        private Animator animator;

        protected override void Awake()
        {
            this.animator = base.GetComponent<Animator>();
        }

        protected override void OnDisable()
        {
            base.CancelInvoke();
            this.animator.SetBool("show", false);
            this.animator.SetBool("remove", false);
            base.GetComponent<CanvasGroup>().alpha = 0f;
        }

        public void ResetItem(long userId, bool delayedLoading = false)
        {
            base.CancelInvoke();
            this.userId = userId;
            this.isVisible = false;
            if (this.userLabelObject != null)
            {
                Destroy(this.userLabelObject);
            }
            this.animator.SetBool("show", false);
            this.animator.SetBool("remove", false);
            if (delayedLoading)
            {
                base.Invoke("SetUserLabelVisible", 0.2f);
            }
            else
            {
                this.SetUserLabelVisible();
            }
        }

        private void SetUserLabelVisible()
        {
            <SetUserLabelVisible>c__AnonStorey0 storey = new <SetUserLabelVisible>c__AnonStorey0();
            this.isVisible = true;
            this.userLabelObject = Instantiate<GameObject>(this.userLabelPrefab);
            UserLabelBuilder builder = new UserLabelBuilder(this.userId, this.userLabelObject, null, false);
            this.userLabelObject = builder.AllowInBattleIcon().Build();
            UidIndicatorComponent componentInChildren = this.userLabelObject.GetComponentInChildren<UidIndicatorComponent>();
            if (string.IsNullOrEmpty(componentInChildren.Uid))
            {
                componentInChildren.OnUserUidInited.AddListener(new UnityAction(this.UserInited));
            }
            else
            {
                this.UserInited();
            }
            storey.entity = this.userLabelObject.GetComponent<EntityBehaviour>().Entity;
            this.userLabelObject.GetComponent<RectTransform>().SetParent(this.userLabelRoot, false);
            this.userLabelObject.GetComponent<Button>().onClick.AddListener(new UnityAction(storey.<>m__0));
        }

        private void UserInited()
        {
            this.userLabelObject.GetComponentInChildren<UidIndicatorComponent>().OnUserUidInited.RemoveListener(new UnityAction(this.UserInited));
            base.GetComponent<Animator>().SetBool("show", true);
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }

        [CompilerGenerated]
        private sealed class <SetUserLabelVisible>c__AnonStorey0
        {
            internal Entity entity;

            internal void <>m__0()
            {
                UserListItemComponent.EngineService.Engine.ScheduleEvent<UserLabelClickEvent>(this.entity);
            }
        }
    }
}

