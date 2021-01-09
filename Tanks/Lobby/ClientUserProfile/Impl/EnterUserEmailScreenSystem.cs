namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientNavigation.API;

    public class EnterUserEmailScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void GoToEnterCodeScreen(NodeAddedEvent e, ScreenNode screen, SingleNode<RestorePasswordCodeSentComponent> email)
        {
            base.ScheduleEvent<ShowScreenLeftEvent<EnterConfirmationCodeScreenComponent>>(screen);
        }

        [OnEventFire]
        public void RequestRestore(ButtonClickEvent e, SingleNode<ContinueButtonComponent> button, [JoinByScreen] SingleNode<EnterUserEmailScreenComponent> screen, [JoinByScreen] SingleNode<InputFieldComponent> emailInput, [JoinAll] SingleNode<ClientSessionComponent> session)
        {
            RestorePasswordByEmailEvent eventInstance = new RestorePasswordByEmailEvent {
                Email = emailInput.component.Input
            };
            base.ScheduleEvent(eventInstance, session);
            screen.Entity.AddComponent<LockedScreenComponent>();
        }

        [OnEventFire]
        public void SwitchToRestorePassword(ButtonClickEvent e, SingleNode<RestorePasswordLinkComponent> node, [JoinAll] SingleNode<ClientSessionComponent> session)
        {
            if (session.Entity.HasComponent<RestorePasswordCodeSentComponent>())
            {
                base.ScheduleEvent<ShowScreenLeftEvent<EnterConfirmationCodeScreenComponent>>(node);
            }
            else
            {
                base.ScheduleEvent<ShowScreenLeftEvent<EnterUserEmailScreenComponent>>(node);
            }
        }

        [OnEventFire]
        public void UnlockScreen(EmailInvalidEvent e, SingleNode<ClientSessionComponent> clientSession, [JoinAll] SingleNode<EnterUserEmailScreenComponent> screen, [JoinByScreen] SingleNode<ContinueButtonComponent> button)
        {
            screen.Entity.RemoveComponentIfPresent<LockedScreenComponent>();
        }

        public class ScreenNode : Node
        {
            public EnterUserEmailScreenComponent enterUserEmailScreen;
            public LockedScreenComponent lockedScreen;
        }
    }
}

