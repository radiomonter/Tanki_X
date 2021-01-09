namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientCommunicator.API;
    using Tanks.Lobby.ClientNavigation.API;

    public class BattleChatSystem : ECSSystem
    {
        [OnEventFire]
        public void AddGeneralChatToScreenGroup(NodeAddedEvent e, GeneralBattleChatNode chatNode, ChatHUDNode chatHUDNode)
        {
            chatHUDNode.screenGroup.Attach(chatNode.Entity);
        }

        [OnEventFire]
        public void AddTeamChatToScreenGroup(NodeAddedEvent e, TeamBattleChatNode chatNode, ChatHUDNode chatHUDNode)
        {
            chatHUDNode.screenGroup.Attach(chatNode.Entity);
        }

        public class ChatHUDNode : Node
        {
            public BattleChatUIComponent battleChatUI;
            public ScreenGroupComponent screenGroup;
        }

        [Not(typeof(ScreenGroupComponent))]
        public class ChatNode : Node
        {
            public ChatComponent chat;
        }

        public class GeneralBattleChatNode : BattleChatSystem.ChatNode
        {
            public GeneralBattleChatComponent generalBattleChat;
        }

        public class TeamBattleChatNode : BattleChatSystem.ChatNode
        {
            public TeamBattleChatComponent teamBattleChat;
        }
    }
}

