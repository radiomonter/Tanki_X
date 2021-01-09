namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientNavigation.API;

    public class EntranceInputValidationSystem : InputValidationSystem
    {
        [Mandatory, OnEventFire]
        public void HandleInvalidCaptcha(CaptchaInvalidEvent e, SingleNode<ClientSessionComponent> session, [JoinAll] CaptchaInputFieldNode captchaField, [JoinByScreen] SingleNode<EntranceScreenComponent> entranceScreenText)
        {
            this.SetInvalidAndNotAccetableState(captchaField.inputField, captchaField.esm, entranceScreenText.component.IncorrectCaptcha, captchaField.interactivityPrerequisiteESM);
        }

        [OnEventFire]
        public void HandleInvalidEmail(EmailInvalidEvent e, SingleNode<ClientSessionComponent> session, [JoinAll] LoginInputFieldNode loginInput, [JoinByScreen] SingleNode<EntranceScreenComponent> entranceScreenText)
        {
            this.SetInvalidAndNotAccetableState(loginInput.inputField, loginInput.esm, entranceScreenText.component.IncorrectLogin, loginInput.interactivityPrerequisiteESM);
        }

        [Mandatory, OnEventFire]
        public void HandleInvalidPassword(InvalidPasswordEvent e, SingleNode<ClientSessionComponent> session, [JoinAll] PasswordInputFieldNode passwordInput, [JoinByScreen] SingleNode<EntranceScreenComponent> entranceScreenText)
        {
            this.SetInvalidAndNotAccetableState(passwordInput.inputField, passwordInput.esm, entranceScreenText.component.IncorrectPassword, passwordInput.interactivityPrerequisiteESM);
        }

        [Mandatory, OnEventFire]
        public void HandleInvalidUid(UidInvalidEvent e, SingleNode<ClientSessionComponent> session, [JoinAll] LoginInputFieldNode loginInput, [JoinByScreen] SingleNode<EntranceScreenComponent> entranceScreenText)
        {
            this.SetInvalidAndNotAccetableState(loginInput.inputField, loginInput.esm, entranceScreenText.component.IncorrectLogin, loginInput.interactivityPrerequisiteESM);
        }

        [Mandatory, OnEventFire]
        public void HandleLoginBlocked(UserBlockedEvent e, SingleNode<ClientSessionComponent> session, [JoinAll] LoginInputFieldNode loginInput, [JoinByScreen] SingleNode<EntranceScreenComponent> entranceScreenText)
        {
            this.SetInvalidAndNotAccetableState(loginInput.inputField, loginInput.esm, e.Reason, loginInput.interactivityPrerequisiteESM);
        }

        [Mandatory, OnEventFire]
        public void HandleUnconfirmedEmail(EmailNotConfirmedEvent e, SingleNode<ClientSessionComponent> session, [JoinAll] LoginInputFieldNode loginInput, [JoinByScreen] SingleNode<EntranceScreenComponent> entranceScreenText)
        {
            this.SetInvalidAndNotAccetableState(loginInput.inputField, loginInput.esm, entranceScreenText.component.UnconfirmedEmail, loginInput.interactivityPrerequisiteESM);
        }

        private void SetInvalidAndNotAccetableState(InputFieldComponent inputField, ESMComponent inputESM, string errorMessage, InteractivityPrerequisiteESMComponent interactivityPrerequisiteESM)
        {
            inputESM.Esm.ChangeState<InputFieldStates.InvalidState>();
            inputField.ErrorMessage = errorMessage;
            this.SetNotAcceptableState(interactivityPrerequisiteESM);
        }

        private void SetNotAcceptableState(InteractivityPrerequisiteESMComponent prerequisiteESM)
        {
            prerequisiteESM.Esm.ChangeState<InteractivityPrerequisiteStates.NotAcceptableState>();
        }

        [OnEventFire]
        public void ValidateCaptchaInput(InputFieldValueChangedEvent e, CaptchaInputFieldNode inputField, [JoinAll] SingleNode<EntranceValidationRulesComponent> validationRules)
        {
            this.ValidateInputAfterChanging(inputField.inputField, inputField.esm, inputField.interactivityPrerequisiteESM, validationRules.component.maxCaptchaLength);
        }

        private void ValidateInputAfterChanging(InputFieldComponent input, ESMComponent inputStateESM, InteractivityPrerequisiteESMComponent interactivityPrerequisiteESM, int maxLength)
        {
            string str = input.Input;
            if (string.IsNullOrEmpty(str))
            {
                this.SetNotAcceptableState(interactivityPrerequisiteESM);
            }
            else
            {
                if (str.Length > maxLength)
                {
                    input.Input = str.Remove(maxLength);
                }
                interactivityPrerequisiteESM.Esm.ChangeState<InteractivityPrerequisiteStates.AcceptableState>();
            }
            inputStateESM.Esm.ChangeState<InputFieldStates.NormalState>();
        }

        [OnEventFire]
        public void ValidateLogin(InputFieldValueChangedEvent e, LoginInputFieldNode loginInput, [JoinAll] SingleNode<EntranceValidationRulesComponent> validationRules)
        {
            this.ValidateInputAfterChanging(loginInput.inputField, loginInput.esm, loginInput.interactivityPrerequisiteESM, validationRules.component.maxEmailLength);
        }

        [OnEventFire]
        public void ValidatePassword(InputFieldValueChangedEvent e, PasswordInputFieldNode passwordInput, [JoinAll] SingleNode<EntranceValidationRulesComponent> validationRules)
        {
            this.ValidateInputAfterChanging(passwordInput.inputField, passwordInput.esm, passwordInput.interactivityPrerequisiteESM, validationRules.component.maxPasswordLength);
        }

        public class CaptchaInputFieldNode : InputValidationSystem.BaseInputFieldNode<CaptchaInputFieldComponent>
        {
        }

        public class LoginInputFieldNode : InputValidationSystem.BaseInputFieldNode<LoginInputFieldComponent>
        {
        }

        public class PasswordInputFieldNode : InputValidationSystem.BaseInputFieldNode<PasswordInputFieldComponent>
        {
        }
    }
}

