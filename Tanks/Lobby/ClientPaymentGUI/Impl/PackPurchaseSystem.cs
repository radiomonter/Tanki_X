namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Lobby.ClientPayment.Impl;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;

    public class PackPurchaseSystem : ECSSystem
    {
        [OnEventFire]
        public void AddMethod(NodeAddedEvent e, PurchaseButtonNode node, [JoinAll] ICollection<SingleNode<PaymentMethodComponent>> methods)
        {
            foreach (SingleNode<PaymentMethodComponent> node2 in methods)
            {
                node.purchaseDialog.AddMethod(node2.Entity);
            }
        }

        [OnEventFire]
        public void Cancel(PaymentIsCancelledEvent e, SingleNode<PaymentMethodComponent> payment, [JoinAll] ICollection<SingleNode<PurchaseDialogComponent>> dialogs)
        {
            foreach (SingleNode<PurchaseDialogComponent> node in dialogs)
            {
                node.component.HandleError();
            }
        }

        [OnEventFire]
        public void Clear(NodeRemoveEvent e, SingleNode<PurchaseDialogComponent> dialog)
        {
            dialog.component.Clear();
        }

        [OnEventFire]
        public void GoToUrl(GoToUrlToPayEvent e, Node any, [JoinAll] ICollection<SingleNode<PurchaseDialogComponent>> dialogs)
        {
            foreach (SingleNode<PurchaseDialogComponent> node in dialogs)
            {
                node.component.HandleGoToLink();
            }
        }

        [OnEventFire]
        public void QiwiError(InvalidQiwiAccountEvent e, Node any, [JoinAll] ICollection<SingleNode<PurchaseDialogComponent>> dialogs)
        {
            foreach (SingleNode<PurchaseDialogComponent> node in dialogs)
            {
                node.component.HandleQiwiError();
            }
        }

        [OnEventFire]
        public void ShowShopDialog(ButtonClickEvent e, PurchaseButtonNode node, [JoinAll] SingleNode<Dialogs60Component> dialogs)
        {
            ShopDialogs dialogs2 = dialogs.component.Get<ShopDialogs>();
            node.purchaseDialog.shopDialogs = dialogs2;
            node.purchaseDialog.ShowDialog(node.purchaseButton.GoodsEntity);
        }

        [OnEventComplete]
        public void SteamComponentAdded(NodeAddedEvent e, SingleNode<SteamComponent> steam, [Context, JoinAll] ICollection<SingleNode<PurchaseDialogComponent>> dialogs)
        {
            foreach (SingleNode<PurchaseDialogComponent> node in dialogs)
            {
                node.component.SteamComponentIsPresent = true;
            }
        }

        [OnEventFire]
        public void Success(SuccessPaymentEvent e, SingleNode<PaymentMethodComponent> payment, [JoinAll] ICollection<SingleNode<PurchaseDialogComponent>> dialogs)
        {
            foreach (SingleNode<PurchaseDialogComponent> node in dialogs)
            {
                node.component.HandleSuccess();
            }
        }

        [OnEventFire]
        public void SuccessMobile(SuccessMobilePaymentEvent e, SingleNode<PaymentMethodComponent> payment, [JoinAll] ICollection<SingleNode<PurchaseDialogComponent>> dialogs)
        {
            foreach (SingleNode<PurchaseDialogComponent> node in dialogs)
            {
                node.component.HandleSuccessMobile(e.TransactionId);
            }
        }

        public class PurchaseButtonNode : Node
        {
            public PurchaseButtonComponent purchaseButton;
            public PurchaseDialogComponent purchaseDialog;
        }
    }
}

