namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientEntrance.Impl;

    public class SteamSystem : ECSSystem
    {
        [OnEventFire]
        public void CheckDLC(NodeAddedEvent e, SingleNode<SelfUserComponent> user, [JoinAll] SingleNode<SteamComponent> steam)
        {
            if (!string.IsNullOrEmpty(SteamComponent.Ticket))
            {
                CheckSteamDlcEvent eventInstance = new CheckSteamDlcEvent(steam.component.SteamID, SteamComponent.Ticket);
                base.NewEvent(eventInstance).Attach(user).Schedule();
            }
        }

        [OnEventFire]
        public void CheckDLC(RequestCheckSteamDlcInstalledEvent e, SingleNode<SteamComponent> steam, [JoinAll] SingleNode<SelfUserComponent> user)
        {
            CheckSteamDlcEvent eventInstance = new CheckSteamDlcEvent(steam.component.SteamID, SteamComponent.Ticket);
            base.NewEvent(eventInstance).Attach(user).Schedule();
        }

        [OnEventFire]
        public void OnAuthSessionRecieved(SteamAuthSessionRecievedEvent @event, SingleNode<SteamComponent> steam)
        {
            if (@event.GoToPayment)
            {
                RetryGoToPaymentRequestEvent eventInstance = new RetryGoToPaymentRequestEvent();
                base.ScheduleEvent(eventInstance, steam);
            }
            else
            {
                RequestCheckSteamDlcInstalledEvent eventInstance = new RequestCheckSteamDlcInstalledEvent();
                base.ScheduleEvent(eventInstance, steam);
            }
        }

        [OnEventFire]
        public void OnMicroTxnAuthorizationResponse(MicroTxnAuthorizationResponseEvent e, SingleNode<SteamComponent> steam, [JoinAll] SingleNode<SelfUserComponent> user)
        {
            base.NewEvent(new SteamFinalizeTransactionEvent(e.OrderId, e.AppId, e.Autorized)).Attach(user).Schedule();
        }

        [OnEventFire]
        public void OnSteam(NodeAddedEvent e, SingleNode<SteamComponent> steam, SingleNode<SelfUserComponent> user)
        {
            if (!string.IsNullOrEmpty(SteamComponent.Ticket))
            {
                CheckTicketRequestEvent eventInstance = new CheckTicketRequestEvent(steam.component.SteamID, SteamComponent.Ticket);
                base.NewEvent(eventInstance).Attach(user).Schedule();
            }
        }

        [OnEventFire]
        public void OnSteam(RequestRetrySteamAuthTicketEvent e, SingleNode<ClientSessionComponent> clientSession, [JoinAll] SingleNode<SteamComponent> steam)
        {
            steam.component.GetTicket(e.GoToPayment);
        }

        [OnEventFire]
        public void Steam(GoToPaymentRequestEvent e, SingleNode<UserComponent> user, [JoinAll] SingleNode<SteamComponent> steam)
        {
            e.SteamIsActive = true;
            if (string.IsNullOrEmpty(SteamComponent.Ticket) || string.IsNullOrEmpty(steam.component.SteamID))
            {
                base.ScheduleEvent<OpenGameCurrencyPaymentSectionEvent>(user);
            }
            else
            {
                e.SteamId = steam.component.SteamID;
                e.Ticket = SteamComponent.Ticket;
            }
        }

        [OnEventFire]
        public void Steam(RetryGoToPaymentRequestEvent e, SingleNode<SteamComponent> steam, [JoinAll] SingleNode<SelfUserComponent> user)
        {
            base.ScheduleEvent(new GoToPaymentRequestEvent(), user);
        }
    }
}

