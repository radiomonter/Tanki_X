namespace Tanks.Lobby.ClientLoading.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientLoading.API;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;

    public class OutputLogSystem : ECSSystem
    {
        private const string LOG_PREFIX = "LOG_MARK: ";
        private bool firstUserLogged;

        [OnEventFire]
        public void LogBattleLoadComplete(NodeAddedEvent e, BattleLoadCompletedNode loadCompleted)
        {
            this.WriteToLog("Battle load is completed");
        }

        [OnEventFire]
        public void LogClientSessionAdded(NodeAddedEvent e, SingleNode<ClientSessionComponent> clientSession)
        {
            this.WriteToLog("Client session added: " + clientSession.Entity.Id);
        }

        [OnEventFire]
        public void LogClientSessionRemoved(NodeRemoveEvent e, SingleNode<ClientSessionComponent> clientSession)
        {
            this.WriteToLog("Client session removed: " + clientSession.Entity.Id);
        }

        [OnEventFire]
        public void LogLoadScene(LoadSceneEvent e, Node node)
        {
            this.WriteToLog(e.SceneName + " scene  start async loading");
        }

        [OnEventFire]
        public void LogLobbyAdded(NodeAddedEvent e, SingleNode<LobbyComponent> lobby)
        {
            this.WriteToLog("Lobby added");
        }

        [OnEventFire]
        public void LogLobbyLoadComplete(NodeAddedEvent e, LobbyLoadCompletedNode loadCompleted)
        {
            this.WriteToLog("Lobby load is completed");
        }

        [OnEventFire]
        public void LogMapInstanceInited(NodeAddedEvent e, SingleNode<MapInstanceComponent> map)
        {
            this.WriteToLog("Battle map is inited");
        }

        [OnEventFire]
        public void LogSelfUserAdded(NodeAddedEvent e, SingleNode<SelfUserComponent> user)
        {
            this.WriteToLog("Self user added: " + user.Entity.Id);
        }

        [OnEventFire]
        public void LogSystemInfo(NodeAddedEvent e, SingleNode<LobbyLoadScreenComponent> lobbyLoadScreen)
        {
            this.WriteToLog("Device type: " + SystemInfo.deviceType);
            this.WriteToLog("Graphics device name: " + SystemInfo.graphicsDeviceName);
            this.WriteToLog("Graphics memory size: " + SystemInfo.graphicsMemorySize + " Mb");
            this.WriteToLog("Graphics shader model: " + SystemInfo.graphicsShaderLevel);
            this.WriteToLog("Operating system: " + SystemInfo.operatingSystem);
            this.WriteToLog("System memory size: ~" + SystemInfo.systemMemorySize + " Mb");
            this.WriteToLog("Processor type: " + SystemInfo.processorType);
            this.WriteToLog("Processor count: " + SystemInfo.processorCount);
        }

        [OnEventFire]
        public void LogTopPanelAuthenticatedAdded(NodeAddedEvent e, SingleNode<TopPanelAuthenticatedComponent> topPanel)
        {
            this.WriteToLog("TopPanelAuthenticatedComponent added");
        }

        [OnEventFire]
        public void LogUserLeftBattle(NodeRemoveEvent e, SingleNode<SelfBattleUserComponent> selfBattleUser)
        {
            this.WriteToLog("User left battle");
        }

        [OnEventFire]
        public void LogUserOnHomeScreen(NodeAddedEvent e, SingleNode<HomeScreenComponent> homeScreen)
        {
            this.WriteToLog("User is on Home screen.");
        }

        [OnEventFire]
        public void LogUserReadyToBattle(NodeAddedEvent e, ReadyToBattleUser readyToBattleUser)
        {
            this.WriteToLog("User is ready to battle");
        }

        [OnEventFire]
        public void LogUserReadyToLobby(NodeAddedEvent e, ReadyToLobbyUser user)
        {
            this.WriteToLog("User ready to lobby");
        }

        [OnEventFire]
        public void LogUserTryGoToBattle(NodeAddedEvent e, BattleUserNode user, [JoinByBattle] BattleNode battle, [JoinByMap] MapNode map)
        {
            this.WriteToLog("User start going to battle " + map.descriptionItem.Name);
        }

        private void WriteToLog(string message)
        {
            Console.WriteLine("LOG_MARK: " + message);
        }

        public class BattleLoadCompletedNode : Node
        {
            public LoadProgressTaskCompleteComponent loadProgressTaskComplete;
            public BattleLoadScreenComponent battleLoadScreen;
        }

        public class BattleNode : Node
        {
            public MapGroupComponent mapGroup;
            public BattleComponent battle;
        }

        public class BattleUserNode : Node
        {
            public BattleGroupComponent battleGroup;
            public SelfBattleUserComponent selfBattleUser;
        }

        public class LobbyLoadCompletedNode : Node
        {
            public LoadProgressTaskCompleteComponent loadProgressTaskComplete;
            public LobbyLoadScreenComponent lobbyLoadScreen;
        }

        public class MapNode : Node
        {
            public MapComponent map;
            public MapGroupComponent mapGroup;
            public DescriptionItemComponent descriptionItem;
        }

        public class ReadyToBattleUser : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public UserReadyToBattleComponent userReadyToBattle;
        }

        public class ReadyToLobbyUser : Node
        {
            public SelfUserComponent selfUser;
            public UserReadyForLobbyComponent userReadyForLobby;
        }
    }
}

