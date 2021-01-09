namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientNavigation.API;

    public class EnterConfirmationCodeScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void Init(NodeAddedEvent e, SingleNode<EnterConfirmationCodeScreenComponent> screen, SingleNode<RestorePasswordCodeSentComponent> data, [JoinAll] SingleNode<EmailConfirmationCodeConfigComponent> emailSendConfig)
        {
            EnterConfirmationCodeScreenComponent component = screen.component;
            component.ConfirmationHintWithUserEmail = component.ConfirmationHint.Replace("%EMAIL%", string.Format("<color=#{1}>{0}</color>", data.component.Email, component.EmailColor.ToHexString()));
            screen.component.ResetSendAgainTimer(emailSendConfig.component.EmailSendThresholdInSeconds);
        }

        [OnEventFire]
        public void InvalidCode(RestorePasswordCodeInvalidEvent e, SingleNode<ClientSessionComponent> session, [JoinAll] SingleNode<EnterConfirmationCodeScreenComponent> screen, [JoinByScreen] CodeInputNode code)
        {
            code.interactivityPrerequisiteEsm.Esm.ChangeState<InteractivityPrerequisiteStates.NotAcceptableState>();
            code.esm.Esm.ChangeState<InputFieldStates.InvalidState>();
            code.inputField.ErrorMessage = screen.component.InvalidCodeMessage;
            screen.Entity.RemoveComponent<LockedScreenComponent>();
        }

        [OnEventFire]
        public void SendAgain(ButtonClickEvent e, SingleNode<SendAgainButtonComponent> button, [JoinByScreen] SingleNode<EnterConfirmationCodeScreenComponent> screen, [JoinAll] SingleNode<RestorePasswordCodeSentComponent> session, [JoinAll] SingleNode<EmailConfirmationCodeConfigComponent> emailSendConfig)
        {
            base.ScheduleEvent<SendAgainRestorePasswordEvent>(session);
            screen.component.ResetSendAgainTimer(emailSendConfig.component.EmailSendThresholdInSeconds);
        }

        [OnEventFire]
        public void SendCode(ButtonClickEvent e, SingleNode<ContinueButtonComponent> button, [JoinByScreen] CodeInputNode code, [JoinByScreen] SingleNode<EnterConfirmationCodeScreenComponent> screen, [JoinAll] SingleNode<ClientSessionComponent> session)
        {
            CheckRestorePasswordCodeEvent eventInstance = new CheckRestorePasswordCodeEvent {
                Code = code.inputField.Input.Trim()
            };
            base.ScheduleEvent(eventInstance, session);
            screen.Entity.AddComponent<LockedScreenComponent>();
        }

        [OnEventFire]
        public void ToNormalState(InputFieldValueChangedEvent e, CodeInputNode node, [JoinByScreen] SingleNode<EnterConfirmationCodeScreenComponent> screen)
        {
            if (string.IsNullOrEmpty(node.inputField.Input))
            {
                node.interactivityPrerequisiteEsm.Esm.ChangeState<InteractivityPrerequisiteStates.NotAcceptableState>();
            }
            else
            {
                node.interactivityPrerequisiteEsm.Esm.ChangeState<InteractivityPrerequisiteStates.AcceptableState>();
            }
            node.esm.Esm.ChangeState<InputFieldStates.NormalState>();
        }

        [OnEventFire]
        public void ValidCode(RestorePasswordCodeValidEvent e, SingleNode<ClientSessionComponent> session, [JoinAll] SingleNode<EnterConfirmationCodeScreenComponent> screen, [JoinByScreen] CodeInputNode code)
        {
            base.ScheduleEvent<ShowScreenLeftEvent<EnterNewPasswordScreenComponent>>(screen);
        }

        public class CodeInputNode : Node
        {
            public ESMComponent esm;
            public InteractivityPrerequisiteESMComponent interactivityPrerequisiteEsm;
            public InputFieldComponent inputField;
        }
    }
}

