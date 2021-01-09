namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientNavigation.API;

    public class BattleLobbyScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void EnableEditButtonOnBattleFinish(NodeRemoveEvent e, SingleNode<BattleGroupComponent> bg, [JoinSelf] CustomLobbyNode lobby, [JoinAll] LobbyUINode lobbyUI)
        {
            lobbyUI.matchLobbyGUI.ShowEditParamsButton(lobby.Entity.HasComponent<SelfComponent>(), true);
        }

        [OnEventFire]
        public void HideElements(NodeRemoveEvent e, LobbyUINode lobbyUI)
        {
            lobbyUI.matchLobbyGUI.ShowCustomLobbyElements(false);
            lobbyUI.matchLobbyGUI.ShowEditParamsButton(false, true);
            lobbyUI.matchLobbyGUI.ShowChat(false);
        }

        [OnEventFire]
        public void LeaveBattleLobby(NodeRemoveEvent e, BattleLobbyNode lobby, [JoinAll] LobbyUINode lobbyUI)
        {
            MainScreenComponent.Instance.ShowHome();
        }

        [OnEventFire]
        public void OnDialogConfirm(DialogConfirmEvent e, SingleNode<EnterToBattleErrorDialog> inviteToLobbyDialog)
        {
            MainScreenComponent.Instance.ClearHistory();
            MainScreenComponent.Instance.ShowHome();
        }

        [OnEventFire]
        public void OnLobbyError(BattleLobbyEnterToBattleErrorEvent e, SingleNode<SelfUserComponent> user, [JoinAll] DialogsNode dialogs)
        {
            dialogs.dialogs60.Get<EnterToBattleErrorDialog>().Show();
        }

        [OnEventFire]
        public void OnMatchBeginning(NodeAddedEvent e, StartingLobbyNode lobby)
        {
            MainScreenComponent.Instance.ShowHome();
            MainScreenComponent.Instance.ShowScreen(MainScreenComponent.MainScreens.MatchLobby, true);
        }

        [OnEventFire]
        public void ShowBattleLobby(NodeAddedEvent e, BattleLobbyNode lobby)
        {
            MainScreenComponent.Instance.ShowHome();
            MainScreenComponent.Instance.ShowScreen(MainScreenComponent.MainScreens.MatchLobby, true);
        }

        [OnEventFire]
        public void ShowCustomLobbyElements(NodeAddedEvent e, LobbyUINode lobbyUI, CustomLobbyNode lobby)
        {
            lobbyUI.matchLobbyGUI.ShowCustomLobbyElements(true);
            lobbyUI.matchLobbyGUI.ShowEditParamsButton(lobby.Entity.HasComponent<SelfComponent>(), !lobby.Entity.HasComponent<BattleGroupComponent>());
            ClientBattleParams @params = lobby.clientBattleParams.Params;
            CreateBattleScreenComponent createBattleScreen = lobbyUI.matchLobbyGUI.createBattleScreen;
            lobbyUI.matchLobbyGUI.paramTimeLimit.text = @params.TimeLimit + " " + createBattleScreen.minutesText;
            lobbyUI.matchLobbyGUI.paramFriendlyFire.text = !@params.FriendlyFire ? ((string) createBattleScreen.offText) : ((string) createBattleScreen.onText);
            lobbyUI.matchLobbyGUI.enabledModules.text = @params.DisabledModules ? ((string) createBattleScreen.offText) : ((string) createBattleScreen.onText);
        }

        [OnEventFire]
        public void ShowHomeOrLobby(GoBackFromBattleScreensEvent e, Node node, [JoinAll] Optional<BattleLobbyNode> lobby)
        {
            MainScreenComponent.Instance.ShowHome();
            if (lobby.IsPresent())
            {
                MainScreenComponent.Instance.ShowScreen(MainScreenComponent.MainScreens.MatchLobby, true);
            }
        }

        [Inject]
        public static ConfigurationService ConfiguratorService { get; set; }

        public class BattleLobbyNode : Node
        {
            public BattleLobbyComponent battleLobby;
            public BattleLobbyGroupComponent battleLobbyGroup;
        }

        public class CustomLobbyNode : BattleLobbyScreenSystem.BattleLobbyNode
        {
            public CustomBattleLobbyComponent customBattleLobby;
            public UserGroupComponent userGroup;
            public ClientBattleParamsComponent clientBattleParams;
        }

        public class DialogsNode : Node
        {
            public Dialogs60Component dialogs60;
        }

        public class LobbyUINode : Node
        {
            public MatchLobbyGUIComponent matchLobbyGUI;
            public ScreenGroupComponent screenGroup;
        }

        public class StartingLobbyNode : Node
        {
            public LobbyStartingStateComponent lobbyStartingState;
        }
    }
}

