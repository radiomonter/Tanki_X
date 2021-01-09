namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Lobby.ClientPayment.Impl;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientGarage.Impl;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;

    public class AdyenWindow : ECSBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI info;
        [SerializeField]
        private Animator continueButton;
        [SerializeField]
        private TMP_InputField cardNumber;
        [SerializeField]
        private TMP_InputField mm;
        [SerializeField]
        private TMP_InputField yy;
        [SerializeField]
        private TMP_InputField cvc;
        [SerializeField]
        private TMP_InputField cardHolder;
        private Action onBack;
        private Action onForward;
        private Entity item;
        private Entity method;

        private void Awake()
        {
            this.cardNumber.onValueChanged.AddListener(new UnityAction<string>(this.ValidateInput));
            this.mm.onValueChanged.AddListener(new UnityAction<string>(this.ValidateInput));
            this.yy.onValueChanged.AddListener(new UnityAction<string>(this.ValidateInput));
            this.cvc.onValueChanged.AddListener(new UnityAction<string>(this.ValidateInput));
            this.cardHolder.onValueChanged.AddListener(new UnityAction<string>(this.ValidateInput));
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
            Card card = new Card {
                number = this.cardNumber.text.Replace(" ", string.Empty),
                expiryMonth = int.Parse(this.mm.text).ToString(),
                expiryYear = "20" + this.yy.text,
                holderName = this.cardHolder.text,
                cvc = this.cvc.text
            };
            AdyenBuyGoodsByCardEvent eventInstance = new AdyenBuyGoodsByCardEvent {
                EncrypedCard = new Encrypter(base.GetComponent<AdyenPublicKeyComponent>().PublicKey).Encrypt(card.ToString())
            };
            Entity[] entities = new Entity[] { this.item, this.method };
            base.NewEvent(eventInstance).AttachAll(entities).Schedule();
            this.onForward();
        }

        public void Show(Entity item, Entity method, Action onBack, Action onForward)
        {
            this.item = item;
            this.method = method;
            this.cardNumber.text = string.Empty;
            this.cvc.text = string.Empty;
            this.mm.text = string.Empty;
            this.yy.text = string.Empty;
            this.cardHolder.text = string.Empty;
            this.cardNumber.Select();
            MainScreenComponent.Instance.OverrideOnBack(new Action(this.Cancel));
            this.onBack = onBack;
            this.onForward = onForward;
            base.gameObject.SetActive(true);
            this.info.text = ShopDialogs.FormatItem(item, method);
        }

        private void ValidateInput(string value)
        {
            bool flag = (((this.cvc.text.Length >= 3) && BankCardUtils.IsBankCard(this.cardNumber.text)) && (this.yy.text.Length == this.yy.characterLimit)) && !string.IsNullOrEmpty(this.cardHolder.text);
            if (flag)
            {
                int num = int.Parse(this.mm.text);
                flag = (flag && (num >= 1)) && (num <= 12);
            }
            this.continueButton.SetBool("Visible", flag);
        }
    }
}

