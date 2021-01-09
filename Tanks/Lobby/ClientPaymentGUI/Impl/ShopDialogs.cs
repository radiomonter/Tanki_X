namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Lobby.ClientPayment.API;
    using Lobby.ClientPayment.Impl;
    using Lobby.ClientPayment.main.csharp.Impl.Platbox;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientPayment.Impl;
    using UnityEngine;

    public class ShopDialogs : ECSBehaviour
    {
        private const int TrMaxMobilePrice = 120;
        [SerializeField]
        private PaymentMethodWindow paymentMethod;
        [SerializeField]
        private PlatboxWindow platbox;
        [SerializeField]
        private AdyenWindow adyen;
        [SerializeField]
        private QiwiWindow qiwi;
        [SerializeField]
        private PlatboxCheckoutWindow platboxCheckout;
        [SerializeField]
        private PaymentProcessWindow process;
        [SerializeField]
        private WarningWindowComponent warning;
        [SerializeField]
        private PaymentErrorWindow error;
        [SerializeField]
        private LocalizedField forText;
        [SerializeField]
        private PaletteColorField xCrystalsColor;
        private static ShopDialogs instance;
        private Entity item;
        private List<Entity> methods;
        private Entity selectedMethod;
        [CompilerGenerated]
        private static Predicate<Entity> <>f__am$cache0;
        [CompilerGenerated]
        private static Predicate<Entity> <>f__am$cache1;

        private void Awake()
        {
            instance = this;
        }

        public void Cancel()
        {
            if (!this.process.gameObject.activeInHierarchy)
            {
                this.CloseAll();
            }
        }

        public void CloseAll()
        {
            MainScreenComponent.Instance.ClearOnBackOverride();
            base.GetComponent<Animator>().SetTrigger("cancel");
            this.HideAllWindows();
        }

        public static string FormatItem(Entity item, Entity method = null)
        {
            string str = string.Empty;
            GoodsComponent component = item.GetComponent<GoodsComponent>();
            if (item.HasComponent<SpecialOfferContentLocalizationComponent>())
            {
                str = item.GetComponent<SpecialOfferContentLocalizationComponent>().Title + " ";
            }
            else if (item.HasComponent<XCrystalsPackComponent>())
            {
                XCrystalsPackComponent component3 = item.GetComponent<XCrystalsPackComponent>();
                str = ("<#" + instance.xCrystalsColor.Color.ToHexString() + ">") + ((long) (((long) (component.SaleState.AmountMultiplier * component3.Amount)) + component3.Bonus)).ToStringSeparatedByThousands() + "<sprite=9></color>";
            }
            GoodsPriceComponent component4 = item.GetComponent<GoodsPriceComponent>();
            double price = component4.Price;
            if (!item.HasComponent<SpecialOfferComponent>())
            {
                price = component4.Round(component.SaleState.PriceMultiplier * component4.Price);
            }
            else if (item.HasComponent<CustomOfferPriceForUIComponent>())
            {
                price = item.GetComponent<CustomOfferPriceForUIComponent>().Price;
            }
            return (str + $" {instance.forText.Value} {price.ToStringSeparatedByThousands()} {component4.Currency}");
        }

        private void HideAllWindows()
        {
            this.paymentMethod.gameObject.SetActive(false);
            this.platbox.gameObject.SetActive(false);
            this.platboxCheckout.gameObject.SetActive(false);
            this.process.gameObject.SetActive(false);
            this.qiwi.gameObject.SetActive(false);
            this.adyen.gameObject.SetActive(false);
            this.error.gameObject.SetActive(false);
            this.warning.gameObject.SetActive(false);
        }

        public void Proceed(Entity method)
        {
            this.selectedMethod = method;
            this.SendStat(PaymentStatisticsAction.MODE_SELECT, method);
            PaymentMethodComponent component = method.GetComponent<PaymentMethodComponent>();
            if (component.MethodName == PaymentMethodNames.PAYGURU)
            {
                Entity[] entities = new Entity[] { method, this.item };
                base.NewEvent<PayguruProcessEvent>().AttachAll(entities).Schedule();
                this.CloseAll();
            }
            else if (component.MethodName == PaymentMethodNames.MOBILE)
            {
                this.ShowPlatbox(method);
            }
            else if ((component.MethodName == PaymentMethodNames.CREDIT_CARD) && (component.ProviderName == "adyen"))
            {
                this.ShowAdyen(method);
            }
            else if ((component.MethodName == PaymentMethodNames.QIWI_WALLET) && (component.ProviderName == "qiwi"))
            {
                this.ShowQiwi(method, string.Empty);
            }
            else
            {
                Entity[] entities = new Entity[] { method, this.item };
                base.NewEvent<ProceedToExternalPaymentEvent>().AttachAll(entities).Schedule();
                this.SendStat(PaymentStatisticsAction.PROCEED, method);
                this.process.Show(this.item, method);
            }
        }

        private void SendStat(PaymentStatisticsAction action, Entity method)
        {
            PaymentStatisticsEvent eventInstance = new PaymentStatisticsEvent {
                Action = action,
                Item = this.item.Id,
                Screen = base.gameObject.name,
                Method = method.Id
            };
            ECSBehaviour.EngineService.Engine.ScheduleEvent(eventInstance, SelfUserComponent.SelfUser);
        }

        public void Show(Entity item, ICollection<Entity> methods, bool xCryPack, string itemDesc = "")
        {
            instance = this;
            this.item = item;
            this.methods = methods.ToList<Entity>();
            if (item.HasComponent<SpecialOfferComponent>())
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = x => !x.HasComponent<PaymentMethodComponent>() || x.GetComponent<PaymentMethodComponent>().IsTerminal;
                }
                this.methods.RemoveAll(<>f__am$cache0);
            }
            GoodsPriceComponent component = item.GetComponent<GoodsPriceComponent>();
            if ((component.Currency.ToLower() == "try") && (component.Price > 120.0))
            {
                if (<>f__am$cache1 == null)
                {
                    <>f__am$cache1 = delegate (Entity x) {
                        string methodName = x.GetComponent<PaymentMethodComponent>().MethodName;
                        return (methodName == "paybymeweb") || (methodName == "paybymemobile");
                    };
                }
                this.methods.RemoveAll(<>f__am$cache1);
            }
            base.gameObject.SetActive(true);
            this.paymentMethod.Show(item, this.methods, new Action(this.Cancel), new Action<Entity>(this.Proceed), itemDesc);
        }

        private void ShowAdyen(Entity method)
        {
            <ShowAdyen>c__AnonStorey2 storey = new <ShowAdyen>c__AnonStorey2 {
                method = method,
                $this = this
            };
            this.adyen.Show(this.item, storey.method, new Action(storey.<>m__0), new Action(storey.<>m__1));
        }

        public void ShowCheckout(string transaction)
        {
            this.HideAllWindows();
            this.platboxCheckout.Show(this.item, this.selectedMethod, transaction, this.platbox.EnteredPhoneNumber, new Action(this.Cancel));
        }

        public void ShowError()
        {
            this.HideAllWindows();
            this.error.Show(this.item, this.selectedMethod, new Action(this.Cancel));
        }

        private void ShowPlatbox(Entity method)
        {
            <ShowPlatbox>c__AnonStorey1 storey = new <ShowPlatbox>c__AnonStorey1 {
                method = method,
                $this = this
            };
            this.platbox.Show(this.item, storey.method, new Action(storey.<>m__0), new Action(storey.<>m__1));
        }

        private void ShowQiwi(Entity method, string acc = "")
        {
            <ShowQiwi>c__AnonStorey0 storey = new <ShowQiwi>c__AnonStorey0 {
                method = method,
                $this = this
            };
            this.qiwi.Show(this.item, storey.method, acc, new Action(storey.<>m__0), new Action(storey.<>m__1));
        }

        public void ShowQiwiError()
        {
            this.HideAllWindows();
            this.ShowQiwi(this.selectedMethod, this.qiwi.Account);
        }

        [CompilerGenerated]
        private sealed class <ShowAdyen>c__AnonStorey2
        {
            internal Entity method;
            internal ShopDialogs $this;

            internal void <>m__0()
            {
                this.$this.paymentMethod.Show(this.$this.item, this.$this.methods, new Action(this.$this.Cancel), new Action<Entity>(this.$this.Proceed), string.Empty);
            }

            internal void <>m__1()
            {
                this.$this.SendStat(PaymentStatisticsAction.PROCEED, this.method);
                MainScreenComponent.Instance.OverrideOnBack(new Action(this.$this.Cancel));
                this.$this.process.Show(this.$this.item, this.method);
            }
        }

        [CompilerGenerated]
        private sealed class <ShowPlatbox>c__AnonStorey1
        {
            internal Entity method;
            internal ShopDialogs $this;

            internal void <>m__0()
            {
                this.$this.paymentMethod.Show(this.$this.item, this.$this.methods, new Action(this.$this.Cancel), new Action<Entity>(this.$this.Proceed), string.Empty);
            }

            internal void <>m__1()
            {
                this.$this.SendStat(PaymentStatisticsAction.PROCEED, this.method);
                MainScreenComponent.Instance.OverrideOnBack(new Action(this.$this.Cancel));
                PlatBoxBuyGoodsEvent eventInstance = new PlatBoxBuyGoodsEvent {
                    Phone = this.$this.platbox.EnteredPhoneNumber
                };
                Entity[] entities = new Entity[] { this.$this.item, this.method };
                ECSBehaviour.EngineService.Engine.NewEvent(eventInstance).AttachAll(entities).Schedule();
            }
        }

        [CompilerGenerated]
        private sealed class <ShowQiwi>c__AnonStorey0
        {
            internal Entity method;
            internal ShopDialogs $this;

            internal void <>m__0()
            {
                this.$this.paymentMethod.Show(this.$this.item, this.$this.methods, new Action(this.$this.Cancel), new Action<Entity>(this.$this.Proceed), string.Empty);
            }

            internal void <>m__1()
            {
                this.$this.SendStat(PaymentStatisticsAction.PROCEED, this.method);
                MainScreenComponent.Instance.OverrideOnBack(new Action(this.$this.Cancel));
                QiwiProcessPaymentEvent eventInstance = new QiwiProcessPaymentEvent {
                    Account = this.$this.qiwi.Account
                };
                Entity[] entities = new Entity[] { this.$this.item, this.method };
                ECSBehaviour.EngineService.Engine.NewEvent(eventInstance).AttachAll(entities).Schedule();
            }
        }
    }
}

