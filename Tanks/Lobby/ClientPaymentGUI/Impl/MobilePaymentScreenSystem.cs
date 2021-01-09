namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Lobby.ClientPayment.API;
    using Lobby.ClientPayment.Impl;
    using Lobby.ClientPayment.main.csharp.Impl.Platbox;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientPayment.Impl;

    public class MobilePaymentScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, SingleNode<MobilePaymentScreenComponent> screen, SelectedOfferNode selectedOffer)
        {
            screen.component.Receipt.AddSpecialOfferText(selectedOffer.receiptText.Text);
        }

        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, SingleNode<MobilePaymentScreenComponent> screen, SingleNode<PhoneCodesComponent> phoneCodes, UserNode user, SelectedGoodNode selectedGood, [JoinAll] SelectedMethodNode selectedMethod)
        {
            screen.component.PhoneCountryCode = phoneCodes.component.Codes[user.userCountry.CountryCode];
            double price = selectedGood.goodsPrice.Price;
            price = !selectedGood.Entity.HasComponent<SpecialOfferComponent>() ? selectedGood.goodsPrice.Round(selectedGood.goods.SaleState.PriceMultiplier * price) : selectedGood.Entity.GetComponent<SpecialOfferComponent>().GetSalePrice(price);
            screen.component.Receipt.SetPrice(price, selectedGood.goodsPrice.Currency);
        }

        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, SingleNode<MobilePaymentScreenComponent> screen, SingleNode<PhoneCodesComponent> phoneCodes, UserNode user, SelectedPackNode selectedPack, [JoinAll] SelectedMethodNode selectedMethod)
        {
            long amount = selectedPack.xCrystalsPack.Amount;
            if (!selectedPack.Entity.HasComponent<SpecialOfferComponent>())
            {
                amount = (long) Math.Round((double) (selectedPack.goods.SaleState.AmountMultiplier * amount));
            }
            screen.component.Receipt.AddItem((string) screen.component.Receipt.Lines["amount"], amount + selectedPack.xCrystalsPack.Bonus);
        }

        [OnEventFire]
        public void SendData(ButtonClickEvent e, SingleNode<ContinueButtonComponent> button, [JoinByScreen] SingleNode<MobilePaymentScreenComponent> screen, [JoinAll] SelectedGoodNode selectedGood, [JoinAll] SelectedMethodNode selectedMethod, [JoinAll] SingleNode<ClientSessionComponent> session)
        {
            string str = screen.component.PhoneCountryCode + screen.component.PhoneNumber.Replace(" ", string.Empty);
            MobilePaymentDataComponent component = new MobilePaymentDataComponent {
                PhoneNumber = str
            };
            base.CreateEntity("MobilePayment").AddComponent(component);
            PlatBoxBuyGoodsEvent eventInstance = new PlatBoxBuyGoodsEvent {
                Phone = str
            };
            Entity[] entities = new Entity[] { selectedGood.Entity, selectedMethod.Entity };
            base.NewEvent(eventInstance).AttachAll(entities).Schedule();
            base.ScheduleEvent<ShowScreenLeftEvent<PaymentProcessingScreenComponent>>(screen);
            PaymentStatisticsEvent event3 = new PaymentStatisticsEvent {
                Action = PaymentStatisticsAction.PROCEED,
                Item = selectedGood.Entity.Id,
                Screen = screen.component.gameObject.name,
                Method = selectedMethod.Entity.Id
            };
            base.ScheduleEvent(event3, session);
        }

        public class SelectedGoodNode : Node
        {
            public SelectedListItemComponent selectedListItem;
            public GoodsPriceComponent goodsPrice;
            public GoodsComponent goods;
        }

        public class SelectedMethodNode : Node
        {
            public PaymentMethodComponent paymentMethod;
            public SelectedListItemComponent selectedListItem;
        }

        public class SelectedOfferNode : MobilePaymentScreenSystem.SelectedGoodNode
        {
            public SpecialOfferComponent specialOffer;
            public ReceiptTextComponent receiptText;
        }

        public class SelectedPackNode : MobilePaymentScreenSystem.SelectedGoodNode
        {
            public XCrystalsPackComponent xCrystalsPack;
        }

        public class UserNode : Node
        {
            public UserCountryComponent userCountry;
            public SelfUserComponent selfUser;
        }
    }
}

