namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class BattleResultBestPlayerScreenAnimationComponent : MonoBehaviour
    {
        [SerializeField]
        private Animator mainContentAnimator;
        [SerializeField]
        private Animator avatarAnimator;
        [SerializeField]
        private Animator infoAnimator;
        [SerializeField]
        private Animator info1Animator;
        [SerializeField]
        private Animator buttonsAnimator;
        private List<Action> actions;
        private float showDelay = 0.2f;
        private float timer;
        private bool playActions;

        public void DisableAll()
        {
            this.mainContentAnimator.SetBool("showTank", false);
            this.avatarAnimator.SetBool("on", false);
            this.infoAnimator.SetBool("on", false);
            this.info1Animator.SetBool("on", false);
            this.buttonsAnimator.SetBool("on", false);
            this.mainContentAnimator.GetComponentInChildren<CanvasGroup>().alpha = 0f;
            this.avatarAnimator.GetComponentInChildren<CanvasGroup>().alpha = 0f;
            this.infoAnimator.GetComponentInChildren<CanvasGroup>().alpha = 0f;
            this.info1Animator.GetComponentInChildren<CanvasGroup>().alpha = 0f;
            this.buttonsAnimator.GetComponentInChildren<CanvasGroup>().alpha = 0f;
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
                new Action(this.ShowAvatar),
                new Action(this.ShowInfo),
                new Action(this.ShowInfo1),
                new Action(this.ShowTank),
                new Action(this.ShowButtons),
                new Action(this.ShowModules)
            };
            this.actions = list;
        }

        public void ShowAvatar()
        {
            this.avatarAnimator.SetBool("on", true);
        }

        public void ShowButtons()
        {
            this.buttonsAnimator.SetBool("on", true);
        }

        public void ShowInfo()
        {
            this.infoAnimator.SetBool("on", true);
        }

        public void ShowInfo1()
        {
            this.info1Animator.SetBool("on", true);
        }

        public void ShowModules()
        {
            base.GetComponentInChildren<MVPModulesInfoComponent>().AnimateCards();
        }

        public void ShowTank()
        {
            EngineService.Engine.ScheduleEvent<BuildBestPlayerTankEvent>(EngineService.EntityStub);
            this.mainContentAnimator.SetBool("showTank", true);
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

