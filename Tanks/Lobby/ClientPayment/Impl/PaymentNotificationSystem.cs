namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using System;
    using Tanks.Lobby.ClientUserProfile.API;

    public class PaymentNotificationSystem : ECSSystem
    {
        [OnEventFire]
        public void CreateNotification(NodeAddedEvent e, PaymentNotificationNode notification)
        {
            NotificationMessageComponent component = new NotificationMessageComponent {
                Message = string.Format(notification.paymentNotificationText.MessageTemplate, notification.paymentNotification.Amount)
            };
            notification.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void CreateNotification(NodeAddedEvent e, SaleEndNotificationNode notification)
        {
            NotificationMessageComponent component = new NotificationMessageComponent {
                Message = string.Format(notification.saleEndNotificationText.Message, new object[0])
            };
            notification.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void CreateNotification(NodeAddedEvent e, SpecialOfferNotificationNode notification)
        {
            NotificationMessageComponent component = new NotificationMessageComponent {
                Message = string.Format(notification.paymentNotificationText.MessageTemplate, notification.specialOfferNotification.OfferName)
            };
            notification.Entity.AddComponent(component);
        }

        public class PaymentNotificationNode : Node
        {
            public PaymentNotificationComponent paymentNotification;
            public PaymentNotificationTextComponent paymentNotificationText;
            public ResourceDataComponent resourceData;
        }

        public class SaleEndNotificationNode : Node
        {
            public SaleEndNotificationTextComponent saleEndNotificationText;
            public ResourceDataComponent resourceData;
        }

        public class SpecialOfferNotificationNode : Node
        {
            public SpecialOfferNotificationComponent specialOfferNotification;
            public PaymentNotificationTextComponent paymentNotificationText;
            public ResourceDataComponent resourceData;
        }
    }
}

