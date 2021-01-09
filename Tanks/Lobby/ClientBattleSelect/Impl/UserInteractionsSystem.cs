namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;

    public class UserInteractionsSystem : ECSSystem
    {
        private void AddTooltipRequestComponent(Entity entity, GameObject tooltipPrefab, long idOfRequestedUser, InteractionSource interactionSource, long sourceId)
        {
            TooltipDataRequestComponent component = !entity.HasComponent<TooltipDataRequestComponent>() ? entity.AddComponentAndGetInstance<TooltipDataRequestComponent>() : entity.GetComponent<TooltipDataRequestComponent>();
            component.MousePosition = Input.mousePosition;
            component.TooltipPrefab = tooltipPrefab;
            component.InteractionSource = interactionSource;
            component.idOfRequestedUser = idOfRequestedUser;
            component.InteractableSourceId = sourceId;
        }

        [OnEventFire]
        public void DisableSelfInteractionInBattle(NodeAddedEvent e, BattleInteractableUserButtonNode userButton, [JoinByUser] UserNode user, [JoinSelf] SelfUserNode selfUser)
        {
            userButton.userInteractionsButton.GetComponent<Button>().interactable = false;
        }

        [OnEventFire]
        public void HideTooltipWhenScreenChanged(ChangeScreenEvent e, Node any, [JoinAll] SelfUserNode selfUser)
        {
            selfUser.Entity.RemoveComponentIfPresent<TooltipDataRequestComponent>();
            TooltipController.Instance.HideTooltip();
        }

        [OnEventFire]
        public void RequestTooltipAtBattleResult(ButtonClickEvent e, SingleNode<UserInteractionsButtonComponent> userButton, [JoinAll] SelfUserNode selfUser)
        {
            this.ShowTooltipAtBattleResult(userButton.component, selfUser.Entity);
        }

        [OnEventFire]
        public void RequestTooltipAtBattleResultRightClick(RightMouseButtonClickEvent e, SingleNode<UserInteractionsButtonComponent> userButton, [JoinAll] SelfUserNode selfUser)
        {
            this.ShowTooltipAtBattleResult(userButton.component, selfUser.Entity);
        }

        private void RequestTooltipDisplay(long interactableUserId, GameObject tooltipPrefab, Entity selfUser, InteractionSource interactionSource, long sourceId)
        {
            if (interactableUserId != selfUser.Id)
            {
                this.AddTooltipRequestComponent(selfUser, tooltipPrefab, interactableUserId, interactionSource, sourceId);
                UserInteractionDataRequestEvent eventInstance = new UserInteractionDataRequestEvent {
                    UserId = interactableUserId
                };
                base.ScheduleEvent(eventInstance, selfUser);
            }
        }

        private void ShowTooltipAtBattleResult(UserInteractionsButtonComponent userButton, Entity selfUser)
        {
            PlayerStatInfoUI component = userButton.gameObject.GetComponent<PlayerStatInfoUI>();
            if (component != null)
            {
                this.RequestTooltipDisplay(component.userId, userButton.tooltipPrefab, selfUser, InteractionSource.BATTLE_RESULT, component.battleId);
            }
        }

        private void ShowTooltipInBattle(BattleInteractableUserButtonNode userButton, UserNode user, SelfUserNode selfUser, BattleNode battle)
        {
            if (!selfUser.Entity.HasComponent<UserInBattleAsSpectatorComponent>())
            {
                GameObject tooltipPrefab = userButton.userInteractionsButton.tooltipPrefab;
                this.RequestTooltipDisplay(user.Entity.Id, tooltipPrefab, selfUser.Entity, InteractionSource.BATTLE, battle.Entity.Id);
            }
        }

        [OnEventFire]
        public void ShowTooltipInBattle(ButtonClickEvent e, BattleInteractableUserButtonNode userButton, [JoinByUser] UserNode user, [JoinAll] SelfUserNode selfUser, [JoinByBattle] BattleNode battle)
        {
            this.ShowTooltipInBattle(userButton, user, selfUser, battle);
        }

        [OnEventFire]
        public void ShowTooltipInBattleRightClick(RightMouseButtonClickEvent e, BattleInteractableUserButtonNode userButton, [JoinByUser] UserNode user, [JoinAll] SelfUserNode selfUser, [JoinByBattle] BattleNode battle)
        {
            this.ShowTooltipInBattle(userButton, user, selfUser, battle);
        }

        private void ShowTooltipInLobby(LobbyInteractableUserButtonNode userButton, SelfUserNode selfUser, LobbyNode lobby)
        {
            if (!userButton.lobbyUserListItem.Empty)
            {
                long id = userButton.lobbyUserListItem.userEntity.Id;
                this.RequestTooltipDisplay(id, userButton.userInteractionsButton.tooltipPrefab, selfUser.Entity, InteractionSource.LOBBY, lobby.Entity.Id);
            }
        }

        [OnEventFire]
        public void ShowTooltipInLobby(ButtonClickEvent e, LobbyInteractableUserButtonNode userButton, [JoinAll] SelfUserNode selfUser, [JoinByBattleLobby] LobbyNode lobby)
        {
            this.ShowTooltipInLobby(userButton, selfUser, lobby);
        }

        [OnEventFire]
        public void ShowTooltipInLobbyRightClick(RightMouseButtonClickEvent e, LobbyInteractableUserButtonNode userButton, [JoinAll] SelfUserNode selfUser, [JoinByBattleLobby] LobbyNode lobby)
        {
            this.ShowTooltipInLobby(userButton, selfUser, lobby);
        }

        [OnEventFire]
        public void ShowTooltipOnServerResponse(UserInteractionDataResponseEvent e, SelfUserWithTooltipRequestNode selfUser)
        {
            long idOfRequestedUser = selfUser.tooltipDataRequest.idOfRequestedUser;
            InteractionSource interactionSource = selfUser.tooltipDataRequest.InteractionSource;
            long interactableSourceId = selfUser.tooltipDataRequest.InteractableSourceId;
            if (e.UserId == idOfRequestedUser)
            {
                UserInteractionsData data = new UserInteractionsData {
                    canRequestFrendship = e.CanRequestFrendship,
                    friendshipRequestWasSend = e.FriendshipRequestWasSend,
                    isMuted = e.Muted,
                    isReported = e.Reported,
                    selfUserEntity = selfUser.Entity,
                    userId = e.UserId,
                    interactionSource = interactionSource,
                    sourceId = interactableSourceId,
                    OtherUserName = e.UserUid
                };
                this.ShowTooltop(data, selfUser.tooltipDataRequest.TooltipPrefab, selfUser.tooltipDataRequest.MousePosition);
            }
        }

        private void ShowTooltop(UserInteractionsData data, GameObject tooltipPrefab, Vector3 position)
        {
            TooltipController.Instance.ShowTooltip(position, data, tooltipPrefab, false);
        }

        public class BattleInteractableUserButtonNode : Node
        {
            public UserInteractionsButtonComponent userInteractionsButton;
            public UserGroupComponent userGroup;
        }

        public class BattleNode : Node
        {
            public BattleComponent battle;
        }

        public class LobbyInteractableUserButtonNode : Node
        {
            public UserInteractionsButtonComponent userInteractionsButton;
            public LobbyUserListItemComponent lobbyUserListItem;
        }

        public class LobbyNode : Node
        {
            public BattleLobbyComponent battleLobby;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
        }

        public class SelfUserWithTooltipRequestNode : UserInteractionsSystem.SelfUserNode
        {
            public TooltipDataRequestComponent tooltipDataRequest;
        }

        public class UserNode : Node
        {
            public UserComponent user;
            public UserUidComponent userUid;
        }
    }
}

