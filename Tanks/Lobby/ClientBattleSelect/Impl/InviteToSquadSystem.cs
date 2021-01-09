namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientFriends.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;

    public class InviteToSquadSystem : ECSSystem
    {
        private bool CanShowInviteWindow(InviteToSquadDialogComponent window) => 
            (window != null) && ((MainScreenComponent.Instance != null) && !window.gameObject.activeSelf);

        [OnEventFire]
        public void CloseSquadInviteDialog(NodeAddedEvent e, LobbyUserNode selfUser, [JoinAll, Combine] SingleNode<InviteToSquadDialogComponent> inviteToSquadDialog)
        {
            inviteToSquadDialog.Entity.GetComponent<InviteDialogComponent>().Hide();
        }

        private void DelayUpdateInviteToSquadButton(Node user)
        {
            base.NewEvent<UpdateUserInviteToSquadButtonEvent>().Attach(user).ScheduleDelayed(0f);
        }

        [OnEventFire]
        public void HideInviteDialogOnEula(NodeAddedEvent e, SingleNode<EulaNotificationComponent> eulaNotification, [Combine] SingleNode<InviteToSquadDialogComponent> inviteDialog)
        {
            inviteDialog.component.Hide();
        }

        [OnEventFire]
        public void HideInviteDialogOnPP(NodeAddedEvent e, SingleNode<PrivacyPolicyNotificationComponent> ppNotification, [Combine] SingleNode<InviteToSquadDialogComponent> inviteDialog)
        {
            inviteDialog.component.Hide();
        }

        private void Invite(FriendUserNode friend, Optional<SquadNode> squad, SelfUserNode selfUser)
        {
            InviteToSquadEvent eventInstance = new InviteToSquadEvent();
            eventInstance.InvitedUsersIds = new long[] { friend.Entity.Id };
            base.NewEvent(eventInstance).Attach(selfUser).Schedule();
        }

        [OnEventFire]
        public void Invite(ButtonClickEvent e, InviteToSquadButtonNode button, [JoinByUser] FriendUserNode friend, [JoinAll] SingleNode<FriendsComponent> friends, [JoinAll] SelfUserNode selfUser, [JoinBySquad] Optional<SquadNode> squad)
        {
            this.ScheduleEvent(new FriendInviteToSquadEvent(friend.Entity.Id, InteractionSource.SQUAD, !squad.IsPresent() ? 0L : squad.Get().Entity.Id), friend);
        }

        [OnEventFire]
        public void InviteToSquad(FriendInviteToSquadEvent e, FriendUserNode friend, [JoinAll] SelfUserNode selfUser, [JoinBySquad] Optional<SquadNode> squad, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens, [JoinAll] SingleNode<FriendsComponent> friends)
        {
            List<Animator> animators = new List<Animator>();
            if (screens.IsPresent())
            {
                animators = screens.Get().component.Animators;
            }
            if (friend.userRank.Rank < 4)
            {
                dialogs.component.Get<CantInviteFriendIntoSquadDialogComponent>().Show(friend.userUid.Uid, animators);
            }
            else if (friend.Entity.HasComponent<MatchMakingUserComponent>())
            {
                dialogs.component.Get<CantInviteFriendIntoSquadDialogComponent>().Show(friend.userUid.Uid, animators);
            }
            else
            {
                if (friend.Entity.HasComponent<InvitedToSquadUserComponent>())
                {
                    friend.Entity.RemoveComponent<InvitedToSquadUserComponent>();
                }
                friends.component.InSquadInvitations[friend.userGroup.Key] = DateTime.Now;
                friend.Entity.AddComponent<InvitedToSquadUserComponent>();
                this.Invite(friend, squad, selfUser);
            }
        }

        [OnEventFire]
        public void InviteToSquadCanceled(InviteToSquadCanceledEvent e, SingleNode<SelfUserComponent> user, [JoinAll] SingleNode<NotificationsStackContainerComponent> container)
        {
            Debug.Log("InviteToLobbySystem.InviteToSquadCanceled");
            IEnumerator enumerator = container.component.transform.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    InviteToSquadDialogComponent component = ((Transform) enumerator.Current).GetComponent<InviteToSquadDialogComponent>();
                    if ((component != null) && component.invite)
                    {
                        component.GetComponent<InviteDialogComponent>().Hide();
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        [OnEventFire]
        public void InviteToSquadRejected(InviteToSquadRejectedEvent e, SingleNode<SelfUserComponent> user)
        {
            Debug.Log("InviteToLobbySystem.InviteToSquadRejected");
        }

        [OnEventFire]
        public void OnDialogConfirm(DialogConfirmEvent e, SingleNode<InviteToSquadDialogComponent> dialog, [JoinAll] SingleNode<SelfUserComponent> user, [JoinByBattleLobby] Optional<SingleNode<BattleLobbyComponent>> lobby)
        {
            if (!lobby.IsPresent() || lobby.Get().Entity.HasComponent<CustomBattleLobbyComponent>())
            {
                bool flag = lobby.IsPresent() && lobby.Get().Entity.HasComponent<CustomBattleLobbyComponent>();
                if (!dialog.component.invite)
                {
                    AcceptRequestToSquadEvent eventInstance = new AcceptRequestToSquadEvent {
                        FromUserId = dialog.component.FromUserId,
                        SquadId = dialog.component.SquadId,
                        SquadEngineId = dialog.component.EngineId
                    };
                    base.ScheduleEvent(eventInstance, user);
                }
                else
                {
                    if (flag)
                    {
                        base.ScheduleEvent<ClientExitLobbyEvent>(lobby.Get());
                    }
                    else
                    {
                        base.ScheduleEvent<CancelMatchSearchingEvent>(user);
                    }
                    AcceptInviteToSquadEvent eventInstance = new AcceptInviteToSquadEvent {
                        EngineId = dialog.component.EngineId,
                        FromUserId = dialog.component.FromUserId
                    };
                    base.NewEvent(eventInstance).Attach(user).Schedule();
                }
            }
        }

        [OnEventFire]
        public void OnDialogDecline(DialogDeclineEvent e, SingleNode<InviteToSquadDialogComponent> dialog, [JoinAll] SingleNode<SelfUserComponent> user)
        {
            object[] objArray1 = new object[] { "InviteToLobbySystem.OnDialogDecline ", user.Entity, " ", dialog.component.invite, " ", dialog.component.FromUserId, " ", dialog.component.EngineId };
            Debug.Log(string.Concat(objArray1));
            if (dialog.component.invite)
            {
                RejectInviteToSquadEvent eventInstance = new RejectInviteToSquadEvent {
                    FromUserId = dialog.component.FromUserId,
                    EngineId = dialog.component.EngineId
                };
                base.ScheduleEvent(eventInstance, user);
            }
            else
            {
                RejectRequestToSquadEvent eventInstance = new RejectRequestToSquadEvent {
                    FromUserId = dialog.component.FromUserId,
                    SquadId = dialog.component.SquadId,
                    SquadEngineId = dialog.component.EngineId
                };
                base.ScheduleEvent(eventInstance, user);
            }
        }

        [OnEventFire]
        public void RequestToSquad(RequestToSquadInternalEvent e, UserInSquadNode friend, [JoinAll] SelfUserNode selfUser, [JoinAll] SingleNode<Dialogs60Component> dialogs)
        {
            RequestToSquadEvent eventInstance = new RequestToSquadEvent {
                ToUserId = friend.userGroup.Key,
                SquadId = friend.squadGroup.Key
            };
            base.NewEvent(eventInstance).Attach(selfUser).Schedule();
        }

        [OnEventFire]
        public void RequestToSquadCanceled(RequestToSquadCanceledEvent e, SingleNode<SelfUserComponent> user)
        {
            Debug.Log("InviteToLobbySystem.RequestToSquadCanceled");
        }

        [OnEventFire]
        public void RequestToSquadRejected(RequestToSquadRejectedEvent e, SingleNode<SelfUserComponent> user, [JoinAll] ICollection<FriendTooltipContentNode> friendTooltips)
        {
            Debug.Log("InviteToLobbySystem.RequestToSquadRejected " + e.Reason);
            foreach (FriendTooltipContentNode node in friendTooltips)
            {
                if ((node.userGroup.Key == e.RequestReceiverId) && (e.Reason == RejectRequestToSquadReason.SQUAD_IS_FULL))
                {
                    node.friendInteractionTooltipContent.SquadIsFull();
                }
            }
        }

        [OnEventFire]
        public void ShowInviteDialog(InvitedToSquadEvent e, NotBattleUser selfUser, [JoinAll] DialogsNode dialogs, [JoinAll] Optional<SingleNode<EulaNotificationComponent>> eulaNotification, [JoinAll] Optional<SingleNode<PrivacyPolicyNotificationComponent>> ppNotification)
        {
            if (!eulaNotification.IsPresent() && !ppNotification.IsPresent())
            {
                object[] objArray1 = new object[] { "InviteToLobbySystem.ShowInviteDialog ", selfUser.Entity, " UserUid=", e.UserUid, " FromUserId=", e.FromUserId, " EngineId=", e.EngineId };
                Debug.Log(string.Concat(objArray1));
                this.ShowInviteDialog(selfUser, e.EngineId, e.UserUid, e.FromUserId, dialogs);
            }
        }

        private void ShowInviteDialog(SelfUserNode user, long engineId, string userUid, long fromUserId, DialogsNode dialogs)
        {
            InviteToSquadDialogComponent window = dialogs.dialogs60.Get<InviteToSquadDialogComponent>();
            if (!this.CanShowInviteWindow(window))
            {
                RejectInviteToSquadEvent eventInstance = new RejectInviteToSquadEvent {
                    FromUserId = fromUserId,
                    EngineId = engineId
                };
                base.ScheduleEvent(eventInstance, user);
            }
            else
            {
                InviteToSquadDialogComponent component = dialogs.dialogs60.Get<NotificationsStackContainerComponent>().CreateNotification(window.gameObject).GetComponent<InviteToSquadDialogComponent>();
                component.FromUserId = fromUserId;
                component.EngineId = engineId;
                component.Show(userUid, false, true);
            }
        }

        [OnEventFire]
        public void ShowRequestDialog(RequestedToSquadEvent e, NotBattleUser selfUser, [JoinAll] DialogsNode dialogs, [JoinAll] Optional<SingleNode<EulaNotificationComponent>> eulaNotification, [JoinAll] Optional<SingleNode<PrivacyPolicyNotificationComponent>> ppNotification)
        {
            if (!eulaNotification.IsPresent() && !ppNotification.IsPresent())
            {
                object[] objArray1 = new object[10];
                objArray1[0] = "InviteToLobbySystem.ShowRequestDialog ";
                objArray1[1] = selfUser.Entity;
                objArray1[2] = " UserUid=";
                objArray1[3] = e.UserUid;
                objArray1[4] = " FromUserId=";
                objArray1[5] = e.FromUserId;
                objArray1[6] = " EngineId=";
                objArray1[7] = e.EngineId;
                objArray1[8] = " SquadId=";
                objArray1[9] = e.SquadId;
                Debug.Log(string.Concat(objArray1));
                this.ShowRequestDialog(selfUser, e.EngineId, e.UserUid, e.FromUserId, e.SquadId, dialogs);
            }
        }

        private void ShowRequestDialog(SelfUserNode user, long engineId, string userUid, long fromUserId, long squadId, DialogsNode dialogs)
        {
            InviteToSquadDialogComponent window = dialogs.dialogs60.Get<InviteToSquadDialogComponent>();
            if (!this.CanShowInviteWindow(window))
            {
                RejectRequestToSquadEvent eventInstance = new RejectRequestToSquadEvent {
                    FromUserId = fromUserId,
                    SquadId = squadId,
                    SquadEngineId = engineId
                };
                base.ScheduleEvent(eventInstance, user);
            }
            else
            {
                InviteToSquadDialogComponent component = dialogs.dialogs60.Get<NotificationsStackContainerComponent>().CreateNotification(window.gameObject).GetComponent<InviteToSquadDialogComponent>();
                component.FromUserId = fromUserId;
                component.EngineId = engineId;
                component.SquadId = squadId;
                component.Show(userUid, false, false);
            }
        }

        [OnEventFire]
        public void UpdateInviteToSquadButton(UpdateUserInviteToSquadButtonEvent e, UserNode user, [JoinByUser, Combine] UserLabelStateNode userLabel, [JoinAll] SingleNode<SelfUserComponent> selfUser)
        {
            bool flag = user.Entity.HasComponent<SquadGroupComponent>();
            bool flag2 = user.Entity.HasComponent<UserOnlineComponent>();
            bool flag3 = user.Entity.HasComponent<MatchMakingUserComponent>();
            bool flag4 = user.Entity.HasComponent<BattleGroupComponent>();
            bool flag5 = selfUser.Entity.HasComponent<MatchMakingUserComponent>();
            userLabel.userLabelState.CanBeInvited = !userLabel.userLabelState.DisableInviteOnlyForSquadState ? ((flag2 && (!flag3 && !flag4)) && !flag5) : !(flag && (flag3 || flag4));
        }

        [OnEventFire]
        public void UserInBattle(NodeAddedEvent e, UserInBattleNode user, [JoinByUser, Context, Combine] UserLabelStateNode userLabel)
        {
            this.DelayUpdateInviteToSquadButton(user);
        }

        [OnEventFire]
        public void UserInMatchMaking(NodeAddedEvent e, UserInMatchMakingNode user, [JoinByUser, Context, Combine] UserLabelStateNode userLabel)
        {
            this.DelayUpdateInviteToSquadButton(user);
        }

        [OnEventComplete]
        public void UserInSquad(NodeAddedEvent e, InviteToSquadUserLabelNode label, [JoinByUser, Context] UserInSquadNode squadUser)
        {
            label.waitingForInviteToSquadAnswerUi.AlreadyInSquad = true;
        }

        [OnEventFire]
        public void UserLeaveSquad(NodeRemoveEvent e, UserInSquadNode squadUser, [JoinByUser] InviteToSquadUserLabelNode label)
        {
            label.waitingForInviteToSquadAnswerUi.AlreadyInSquad = false;
        }

        [OnEventFire]
        public void UserOffline(NodeRemoveEvent e, UserOnlineNode user, [JoinByUser, Combine] UserLabelStateNode userLabel)
        {
            this.DelayUpdateInviteToSquadButton(user);
        }

        [OnEventFire]
        public void UserOnline(NodeAddedEvent e, UserOnlineNode user, [Context, JoinByUser, Combine] UserLabelStateNode userLabel)
        {
            this.DelayUpdateInviteToSquadButton(user);
        }

        [OnEventFire]
        public void UserOutBattle(NodeRemoveEvent e, UserInBattleNode user, [JoinByUser, Combine] UserLabelStateNode userLabel)
        {
            this.DelayUpdateInviteToSquadButton(user);
        }

        [OnEventFire]
        public void UserOutMatchMaking(NodeRemoveEvent e, UserInMatchMakingNode user, [JoinByUser, Combine] UserLabelStateNode userLabel)
        {
            this.DelayUpdateInviteToSquadButton(user);
        }

        public class DialogsNode : Node
        {
            public Dialogs60Component dialogs60;
        }

        public class FriendInBattleNode : Node
        {
            public UserComponent user;
            public UserGroupComponent userGroup;
            public BattleGroupComponent battleGroup;
        }

        public class FriendTooltipContentNode : Node
        {
            public FriendInteractionTooltipContentComponent friendInteractionTooltipContent;
            public UserGroupComponent userGroup;
        }

        public class FriendUserNode : Node
        {
            public UserComponent user;
            public AcceptedFriendComponent acceptedFriend;
            public UserGroupComponent userGroup;
            public UserRankComponent userRank;
            public UserUidComponent userUid;
        }

        public class InviteToSquadButtonNode : Node
        {
            public UserGroupComponent userGroup;
            public SendInviteToSquadButtonComponent sendInviteToSquadButton;
        }

        public class InviteToSquadUserLabelNode : Node
        {
            public UserLabelComponent userLabel;
            public UserGroupComponent userGroup;
            public WaitingForInviteToSquadAnswerUIComponent waitingForInviteToSquadAnswerUi;
        }

        public class LobbyUserNode : InviteToSquadSystem.SelfUserNode
        {
            public BattleLobbyGroupComponent battleLobbyGroup;
        }

        [Not(typeof(BattleGroupComponent))]
        public class NotBattleUser : InviteToSquadSystem.SelfUserNode
        {
        }

        public class SelfUserInSquadNode : InviteToSquadSystem.SelfUserNode
        {
            public SquadGroupComponent squadGroup;
        }

        public class SelfUserNode : Node
        {
            public UserComponent user;
            public SelfUserComponent selfUser;
        }

        public class SquadNode : Node
        {
            public SquadComponent squad;
            public SquadConfigComponent squadConfig;
            public SquadGroupComponent squadGroup;
        }

        public class UpdateUserInviteToSquadButtonEvent : Event
        {
        }

        public class UserInBattleNode : InviteToSquadSystem.UserNode
        {
            public BattleGroupComponent battleGroup;
        }

        public class UserInMatchMakingNode : Node
        {
            public UserComponent user;
            public UserGroupComponent userGroup;
            public MatchMakingUserComponent matchMakingUser;
        }

        public class UserInSquadNode : Node
        {
            public UserComponent user;
            public UserGroupComponent userGroup;
            public SquadGroupComponent squadGroup;
        }

        public class UserLabelStateNode : Node
        {
            public UserLabelComponent userLabel;
            public UserLabelStateComponent userLabelState;
            public UserGroupComponent userGroup;
        }

        public class UserNode : Node
        {
            public UserComponent user;
            public UserGroupComponent userGroup;
        }

        public class UserOnlineNode : InviteToSquadSystem.UserNode
        {
            public UserOnlineComponent userOnline;
        }
    }
}

