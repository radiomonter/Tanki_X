namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class ConfirmWindowComponent : UIBehaviour, Component
    {
        [SerializeField]
        protected TextMeshProUGUI confirmText;
        [SerializeField]
        protected TextMeshProUGUI declineText;
        [SerializeField]
        protected TextMeshProUGUI headerText;
        [SerializeField]
        protected TextMeshProUGUI mainText;
        [SerializeField]
        protected Button confirmButton;
        [SerializeField]
        protected Button declineButton;
        private List<Animator> animators;
        private bool _show;

        public void Hide()
        {
            MainScreenComponent.Instance.ClearOnBackOverride();
            this.show = false;
            base.GetComponent<Animator>().SetBool("show", false);
            if (this.animators != null)
            {
                foreach (Animator animator in this.animators)
                {
                    animator.SetBool("Visible", true);
                }
            }
        }

        public void HideByBackgorundClick()
        {
            if (!TutorialCanvas.Instance.IsShow)
            {
                this.Hide();
            }
        }

        protected override void OnDisable()
        {
        }

        protected override void OnEnable()
        {
            base.GetComponentInChildren<CanvasGroup>().alpha = 0f;
            base.GetComponent<Animator>().SetBool("show", true);
            if (this.animators != null)
            {
                foreach (Animator animator in this.animators)
                {
                    animator.SetBool("Visible", false);
                }
            }
        }

        public void OnHide()
        {
            if (this.show)
            {
                this.OnEnable();
            }
            else
            {
                base.gameObject.SetActive(false);
            }
        }

        private void OnNo()
        {
            this.Hide();
            if (base.GetComponent<EntityBehaviour>() != null)
            {
                Entity entity = base.GetComponent<EntityBehaviour>().Entity;
                EngineService.Engine.ScheduleEvent<DialogDeclineEvent>(entity);
            }
        }

        private void OnYes()
        {
            this.Hide();
            if (base.GetComponent<EntityBehaviour>() != null)
            {
                Entity entity = base.GetComponent<EntityBehaviour>().Entity;
                EngineService.Engine.ScheduleEvent<DialogConfirmEvent>(entity);
            }
        }

        public void Show(List<Animator> animators)
        {
            MainScreenComponent.Instance.OverrideOnBack(new Action(this.Hide));
            this.animators = animators;
            this.show = true;
            if (base.gameObject.activeInHierarchy)
            {
                this.OnEnable();
            }
            else
            {
                base.gameObject.SetActive(true);
            }
        }

        protected override void Start()
        {
            this.confirmButton.onClick.AddListener(new UnityAction(this.OnYes));
            if (this.declineButton != null)
            {
                this.declineButton.onClick.AddListener(new UnityAction(this.OnNo));
            }
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }

        public string ConfirmText
        {
            set => 
                this.confirmText.text = value;
        }

        public string DeclineText
        {
            set => 
                this.declineText.text = value;
        }

        public string HeaderText
        {
            set
            {
                this.headerText.text = value;
                this.headerText.gameObject.SetActive(!string.IsNullOrEmpty(value));
            }
        }

        public string MainText
        {
            set
            {
                this.mainText.text = value;
                this.mainText.gameObject.SetActive(!string.IsNullOrEmpty(value));
            }
        }

        public bool show
        {
            get => 
                this._show;
            set => 
                this._show = value;
        }
    }
}

