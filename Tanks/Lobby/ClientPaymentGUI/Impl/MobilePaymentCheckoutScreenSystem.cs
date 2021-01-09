namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Lobby.ClientPayment.Impl;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientPayment.Impl;

    public class MobilePaymentCheckoutScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, SingleNode<MobilePaymentCheckoutScreenComponent> screen, [JoinAll] SingleNode<MobilePaymentDataComponent> mobilePayment, [JoinAll] SelectedOfferNode selectedOffer)
        {
        }

        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, SingleNode<MobilePaymentCheckoutScreenComponent> screen, [JoinAll] SingleNode<MobilePaymentDataComponent> mobilePayment, [JoinAll] SelectedGoodNode selectedGood, [JoinAll] SelectedMethodNode selectedMethod)
        {
            double price = selectedGood.goodsPrice.Price;
            price = !selectedGood.Entity.HasComponent<SpecialOfferComponent>() ? selectedGood.goodsPrice.Round(selectedGood.goods.SaleState.PriceMultiplier * price) : selectedGood.Entity.GetComponent<SpecialOfferComponent>().GetSalePrice(price);
            screen.component.SetPrice(price, selectedGood.goodsPrice.Currency);
            base.DeleteEntity(mobilePayment.Entity);
            screen.component.SetTransactionNumber(mobilePayment.component.TransactionId);
            screen.component.SetPhoneNumber(mobilePayment.component.PhoneNumber);
        }

        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, SingleNode<MobilePaymentCheckoutScreenComponent> screen, [JoinAll] SingleNode<MobilePaymentDataComponent> mobilePayment, [JoinAll] SelectedPackNode selectedPack, [JoinAll] SelectedMethodNode selectedMethod)
        {
            long amount = selectedPack.xCrystalsPack.Amount;
            if (!selectedPack.Entity.HasComponent<SpecialOfferComponent>())
            {
                amount = (long) Math.Round((double) (selectedPack.goods.SaleState.AmountMultiplier * amount));
            }
            screen.component.SetCrystalsAmount(amount + selectedPack.xCrystalsPack.Bonus);
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

        public class SelectedOfferNode : MobilePaymentCheckoutScreenSystem.SelectedGoodNode
        {
            public SpecialOfferComponent specialOffer;
            public ReceiptTextComponent receiptText;
        }

        public class SelectedPackNode : MobilePaymentCheckoutScreenSystem.SelectedGoodNode
        {
            public XCrystalsPackComponent xCrystalsPack;
        }
    }
}

