namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using tanks.modules.lobby.ClientPayment.Scripts.API;
    using TMPro;
    using UnityEngine;

    public class BuyConfirmationDialog : ECSBehaviour
    {
        [SerializeField]
        private GameObject buyButton;
        [SerializeField]
        private GameObject xBuyButton;
        [SerializeField]
        private TextMeshProUGUI confirmationText;
        [SerializeField]
        private TextMeshProUGUI price;
        [SerializeField]
        private TextMeshProUGUI xPrice;
        [SerializeField]
        private LocalizedField confirmation;
        [SerializeField]
        private GameObject confirmationDialog;
        [SerializeField]
        private GameObject addXCryDialog;
        [SerializeField]
        private GameObject addCryDialog;
        [SerializeField]
        private LocalizedField addXCryText;
        [SerializeField]
        private LocalizedField addCryText;
        [SerializeField]
        private TextMeshProUGUI addXCry;
        [SerializeField]
        private TextMeshProUGUI addCry;
        private Action onBought;
        private Action onCancel;
        private int priceValue;
        private int amount;
        private GarageItem item;
        private bool contextShop;

        public void AddCry()
        {
            long money = SelfUserComponent.SelfUser.GetComponent<UserMoneyComponent>().Money;
            long exchangingCrystalls = this.priceValue - money;
            base.ScheduleEvent(new GoToExchangeCryScreen(exchangingCrystalls), SelfUserComponent.SelfUser);
            this.Cancel();
        }

        public void AddXCry()
        {
            if (!this.contextShop)
            {
                this.SendEvent<GoToXCryShopScreen>(SelfUserComponent.SelfUser);
            }
            else
            {
                FindObjectOfType<Dialogs60Component>().Get<BuyXCrystalsDialogComponent>().Show(true);
            }
            this.Cancel();
        }

        public void Cancel()
        {
            base.GetComponent<Animator>().SetTrigger("cancel");
            MainScreenComponent.Instance.ClearOnBackOverride();
            if (this.onCancel != null)
            {
                this.onCancel();
            }
        }

        public void Confirm()
        {
            this.buyButton.GetComponent<Button>().interactable = false;
            base.GetComponent<Animator>().SetTrigger("buy");
            this.item.WaitForBuy = true;
            this.item.Buy(this.onBought);
            MainScreenComponent.Instance.ClearOnBackOverride();
        }

        private string GetName(GarageItem item)
        {
            VisualItem item2 = item as VisualItem;
            return (((item2 == null) || (item2.ParentItem == null)) ? item.Name : MarketItemNameLocalization.GetDetailedName(item2.MarketItem));
        }

        private string GetName(GarageItem item, int amount, string customLabel) => 
            !string.IsNullOrEmpty(customLabel) ? customLabel : (((amount <= 1) ? string.Empty : (amount + " ")) + this.GetName(item));

        public void Hide()
        {
            CheckForTutorialEvent eventInstance = new CheckForTutorialEvent();
            base.ScheduleEvent(eventInstance, EngineService.EntityStub);
            if (!eventInstance.TutorialIsActive)
            {
                this.Cancel();
            }
        }

        public void Show(GarageItem item, Action boughtAction, string customLabel = null, Action cancelAction = null)
        {
            this.amount = 1;
            this.item = item;
            base.gameObject.SetActive(true);
            MainScreenComponent.Instance.OverrideOnBack(new Action(this.Cancel));
            long money = SelfUserComponent.SelfUser.GetComponent<UserMoneyComponent>().Money;
            this.priceValue = item.Price;
            if (money < this.priceValue)
            {
                this.addCryDialog.SetActive(true);
                this.addXCryDialog.SetActive(false);
                this.confirmationDialog.SetActive(false);
                this.addCry.text = string.Format((string) this.addCryText, item.Price - money);
            }
            else
            {
                this.buyButton.GetComponent<Button>().interactable = true;
                this.addCryDialog.SetActive(false);
                this.addXCryDialog.SetActive(false);
                this.confirmationDialog.SetActive(true);
                this.confirmationText.text = string.Format((string) this.confirmation, this.GetName(item, 1, customLabel));
                this.buyButton.SetActive(true);
                this.xBuyButton.SetActive(false);
                this.onBought = boughtAction;
                this.onCancel = cancelAction;
                this.price.text = item.Price.ToStringSeparatedByThousands();
            }
        }

        public void ShowAddCrystals(int price)
        {
            base.gameObject.SetActive(true);
            MainScreenComponent.Instance.OverrideOnBack(new Action(this.Cancel));
            long money = SelfUserComponent.SelfUser.GetComponent<UserMoneyComponent>().Money;
            this.addCryDialog.SetActive(true);
            this.addXCryDialog.SetActive(false);
            this.confirmationDialog.SetActive(false);
            this.addCry.text = string.Format((string) this.addCryText, price - money);
        }

        public void XConfirm()
        {
            this.xBuyButton.GetComponent<Button>().interactable = false;
            base.GetComponent<Animator>().SetTrigger("buy");
            this.item.WaitForBuy = true;
            this.item.XBuy(this.onBought, this.priceValue, this.amount);
            MainScreenComponent.Instance.ClearOnBackOverride();
        }

        public void XShow(GarageItem item, Action boughtAction, int price, int amount = 1, string customLabel = null, bool shopContext = false, Action cancelAction = null)
        {
            this.amount = amount;
            this.item = item;
            this.contextShop = shopContext;
            base.gameObject.SetActive(true);
            MainScreenComponent.Instance.OverrideOnBack(new Action(this.Cancel));
            long money = SelfUserComponent.SelfUser.GetComponent<UserXCrystalsComponent>().Money;
            this.priceValue = price;
            if (money < this.priceValue)
            {
                this.addCryDialog.SetActive(false);
                this.addXCryDialog.SetActive(true);
                this.confirmationDialog.SetActive(false);
                this.addXCry.text = string.Format((string) this.addXCryText, this.priceValue - money);
            }
            else
            {
                this.xBuyButton.GetComponent<Button>().interactable = true;
                this.onBought = boughtAction;
                this.onCancel = cancelAction;
                this.addCryDialog.SetActive(false);
                this.addXCryDialog.SetActive(false);
                this.confirmationDialog.SetActive(true);
                this.confirmationText.text = string.Format((string) this.confirmation, this.GetName(item, amount, customLabel));
                this.buyButton.SetActive(false);
                this.xBuyButton.SetActive(true);
                this.xPrice.text = this.priceValue.ToStringSeparatedByThousands();
            }
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }
    }
}

