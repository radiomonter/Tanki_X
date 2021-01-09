namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientEntrance.Impl;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientUserProfile.API;

    public class EnterNewPasswordScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void Continue(ButtonClickEvent e, SingleNode<ContinueButtonComponent> buton, [JoinByScreen] SingleNode<EnterNewPasswordScreenComponent> screen, [JoinByScreen] PasswordInputNode password, [JoinAll] SingleNode<SessionSecurityPublicComponent> session)
        {
            byte[] digest = PasswordSecurityUtils.GetDigest(password.inputField.Input);
            RequestChangePasswordEvent eventInstance = new RequestChangePasswordEvent {
                HardwareFingerprint = HardwareFingerprintUtils.HardwareFingerprint,
                PasswordDigest = PasswordSecurityUtils.RSAEncryptAsString(session.component.PublicKey, digest)
            };
            base.ScheduleEvent(eventInstance, session);
            screen.Entity.AddComponent<LockedScreenComponent>();
            session.Entity.AddComponent(new AutoLoginTokenAwaitingComponent(digest));
        }

        [OnEventFire]
        public void SetUid(NodeAddedEvent e, UidIndicatorNode uidIndicator, [JoinByScreen, Context] ScreenNode screen, [Context] SessionNode session)
        {
            uidIndicator.uidIndicator.Uid = session.restorePasswordUserData.Uid;
        }

        [OnEventFire]
        public void ValidatePassword(InputFieldValueChangedEvent e, PasswordInputNode passwordInput, [JoinByScreen] SingleNode<UidIndicatorComponent> uid, [JoinByScreen] SingleNode<PasswordErrorsComponent> strings)
        {
            if (passwordInput.inputField.Input == uid.component.Uid)
            {
                passwordInput.esm.Esm.ChangeState<InputFieldStates.InvalidState>();
                passwordInput.inputField.ErrorMessage = strings.component.PasswordEqualsLogin;
                passwordInput.interactivityPrerequisiteEsm.Esm.ChangeState<InteractivityPrerequisiteStates.NotAcceptableState>();
            }
        }

        public class PasswordInputNode : Node
        {
            public RegistrationPasswordInputComponent registrationPasswordInput;
            public ESMComponent esm;
            public InteractivityPrerequisiteESMComponent interactivityPrerequisiteEsm;
            public InputFieldComponent inputField;
        }

        public class ScreenNode : Node
        {
            public EnterNewPasswordScreenComponent enterNewPasswordScreen;
            public ScreenGroupComponent screenGroup;
        }

        public class SessionNode : Node
        {
            public RestorePasswordUserDataComponent restorePasswordUserData;
        }

        public class UidIndicatorNode : Node
        {
            public UidIndicatorComponent uidIndicator;
            public ScreenGroupComponent screenGroup;
        }
    }
}

