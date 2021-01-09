namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientMatchMaking.API;

    public class PlayButtonClickSystem : ECSSystem
    {
        [OnEventFire]
        public void ExitedFromMatchMaking(ExitedFromMatchMakingEvent e, SingleNode<MatchMakingComponent> matchMaking, [JoinAll] ButtonNode button)
        {
            button.esm.Esm.ChangeState<PlayButtonStates.NormalState>();
            if (MainScreenComponent.Instance.gameObject.activeSelf && (MainScreenComponent.Instance.GetCurrentPanel() == MainScreenComponent.MainScreens.MatchSearching))
            {
                if (e.SelfAction && !button.playButton.SearchingDefaultGameMode)
                {
                    MainScreenComponent.Instance.ShowScreen(MainScreenComponent.MainScreens.PlayScreen, true);
                }
                else
                {
                    MainScreenComponent.Instance.ShowHome();
                }
            }
        }

        [OnEventFire]
        public void GoToLobby(ButtonClickEvent e, SingleNode<GoToLobbyButtonComponent> button)
        {
            MainScreenComponent.Instance.ShowScreen(MainScreenComponent.MainScreens.MatchLobby, true);
        }

        [OnEventFire]
        public void GoToMatchSearching(EnteredToMatchMakingEvent e, MatchMakingModeNode mode, [JoinAll] SingleNode<SelfUserComponent> user, [JoinAll] ButtonNode button)
        {
            user.Entity.RemoveComponentIfPresent<UserEnteringToMatchMakingComponent>();
            button.esm.Esm.ChangeState<PlayButtonStates.SearchingState>();
            MainScreenComponent.Instance.ShowMatchSearching(mode.descriptionItem.Name);
        }

        [OnEventFire]
        public void SendExitLobby(ButtonClickEvent e, ExitLobbyButtonNode button, [JoinAll] CustomLobbyNode lobby)
        {
            base.ScheduleEvent<ClientExitLobbyEvent>(lobby);
        }

        [OnEventFire]
        public void SendReturnToCustomBattle(ButtonClickEvent e, SingleNode<PlayButtonReturnToBattleStateComponent> button, [JoinAll] LobbyNode lobby)
        {
            base.ScheduleEvent<ReturnToCustomBattleEvent>(lobby);
        }

        [OnEventFire]
        public void SendStartCustomBattle(ButtonClickEvent e, SingleNode<PlayButtonStartCustomBattleStateComponent> button, [JoinAll] LobbyNode lobby)
        {
            base.ScheduleEvent<StartBattleEvent>(lobby);
        }

        [OnEventFire]
        public void ShowEnergyShareScreen(ButtonClickEvent e, PlayButtonViewSystem.ShareEnergyStateNode button)
        {
            MainScreenComponent.Instance.ShowShareEnergyScreen();
        }

        [OnEventFire]
        public void ShowGameModeSelect(ButtonClickEvent e, PlayButtonViewSystem.NormalStateNode playButton, [JoinAll] ButtonNode button, [JoinAll] Optional<SingleNode<RankedBattleGUIComponent>> rankedModeButton)
        {
            SelectDefaultMatchMakingModeEvent eventInstance = new SelectDefaultMatchMakingModeEvent();
            base.ScheduleEvent(eventInstance, playButton);
            playButton.playButton.SearchingDefaultGameMode = eventInstance.DefaultMode.IsPresent();
            if (eventInstance.DefaultMode.IsPresent())
            {
                button.esm.Esm.ChangeState<PlayButtonStates.SearchingState>();
                string name = eventInstance.DefaultMode.Get().GetComponent<DescriptionItemComponent>().Name;
                MainScreenComponent.Instance.ShowMatchSearching(name);
            }
            else if (rankedModeButton.IsPresent())
            {
                rankedModeButton.Get().component.Click();
            }
            else
            {
                MainScreenComponent.Instance.ShowOrHideScreen(MainScreenComponent.MainScreens.PlayScreen, true);
            }
        }

        [OnEventFire]
        public void ShowOrHideLobby(ButtonClickEvent e, PlayButtonViewSystem.CustomBattleStateNode button)
        {
            MainScreenComponent.Instance.ShowOrHideScreen(MainScreenComponent.MainScreens.MatchLobby, true);
        }

        [OnEventFire]
        public void ShowOrHideLobby(ButtonClickEvent e, PlayButtonViewSystem.MatchBeginTimerStateNode button)
        {
            MainScreenComponent.Instance.ShowOrHideScreen(MainScreenComponent.MainScreens.MatchLobby, true);
        }

        [OnEventFire]
        public void ShowOrHideLobby(ButtonClickEvent e, PlayButtonViewSystem.NotEnoughtPlayersStateNode button)
        {
            MainScreenComponent.Instance.ShowOrHideScreen(MainScreenComponent.MainScreens.MatchLobby, true);
        }

        [OnEventFire]
        public void ShowOrHideSearching(ButtonClickEvent e, PlayButtonViewSystem.SearchingStateNode button)
        {
            MainScreenComponent.Instance.ShowOrHideScreen(MainScreenComponent.MainScreens.MatchSearching, false);
        }

        public class ButtonNode : Node
        {
            public PlayButtonComponent playButton;
            public ESMComponent esm;
        }

        public class CustomLobbyNode : PlayButtonClickSystem.LobbyNode
        {
            public CustomBattleLobbyComponent customBattleLobby;
        }

        public class ExitLobbyButtonNode : Node
        {
            public ExitBattleLobbyButtonComponent exitBattleLobbyButton;
            public ButtonMappingComponent buttonMapping;
        }

        public class LobbyNode : Node
        {
            public BattleLobbyComponent battleLobby;
            public BattleLobbyGroupComponent battleLobbyGroup;
        }

        public class MatchMakingModeNode : Node
        {
            public MatchMakingModeComponent matchMakingMode;
            public DescriptionItemComponent descriptionItem;
        }

        public class UserEnteringToMMNode : Node
        {
            public SelfUserComponent selfUser;
            public UserEnteringToMatchMakingComponent userEnteringToMatchMaking;
        }
    }
}

