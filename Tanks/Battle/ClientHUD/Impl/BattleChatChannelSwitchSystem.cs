namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientHUD.API;
    using Tanks.Lobby.ClientCommunicator.API;
    using Tanks.Lobby.ClientCommunicator.Impl;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;

    public class BattleChatChannelSwitchSystem : ECSSystem
    {
        [OnEventFire]
        public void OnEnterBattle(NodeAddedEvent e, GeneralChatChannelNode battleChat, [JoinAll] ActiveHomeChannel activeChannel, [JoinAll] Dialog dialog)
        {
            dialog.chatDialog.SelectChannel(activeChannel.chatChannel.ChatType, new List<ChatMessage>());
        }

        [OnEventFire]
        public void OnExitBattle(NodeRemoveEvent e, GeneralChatChannelNode battleChat, [JoinAll] ActiveHomeChannel activeChannel, [JoinAll] Dialog dialog)
        {
            dialog.chatDialog.SelectChannel(activeChannel.chatChannel.ChatType, activeChannel.chatChannel.Messages);
        }

        [OnEventFire]
        public void OnRecievedMessage(RecievedLobbyChatMessageEvent e, SingleNode<ChatChannelUIComponent> chat, [JoinAll] Optional<GeneralChatChannelNode> battleChat, [JoinAll] Dialog dialog)
        {
            if (!battleChat.IsPresent())
            {
                dialog.chatDialog.SetLastMessage(e.Message);
            }
        }

        [OnEventFire]
        public void OnRecievedMessage(RecievedLobbyChatMessageEvent e, ActiveHomeChannel activeChannel, [JoinAll] Optional<GeneralChatChannelNode> battleChat, [JoinAll] Dialog dialog)
        {
            if (!battleChat.IsPresent())
            {
                dialog.chatDialog.AddUIMessage(e.Message);
            }
        }

        private void SetActiveChannelGUI(BattleChatGUINode battleChatGUINode, TeamColor teamColor)
        {
            ChatUIComponent chatUI = battleChatGUINode.chatUI;
            BattleChatLocalizedStringsComponent battleChatLocalizedStrings = battleChatGUINode.battleChatLocalizedStrings;
            string teamChatInputHint = string.Empty;
            Color commonTextColor = new Color();
            Color blueTeamNicknameColor = new Color();
            if (teamColor == TeamColor.BLUE)
            {
                blueTeamNicknameColor = chatUI.BlueTeamNicknameColor;
                commonTextColor = chatUI.BlueTeamNicknameColor;
                teamChatInputHint = battleChatLocalizedStrings.TeamChatInputHint;
            }
            else if (teamColor != TeamColor.RED)
            {
                blueTeamNicknameColor = chatUI.CommonTextColor;
                commonTextColor = chatUI.CommonTextColor;
                teamChatInputHint = battleChatLocalizedStrings.GeneralChatInputHint;
            }
            else
            {
                blueTeamNicknameColor = chatUI.RedTeamNicknameColor;
                commonTextColor = chatUI.RedTeamNicknameColor;
                teamChatInputHint = battleChatLocalizedStrings.TeamChatInputHint;
            }
            chatUI.InputHintText = $"{teamChatInputHint}: ";
            chatUI.InputHintColor = new Color(commonTextColor.r, commonTextColor.g, commonTextColor.b, chatUI.InputHintColor.a);
            chatUI.InputTextColor = chatUI.InputHintColor;
            chatUI.BottomLineColor = blueTeamNicknameColor;
            chatUI.SetHintSize((teamColor == TeamColor.BLUE) || (teamColor == TeamColor.RED));
        }

        [OnEventFire]
        public void SetGeneralChannelLoaded(NodeAddedEvent e, NotLoadedGeneralChatChannelNode notLoadedGeneralChatChannelNode, BattleChatGUINode battleChatGUINode)
        {
            this.SetActiveChannelGUI(battleChatGUINode, TeamColor.NONE);
            notLoadedGeneralChatChannelNode.Entity.AddComponent<ActiveBattleChannelComponent>();
            notLoadedGeneralChatChannelNode.Entity.AddComponent<LoadedChannelComponent>();
        }

        [OnEventFire]
        public void SetTeamChannelLoaded(NodeAddedEvent e, BattleChatGUINode battleChatGUINode, ActiveGeneralChatChannelNode activeGeneralChatChannelNode, NotLoadedTeamChatChannelNode notLoadedTeamChatChannelNode, [JoinAll] SelfUserTeamNode selfUserTeamNode, [JoinByTeam] TeamNode teamNode)
        {
            activeGeneralChatChannelNode.Entity.RemoveComponent<ActiveBattleChannelComponent>();
            notLoadedTeamChatChannelNode.Entity.AddComponent<ActiveBattleChannelComponent>();
            this.SetActiveChannelGUI(battleChatGUINode, teamNode.colorInBattle.TeamColor);
            notLoadedTeamChatChannelNode.Entity.AddComponent<LoadedChannelComponent>();
        }

        private void SwitchActiveChannel(ActiveChannelNode activeChannelNode, InactiveChannelNode inactiveChannelNode)
        {
            activeChannelNode.Entity.RemoveComponent<ActiveBattleChannelComponent>();
            inactiveChannelNode.Entity.AddComponent<ActiveBattleChannelComponent>();
        }

        [OnEventFire]
        public void SwitchChannelOnTabPressed(UpdateEvent e, ActiveChannelNode activeChannelNode, [JoinByScreen] InactiveChannelNode inactiveChannelNode)
        {
            if (InputManager.GetActionKeyDown(BattleChatActions.SWITCH_CHANNEL))
            {
                base.ScheduleEvent<BattleChannelSwitchEvent>(inactiveChannelNode);
            }
        }

        [OnEventFire]
        public void SwitchToGeneralChannel(BattleChannelSwitchEvent e, InactiveGeneralChatChannelNode inactiveChannelNode, [JoinAll] ActiveChannelNode activeChannelNode, [JoinByScreen] BattleChatGUINode battleChatGUINode)
        {
            this.SwitchActiveChannel(activeChannelNode, inactiveChannelNode);
            this.SetActiveChannelGUI(battleChatGUINode, TeamColor.NONE);
        }

        [OnEventFire]
        public void SwitchToGeneralChannelOnTeamChatRemove(NodeRemoveEvent e, TeamChatChannelNode teamChatChannelNode, [JoinAll] InactiveGeneralChatChannelNode inactiveChannelNode, [JoinByScreen] BattleChatGUINode battleChatGUINode)
        {
            if (teamChatChannelNode.Entity.HasComponent<ActiveBattleChannelComponent>())
            {
                inactiveChannelNode.Entity.AddComponent<ActiveBattleChannelComponent>();
                this.SetActiveChannelGUI(battleChatGUINode, TeamColor.NONE);
            }
        }

        [OnEventFire]
        public void SwitchToTeamChannel(BattleChannelSwitchEvent e, InactiveTeamChatChannelNode inactiveChannelNode, [JoinAll] SelfUserTeamNode selfIUserNode, [JoinByTeam] TeamNode teamNode, [JoinAll] ActiveChannelNode activeChannelNode, [JoinByScreen] BattleChatGUINode battleChatGUINode)
        {
            this.SwitchActiveChannel(activeChannelNode, inactiveChannelNode);
            this.SetActiveChannelGUI(battleChatGUINode, teamNode.colorInBattle.TeamColor);
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class ActiveChannelNode : Node
        {
            public ChatComponent chat;
            public ScreenGroupComponent screenGroup;
            public ActiveBattleChannelComponent activeBattleChannel;
        }

        public class ActiveGeneralChatChannelNode : BattleChatChannelSwitchSystem.GeneralChatChannelNode
        {
            public ActiveBattleChannelComponent activeBattleChannel;
        }

        public class ActiveHomeChannel : Node
        {
            public ChatChannelComponent chatChannel;
            public ChatChannelUIComponent chatChannelUI;
            public ActiveChannelComponent activeChannel;
        }

        public class BattleChatGUINode : Node
        {
            public ChatUIComponent chatUI;
            public BattleChatLocalizedStringsComponent battleChatLocalizedStrings;
            public ScreenGroupComponent screenGroup;
        }

        public class ChatChannelNode : Node
        {
            public ChatComponent chat;
        }

        public class Dialog : Node
        {
            public ChatDialogComponent chatDialog;
        }

        public class GeneralChatChannelNode : BattleChatChannelSwitchSystem.ChatChannelNode
        {
            public GeneralBattleChatComponent generalBattleChat;
        }

        [Not(typeof(ActiveBattleChannelComponent))]
        public class InactiveChannelNode : Node
        {
            public ChatComponent chat;
            public ScreenGroupComponent screenGroup;
        }

        public class InactiveGeneralChatChannelNode : BattleChatChannelSwitchSystem.InactiveChannelNode
        {
            public GeneralBattleChatComponent generalBattleChat;
        }

        public class InactiveTeamChatChannelNode : BattleChatChannelSwitchSystem.InactiveChannelNode
        {
            public TeamBattleChatComponent teamBattleChat;
        }

        [Not(typeof(LoadedChannelComponent))]
        public class NotLoadedGeneralChatChannelNode : BattleChatChannelSwitchSystem.GeneralChatChannelNode
        {
        }

        [Not(typeof(LoadedChannelComponent))]
        public class NotLoadedTeamChatChannelNode : BattleChatChannelSwitchSystem.TeamChatChannelNode
        {
        }

        public class SelfUserTeamNode : Node
        {
            public TeamGroupComponent teamGroup;
            public SelfBattleUserComponent selfBattleUser;
        }

        public class TeamChatChannelNode : BattleChatChannelSwitchSystem.ChatChannelNode
        {
            public TeamBattleChatComponent teamBattleChat;
        }

        public class TeamNode : Node
        {
            public TeamGroupComponent teamGroup;
            public ColorInBattleComponent colorInBattle;
        }
    }
}

