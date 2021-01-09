namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientEntrance.Impl;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientPayment.API;
    using Tanks.Lobby.ClientProfile.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using Tanks.Lobby.ClientUserProfile.Impl;
    using UnityEngine;

    public class ProfileAccountSectionUISystem : ECSSystem
    {
        [OnEventFire]
        public void BlockEmailConfirmationNotification(NodeAddedEvent e, SingleNode<EmailConfirmationNotificationComponent> notification, [JoinAll] SingleNode<ForceEnterEmailDialogComponent> dialog)
        {
            base.DeleteEntity(notification.Entity);
        }

        [OnEventFire]
        public void ChangeNickname(ButtonClickEvent e, SingleNode<ChangeNicknameButtonComponent> changeNicknameButton, [JoinAll] LoginInputFieldValidStateNode inputField, [JoinAll] SelfUserXMoneyNode selfUserXMoney, [JoinAll] ChangeUIDNode changeUID, [JoinAll] SingleNode<Dialogs60Component> dialogs)
        {
            long price = changeUID.goodsXPrice.Price;
            bool flag = IsFreeNickChange(selfUserXMoney);
            if ((selfUserXMoney.userXCrystals.Money < price) && !flag)
            {
                dialogs.component.Get<NicknameChangeDialog>().Hide();
                ShopTabManager.shopTabIndex = 3;
                MainScreenComponent.Instance.ShowHome();
                MainScreenComponent.Instance.ShowShopIfNotVisible();
            }
            else
            {
                if (flag)
                {
                    price = 0L;
                }
                BuyUIDChangeEvent eventInstance = new BuyUIDChangeEvent {
                    Uid = inputField.inputField.Input,
                    Price = price
                };
                base.ScheduleEvent(eventInstance, selfUserXMoney);
            }
        }

        [OnEventFire]
        public void CompleteBuyUIDChange(CompleteBuyUIDChangeEvent e, SelfUserNode userNode, [JoinAll] ScreenNode screen, [JoinAll] SingleNode<ChangeNicknameButtonComponent> changeNicknameButton, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] Optional<SingleNode<UserProfileUI>> profileUI)
        {
            dialogs.component.Get<NicknameChangeDialog>().Hide();
            if (e.Success)
            {
                screen.profileAccountSectionUI.UserNickname = userNode.userUid.Uid;
                if (profileUI.IsPresent())
                {
                    profileUI.Get().component.UpdateNickname();
                }
            }
        }

        [OnEventFire]
        public void EmailChanged(ConfirmedUserEmailChangedEvent e, SelfUserWithConfirmedEmailNode user, [JoinAll] Optional<SelfUserWithUnconfirmedEmailNode> unconfirmedOptional, [JoinAll] ScreenNode screen)
        {
            string format = "%EMAIL%";
            string email = user.confirmedUserEmail.Email;
            string unconfirmedEmail = string.Empty;
            if (unconfirmedOptional.IsPresent())
            {
                string unconfirmedEmailFormatText = this.GetUnconfirmedEmailFormatText(screen.profileAccountSectionUI);
                format = format + "\n" + unconfirmedEmailFormatText;
                unconfirmedEmail = unconfirmedOptional.Get().unconfirmedUserEmail.Email;
            }
            screen.profileAccountSectionUI.SetEmail(format, email, unconfirmedEmail);
        }

        [OnEventFire]
        public void Exit(DialogDeclineEvent e, SingleNode<ExitGameDialog> exitDialog, [JoinAll] SingleNode<RequireEnterEmailComponent> require, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            ForceEnterEmailDialogComponent dialog = dialogs.component.Get<ForceEnterEmailDialogComponent>();
            this.ShowDialog(dialog, screens);
        }

        [OnEventFire]
        public void ExitFromGame(ButtonClickEvent e, SingleNode<ExitTheGameButtonComponent> exitButton, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            List<Animator> animators = new List<Animator>();
            if (screens.IsPresent())
            {
                animators = screens.Get().component.Animators;
            }
            dialogs.component.Get<ForceEnterEmailDialogComponent>().Hide();
            dialogs.component.Get<ExitGameDialog>().Show(animators);
        }

        private static string getColorFormattedEmail(ProfileAccountSectionUIComponent screen, string type = "%EMAIL%") => 
            string.Format("<color=#{1}>{0}</color>", type, screen.EmailColor.ToHexString());

        private string GetConfirmedEmailFormatText(ProfileAccountSectionUIComponent screen)
        {
            string newValue = getColorFormattedEmail(screen, "%EMAIL%");
            return $"<size={screen.EmailMessageSize}>{screen.UnconfirmedLocalization.Value.Replace("%EMAIL%", newValue)}</size>";
        }

        private string GetUnconfirmedEmailFormatText(ProfileAccountSectionUIComponent screen)
        {
            string newValue = getColorFormattedEmail(screen, "%UNCEMAIL%");
            return $"<size={screen.EmailMessageSize}>{screen.UnconfirmedLocalization.Value.Replace("%EMAIL%", newValue)}</size>";
        }

        [OnEventFire]
        public void GoToEmailScreen(NotificationClickEvent e, SingleNode<EmailConfirmationNotificationComponent> notification, [JoinAll] SingleNode<SelfUserComponent> selfUser, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            if (!selfUser.Entity.HasComponent<UnconfirmedUserEmailComponent>())
            {
                ChangeEmailDialogComponent dialog = dialogs.component.Get<ChangeEmailDialogComponent>();
                this.ShowDialog(dialog, screens);
            }
            else
            {
                EmailConfirmDialog dialog = dialogs.component.Get<EmailConfirmDialog>();
                dialog.ShowEmailConfirm(selfUser.Entity.GetComponent<UnconfirmedUserEmailComponent>().Email);
                this.ShowDialog(dialog, screens);
            }
        }

        [OnEventFire]
        public void GoToForum(ButtonClickEvent e, SingleNode<ForumButtonComponent> button)
        {
            Application.OpenURL(button.component.forumUrl.Value);
        }

        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, ScreenNode screen, [JoinAll] SelfUserNode selfUser)
        {
            screen.profileAccountSectionUI.UserNickname = selfUser.userUid.Uid;
        }

        private static bool IsFreeNickChange(SelfUserXMoneyNode selfUserXMoney) => 
            selfUserXMoney.Entity.HasComponent<SteamUserComponent>() && selfUserXMoney.Entity.GetComponent<SteamUserComponent>().FreeUidChange;

        [OnEventFire]
        public void OpenChangeCountryDialog(ButtonClickEvent e, SingleNode<OpenSelectCountryButtonComponent> button, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            SelectCountryDialogComponent dialog = dialogs.component.Get<SelectCountryDialogComponent>();
            this.ShowDialog(dialog, screens);
        }

        [OnEventFire]
        public void Proceed(EmailVacantEvent e, Node any, [JoinAll] LockedForceChangeEmailDialog lockedDialog)
        {
            lockedDialog.forceEnterEmailDialog.Hide();
        }

        [OnEventFire]
        public void Proceed(EmailVacantEvent e, Node any, [JoinAll] LockedChangeEmailDialog lockedDialog, [JoinAll] SingleNode<SelfUserComponent> user)
        {
            string email = !user.Entity.HasComponent<ConfirmedUserEmailComponent>() ? e.Email : user.Entity.GetComponent<ConfirmedUserEmailComponent>().Email;
            lockedDialog.changeEmailDialog.ShowEmailConfirm(email);
        }

        [OnEventFire]
        public void RequestChangeEmail(ButtonClickEvent e, SingleNode<ChangeUserEmailButtonComponent> button, [JoinByScreen] EmailInputNode emailInput, [JoinAll] SingleNode<SelfUserComponent> selfUser, [JoinAll] SingleNode<ChangeEmailDialogComponent> dialog)
        {
            base.ScheduleEvent(new RequestChangeUserEmailEvent(emailInput.inputField.Input), selfUser);
            if (!dialog.Entity.HasComponent<LockedScreenComponent>())
            {
                dialog.Entity.AddComponent<LockedScreenComponent>();
            }
        }

        [OnEventFire]
        public void RequestChangeEmail(ButtonClickEvent e, SingleNode<ChangeUserEmailButtonComponent> button, [JoinByScreen] EmailInputNode emailInput, [JoinAll] SingleNode<SelfUserComponent> selfUser, [JoinAll] SingleNode<ForceEnterEmailDialogComponent> dialog)
        {
            base.ScheduleEvent(new RequestChangeUserEmailEvent(emailInput.inputField.Input), selfUser);
            if (!dialog.Entity.HasComponent<LockedScreenComponent>())
            {
                dialog.Entity.AddComponent<LockedScreenComponent>();
            }
        }

        [OnEventFire]
        public void SendAgain(ButtonClickEvent e, SingleNode<SendAgainButtonComponent> button, [JoinAll] SelfUserWithUnconfirmedEmailNode user)
        {
            base.ScheduleEvent<RequestSendAgainConfirmationEmailEvent>(user);
        }

        [OnEventComplete]
        public void SetActiveHint(NodeAddedEvent e, SingleNode<ChangeEmailDialogComponent> screen, [JoinAll] Optional<SelfUserWithConfirmedEmailNode> user)
        {
            screen.component.SetActiveHint(!user.IsPresent());
        }

        [OnEventFire]
        public void SetChangeNicknamePrice(NodeAddedEvent e, SingleNode<ChangeNicknameButtonComponent> changeNicknameButton, [JoinAll] ChangeUIDNode changeUID, [JoinAll] SelfUserXMoneyNode selfUserXMoney)
        {
            if (IsFreeNickChange(selfUserXMoney))
            {
                changeNicknameButton.component.XPrice = "0";
                changeNicknameButton.component.Enough = true;
            }
            else
            {
                changeNicknameButton.component.XPrice = changeUID.goodsXPrice.Price.ToString();
                changeNicknameButton.component.Enough = selfUserXMoney.userXCrystals.Money >= changeUID.goodsXPrice.Price;
            }
        }

        [OnEventFire]
        public void ShowChangeEmailDialog(ButtonClickEvent e, SingleNode<ShowChangeEmailDialogButtonComponent> changeNicknameButton, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            this.ShowDialog(dialogs.component.Get<ChangeEmailDialogComponent>(), screens);
        }

        [OnEventFire]
        public void ShowChangeNicknameDialog(ButtonClickEvent e, SingleNode<StartChangeNicknameButtonComponent> changeNicknameButton, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            this.ShowDialog(dialogs.component.Get<NicknameChangeDialog>(), screens);
        }

        private void ShowDialog(ConfirmDialogComponent dialog, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            List<Animator> animators = new List<Animator>();
            if (screens.IsPresent())
            {
                animators = screens.Get().component.Animators;
            }
            dialog.Show(animators);
        }

        [OnEventComplete]
        public void ShowForceEnterEmailDialog(NodeAddedEvent e, SelfUserRequiredEmailNode require, [JoinAll, Context] MainScreenNode mainScreen, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            ForceEnterEmailDialogComponent dialog = dialogs.component.Get<ForceEnterEmailDialogComponent>();
            this.ShowDialog(dialog, screens);
        }

        [OnEventFire]
        public void ShowPromocodeDialog(ButtonClickEvent e, SingleNode<ShowPromocodeDialogButtonComponent> showPromocodeButton, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            this.ShowDialog(dialogs.component.Get<PromocodeDialog>(), screens);
        }

        [OnEventFire]
        public void UnlockScreen(EmailInvalidEvent e, Node any, [JoinAll] LockedChangeEmailDialog screen, [JoinByScreen] EmailInputNode emailInput)
        {
            if (screen.Entity.HasComponent<LockedScreenComponent>())
            {
                screen.Entity.RemoveComponent<LockedScreenComponent>();
            }
            emailInput.esm.Esm.ChangeState<InputFieldStates.InvalidState>();
        }

        [OnEventFire]
        public void UnlockScreen(EmailOccupiedEvent e, Node any, [JoinAll] LockedChangeEmailDialog screen, [JoinByScreen] EmailInputNode emailInput)
        {
            if (screen.Entity.HasComponent<LockedScreenComponent>())
            {
                screen.Entity.RemoveComponent<LockedScreenComponent>();
            }
            emailInput.esm.Esm.ChangeState<InputFieldStates.InvalidState>();
        }

        [OnEventFire]
        public void ViewConfirmedEmail(NodeAddedEvent e, ScreenNode screen, SelfUserWithConfirmedEmailNode user, [JoinAll] Optional<SelfUserWithUnconfirmedEmailNode> unconfirmedOptional)
        {
            string format = "%EMAIL%";
            string email = user.confirmedUserEmail.Email;
            string unconfirmedEmail = string.Empty;
            if (unconfirmedOptional.IsPresent())
            {
                string str4 = this.GetConfirmedEmailFormatText(screen.profileAccountSectionUI) + " " + getColorFormattedEmail(screen.profileAccountSectionUI, "%UNCEMAIL%");
                format = format + "\n" + str4;
                unconfirmedEmail = unconfirmedOptional.Get().unconfirmedUserEmail.Email;
            }
            screen.profileAccountSectionUI.SetEmail(format, email, unconfirmedEmail);
        }

        [OnEventFire]
        public void ViewUnconfirmedEmail(NodeAddedEvent e, ScreenNode screen, SelfUserWithUnconfirmedEmailNode userEmail, [JoinAll] Optional<SelfUserWithConfirmedEmailNode> confirmedOptional)
        {
            string unconfirmedEmailFormatText = this.GetUnconfirmedEmailFormatText(screen.profileAccountSectionUI);
            string email = string.Empty;
            string unconfirmedEmail = userEmail.unconfirmedUserEmail.Email;
            if (confirmedOptional.IsPresent())
            {
                unconfirmedEmailFormatText = "%EMAIL%\n" + this.GetConfirmedEmailFormatText(screen.profileAccountSectionUI) + " " + getColorFormattedEmail(screen.profileAccountSectionUI, "%UNCEMAIL%");
                email = confirmedOptional.Get().confirmedUserEmail.Email;
            }
            screen.profileAccountSectionUI.SetEmail(unconfirmedEmailFormatText, email, unconfirmedEmail);
        }

        public class ChangeUIDNode : Node
        {
            public ChangeUIDComponent changeUid;
            public GoodsXPriceComponent goodsXPrice;
        }

        public class EmailInputNode : Node
        {
            public ESMComponent esm;
            public InputFieldComponent inputField;
            public EmailInputFieldComponent emailInputField;
            public InputFieldValidStateComponent inputFieldValidState;
        }

        public class LockedChangeEmailDialog : Node
        {
            public ChangeEmailDialogComponent changeEmailDialog;
            public LockedScreenComponent lockedScreen;
        }

        public class LockedForceChangeEmailDialog : Node
        {
            public ForceEnterEmailDialogComponent forceEnterEmailDialog;
            public LockedScreenComponent lockedScreen;
        }

        public class LoginInputFieldValidStateNode : Node
        {
            public InputFieldComponent inputField;
            public RegistrationLoginInputComponent registrationLoginInput;
            public InputFieldValidStateComponent inputFieldValidState;
        }

        public class MainScreenNode : Node
        {
            public MainScreenComponent mainScreen;
            public ActiveScreenComponent activeScreen;
        }

        public class ScreenNode : Node
        {
            public ProfileAccountSectionUIComponent profileAccountSectionUI;
        }

        public class SelfUserNode : Node
        {
            public UserComponent user;
            public UserUidComponent userUid;
            public SelfUserComponent selfUser;
        }

        public class SelfUserRequiredEmailNode : Node
        {
            public SelfUserComponent selfUser;
            public RequireEnterEmailComponent requireEnterEmail;
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

        public class SelfUserXMoneyNode : ProfileAccountSectionUISystem.SelfUserNode
        {
            public UserXCrystalsComponent userXCrystals;
        }
    }
}

