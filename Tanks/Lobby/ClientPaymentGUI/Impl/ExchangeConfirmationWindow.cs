namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientPayment.Impl;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    [RequireComponent(typeof(Animator))]
    public class ExchangeConfirmationWindow : LocalizedControl
    {
        [SerializeField]
        private Text questionText;
        [SerializeField]
        private TextMeshProUGUI confirmText;
        [SerializeField]
        private TextMeshProUGUI cancelText;
        [SerializeField]
        private Text forText;
        [SerializeField]
        private Button confirm;
        [SerializeField]
        private Button cancel;
        [SerializeField]
        private Text crystalsText;
        [SerializeField]
        private Text xCrystalsText;
        private Entity user;
        private long xCrystals;

        protected override void Awake()
        {
            base.Awake();
            this.confirm.onClick.AddListener(new UnityAction(this.OnConfirm));
            this.cancel.onClick.AddListener(new UnityAction(this.OnCancel));
        }

        private void Hide()
        {
            MainScreenComponent.Instance.ClearOnBackOverride();
            base.GetComponent<Animator>().SetBool("Visible", false);
        }

        private void OnCancel()
        {
            this.Hide();
        }

        private void OnConfirm()
        {
            this.Hide();
            ExchangeCrystalsEvent eventInstance = new ExchangeCrystalsEvent {
                XCrystals = this.xCrystals
            };
            Entity[] entities = new Entity[] { this.user };
            ECSBehaviour.EngineService.Engine.NewEvent(eventInstance).AttachAll(entities).Schedule();
        }

        public void Show(Entity user, long xCrystals, long crystals)
        {
            this.xCrystals = xCrystals;
            MainScreenComponent.Instance.OverrideOnBack(new Action(this.Hide));
            this.user = user;
            base.gameObject.SetActive(true);
            this.crystalsText.text = crystals.ToStringSeparatedByThousands();
            this.xCrystalsText.text = xCrystals.ToStringSeparatedByThousands();
        }

        public string QuestionText
        {
            set => 
                this.questionText.text = value;
        }

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

        public string ForText
        {
            set => 
                this.forText.text = value;
        }
    }
}

