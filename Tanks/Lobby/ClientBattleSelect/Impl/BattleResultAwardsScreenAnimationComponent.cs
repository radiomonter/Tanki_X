namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class BattleResultAwardsScreenAnimationComponent : BehaviourComponent
    {
        [SerializeField]
        private Animator headerAnimator;
        [SerializeField]
        private Animator infoAnimator;
        [SerializeField]
        private Animator tankInfoAnimator;
        [SerializeField]
        private Animator specialOfferAnimator;
        [SerializeField]
        private CircleProgressBar rankProgressBar;
        [SerializeField]
        private CircleProgressBar containerProgressBar;
        [SerializeField]
        private CircleProgressBar hullProgressBar;
        [SerializeField]
        private CircleProgressBar turretProgressBar;
        private List<Action> actions;
        private float showDelay = 0.2f;
        private float timer;
        public bool playActions;

        public void DisableAll()
        {
            this.playActions = false;
            this.headerAnimator.SetBool("on", false);
            this.infoAnimator.SetBool("on", false);
            this.tankInfoAnimator.SetBool("on", false);
            this.specialOfferAnimator.SetBool("on", false);
            this.headerAnimator.GetComponentInChildren<CanvasGroup>().alpha = 0f;
            this.infoAnimator.GetComponentInChildren<CanvasGroup>().alpha = 0f;
            this.tankInfoAnimator.GetComponentInChildren<CanvasGroup>().alpha = 0f;
            this.specialOfferAnimator.GetComponentInChildren<CanvasGroup>().alpha = 0f;
        }

        private void NextAction()
        {
            if (this.actions.Count > 0)
            {
                Action item = this.actions[0];
                this.actions.Remove(item);
                this.playActions = this.actions.Count > 0;
                item();
            }
        }

        private void OnDisable()
        {
            this.DisableAll();
        }

        private void OnEnable()
        {
            this.timer = 0f;
            this.showDelay = 0.2f;
            this.playActions = true;
            List<Action> list = new List<Action> {
                new Action(this.ShowHeader),
                new Action(this.ShowInfo),
                new Action(this.ShowTankInfo),
                new Action(this.ShowSpecialOffer)
            };
            this.actions = list;
            this.rankProgressBar.StopAnimation();
            this.containerProgressBar.StopAnimation();
            this.hullProgressBar.StopAnimation();
            this.turretProgressBar.StopAnimation();
        }

        public void ShowButtons()
        {
            base.GetComponentInParent<BattleResultCommonUIComponent>().ShowBottomPanel();
            base.GetComponentInParent<BattleResultCommonUIComponent>().ShowTopPanel();
        }

        public void ShowHeader()
        {
            this.headerAnimator.SetBool("on", true);
        }

        public void ShowInfo()
        {
            this.playActions = false;
            this.infoAnimator.SetBool("on", true);
            this.showDelay = 0.5f;
            ShowBattleResultsScreenNotificationEvent eventInstance = new ShowBattleResultsScreenNotificationEvent {
                Index = 1
            };
            EngineService.Engine.ScheduleEvent(eventInstance, EngineService.EntityStub);
            if (!eventInstance.NotificationExist)
            {
                this.playActions = true;
            }
            this.rankProgressBar.Animate(1f);
            this.containerProgressBar.Animate(1f);
        }

        public void ShowSpecialOffer()
        {
            this.specialOfferAnimator.SetBool("on", true);
            this.ShowButtons();
        }

        public void ShowTankInfo()
        {
            EngineService.Engine.ScheduleEvent<BuildSelfPlayerTankEvent>(EngineService.EntityStub);
            this.tankInfoAnimator.SetBool("on", true);
            this.hullProgressBar.Animate(1f);
            this.turretProgressBar.Animate(1f);
        }

        private void Update()
        {
            if (this.playActions)
            {
                this.timer += Time.deltaTime;
                if (this.timer > this.showDelay)
                {
                    this.timer = 0f;
                    this.NextAction();
                }
            }
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }
    }
}

