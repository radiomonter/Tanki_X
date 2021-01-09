namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientEntrance.Impl;

    public class EmailInputValidationSystem : InputValidationSystem
    {
        [OnEventComplete]
        public void EmailInvalid(EmailInvalidEvent e, Node node, [JoinAll] EmailInputFieldNode emailInputField)
        {
            if (string.Equals(e.Email, emailInputField.Input, StringComparison.InvariantCultureIgnoreCase))
            {
                emailInputField.ToInvalidState(emailInputField.marker.EmailIsInvalid);
            }
        }

        [OnEventComplete]
        public void EmailOccupied(EmailOccupiedEvent e, Node node, [JoinAll] EmailInputFieldNode emailInputField)
        {
            if (string.Equals(e.Email, emailInputField.Input, StringComparison.InvariantCultureIgnoreCase))
            {
                if (emailInputField.marker.ExistsIsValid)
                {
                    emailInputField.ToValidState();
                }
                else
                {
                    emailInputField.ToInvalidState(emailInputField.marker.EmailIsOccupied);
                }
            }
        }

        [OnEventComplete]
        public void EmailVacant(EmailVacantEvent e, Node node, [JoinAll] EmailInputFieldNode emailInputField)
        {
            if (string.Equals(e.Email, emailInputField.Input, StringComparison.InvariantCultureIgnoreCase))
            {
                if (emailInputField.marker.ExistsIsValid)
                {
                    emailInputField.ToInvalidState(emailInputField.marker.EmailIsNotConfirmed);
                }
                else
                {
                    emailInputField.ToValidState();
                }
            }
        }

        [OnEventFire]
        public void SetNormalStateWhenEmailInputChanged(InputFieldValueChangedEvent e, EmailInputFieldNode emailInputField)
        {
            emailInputField.ToNormalState();
        }

        [OnEventFire]
        public void ValidateEmail(InputPausedEvent e, EmailInputFieldNode emailInputField, [JoinAll] SingleNode<EntranceValidationRulesComponent> rules, [JoinAll] SingleNode<ClientSessionComponent> clientSession)
        {
            if (string.IsNullOrEmpty(emailInputField.Input))
            {
                emailInputField.ToNormalState();
            }
            else if (!rules.component.IsEmailValid(emailInputField.Input))
            {
                emailInputField.ToInvalidState(emailInputField.marker.EmailIsInvalid);
            }
            else
            {
                emailInputField.ToAwaitState();
                base.ScheduleEvent(new CheckEmailEvent(emailInputField.Input, emailInputField.marker.IncludeUnconfirmed), clientSession);
            }
        }

        public class EmailInputFieldNode : InputValidationSystem.BaseInputFieldNode<EmailInputFieldComponent>
        {
        }
    }
}

