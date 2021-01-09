namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientFriends.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;

    public class SquadInfoSystem : ECSSystem
    {
        [OnEventFire]
        public void AddTeammateIcon(NodeAddedEvent e, SquadInfoUINode squadInfo, [Combine] UserInSquadNode teammate, [JoinBySquad, Context] SquadNode squad)
        {
            LeagueConfigComponent leagueConfig = base.Select<LeagueNode>(teammate.Entity, typeof(LeagueGroupComponent)).First<LeagueNode>().leagueConfig;
            squadInfo.squadInfoUI.AddTeammate(teammate.Entity.Id, teammate.userAvatar.Id, leagueConfig.LeagueIndex);
        }

        [OnEventFire]
        public void CheckForSelfLeaderIcon(NodeAddedEvent e, SingleNode<SelfUserLeaderIconComponent> selfLeaderIcon, [JoinAll] Optional<SelfSquadLeaderNode> selfSquadLeaderNode, [JoinAll] ICollection<UserInSquadNode> allTeammates)
        {
            selfLeaderIcon.component.icon.SetActive(selfSquadLeaderNode.IsPresent() && (allTeammates.Count > 0));
        }

        [OnEventFire]
        public void CheckForShowContextMenuButtons(CheckForShowInviteToSquadEvent e, FriendNode friend, [JoinByUser] Optional<UserInSquadNode> friendSquadUser, [JoinAll] Optional<SelfUserInSquadNode> selfUserInSquad, [JoinBySquad] Optional<SquadNode> squad, [JoinAll] SelfUserNode selfUser)
        {
            bool flag2 = friend.Entity.HasComponent<MatchMakingUserComponent>();
            bool flag3 = friend.Entity.HasComponent<BattleGroupComponent>();
            if (!selfUser.Entity.HasComponent<MatchMakingUserComponent>() && (!flag2 && !flag3))
            {
                bool flag4 = selfUserInSquad.IsPresent();
                bool flag5 = false;
                bool flag6 = friend.Entity.HasComponent<UserOnlineComponent>();
                bool flag7 = friendSquadUser.IsPresent();
                bool flag8 = (flag7 && flag4) && (friendSquadUser.Get().squadGroup.Key == squad.Get().squadGroup.Key);
                if (flag4 && squad.IsPresent())
                {
                    flag5 = base.Select<UserInSquadNode>(squad.Get().Entity, typeof(SquadGroupComponent)).Count >= (squad.Get().squadConfig.MaxSquadSize - 1);
                }
                e.ShowInviteToSquadButton = ((!flag4 || !flag5) && (!flag7 && flag6)) && (selfUser.userRank.Rank > 3);
                e.ActiveInviteToSquadButton = true;
                e.ShowRequestToInviteToSquadButton = (!flag4 && (flag7 && !flag8)) && (selfUser.userRank.Rank > 3);
            }
        }

        [OnEventFire]
        public void DisableInBattleState(NodeRemoveEvent e, FriendInMatchMakingNode user, [JoinByUser, Context, Combine] UserLabelStateNode userLabel)
        {
            if (!user.Entity.HasComponent<BattleGroupComponent>())
            {
                userLabel.userLabelState.UserOutBattle(user.Entity.HasComponent<UserOnlineComponent>());
            }
        }

        [OnEventFire]
        public void DisableLeaderIcon(NodeRemoveEvent e, SquadLeaderNode squadLeader, [JoinByUser] SquadLeaderIconNode squadLeaderIcon)
        {
            squadLeaderIcon.squadLeaderIcon.icon.SetActive(false);
        }

        [OnEventFire]
        public void DisableSelfLeaderIcon(NodeRemoveEvent e, SelfSquadLeaderNode selfSquadLeaderNode, [JoinAll] SingleNode<SelfUserLeaderIconComponent> selfLeaderIcon)
        {
            selfLeaderIcon.component.icon.SetActive(false);
        }

        [OnEventFire]
        public void DisableSquadInfo(NodeRemoveEvent e, SelfUserInSquadNode selfUserInSquad, [JoinAll] UserInfoNode userInfo)
        {
            userInfo.userInfoUI.SwitchSquadInfo(false);
        }

        [OnEventFire]
        public void EnableInBattleState(NodeAddedEvent e, FriendInMatchMakingNode user, [JoinByUser, Context, Combine] UserLabelStateNode userLabel)
        {
            if (!user.Entity.HasComponent<BattleGroupComponent>())
            {
                userLabel.userLabelState.UserInBattle();
            }
        }

        [OnEventFire]
        public void EnableLeaderIcon(NodeAddedEvent e, SquadLeaderIconNode squadLeaderIcon, [JoinByUser, Context] SquadLeaderNode squadLeader, [JoinBySquad] ICollection<UserInSquadNode> allTeammates)
        {
            squadLeaderIcon.squadLeaderIcon.icon.SetActive(allTeammates.Count > 0);
        }

        [OnEventFire]
        public void EnableSelfLeaderIcon(NodeAddedEvent e, SelfSquadLeaderNode selfSquadLeaderNode, [JoinAll, Context] SingleNode<SelfUserLeaderIconComponent> selfLeaderIcon, [JoinAll] ICollection<UserInSquadNode> allTeammates)
        {
            selfLeaderIcon.component.icon.SetActive(allTeammates.Count > 0);
        }

        [OnEventFire]
        public void EnableSquadInfo(NodeAddedEvent e, SelfUserInSquadNode selfUserInSquad, [JoinAll] UserInfoNode userInfo)
        {
            userInfo.userInfoUI.SwitchSquadInfo(true);
        }

        [OnEventFire]
        public void InitUserInfo(NodeAddedEvent e, UserInfoNode userInfo, [JoinAll] Optional<SelfUserInSquadNode> selfUserInSquadNode)
        {
            userInfo.userInfoUI.SwitchSquadInfo(selfUserInSquadNode.IsPresent());
        }

        [OnEventFire]
        public void ProfileUiCreated(NodeAddedEvent e, SingleNode<UserProfileUI> userProfileUi, SingleNode<InviteFriendsPopupComponent> inviteFriendsPopup, [JoinAll] Optional<SelfUserInMatchMakingNode> selfUser)
        {
            bool flag = !selfUser.IsPresent();
            userProfileUi.component.CanCreateSquad = flag;
            if (!flag)
            {
                inviteFriendsPopup.component.Close();
            }
        }

        [OnEventFire]
        public void RemoveTeammateIcon(NodeRemoveEvent e, UserInSquadNode teammate, [JoinAll] SquadInfoUINode squadInfo)
        {
            squadInfo.squadInfoUI.RemoveTeammate(teammate.Entity.Id);
        }

        [OnEventFire]
        public void SelfUserInMatchMaking(NodeAddedEvent e, SelfUserInMatchMakingNode selfUser, [JoinAll] SingleNode<UserProfileUI> userProfileUi, [JoinAll] SingleNode<InviteFriendsPopupComponent> inviteFriendsPopup)
        {
            userProfileUi.component.CanCreateSquad = false;
            inviteFriendsPopup.component.Close();
        }

        [OnEventFire]
        public void SelfUserOutMatchMaking(NodeRemoveEvent e, SelfUserInMatchMakingNode selfUser, [JoinAll] SingleNode<UserProfileUI> userProfileUi)
        {
            userProfileUi.component.CanCreateSquad = true;
        }

        [OnEventFire]
        public void ShowCancelButton(ButtonClickEvent e, SingleNode<AddTeammateToSquadButtonComponent> button, [JoinAll] SingleNode<InviteFriendsPopupComponent> friendsPopup, [JoinAll] SelfUserNode selfUser)
        {
            if (selfUser.userRank.Rank > 3)
            {
                friendsPopup.component.ShowInvite(button.component.PopupPosition, new Vector2(0f, 1f), InviteMode.Squad);
            }
        }

        [OnEventFire]
        public void ShowCancelButton(ButtonClickEvent e, SingleNode<AddTeammateToSquadButtonComponent> button, [JoinAll] SingleNode<UserProfileUI> userProfileUi, [JoinAll] SingleNode<InviteFriendsPopupComponent> friendsPopup, [JoinAll] SelfUserNode selfUser)
        {
            if (selfUser.userRank.Rank > 3)
            {
                userProfileUi.component.SwitchButtons(false);
            }
            else
            {
                button.component.GetComponent<TooltipShowBehaviour>().ShowTooltip(Input.mousePosition);
            }
        }

        [OnEventFire]
        public void ShowCreateSquadButton(NodeAddedEvent e, SingleNode<UserProfileUI> userProfileUi)
        {
            userProfileUi.component.SwitchButtons(true);
        }

        [OnEventFire]
        public void ShowCreateSquadButton(NodeRemoveEvent e, SingleNode<InviteFriendsUIComponent> inviteUI, [JoinAll] SingleNode<UserProfileUI> userProfileUi)
        {
            userProfileUi.component.SwitchButtons(true);
        }

        [OnEventFire]
        public void ShowCreateSquadButton(ButtonClickEvent e, SingleNode<CancelSquadCreatingButtonComponent> button, [JoinAll] SingleNode<UserProfileUI> userProfileUi)
        {
            userProfileUi.component.SwitchButtons(true);
        }

        private void SwitchAddButton(bool showAddButton, SquadInfoUINode squadInfo, InviteFriendsPopupComponent popup)
        {
            squadInfo.squadInfoUI.SwitchAddButton(showAddButton);
            if (!showAddButton)
            {
                popup.Close();
            }
        }

        [OnEventFire]
        public void TeammateAdded(NodeAddedEvent e, UserInSquadNode teammate, [JoinBySquad] SelfSquadLeaderNode selfSquadLeader, [JoinBySquad] ICollection<UserInSquadNode> allTeammates, [JoinAll] SingleNode<SelfUserLeaderIconComponent> selfLeaderIcon)
        {
            if (allTeammates.Count > 0)
            {
                selfLeaderIcon.component.icon.SetActive(true);
            }
        }

        [OnEventFire]
        public void TeammateAdded(NodeAddedEvent e, SquadInfoUINode squadInfo, [Combine] UserInSquadNode teammate, [JoinBySquad] SquadNode squad, [JoinBySquad] ICollection<UserInSquadNode> allTeammates, [JoinAll] SingleNode<InviteFriendsPopupComponent> inviteFriendsPopup)
        {
            bool showAddButton = new HashSet<UserInSquadNode>(allTeammates).Count < (squad.squadConfig.MaxSquadSize - 1);
            this.SwitchAddButton(showAddButton, squadInfo, inviteFriendsPopup.component);
        }

        [OnEventFire]
        public void TeammateRemoved(NodeRemoveEvent e, UserInSquadNode teammate, [JoinBySquad] SelfSquadLeaderNode selfSquadLeader, [JoinBySquad] ICollection<UserInSquadNode> allTeammates, [JoinAll] SingleNode<SelfUserLeaderIconComponent> selfLeaderIcon)
        {
            if (allTeammates.Count == 1)
            {
                selfLeaderIcon.component.icon.SetActive(false);
            }
        }

        [OnEventFire]
        public void TeammateRemoved(NodeRemoveEvent e, UserInSquadNode teammate, [JoinBySquad] SquadNode squad, [JoinBySquad] ICollection<UserInSquadNode> allTeammates, [JoinAll] SquadInfoUINode squadInfo, [JoinAll] SingleNode<InviteFriendsPopupComponent> inviteFriendsPopup)
        {
            bool showAddButton = new HashSet<UserInSquadNode>(allTeammates).Count <= (squad.squadConfig.MaxSquadSize - 1);
            this.SwitchAddButton(showAddButton, squadInfo, inviteFriendsPopup.component);
        }

        [OnEventFire]
        public void UserInSquadAdded(NodeAddedEvent e, UserInSquadNode user, [JoinByUser, Context] UserLabelStateNode userLabelState)
        {
            userLabelState.userLabelState.UserInSquad = true;
        }

        [OnEventFire]
        public void UserInSquadRemoved(NodeRemoveEvent e, UserInSquadNode teammate, [JoinByUser] UserLabelStateNode userLabelState)
        {
            userLabelState.userLabelState.UserInSquad = false;
        }

        public class FriendInMatchMakingNode : SquadInfoSystem.FriendNode
        {
            public MatchMakingUserComponent matchMakingUser;
        }

        public class FriendNode : Node
        {
            public UserComponent user;
            public AcceptedFriendComponent acceptedFriend;
        }

        public class LeagueNode : Node
        {
            public LeagueConfigComponent leagueConfig;
            public LeagueGroupComponent leagueGroup;
        }

        public class SelfSquadLeaderNode : SquadInfoSystem.SquadLeaderNode
        {
            public SelfUserComponent selfUser;
        }

        public class SelfUserInMatchMakingNode : SquadInfoSystem.SelfUserNode
        {
            public MatchMakingUserComponent matchMakingUser;
        }

        public class SelfUserInSquadNode : Node
        {
            public UserComponent user;
            public SelfUserComponent selfUser;
            public SquadGroupComponent squadGroup;
            public UserAvatarComponent userAvatar;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserRankComponent userRank;
        }

        public class SquadInfoUINode : Node
        {
            public SquadInfoUIComponent squadInfoUI;
        }

        public class SquadLeaderIconNode : Node
        {
            public UserLabelComponent userLabel;
            public SquadLeaderIconComponent squadLeaderIcon;
            public UserGroupComponent userGroup;
        }

        public class SquadLeaderNode : Node
        {
            public UserComponent user;
            public SquadLeaderComponent squadLeader;
            public UserGroupComponent userGroup;
        }

        public class SquadNode : Node
        {
            public SquadComponent squad;
            public SquadConfigComponent squadConfig;
            public SquadGroupComponent squadGroup;
        }

        public class UserInfoNode : Node
        {
            public UserInfoUIComponent userInfoUI;
        }

        [Not(typeof(SelfUserComponent))]
        public class UserInSquadNode : Node
        {
            public UserComponent user;
            public UserGroupComponent userGroup;
            public SquadGroupComponent squadGroup;
            public UserAvatarComponent userAvatar;
            public LeagueGroupComponent leagueGroup;
        }

        public class UserLabelStateNode : Node
        {
            public UserGroupComponent userGroup;
            public UserLabelStateComponent userLabelState;
        }
    }
}

