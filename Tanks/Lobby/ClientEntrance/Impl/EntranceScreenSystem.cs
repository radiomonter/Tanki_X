namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using Platform.System.Data.Exchange.ClientNetwork.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientNavigation.API;

    public class EntranceScreenSystem : ECSSystem
    {
        private void AddAwaitingTokenComponent(string password, Entity clientSession)
        {
            clientSession.AddComponent(new AutoLoginTokenAwaitingComponent(PasswordSecurityUtils.GetDigest(password)));
        }

        [OnEventFire]
        public void ClearCaptchInput(UpdateCaptchaEvent e, Node captcha, [JoinByScreen] CaptchaInputFieldNode captchInput)
        {
            captchInput.inputField.Input = string.Empty;
        }

        [OnEventFire]
        public void ClientLoginSuccessful(NodeAddedEvent e, SelfUserNode selfUser, [JoinByUser] ClientSessionNode clientSessionNode, [JoinAll] SingleNode<TopPanelComponent> topPanel)
        {
            topPanel.Entity.AddComponent<TopPanelAuthenticatedComponent>();
        }

        [OnEventFire]
        public void ClientPassRegistration(NodeAddedEvent e, SelfUserNode selfUser, [JoinByUser] ClientSessionNode clientSessionNode)
        {
            base.ScheduleEvent<ShowLobbyScreenEvent>(selfUser);
        }

        [OnEventFire]
        public void EnableCaptcha(NodeAddedEvent e, ClientSessionWithCaptchaNode sessionWithCaptcha, SingleNode<EntranceScreenComponent> entranceScreen)
        {
            entranceScreen.component.ActivateCaptcha();
        }

        [OnEventFire]
        public void FillCaptcha(IntroduceUserEvent e, ClientSessionWithCaptchaNode clientSession, [JoinAll] CaptchaInputFieldNode captchaInput)
        {
            e.Captcha = captchaInput.inputField.Input;
        }

        [OnEventFire]
        public void IntroduceUserToServer(ButtonClickEvent e, SingleNode<LoginButtonComponent> loginButton, [JoinByScreen] SingleNode<EntranceScreenComponent> entranceScreen, [JoinAll] ClientSessionRegularNode clientSession)
        {
            entranceScreen.component.SetWaitIndicator(true);
            entranceScreen.component.LockScreen(true);
            string login = entranceScreen.component.Login;
            if (login.Contains("@"))
            {
                IntroduceUserByEmailEvent eventInstance = new IntroduceUserByEmailEvent {
                    Email = login
                };
                base.ScheduleEvent(eventInstance, clientSession);
            }
            else
            {
                IntroduceUserByUidEvent eventInstance = new IntroduceUserByUidEvent {
                    Uid = login
                };
                base.ScheduleEvent(eventInstance, clientSession);
            }
        }

        [OnEventFire]
        public void RemoveAutoLoginData(LoginFailedEvent e, SingleNode<AutoLoginTokenAwaitingComponent> clientSession)
        {
            clientSession.Entity.RemoveComponent<AutoLoginTokenAwaitingComponent>();
        }

        [OnEventFire]
        public void RequestNewCaptcha(UpdateCaptchaEvent e, SingleNode<CaptchaComponent> captcha, [JoinAll] SingleNode<ClientSessionComponent> session)
        {
            base.ScheduleEvent<ShowCaptchaWaitAnimationEvent>(captcha);
            base.ScheduleEvent<CaptchaRequestEvent>(session);
        }

        [OnEventFire]
        public void SendPasswordToServer(PersonalPasscodeEvent e, ClientSessionRegularNode clientSession, [JoinAll] SingleNode<EntranceScreenComponent> entranceScreen)
        {
            if (e.Passcode.Length == 0)
            {
                base.Log.Error("Invalid passcode for user: " + entranceScreen.component.Login);
            }
            else
            {
                string password = entranceScreen.component.Password;
                bool rememberMe = entranceScreen.component.RememberMe;
                LoginByPasswordEvent eventInstance = new LoginByPasswordEvent {
                    RememberMe = rememberMe,
                    PasswordEncipher = PasswordSecurityUtils.SaltPassword(e.Passcode, password),
                    HardwareFingerprint = HardwareFingerprintUtils.HardwareFingerprint
                };
                base.ScheduleEvent(eventInstance, clientSession);
                if (rememberMe)
                {
                    this.AddAwaitingTokenComponent(password, clientSession.Entity);
                }
            }
        }

        [OnEventFire]
        public void ServerOfflineStatus(ServerDisconnectedEvent e, SingleNode<EntranceScreenComponent> entranceScreen)
        {
            entranceScreen.component.SetOfflineStatus();
        }

        [OnEventFire]
        public void ServerOnlineStatus(NodeAddedEvent e, SingleNode<ClientSessionComponent> clientSession, SingleNode<EntranceScreenComponent> entranceScreen)
        {
            entranceScreen.component.SetOnlineStatus();
        }

        [OnEventFire]
        public void SwitchToRegistration(ButtonClickEvent e, SingleNode<SwitchToRegistrationButtonComponent> node, [JoinAll] ClientSessionWithoutInvitesNode clientSession)
        {
            base.ScheduleEvent<ShowScreenDownEvent<RegistrationScreenComponent>>(node);
        }

        [OnEventFire]
        public void TurnOffWaitingCover(LoginFailedEvent e, SingleNode<ClientSessionComponent> clientSession, [JoinAll] SingleNode<EntranceScreenComponent> entranceScreen)
        {
            entranceScreen.component.SetWaitIndicator(false);
            entranceScreen.component.LockScreen(false);
        }

        [OnEventFire]
        public void UpdateCaptchaImage(CaptchaImageEvent e, SingleNode<ClientSessionComponent> session, [JoinAll] SingleNode<EntranceScreenComponent> entranceScreen, [JoinByScreen] SingleNode<CaptchaComponent> captcha)
        {
            captcha.Entity.AddComponent(new CaptchaBytesComponent(e.CaptchaBytes));
        }

        public class CaptchaInputFieldNode : Node
        {
            public CaptchaInputFieldComponent captchaInputField;
            public InputFieldComponent inputField;
            public ESMComponent esm;
            public InteractivityPrerequisiteESMComponent interactivityPrerequisiteESM;
        }

        public class ClientSessionNode : Node
        {
            public ClientSessionComponent clientSession;
        }

        public class ClientSessionRegularNode : Node
        {
            public ClientSessionComponent clientSession;
        }

        public class ClientSessionWithCaptchaNode : Node
        {
            public ClientSessionComponent clientSession;
            public CaptchaRequiredComponent captchaRequired;
        }

        [Not(typeof(InviteComponent))]
        public class ClientSessionWithoutInvitesNode : Node
        {
            public ClientSessionComponent clientSession;
        }

        public class SelfUserNode : Node
        {
            public UserComponent user;
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
        }
    }
}

