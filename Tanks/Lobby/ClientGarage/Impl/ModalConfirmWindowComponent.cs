namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class ModalConfirmWindowComponent : LocalizedControl, Component
    {
        [SerializeField]
        private string localizePath;
        [SerializeField]
        private Text confirmText;
        [SerializeField]
        private Text cancelText;
        [SerializeField]
        private Text headerText;
        [SerializeField]
        private Text mainText;
        [SerializeField]
        private Text additionalText;
        [SerializeField]
        private ImageSkin icon;
        [SerializeField]
        private Button confirmButton;
        [SerializeField]
        private Button cancelButton;
        [SerializeField]
        private RectTransform contentRoot;
        private Entity screen;
        private bool alive;

        protected override string GetRelativeConfigPath() => 
            this.localizePath;

        public void Hide()
        {
            base.GetComponent<Animator>().SetBool("Visible", false);
            if (this.alive && this.screen.HasComponent<LockedScreenComponent>())
            {
                this.screen.RemoveComponent<LockedScreenComponent>();
            }
        }

        private void OnApplicationQuit()
        {
            this.alive = false;
        }

        private void OnNo()
        {
            this.Hide();
            this.SendEvent<DialogDeclineEvent>();
        }

        private void OnYes()
        {
            this.Hide();
            this.SendEvent<DialogConfirmEvent>();
        }

        private void SendEvent<T>() where T: Event, new()
        {
            if (base.GetComponent<EntityBehaviour>() != null)
            {
                Entity entity = base.GetComponent<EntityBehaviour>().Entity;
                base.ScheduleEvent<T>(entity);
            }
        }

        public void Show(Entity screen)
        {
            this.screen = screen;
            base.gameObject.SetActive(true);
            if (!screen.HasComponent<LockedScreenComponent>())
            {
                screen.AddComponent<LockedScreenComponent>();
            }
        }

        private void Start()
        {
            this.alive = true;
            this.confirmButton.onClick.AddListener(new UnityAction(this.OnYes));
            this.cancelButton.onClick.AddListener(new UnityAction(this.OnNo));
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }

        public string ConfirmText
        {
            set => 
                this.confirmText.text = value;
        }

        public string CancelText
        {
            set => 
                this.cancelText.text = value;
        }

        public string HeaderText
        {
            get => 
                this.headerText.text;
            set
            {
                this.headerText.text = value;
                this.headerText.gameObject.SetActive(!string.IsNullOrEmpty(value));
            }
        }

        public string MainText
        {
            get => 
                this.mainText.text;
            set
            {
                this.mainText.text = value;
                this.mainText.gameObject.SetActive(!string.IsNullOrEmpty(value));
            }
        }

        public string AdditionalText
        {
            get => 
                this.additionalText.text;
            set
            {
                this.additionalText.text = value;
                this.additionalText.gameObject.SetActive(!string.IsNullOrEmpty(value));
            }
        }

        public string SpriteUid
        {
            set => 
                this.icon.SpriteUid = value;
        }

        public RectTransform ContentRoot =>
            this.contentRoot;
    }
}

