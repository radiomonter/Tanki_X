namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Lobby.ClientPayment.Impl;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientPayment.Impl;

    public class QiwiWalletScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void HandleError(InvalidQiwiAccountEvent e, SingleNode<PaymentMethodComponent> method, [JoinAll] LockedScreenNode screen)
        {
            screen.Entity.RemoveComponent<LockedScreenComponent>();
        }

        [OnEventComplete]
        public void HandleError(InvalidQiwiAccountEvent e, SingleNode<PaymentMethodComponent> method, [JoinAll] SingleNode<QiwiWalletScreenComponent> screen, [JoinAll] QiwiInputFieldNode inputField)
        {
            screen.component.DisableContinueButton();
        }

        [OnEventFire]
        public void InitCodes(NodeAddedEvent e, QiwiInputFieldNode format, SingleNode<PhoneCodesComponent> codes, UserNode user)
        {
            format.qiwiAccountFormatter.SetCodes(codes.component.Codes.Values);
            format.qiwiAccountFormatter.GetComponent<TMP_InputField>().text = codes.component.Codes[user.userCountry.CountryCode];
        }

        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, SingleNode<QiwiWalletScreenComponent> screen, SelectedOfferNode selectedOffer)
        {
            screen.component.Receipt.AddSpecialOfferText(selectedOffer.receiptText.Text);
        }

        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, SingleNode<QiwiWalletScreenComponent> screen, UserNode user, SelectedGoodNode selectedGood, [JoinAll] SelectedMethodNode selectedMethod)
        {
            double price = selectedGood.goodsPrice.Price;
            price = !selectedGood.Entity.HasComponent<SpecialOfferComponent>() ? selectedGood.goodsPrice.Round(selectedGood.goods.SaleState.PriceMultiplier * price) : selectedGood.Entity.GetComponent<SpecialOfferComponent>().GetSalePrice(price);
            screen.component.Receipt.SetPrice(price, selectedGood.goodsPrice.Currency);
        }

        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, SingleNode<QiwiWalletScreenComponent> screen, UserNode user, SelectedPackNode selectedPack, [JoinAll] SelectedMethodNode selectedMethod)
        {
            long amount = selectedPack.xCrystalsPack.Amount;
            if (!selectedPack.Entity.HasComponent<SpecialOfferComponent>())
            {
                amount = (long) Math.Round((double) (selectedPack.goods.SaleState.AmountMultiplier * amount));
            }
            screen.component.Receipt.AddItem((string) screen.component.Receipt.Lines["amount"], amount + selectedPack.xCrystalsPack.Bonus);
        }

        [OnEventFire]
        public void SendData(ButtonClickEvent e, SingleNode<ContinueButtonComponent> button, [JoinByScreen] SingleNode<QiwiWalletScreenComponent> screen, [JoinAll] SelectedGoodNode selectedGood, [JoinAll] SelectedMethodNode selectedMethod, [JoinAll] SingleNode<ClientSessionComponent> session)
        {
            QiwiProcessPaymentEvent eventInstance = new QiwiProcessPaymentEvent {
                Account = screen.component.Account
            };
            Node[] nodes = new Node[] { selectedGood, selectedMethod };
            base.NewEvent(eventInstance).AttachAll(nodes).Schedule();
            screen.Entity.AddComponent<LockedScreenComponent>();
        }

        public class LockedScreenNode : Node
        {
            public LockedScreenComponent lockedScreen;
            public QiwiWalletScreenComponent qiwiWalletScreen;
        }

        public class QiwiInputFieldNode : Node
        {
            public QiwiAccountFormatterComponent qiwiAccountFormatter;
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

        public class SelectedOfferNode : QiwiWalletScreenSystem.SelectedGoodNode
        {
            public SpecialOfferComponent specialOffer;
            public ReceiptTextComponent receiptText;
        }

        public class SelectedPackNode : QiwiWalletScreenSystem.SelectedGoodNode
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

