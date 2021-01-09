namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientGarage.Impl;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;

    public class PlatboxWindow : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI info;
        [SerializeField]
        private TMP_InputField phone;
        [SerializeField]
        private TextMeshProUGUI code;
        [SerializeField]
        private Animator continueButton;
        private Action onBack;
        private Action onForward;

        private void Awake()
        {
            this.phone.onValueChanged.AddListener(new UnityAction<string>(this.ValidateInput));
        }

        public void Cancel()
        {
            MainScreenComponent.Instance.ClearOnBackOverride();
            base.GetComponent<Animator>().SetTrigger("cancel");
            this.onBack();
        }

        public void Proceed()
        {
            MainScreenComponent.Instance.ClearOnBackOverride();
            base.GetComponent<Animator>().SetTrigger("cancel");
            this.onForward();
        }

        public void Show(Entity item, Entity method, Action onBack, Action onForward)
        {
            this.phone.text = string.Empty;
            this.phone.Select();
            MainScreenComponent.Instance.OverrideOnBack(new Action(this.Cancel));
            this.onBack = onBack;
            this.onForward = onForward;
            base.gameObject.SetActive(true);
            this.info.text = ShopDialogs.FormatItem(item, method);
        }

        private void ValidateInput(string value)
        {
            this.continueButton.SetBool("Visible", this.phone.text.Length == this.phone.characterLimit);
        }

        public string EnteredPhoneNumber =>
            this.code.text + this.phone.text.Replace(" ", string.Empty);
    }
}

