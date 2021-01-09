﻿namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientEntrance.Impl;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientUserProfile.API;

    public class ConfirmUserEmailScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void ActivateCancelButton(NodeAddedEvent e, SingleNode<ConfirmUserEmailScreenComponent> screen, SelfUserWithConfirmedEmailNode user)
        {
            screen.component.ActivateCancel();
        }

        [OnEventFire]
        public void Cancel(ButtonClickEvent e, SingleNode<CancelButtonComponent> button, [JoinByScreen] SingleNode<ConfirmUserEmailScreenComponent> screen, [Mandatory, JoinAll] SelfUserWithUnconfirmedEmailNode user)
        {
            base.ScheduleEvent<CancelChangeUserEmailEvent>(user);
            base.ScheduleEvent<ShowScreenRightEvent<ViewUserEmailScreenComponent>>(screen);
        }

        [OnEventFire]
        public void GoToChangeUserEmailScreen(EmailOccupiedEvent e, Node any, [JoinAll] LockedConfirmUserEmailScreenNode screen, [JoinByScreen] UserEmailConfirmationCodeInputNode codeInput)
        {
            base.ScheduleEvent<ShowScreenLeftEvent<ChangeUserEmailScreenComponent>>(screen);
        }

        [OnEventFire]
        public void GoToViewUserEmailScreen(NodeAddedEvent e, SelfUserWithConfirmedEmailNode confirmedEmail, [JoinAll] ConfirmUserEmailScreenNode screen)
        {
            base.ScheduleEvent<ShowScreenLeftEvent<ViewUserEmailScreenComponent>>(screen);
        }

        [OnEventFire]
        public void InputToInvalid(UserEmailConfirmationCodeInvalidEvent e, Node any, [JoinAll] LockedConfirmUserEmailScreenNode screen, [JoinByScreen] UserEmailConfirmationCodeInputNode codeInput)
        {
            screen.Entity.RemoveComponent<LockedScreenComponent>();
            codeInput.esm.Esm.ChangeState<InputFieldStates.InvalidState>();
            codeInput.inputField.ErrorMessage = screen.confirmUserEmailScreen.InvalidCodeMessage;
        }

        [OnEventFire]
        public void InsertUserEmail(NodeAddedEvent e, SingleNode<ConfirmUserEmailScreenComponent> screen, SelfUserWithUnconfirmedEmailNode user)
        {
            ConfirmUserEmailScreenComponent component = screen.component;
            component.ConfirmationHintWithUserEmail = component.ConfirmationHint.Replace("%EMAIL%", string.Format("<color=#{1}>{0}</color>", user.unconfirmedUserEmail.Email, component.EmailColor.ToHexString()));
        }

        [OnEventFire]
        public void ToNormalState(InputFieldValueChangedEvent e, UserEmailConfirmationCodeInputNode node)
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
        public void ValidatePassword(InputFieldValueChangedEvent e, InputValidationSystem.BaseInputFieldNode<UserEmailConfirmationCodeInputFieldComponent> passwordInputField, [JoinAll] SingleNode<EmailConfirmationCodeConfigComponent> emailSendConfig)
        {
            string input = passwordInputField.Input;
            int confirmationCodeInputMaxLength = (int) emailSendConfig.component.ConfirmationCodeInputMaxLength;
            if (input.Length > confirmationCodeInputMaxLength)
            {
                passwordInputField.inputField.Input = input.Remove(confirmationCodeInputMaxLength, input.Length - confirmationCodeInputMaxLength);
            }
        }

        public class ConfirmUserEmailScreenNode : Node
        {
            public ConfirmUserEmailScreenComponent confirmUserEmailScreen;
        }

        public class LockedConfirmUserEmailScreenNode : Node
        {
            public ConfirmUserEmailScreenComponent confirmUserEmailScreen;
            public LockedScreenComponent lockedScreen;
        }

        public class SelfUserWithConfirmedEmailNode : Node
        {
            public ConfirmedUserEmailComponent confirmedUserEmail;
            public SelfUserComponent selfUser;
        }

        public class SelfUserWithUnconfirmedEmailNode : Node
        {
            public UnconfirmedUserEmailComponent unconfirmedUserEmail;
            public SelfUserComponent selfUser;
        }

        public class UserEmailConfirmationCodeInputNode : Node
        {
            public UserEmailConfirmationCodeInputFieldComponent userEmailConfirmationCodeInputField;
            public InputFieldComponent inputField;
            public ESMComponent esm;
            public InteractivityPrerequisiteESMComponent interactivityPrerequisiteEsm;
        }
    }
}

