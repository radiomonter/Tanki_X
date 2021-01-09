namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientMatchMaking.API;

    public class PlayAgainSystem : ECSSystem
    {
        [OnEventFire]
        public void CheckModeRestrictions(PlayAgainEvent e, SingleNode<ChosenMatchMackingModeComponent> chosenMode, [JoinAll] ICollection<MatchMakingModeNode> modes, [JoinAll] UserNode user, [JoinByUser] MountedHullNode hull, [JoinAll] ButtonNode button)
        {
            int level = hull.upgradeLevelItem.Level;
            Entity modeEntity = chosenMode.component.ModeEntity;
            if (modeEntity != null)
            {
                MatchMakingModeRestrictionsComponent component = modeEntity.GetComponent<MatchMakingModeRestrictionsComponent>();
                int rank = user.userRank.Rank;
                e.MatchMackingMode = modeEntity;
                e.ModeIsAvailable = (rank <= component.MaximalShowRank) && (rank >= component.MinimalShowRank);
                MainScreenComponent.Instance.ShowHome();
                if (!e.ModeIsAvailable)
                {
                    MainScreenComponent.Instance.ShowOrHideScreen(MainScreenComponent.MainScreens.PlayScreen, true);
                }
                else
                {
                    button.esm.Esm.ChangeState<PlayButtonStates.SearchingState>();
                    MainScreenComponent.Instance.ShowMatchSearching(modeEntity.GetComponent<DescriptionItemComponent>().Name);
                }
            }
        }

        [OnEventFire]
        public void HideAfterReturn(NodeAddedEvent e, SingleNode<PlayAgainButtonComponent> button, [JoinAll] Optional<SingleNode<ChosenMatchMackingModeComponent>> chosenMode)
        {
            button.component.gameObject.SetActive(chosenMode.IsPresent());
        }

        [OnEventFire]
        public void PlayAgain(ButtonClickEvent e, SingleNode<PlayAgainButtonComponent> button, [JoinAll] Optional<SingleNode<ChosenMatchMackingModeComponent>> chosenMode)
        {
            if (chosenMode.IsPresent())
            {
                base.NewEvent<PlayAgainEvent>().Attach(chosenMode.Get()).Schedule();
            }
        }

        [OnEventFire]
        public void RemoveChosenModes(NodeAddedEvent e, SingleNode<CustomBattleLobbyComponent> customBattle, ICollection<SingleNode<ChosenMatchMackingModeComponent>> modes)
        {
            foreach (SingleNode<ChosenMatchMackingModeComponent> node in modes)
            {
                base.DeleteEntity(node.Entity);
            }
        }

        [OnEventFire]
        public void RemoveChosenModes(ExitFromMatchMakingEvent e, Node any, ICollection<SingleNode<ChosenMatchMackingModeComponent>> modes)
        {
            foreach (SingleNode<ChosenMatchMackingModeComponent> node in modes)
            {
                base.DeleteEntity(node.Entity);
            }
        }

        [OnEventFire]
        public void SaveChosenMode(SaveBattleModeEvent e, MatchMakingModeNode mode, [JoinAll] ICollection<SingleNode<ChosenMatchMackingModeComponent>> modes)
        {
            foreach (SingleNode<ChosenMatchMackingModeComponent> node in modes)
            {
                base.DeleteEntity(node.Entity);
            }
            ChosenMatchMackingModeComponent component = new ChosenMatchMackingModeComponent {
                ModeEntity = mode.Entity
            };
            base.CreateEntity("ChosenMode").AddComponent(component);
        }

        public class ButtonNode : Node
        {
            public PlayButtonComponent playButton;
            public ESMComponent esm;
        }

        public class MatchMakingModeNode : Node
        {
            public MatchMakingModeComponent matchMakingMode;
            public MatchMakingModeRestrictionsComponent matchMakingModeRestrictions;
            public DescriptionItemComponent descriptionItem;
        }

        public class MountedHullNode : Node
        {
            public TankItemComponent tankItem;
            public MountedItemComponent mountedItem;
            public UpgradeLevelItemComponent upgradeLevelItem;
        }

        public class UserNode : Node
        {
            public UserComponent user;
            public SelfUserComponent selfUser;
            public UserRankComponent userRank;
        }
    }
}

