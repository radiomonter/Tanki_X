namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using Platform.System.Data.Exchange.ClientNetwork.API;
    using System;
    using System.IO;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientNavigation.main.csharp.API.Screens;
    using UnityEngine;

    public class RegistrationScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void ClearCheckbox(NodeRemoveEvent e, [Combine] SingleNode<CheckboxComponent> checkbox, [Context, JoinByScreen] SingleNode<RegistrationScreenComponent> registrationScreen)
        {
            checkbox.component.IsChecked = false;
        }

        [OnEventFire]
        public void CompleteQuickRegistration(NodeRemoveEvent e, IncompleteRegUserNode user, [JoinAll] SingleNode<RegistrationScreenComponent> screen)
        {
            PlayerPrefs.SetString("TOLogin", user.userUid.Uid);
        }

        [OnEventFire]
        public void NavigateLicenseAgreement(ButtonClickEvent e, SingleNode<LicenseAgreementLinkComponent> licenseAgreementLink, [JoinAll] SingleNode<RegistrationScreenLocalizedStringsComponent> strings)
        {
            this.NavigateToUrl(strings.component.LicenseAgreementUrl);
        }

        [OnEventFire]
        public void NavigateToGameRules(ButtonClickEvent e, SingleNode<GameRulesLinkComponent> gameRulesLink, [JoinAll] SingleNode<RegistrationScreenLocalizedStringsComponent> strings)
        {
            this.NavigateToUrl(strings.component.GameRulesUrl);
        }

        [OnEventFire]
        public void NavigateToPrivacyPolicy(ButtonClickEvent e, SingleNode<PrivacyPolicyLinkComponent> privacyPolicyLink, [JoinAll] SingleNode<RegistrationScreenLocalizedStringsComponent> strings)
        {
            this.NavigateToUrl(strings.component.PrivacyPolicyUrl);
        }

        private void NavigateToUrl(string url)
        {
            Application.OpenURL(url);
        }

        [OnEventFire]
        public void QuickRegistrationNewUser(ButtonClickEvent e, SingleNode<QuickRegistrationButtonComponent> node, [JoinByScreen] SingleNode<RegistrationScreenComponent> screen, [JoinAll] SecuredClientSessionNode clientSession, [JoinAll] Optional<SingleNode<SteamMarkerComponent>> steam, [JoinAll] Optional<SubscribeCheckboxNode> subscribeCheckbox)
        {
            byte[] digest = PasswordSecurityUtils.GetDigest(Path.GetRandomFileName());
            RequestRegisterUserEvent eventInstance = new RequestRegisterUserEvent {
                Uid = string.Empty,
                Email = string.Empty,
                HardwareFingerprint = HardwareFingerprintUtils.HardwareFingerprint,
                EncryptedPasswordDigest = PasswordSecurityUtils.RSAEncryptAsString(clientSession.sessionSecurityPublic.PublicKey, digest),
                Steam = steam.IsPresent(),
                Subscribed = !subscribeCheckbox.IsPresent() || subscribeCheckbox.Get().checkbox.IsChecked,
                QuickRegistration = true
            };
            base.ScheduleEvent(eventInstance, clientSession);
            clientSession.Entity.AddComponent(new AutoLoginTokenAwaitingComponent(digest));
            screen.component.LockScreen(true);
        }

        [OnEventFire]
        public void RegisterNewUser(ButtonClickEvent e, SingleNode<FinishRegistrationButtonComponent> node, [JoinByScreen] SingleNode<RegistrationScreenComponent> screen, [JoinAll] SecuredClientSessionNode clientSession, [JoinByUser] Optional<SingleNode<UserIncompleteRegistrationComponent>> user, [JoinAll] Optional<SingleNode<SteamMarkerComponent>> steam, [JoinAll] Optional<SubscribeCheckboxNode> subscribeCheckbox)
        {
            RegistrationScreenComponent component = screen.component;
            byte[] digest = PasswordSecurityUtils.GetDigest(component.Password);
            RequestRegisterUserEvent eventInstance = new RequestRegisterUserEvent {
                Uid = component.Uid,
                HardwareFingerprint = HardwareFingerprintUtils.HardwareFingerprint,
                EncryptedPasswordDigest = PasswordSecurityUtils.RSAEncryptAsString(clientSession.sessionSecurityPublic.PublicKey, digest),
                Email = component.Email,
                Steam = steam.IsPresent(),
                Subscribed = !subscribeCheckbox.IsPresent() || subscribeCheckbox.Get().checkbox.IsChecked
            };
            EventBuilder builder = base.NewEvent(eventInstance).Attach(clientSession);
            if (user.IsPresent())
            {
                builder.Attach(user.Get());
            }
            builder.Schedule();
            clientSession.Entity.AddComponent(new AutoLoginTokenAwaitingComponent(digest));
            screen.component.LockScreen(true);
        }

        [OnEventFire]
        public void SetupBackButtonAndExit(NodeAddedEvent e, SingleNode<RegistrationScreenComponent> registration, [JoinAll] Optional<SingleNode<UserIncompleteRegistrationComponent>> user)
        {
            BackButtonComponent componentInChildren = registration.component.GetComponentInChildren<BackButtonComponent>(true);
            ExitButtonComponent component2 = registration.component.GetComponentInChildren<ExitButtonComponent>(true);
            if (user.IsPresent())
            {
                if (componentInChildren)
                {
                    componentInChildren.gameObject.SetActive(true);
                }
                if (component2)
                {
                    component2.gameObject.SetActive(false);
                }
            }
            else
            {
                if (componentInChildren)
                {
                    componentInChildren.gameObject.SetActive(false);
                }
                if (component2)
                {
                    component2.gameObject.SetActive(true);
                }
            }
        }

        [OnEventFire]
        public void SetupQuickRegistration(NodeAddedEvent e, SingleNode<QuickRegistrationButtonComponent> node)
        {
            node.component.GetComponent<Selectable>().gameObject.SetActive(false);
        }

        [OnEventFire]
        public void SwitchToEntrance(ButtonClickEvent e, SingleNode<SwitchToEntranceButtonComponent> node, [JoinAll] SecuredClientSessionNode clientSession, [JoinByUser] Optional<SingleNode<UserIncompleteRegistrationComponent>> user)
        {
            if (!user.IsPresent())
            {
                base.ScheduleEvent<ShowScreenDownEvent<EntranceScreenComponent>>(node);
            }
            else
            {
                PlayerPrefs.DeleteKey("TOLogin");
                PlayerPrefs.DeleteKey("TOToken");
                PlayerPrefs.SetInt("RemeberMeFlag", 0);
                base.ScheduleEvent<SwitchToEntranceSceneEvent>(node);
            }
        }

        [OnEventFire]
        public void UnblockUserInput(NodeAddedEvent e, SingleNode<RegistrationScreenComponent> registrationScreen, [JoinByScreen] FinishRegistrationButtonNode finishRegistrationButton)
        {
            registrationScreen.component.SetUidInputInteractable(true);
            registrationScreen.component.SetEmailInputInteractable(true);
            if (!finishRegistrationButton.dependentInteractivity.prerequisitesObjects.Contains(registrationScreen.component.GetUidInput()))
            {
                finishRegistrationButton.dependentInteractivity.prerequisitesObjects.Add(registrationScreen.component.GetUidInput());
            }
            if (!finishRegistrationButton.dependentInteractivity.prerequisitesObjects.Contains(registrationScreen.component.GetEmailInput()))
            {
                finishRegistrationButton.dependentInteractivity.prerequisitesObjects.Add(registrationScreen.component.GetUidInput());
            }
        }

        [OnEventFire]
        public void UnlockScreenOnFail(RegistrationFailedEvent e, Node any, SingleNode<RegistrationScreenComponent> screen)
        {
            screen.component.LockScreen(false);
        }

        public class FinishRegistrationButtonNode : Node
        {
            public FinishRegistrationButtonComponent finishRegistrationButton;
            public DependentInteractivityComponent dependentInteractivity;
        }

        public class IncompleteRegUserNode : Node
        {
            public UserIncompleteRegistrationComponent userIncompleteRegistration;
            public UserUidComponent userUid;
        }

        public class SecuredClientSessionNode : Node
        {
            public ClientSessionComponent clientSession;
            public SessionSecurityPublicComponent sessionSecurityPublic;
        }

        public class SubscribeCheckboxNode : Node
        {
            public SubscribeCheckboxComponent subscribeCheckbox;
            public CheckboxComponent checkbox;
        }
    }
}

