namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Lobby.ClientPayment.API;
    using Lobby.ClientPayment.Impl;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientPayment.Impl;

    public class BankCardPaymentScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, SingleNode<BankCardPaymentScreenComponent> screen, SelectedOfferNode selectedOffer)
        {
            screen.component.Receipt.AddSpecialOfferText(selectedOffer.receiptText.Text);
        }

        [OnEventFire]
        public void InitScreenPrice(NodeAddedEvent e, SingleNode<BankCardPaymentScreenComponent> screen, SelectedGoodNode selectedGood)
        {
            double price = selectedGood.goodsPrice.Price;
            price = !selectedGood.Entity.HasComponent<SpecialOfferComponent>() ? selectedGood.goodsPrice.Round(selectedGood.goods.SaleState.PriceMultiplier * price) : selectedGood.Entity.GetComponent<SpecialOfferComponent>().GetSalePrice(price);
            screen.component.Receipt.SetPrice(price, selectedGood.goodsPrice.Currency);
        }

        [OnEventFire]
        public void InitScreenXCrystalsPack(NodeAddedEvent e, SingleNode<BankCardPaymentScreenComponent> screen, SelectedPackNode selectedGood)
        {
            long amount = selectedGood.xCrystalsPack.Amount;
            if (selectedGood.Entity.HasComponent<SpecialOfferComponent>())
            {
                amount = (long) Math.Round((double) (selectedGood.goods.SaleState.AmountMultiplier * amount));
            }
            screen.component.Receipt.AddItem((string) screen.component.Receipt.Lines["amount"], amount + selectedGood.xCrystalsPack.Bonus);
        }

        [OnEventFire]
        public void SendData(ButtonClickEvent e, SingleNode<ContinueButtonComponent> button, [JoinByScreen] SingleNode<BankCardPaymentScreenComponent> screen, [JoinAll] SelectedGoodNode selectedGood, [JoinAll] SelectedMethodNode selectedMethod, [JoinAll] SingleNode<AdyenPublicKeyComponent> adyenProvider, [JoinAll] SingleNode<ClientSessionComponent> session)
        {
            BankCardPaymentScreenComponent component = screen.component;
            Card card = new Card {
                number = component.Number.Replace(" ", string.Empty),
                expiryMonth = int.Parse(component.MM).ToString(),
                expiryYear = "20" + component.YY,
                holderName = component.CardHolder,
                cvc = component.CVC
            };
            AdyenBuyGoodsByCardEvent eventInstance = new AdyenBuyGoodsByCardEvent {
                EncrypedCard = new Encrypter(adyenProvider.component.PublicKey).Encrypt(card.ToString())
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

        [OnEventFire]
        public void Success(SuccessPaymentEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<PaymentProcessingScreenComponent> screen)
        {
            base.ScheduleEvent<ShowScreenLeftEvent<PaymentSuccessScreenComponent>>(screen);
        }

        public class SelectedGoodNode : Node
        {
            public GoodsComponent goods;
            public SelectedListItemComponent selectedListItem;
            public GoodsPriceComponent goodsPrice;
        }

        public class SelectedMethodNode : Node
        {
            public PaymentMethodComponent paymentMethod;
            public SelectedListItemComponent selectedListItem;
        }

        public class SelectedOfferNode : BankCardPaymentScreenSystem.SelectedGoodNode
        {
            public SpecialOfferComponent specialOffer;
            public ReceiptTextComponent receiptText;
        }

        public class SelectedPackNode : BankCardPaymentScreenSystem.SelectedGoodNode
        {
            public XCrystalsPackComponent xCrystalsPack;
        }
    }
}

