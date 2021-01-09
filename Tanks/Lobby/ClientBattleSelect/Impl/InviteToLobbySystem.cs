namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientFriends.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientUserProfile.API;

    public class InviteToLobbySystem : ECSSystem
    {
        [OnEventFire]
        public void AcceptInviteAfterExitLobby(NodeRemoveEvent e, LobbyNode lobby, [JoinAll, Combine] SingleNode<WaitingLobbyExitComponent> dialog, [JoinAll] SingleNode<SelfUserComponent> user)
        {
            AcceptInviteEvent acceptInviteEvent = dialog.Entity.GetComponent<WaitingLobbyExitComponent>().AcceptInviteEvent;
            dialog.Entity.RemoveComponent<WaitingLobbyExitComponent>();
            base.ScheduleEvent(acceptInviteEvent, user);
        }

        [OnEventFire]
        public void ExitLobbyOrAcceptInvite(ExitOtherLobbyAndAcceptInviteEvent e, SingleNode<SelfUserComponent> user, [Combine] SingleNode<InviteToLobbyDialogComponent> dialog, [JoinAll] Optional<LobbyNode> lobby)
        {
            if (!lobby.IsPresent() || (lobby.Get().battleLobbyGroup.Key == e.AcceptInviteEvent.lobbyId))
            {
                base.ScheduleEvent(e.AcceptInviteEvent, user);
            }
            else
            {
                WaitingLobbyExitComponent component = new WaitingLobbyExitComponent {
                    AcceptInviteEvent = e.AcceptInviteEvent
                };
                dialog.Entity.AddComponent(component);
                base.ScheduleEvent<ClientExitLobbyEvent>(lobby.Get());
            }
        }

        [OnEventFire]
        public void HideInviteDialogOnEula(NodeAddedEvent e, SingleNode<EulaNotificationComponent> eulaNotification, [Combine] SingleNode<InviteDialogComponent> inviteDialog)
        {
            inviteDialog.component.OnNo();
        }

        [OnEventFire]
        public void HideInviteDialogOnPP(NodeAddedEvent e, SingleNode<PrivacyPolicyNotificationComponent> ppNotification, [Combine] SingleNode<InviteDialogComponent> inviteDialog)
        {
            inviteDialog.component.OnNo();
        }

        private void Invite(List<long> userIds, LobbyNode lobby)
        {
            InviteToLobbyEvent eventInstance = new InviteToLobbyEvent {
                InvitedUserIds = userIds.ToArray()
            };
            base.ScheduleEvent(eventInstance, lobby);
        }

        [OnEventFire]
        public void Invite(ButtonClickEvent e, InviteToLobbyButtonNode button, [JoinByUser] UserNode user, [JoinAll] LobbyNode lobby, [JoinAll] SingleNode<FriendsComponent> friends)
        {
            List<long> userIds = new List<long> {
                user.Entity.Id
            };
            this.Invite(userIds, lobby);
            if (user.Entity.HasComponent<InvitedToLobbyUserComponent>())
            {
                user.Entity.RemoveComponent<InvitedToLobbyUserComponent>();
            }
            friends.component.InLobbyInvitations[user.userGroup.Key] = DateTime.Now;
            user.Entity.AddComponent<InvitedToLobbyUserComponent>();
        }

        [OnEventFire]
        public void OnDialogConfirm(DialogConfirmEvent e, SingleNode<InviteToLobbyDialogComponent> dialog, [JoinAll] SingleNode<SelfUserComponent> user)
        {
            AcceptInviteEvent event3 = new AcceptInviteEvent {
                lobbyId = dialog.component.lobbyId,
                engineId = dialog.component.engineId
            };
            ExitOtherLobbyAndAcceptInviteEvent eventInstance = new ExitOtherLobbyAndAcceptInviteEvent {
                AcceptInviteEvent = event3
            };
            base.NewEvent(eventInstance).Attach(user).Attach(dialog).Schedule();
        }

        [OnEventFire]
        public void ShowInviteDialog(InvitedToLobbyEvent e, SingleNode<SelfUserComponent> user, [JoinAll] DialogsNode dialogs, [JoinAll] Optional<LobbyNode> lobby, [JoinAll] Optional<SingleNode<SelfBattleUserComponent>> selfBattleUser, [JoinAll] Optional<SingleNode<EulaNotificationComponent>> eulaNotification, [JoinAll] Optional<SingleNode<PrivacyPolicyNotificationComponent>> ppNotification)
        {
            InviteToLobbyDialogComponent component = dialogs.dialogs60.Get<InviteToLobbyDialogComponent>();
            if (((((component != null) && (MainScreenComponent.Instance != null)) && !component.gameObject.activeSelf) && (!lobby.IsPresent() || (lobby.Get().Entity.Id != e.lobbyId))) && (!eulaNotification.IsPresent() && !ppNotification.IsPresent()))
            {
                component.engineId = e.engineId;
                component.lobbyId = e.lobbyId;
                string messageText = !user.Entity.HasComponent<SquadGroupComponent>() ? ((string) component.messageForSingleUser) : (!user.Entity.HasComponent<SquadLeaderComponent>() ? ((string) component.messageForSquadMember) : ((string) component.messageForSquadLeader));
                messageText = messageText.Replace("{0}", e.userUid);
                dialogs.dialogs60.Get<NotificationsStackContainerComponent>().CreateNotification(component.gameObject).GetComponent<InviteToLobbyDialogComponent>().GetComponent<InviteDialogComponent>().Show(messageText, selfBattleUser.IsPresent());
            }
        }

        [OnEventComplete]
        public void UserInSameLobby(NodeAddedEvent e, WaitingInviteAnswerUserLabelNode label, [JoinByUser] LobbyUserNode lobbyUser, [JoinByBattleLobby] SelfLobbyUserNode selfLobbyUser)
        {
            label.waitingForInviteToLobbyAnswerUi.AlreadyInLobby = true;
        }

        [OnEventComplete]
        public void UserInSameLobby(NodeAddedEvent e, LobbyUserNode lobbyUser, [JoinByUser] WaitingInviteAnswerUserLabelNode label, LobbyUserNode lobbyUser1, [JoinByBattleLobby] SelfLobbyUserNode selfLobbyUser)
        {
            label.waitingForInviteToLobbyAnswerUi.AlreadyInLobby = true;
        }

        [OnEventFire]
        public void UserLeaveLobby(NodeRemoveEvent e, LobbyUserNode lobbyUser, [JoinByUser] WaitingInviteAnswerUserLabelNode label)
        {
            label.waitingForInviteToLobbyAnswerUi.AlreadyInLobby = false;
        }

        public class DialogsNode : Node
        {
            public Dialogs60Component dialogs60;
        }

        public class InviteToLobbyButtonNode : Node
        {
            public SendInviteToLobbyButtonComponent sendInviteToLobbyButton;
            public UserGroupComponent userGroup;
        }

        public class LobbyNode : Node
        {
            public BattleLobbyComponent battleLobby;
            public BattleLobbyGroupComponent battleLobbyGroup;
        }

        public class LobbyUserNode : InviteToLobbySystem.UserNode
        {
            public BattleLobbyGroupComponent battleLobbyGroup;
        }

        public class SelfLobbyUserNode : Node
        {
            public UserComponent user;
            public UserGroupComponent userGroup;
            public SelfUserComponent selfUser;
            public BattleLobbyGroupComponent battleLobbyGroup;
        }

        [Not(typeof(SelfUserComponent))]
        public class UserNode : Node
        {
            public UserComponent user;
            public UserGroupComponent userGroup;
        }

        public class WaitingInviteAnswerUserLabelNode : Node
        {
            public UserLabelComponent userLabel;
            public UserGroupComponent userGroup;
            public WaitingForInviteToLobbyAnswerUIComponent waitingForInviteToLobbyAnswerUi;
        }
    }
}

