namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class CraftedModuleWindow : UIBehaviour, ICancelHandler, IEventSystemHandler
    {
        [SerializeField]
        private Text okText;
        [SerializeField]
        private Text moduleNameText;
        [SerializeField]
        private Text moduleDescriptionText;
        [SerializeField]
        private Text additionalText;
        [SerializeField]
        private Button okButton;
        [SerializeField]
        private ImageSkin icon;
        private Entity screen;
        private bool alive;
        private string newAdditionalText;

        private void ApplyText()
        {
            this.additionalText.text = this.newAdditionalText;
        }

        private void GoToCards()
        {
            ShowGarageItemEvent eventInstance = new ShowGarageItemEvent {
                Item = Flow.Current.EntityRegistry.GetEntity(-370755132L)
            };
            EngineService.Engine.ScheduleEvent(eventInstance, this.screen);
        }

        public void Hide()
        {
            MainScreenComponent.Instance.ClearOnBackOverride();
            base.GetComponent<Animator>().SetTrigger("hide");
        }

        private void OnApplicationQuit()
        {
            this.alive = false;
        }

        public void OnCancel(BaseEventData eventData)
        {
            this.Hide();
        }

        protected override void OnDisable()
        {
            if (this.alive && this.screen.HasComponent<LockedScreenComponent>())
            {
                this.screen.RemoveComponent<LockedScreenComponent>();
            }
        }

        public void Show(Entity screen, Entity module, Entity marketModule, Entity user)
        {
            MainScreenComponent.Instance.OverrideOnBack(new Action(this.Hide));
            Animator component = base.GetComponent<Animator>();
            this.screen = screen;
            if (!base.gameObject.activeSelf)
            {
                base.gameObject.SetActive(true);
                base.GetComponent<CanvasGroup>().alpha = 0f;
                component.SetTrigger("craft");
            }
            component.SetInteger("type", 0);
            if (!screen.HasComponent<LockedScreenComponent>())
            {
                screen.AddComponent<LockedScreenComponent>();
            }
        }

        protected override void Start()
        {
            this.alive = true;
            this.okButton.onClick.AddListener(new UnityAction(this.Hide));
        }

        private void Update()
        {
            if (InputMapping.Cancel)
            {
                this.Hide();
            }
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }

        public string SpriteUid
        {
            set => 
                this.icon.SpriteUid = value;
        }

        public string ModuleNameText
        {
            set => 
                this.moduleNameText.text = value;
        }

        public string ModuleDescriptionText
        {
            set => 
                this.moduleDescriptionText.text = value;
        }

        public string AdditionalText
        {
            set => 
                this.newAdditionalText = value;
        }

        public string OkText
        {
            set => 
                this.okText.text = value;
        }
    }
}

