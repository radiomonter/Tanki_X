namespace Tanks.Lobby.ClientNavigation.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class NotificationDialogComponent : UIBehaviour, Component, ICancelHandler, IEventSystemHandler
    {
        [SerializeField]
        private TextMeshProUGUI message;
        [SerializeField]
        private Button okButton;

        public void Hide()
        {
            base.GetComponent<Animator>().SetBool("Visible", false);
        }

        public void OnCancel(BaseEventData eventData)
        {
            this.Hide();
        }

        protected override void OnDisable()
        {
        }

        private void OnOk()
        {
            this.Hide();
            if (base.GetComponent<EntityBehaviour>() != null)
            {
                Entity entity = base.GetComponent<EntityBehaviour>().Entity;
                EngineService.Engine.ScheduleEvent<DialogConfirmEvent>(entity);
            }
        }

        public virtual void Show(string message)
        {
            this.message.text = message;
            base.gameObject.SetActive(true);
        }

        protected override void Start()
        {
            this.okButton.onClick.AddListener(new UnityAction(this.OnOk));
        }

        private void Update()
        {
            if (InputMapping.Cancel)
            {
                this.OnOk();
            }
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }
    }
}

