namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientBattleSelect.Impl;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientMatchMaking.API;

    public class MatchMakingEntranceSystem : ECSSystem
    {
        [CompilerGenerated]
        private static Func<SquadUserNode, long> <>f__am$cache0;

        [OnEventFire]
        public void EnteredToMatchMakingEvent(Tanks.Lobby.ClientMatchMaking.API.EnteredToMatchMakingEvent e, SingleNode<MatchMakingModeComponent> mode)
        {
            base.ScheduleEvent<SaveBattleModeEvent>(mode);
        }

        [OnEventFire]
        public void EnteredToMatchMakingEvent(Tanks.Lobby.ClientMatchMaking.API.EnteredToMatchMakingEvent e, Node any, [JoinAll] SingleNode<ShareEnergyScreenComponent> shareScreen)
        {
            MainScreenComponent.Instance.HideShareEnergyScreen();
        }

        [OnEventFire]
        public void EnterFailed(EnterToMatchMakingFailedEvent e, SingleNode<MatchMakingModeComponent> mode, [JoinAll] SingleNode<SelfUserComponent> user)
        {
            base.ScheduleEvent<BattleLobbyEnterToBattleErrorEvent>(user);
            user.Entity.RemoveComponentIfPresent<UserEnteringToMatchMakingComponent>();
        }

        [OnEventFire]
        public void EnterToMatchMaking(ButtonClickEvent e, SingleNode<StartSquadBattleButtonComponent> button, [JoinAll] SelfUserInSquadNode user)
        {
            SquadTryEnterToMatchMakingAfterEnergySharingEvent eventInstance = new SquadTryEnterToMatchMakingAfterEnergySharingEvent {
                MatchMakingModeId = MainScreenComponent.Instance.lastSelectedGameModeId.Id
            };
            base.ScheduleEvent(eventInstance, user);
        }

        [OnEventFire]
        public void EnterToMatchMaking(UserEnterToMatchMakingEvent e, SingleNode<MatchMakingDefaultModeComponent> mode, [JoinAll] SelfUserNotInSquadNode user)
        {
            base.ScheduleEvent(new EnterToMatchMakingEvent(), mode);
        }

        [OnEventFire]
        public void EnterToMatchMaking(UserEnterToMatchMakingEvent e, NotDefaultMatchMakingModeNode mode, [JoinAll] SelfUserNotInSquadNode user)
        {
            base.ScheduleEvent(new EnterToMatchMakingEvent(), mode);
        }

        [OnEventFire]
        public void EnterToMatchMaking(UserEnterToMatchMakingEvent e, SelectedMatchMakingModeNode mode, [JoinAll] SelfUserInSquadNode user)
        {
            if (user.Entity.HasComponent<SquadLeaderComponent>())
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = sm => sm.Entity.Id;
                }
                List<long> list = base.Select<SquadUserNode>(user.Entity, typeof(SquadGroupComponent)).Select<SquadUserNode, long>(<>f__am$cache0).ToList<long>();
                MainScreenComponent.Instance.lastSelectedGameModeId = mode.Entity;
                SquadTryEnterToMatchMakingEvent eventInstance = new SquadTryEnterToMatchMakingEvent {
                    MatchMakingModeId = mode.Entity.Id,
                    RatingMatchMakingMode = mode.Entity.HasComponent<MatchMakingRatingModeComponent>()
                };
                base.ScheduleEvent(eventInstance, user);
            }
        }

        [OnEventFire]
        public void EnterToMatchMaking(ButtonClickEvent e, SelectedMatchMakingModeNode matchMakingModeNode, [JoinAll] SingleNode<MatchMakingComponent> matchMaking, [JoinAll] SingleNode<SelfUserComponent> user)
        {
            if (!matchMakingModeNode.gameModeSelectButton.Restricted)
            {
                bool flag2 = user.Entity.HasComponent<SquadLeaderComponent>();
                if (!user.Entity.HasComponent<SquadGroupComponent>() || flag2)
                {
                    this.RequestEnterToMatchMaking(user.Entity, matchMakingModeNode.Entity);
                }
            }
        }

        [OnEventComplete]
        public void EnterToMatchMaking(PlayAgainEvent e, Node any, [JoinAll] SingleNode<MatchMakingComponent> matchMaking, [JoinAll] SingleNode<SelfUserComponent> user)
        {
            if (e.ModeIsAvailable)
            {
                this.RequestEnterToMatchMaking(user.Entity, e.MatchMackingMode);
            }
        }

        [OnEventFire]
        public void ExitFromLobby(ButtonClickEvent e, SingleNode<ExitBattleLobbyButtonComponent> exitButton, [JoinAll] SingleNode<MatchMakingComponent> matchMaking)
        {
            base.ScheduleEvent(new ExitFromMatchMakingEvent(), matchMaking);
        }

        [OnEventFire]
        public void ExitFromMatchMaking(ButtonClickEvent e, SingleNode<CancelMatchSearchingButtonComponent> battleSelect, [JoinAll] SingleNode<MatchMakingComponent> matchMaking)
        {
            base.ScheduleEvent(new ExitFromMatchMakingEvent(), matchMaking);
        }

        [OnEventFire]
        public void ExitFromMatchMaking(CancelMatchSearchingEvent e, Node any, [JoinAll] SingleNode<MatchMakingComponent> matchMaking)
        {
            base.ScheduleEvent(new ExitFromMatchMakingEvent(), matchMaking);
        }

        [OnEventFire]
        public void ExitFromSharingDialog(ButtonClickEvent e, SingleNode<CancelEnergySharingButtonComponent> button, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] SelfUserInSquadNode user)
        {
            base.ScheduleEvent<CancelEnergySharingEvent>(user);
            user.Entity.RemoveComponentIfPresent<UserEnteringToMatchMakingComponent>();
        }

        [OnEventFire]
        public void HideShareEnergyWindow(NodeRemoveEvent e, SquadInEnergySharingStateNode squad, [JoinAll] SelfUserNode user)
        {
            MainScreenComponent.Instance.HideShareEnergyScreen();
            user.Entity.RemoveComponentIfPresent<UserEnteringToMatchMakingComponent>();
        }

        private bool RequestEnterToMatchMaking(Entity user, Entity mode)
        {
            if (user.HasComponent<UserEnteringToMatchMakingComponent>())
            {
                return false;
            }
            user.AddComponent<UserEnteringToMatchMakingComponent>();
            base.ScheduleEvent(new UserEnterToMatchMakingEvent(), mode);
            return true;
        }

        [OnEventFire]
        public void ShowCreateBattle(ButtonClickEvent e, CustomGameModeButtonNode modeButton, [JoinAll] SelfUserNode user, [JoinAll] SingleNode<Dialogs60Component> dialogs)
        {
            if (!user.Entity.HasComponent<SquadGroupComponent>() || user.Entity.HasComponent<SquadLeaderComponent>())
            {
                MainScreenComponent.Instance.ShowScreen(MainScreenComponent.MainScreens.CreateBattle, false);
            }
            else
            {
                CantStartGameInSquadDialogComponent component = dialogs.component.Get<CantStartGameInSquadDialogComponent>();
                component.CantSearch = false;
                component.Show(null);
            }
        }

        [OnEventFire]
        public void ShowCustomModesWindow(ButtonClickEvent e, SingleNode<ShowCustomModesScreenButtonComponent> button)
        {
            MainScreenComponent.Instance.ShowCustomBattleScreen();
        }

        [OnEventFire]
        public void ShowShareEnergyWindow(NodeAddedEvent e, SquadInEnergySharingStateNode squad)
        {
            MainScreenComponent.Instance.ShowShareEnergyScreen();
        }

        public class CustomGameModeButtonNode : Node
        {
            public ButtonMappingComponent buttonMapping;
            public CustomGameModeComponent customGameMode;
        }

        [Not(typeof(MatchMakingDefaultModeComponent))]
        public class NotDefaultMatchMakingModeNode : Node
        {
            public MatchMakingModeComponent matchMakingMode;
        }

        public class SelectedMatchMakingModeNode : Node
        {
            public GameModeSelectButtonComponent gameModeSelectButton;
            public MatchMakingModeComponent matchMakingMode;
            public DescriptionItemComponent descriptionItem;
        }

        public class SelfUserInSquadNode : MatchMakingEntranceSystem.SelfUserNode
        {
            public SquadGroupComponent squadGroup;
        }

        public class SelfUserNode : Node
        {
            public UserComponent user;
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
        }

        [Not(typeof(SquadGroupComponent))]
        public class SelfUserNotInSquadNode : MatchMakingEntranceSystem.SelfUserNode
        {
        }

        public class SquadInEnergySharingStateNode : MatchMakingEntranceSystem.SquadNode
        {
            public EnergySharingStateComponent energySharingState;
        }

        public class SquadNode : Node
        {
            public SquadComponent squad;
            public SquadGroupComponent squadGroup;
        }

        public class SquadUserNode : Node
        {
            public UserComponent user;
            public SquadGroupComponent squadGroup;
        }
    }
}

