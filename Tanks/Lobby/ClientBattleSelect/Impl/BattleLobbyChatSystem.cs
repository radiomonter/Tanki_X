namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientCommunicator.API;
    using Tanks.Lobby.ClientCommunicator.Impl;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientNavigation.API;

    public class BattleLobbyChatSystem : ECSSystem
    {
        [OnEventFire]
        public void CleanChatOnBattleStart(NodeAddedEvent e, SingleNode<BattleLoadScreenComponent> battleScreen, [JoinAll] LobbyChatNode chat, [JoinByScreen] ChatUINode chatUI)
        {
            chatUI.chatUI.MessagesContainer.GetComponent<ChatContentGUIComponent>().ClearMessages();
        }

        [OnEventFire]
        public void CreateChatUI(NodeAddedEvent e, LobbyChatNode chat)
        {
            MatchLobbyGUIComponent component = MainScreenComponent.Instance.GetComponent<HomeScreenComponent>().BattleLobbyScreen.GetComponent<MatchLobbyGUIComponent>();
            EntityBehaviour behaviour = component.chat.GetComponent<EntityBehaviour>();
            if (behaviour.Entity != null)
            {
                behaviour.DestroyEntity();
            }
            EntityBehaviour behaviour2 = component.chat.GetComponent<ChatUIComponent>().MessagesContainer.GetComponent<EntityBehaviour>();
            if (behaviour2.Entity != null)
            {
                behaviour2.DestroyEntity();
            }
            behaviour.BuildEntity(base.CreateEntity("LobbyChat"));
            behaviour2.BuildEntity(base.CreateEntity("LobbyChatContent"));
            chat.Entity.AddComponent<ActiveBattleChannelComponent>();
        }

        [OnEventFire]
        public void DeleteChatUI(NodeRemoveEvent e, LobbyChatNode chat, [JoinAll] ChatUINode chatUI)
        {
            chat.Entity.RemoveComponent<ActiveBattleChannelComponent>();
            chatUI.battleLobbyChatUI.GetComponent<ChatUIComponent>().MessagesContainer.GetComponent<EntityBehaviour>().DestroyEntity();
            chatUI.battleLobbyChatUI.gameObject.GetComponent<EntityBehaviour>().DestroyEntity();
        }

        [OnEventFire]
        public void GroupChat(NodeAddedEvent e, LobbyUINode lobbyUI, LobbyChatNode chat)
        {
            lobbyUI.matchLobbyGUI.ShowChat(true);
            if (chat.Entity.HasComponent<ScreenGroupComponent>())
            {
                chat.Entity.RemoveComponent<ScreenGroupComponent>();
            }
            lobbyUI.screenGroup.Attach(chat.Entity);
        }

        public class ChatUINode : Node
        {
            public BattleLobbyChatUIComponent battleLobbyChatUI;
            public ChatUIComponent chatUI;
            public ScreenGroupComponent screenGroup;
        }

        public class CustomLobbyNode : Node
        {
            public CustomBattleLobbyComponent customBattleLobby;
        }

        public class LobbyChatNode : Node
        {
            public ChatComponent chat;
            public BattleLobbyChatComponent battleLobbyChat;
        }

        public class LobbyUINode : Node
        {
            public MatchLobbyGUIComponent matchLobbyGUI;
            public ScreenGroupComponent screenGroup;
        }

        public class LobbyWithBattleGroupNode : Node
        {
            public BattleLobbyComponent battleLobby;
            public BattleGroupComponent battleGroup;
        }
    }
}

