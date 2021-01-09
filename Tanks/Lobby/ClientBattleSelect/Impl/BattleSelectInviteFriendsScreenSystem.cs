namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;

    public class BattleSelectInviteFriendsScreenSystem : ECSSystem
    {
        private void CloseFriendsPanel(GameObject friendsPanel, GameObject entrancePanel, Entity user)
        {
            friendsPanel.SetActive(false);
            entrancePanel.SetActive(true);
        }

        [OnEventFire]
        public void CloseFriendsPanel(NodeRemoveEvent e, BattleSelectScreenNode screen, [JoinByScreen] SingleNode<InviteFriendsListComponent> inviteFriendsList, [JoinAll] SingleNode<SelfUserComponent> user)
        {
            this.CloseFriendsPanel(screen.battleSelectScreen.FriendsPanel, screen.battleSelectScreen.EntrancePanel, user.Entity);
        }

        [OnEventFire]
        public void CloseFriendsPanel(ButtonClickEvent e, SingleNode<HideInviteFriendsButtonComponent> hideButton, [JoinByScreen] SingleNode<BattleSelectScreenComponent> screen, [JoinAll] SingleNode<SelfUserComponent> user)
        {
            this.CloseFriendsPanel(screen.component.FriendsPanel, screen.component.EntrancePanel, user.Entity);
        }

        [OnEventFire]
        public void CloseFriendsPanel(NodeAddedEvent e, SingleNode<SelectedListItemComponent> battle, [JoinAll] SingleNode<InviteFriendsListComponent> inviteFriendsList, [JoinByScreen] SingleNode<BattleSelectScreenComponent> screen, [JoinAll] SingleNode<SelfUserComponent> user)
        {
            this.CloseFriendsPanel(screen.component.FriendsPanel, screen.component.EntrancePanel, user.Entity);
        }

        [OnEventFire]
        public void ShowFriendsPanel(ButtonClickEvent e, SingleNode<ShowInviteFriendsButtonComponent> showButton, [JoinByScreen] SingleNode<BattleSelectScreenComponent> screen, [JoinAll] SingleNode<SelfUserComponent> user)
        {
            screen.component.FriendsPanel.SetActive(true);
            screen.component.EntrancePanel.SetActive(false);
        }

        public class BattleSelectScreenNode : Node
        {
            public BattleSelectScreenComponent battleSelectScreen;
            public ActiveScreenComponent activeScreen;
        }
    }
}

