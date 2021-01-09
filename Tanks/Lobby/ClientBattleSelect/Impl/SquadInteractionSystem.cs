namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientFriends.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;

    public class SquadInteractionSystem : ECSSystem
    {
        [OnEventFire]
        public void ChangeSquadLeader(ChangeSquadLeaderInternalEvent e, UserNode teammate, [JoinAll] SelfSquadLeaderNode selfSquadLeader)
        {
            ChangeSquadLeaderEvent eventInstance = new ChangeSquadLeaderEvent {
                NewLeaderUserId = teammate.Entity.Id
            };
            base.ScheduleEvent(eventInstance, selfSquadLeader);
        }

        [OnEventFire]
        public void KickOutFromSquad(KickOutFromSquadInternalEvent e, UserNode teammate, [JoinAll] SelfSquadLeaderNode selfSquadLeader)
        {
            KickOutFromSquadEvent eventInstance = new KickOutFromSquadEvent {
                KickedOutUserId = teammate.Entity.Id
            };
            base.ScheduleEvent(eventInstance, selfSquadLeader);
        }

        [OnEventFire]
        public void LeaveSquad(LeaveSquadInternalEvent e, UserNode teammate)
        {
            base.ScheduleEvent<LeaveSquadEvent>(teammate);
        }

        [OnEventFire]
        public void RequestFriend(RequestFriendSquadInternalEvent e, UserNode user, [JoinBySquad] SingleNode<SquadComponent> squad, [JoinAll] SingleNode<SelfUserComponent> selfUser)
        {
            base.ScheduleEvent(new RequestFriendshipByUserId(user.Entity.Id, InteractionSource.SQUAD, squad.Entity.Id), selfUser);
        }

        [OnEventFire]
        public void ShowSelfUserTooltip(RightMouseButtonClickEvent e, SelfUserLabelNode selfUserButton, [JoinAll] SelfUserInSquadNode selfUser, [JoinAll] ICollection<TeammateNode> teammates, [JoinAll] SingleNode<FriendsComponent> friends)
        {
            if (teammates.Count > 0)
            {
                this.ShowTooltip(selfUser, selfUser, selfUserButton.squadTeammateInteractionButton, friends.component);
            }
        }

        public void ShowTooltip(UserNode user, SelfUserNode selfUser, SquadTeammateInteractionButtonComponent squadTeammateInteractionButton, FriendsComponent friends)
        {
            SquadTeammateInteractionTooltipContentData data;
            SquadTeammateInteractionTooltipContentData data2;
            bool flag2 = selfUser.Entity.HasComponent<SquadLeaderComponent>();
            bool flag3 = user.Entity.HasComponent<AcceptedFriendComponent>();
            bool flag4 = selfUser.Entity.HasComponent<MatchMakingUserComponent>();
            bool flag5 = friends.OutgoingFriendsIds.Contains(user.Entity.Id);
            if (user.Entity.HasComponent<SelfUserComponent>())
            {
                if (flag4)
                {
                    return;
                }
                data2 = new SquadTeammateInteractionTooltipContentData {
                    teammateEntity = user.Entity,
                    ShowLeaveSquadButton = true
                };
                data = data2;
            }
            else
            {
                data2 = new SquadTeammateInteractionTooltipContentData {
                    teammateEntity = user.Entity,
                    ShowProfileButton = true,
                    ShowLeaveSquadButton = false,
                    ShowRemoveFromSquadButton = !flag4,
                    ActiveRemoveFromSquadButton = flag2,
                    ShowGiveLeaderButton = !flag4,
                    ActiveGiveLeaderButton = flag2,
                    ShowAddFriendButton = !flag3 && !flag5,
                    ShowFriendRequestSentButton = !flag3 && flag5
                };
                data = data2;
            }
            TooltipController.Instance.ShowTooltip(Input.mousePosition, data, squadTeammateInteractionButton.tooltipPrefab, false);
        }

        [OnEventFire]
        public void ShowTooltipInLobby(RightMouseButtonClickEvent e, TeammateLabelNode userButton, [JoinByUser] UserNode user, [JoinAll] SelfUserNode selfUser, [JoinAll] SingleNode<FriendsComponent> friends)
        {
            this.ShowTooltip(user, selfUser, userButton.squadTeammateInteractionButton, friends.component);
        }

        public class SelfSquadLeaderNode : SquadInteractionSystem.SelfUserNode
        {
            public SquadLeaderComponent squadLeader;
        }

        public class SelfUserInSquadNode : SquadInteractionSystem.SelfUserNode
        {
            public SquadGroupComponent squadGroup;
        }

        [Not(typeof(UserLabelComponent))]
        public class SelfUserLabelNode : Node
        {
            public SquadTeammateInteractionButtonComponent squadTeammateInteractionButton;
        }

        public class SelfUserNode : SquadInteractionSystem.UserNode
        {
            public SelfUserComponent selfUser;
        }

        public class TeammateLabelNode : Node
        {
            public UserLabelComponent userLabel;
            public UserGroupComponent userGroup;
            public SquadTeammateInteractionButtonComponent squadTeammateInteractionButton;
        }

        [Not(typeof(SelfUserComponent))]
        public class TeammateNode : Node
        {
            public UserComponent user;
            public SquadGroupComponent squadGroup;
        }

        public class UserNode : Node
        {
            public UserComponent user;
            public UserGroupComponent userGroup;
        }
    }
}

