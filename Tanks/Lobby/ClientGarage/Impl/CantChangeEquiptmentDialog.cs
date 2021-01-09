namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class CantChangeEquiptmentDialog : UIBehaviour, Component, ICancelHandler, IEventSystemHandler
    {
        [SerializeField]
        private TextMeshProUGUI message;
        [SerializeField]
        private Button okButton;
        public LocalizedField messageLocalizedField;

        public void Hide()
        {
            MainScreenComponent.Instance.Unlock();
            base.GetComponent<Animator>().SetBool("Visible", false);
        }

        public void OnCancel(BaseEventData eventData)
        {
            this.Hide();
        }

        protected override void OnDisable()
        {
            base.gameObject.SetActive(false);
            base.GetComponent<Animator>().SetBool("Visible", false);
        }

        private void OnOk()
        {
            this.Hide();
        }

        public void Show()
        {
            MainScreenComponent.Instance.Lock();
            this.message.text = this.messageLocalizedField.Value;
            base.gameObject.SetActive(true);
        }

        protected override void Start()
        {
            this.okButton.onClick.AddListener(new UnityAction(this.OnOk));
        }
    }
}

