namespace Tanks.Battle.ClientHUD.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientBattleSelect.Impl;
    using Tanks.Lobby.ClientCommunicator.API;
    using Tanks.Lobby.ClientCommunicator.Impl;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientUserProfile.Impl;
    using UnityEngine;

    public class ReceiveBattleMessageSystem : ECSSystem
    {
        private void CreateMessage(ChatContentGUINode chatContentGUINode, BattleChatGUINode battleChatGUINode, Entity user, string message, bool isTeamMessage, TeamColor teamColor)
        {
            Color blueTeamNicknameColor;
            Color commonTextColor;
            ChatUIComponent chatUI = battleChatGUINode.chatUI;
            GameObject obj2 = Object.Instantiate<GameObject>(chatContentGUINode.chatContentGUI.MessagePrefab);
            Entity entity = obj2.GetComponent<EntityBehaviour>().Entity;
            if (teamColor == TeamColor.BLUE)
            {
                blueTeamNicknameColor = chatUI.BlueTeamNicknameColor;
                commonTextColor = !isTeamMessage ? chatUI.CommonTextColor : chatUI.BlueTeamTextColor;
            }
            else if (teamColor != TeamColor.RED)
            {
                blueTeamNicknameColor = chatUI.CommonNicknameColor;
                commonTextColor = chatUI.CommonTextColor;
            }
            else
            {
                blueTeamNicknameColor = chatUI.RedTeamNicknameColor;
                commonTextColor = !isTeamMessage ? chatUI.CommonTextColor : chatUI.RedTeamTextColor;
            }
            ChatMessageUIComponent component = obj2.GetComponent<ChatMessageUIComponent>();
            component.FirstPartText = user.GetComponent<UserUidComponent>().Uid + ": ";
            component.FirstPartTextColor = blueTeamNicknameColor;
            component.SecondPartText = message;
            component.SecondPartTextColor = commonTextColor;
            entity.AddComponent(new UserGroupComponent(user));
            RectTransform parent = chatContentGUINode.chatContentGUI.gameObject.GetComponent<RectTransform>();
            obj2.transform.SetParent(parent, false);
            base.ScheduleEvent<ResizeBattleChatScrollViewEvent>(entity);
            chatUI.SendMessage("RefreshCurve", SendMessageOptions.DontRequireReceiver);
        }

        [OnEventFire]
        public void ShowReceivedGeneralMessage(BattleChatUserMessageReceivedEvent e, UserNode user, SingleNode<BattleLobbyChatComponent> lobbyChat, [JoinByScreen] BattleChatGUINode battleChatGUINode, [JoinByScreen] ChatContentGUINode chatContentGUINode, [JoinAll] SingleNode<SelfUserComponent> selfUser)
        {
            TeamColor color = !user.Entity.HasComponent<TeamColorComponent>() ? TeamColor.NONE : user.Entity.GetComponent<TeamColorComponent>().TeamColor;
            TeamColor teamColor = (((color == (!selfUser.Entity.HasComponent<TeamColorComponent>() ? TeamColor.NONE : selfUser.Entity.GetComponent<TeamColorComponent>().TeamColor)) && (color != TeamColor.NONE)) || user.Entity.Equals(selfUser.Entity)) ? TeamColor.BLUE : TeamColor.RED;
            this.CreateMessage(chatContentGUINode, battleChatGUINode, user.Entity, e.Message, false, teamColor);
        }

        [OnEventFire]
        public void ShowReceivedGeneralMessage(BattleChatUserMessageReceivedEvent e, UserNode userNode, [JoinByUser] NotTeamBattleUserNode notTeamBattleUserNode, GeneralChatNode generalChatNode, [JoinByScreen] BattleChatGUINode battleChatGUINode, [JoinByScreen] ChatContentGUINode chatContentGUINode)
        {
            this.CreateMessage(chatContentGUINode, battleChatGUINode, userNode.Entity, e.Message, false, notTeamBattleUserNode.colorInBattle.TeamColor);
        }

        [OnEventFire]
        public void ShowReceivedGeneralMessage(BattleChatUserMessageReceivedEvent e, UserNode userNode, [JoinByUser] TeamBattleUserNode teamBattleUserNode, [JoinByTeam] TeamNode teamNode, GeneralChatNode generalChatNode, [JoinByScreen] BattleChatGUINode battleChatGUINode, [JoinByScreen] ChatContentGUINode chatContentGUINode)
        {
            this.CreateMessage(chatContentGUINode, battleChatGUINode, userNode.Entity, e.Message, false, teamNode.colorInBattle.TeamColor);
        }

        [OnEventFire]
        public void ShowReceivedMessage(ChatMessageReceivedEvent e, ChatNode chatNode, [JoinAll] SelfUserNode selfUser)
        {
            if (e.SystemMessage)
            {
                base.NewEvent(new BattleChatSystemMessageReceivedEvent(e.Message)).Attach(chatNode).Schedule();
            }
            else if (!selfUser.blackList.BlockedUsers.Contains(e.UserId))
            {
                base.NewEvent(new BattleChatValidMessageReceivedEvent(e.Message, e.UserId)).Attach(chatNode).Schedule();
            }
        }

        [OnEventFire]
        public void ShowReceivedMessage(BattleChatValidMessageReceivedEvent e, BattleLobbyChatNode chatNode, [JoinAll] SelfBattleLobbyUserNode selfBattleLobbyUser, [Combine, JoinByBattleLobby] BattleLobbyUserNode battleLobbyUserNode)
        {
            if (e.UserId == battleLobbyUserNode.Entity.Id)
            {
                base.NewEvent(new BattleChatUserMessageReceivedEvent(e.Message)).Attach(chatNode).Attach(battleLobbyUserNode).Schedule();
            }
        }

        [OnEventFire]
        public void ShowReceivedMessage(BattleChatValidMessageReceivedEvent e, BattleChatNode chatNode, [JoinAll] SelfBattleUserNode selfBattleUser, [Combine, JoinByBattle] BattleUserNode battleUserNode, [JoinByUser] UserNode userNode)
        {
            if (e.UserId == userNode.Entity.Id)
            {
                base.NewEvent(new BattleChatUserMessageReceivedEvent(e.Message)).Attach(chatNode).Attach(userNode).Schedule();
            }
        }

        [OnEventFire]
        public void ShowReceivedTeamMessage(BattleChatSystemMessageReceivedEvent e, ChatNode chatNode, [JoinByScreen] BattleChatGUINode battleChatGUINode, [JoinByScreen] ChatContentGUINode chatContentGUINode)
        {
            ChatUIComponent chatUI = battleChatGUINode.chatUI;
            GameObject obj2 = Object.Instantiate<GameObject>(chatContentGUINode.chatContentGUI.MessagePrefab);
            Entity entity = obj2.GetComponent<EntityBehaviour>().Entity;
            ChatMessageUIComponent component = obj2.GetComponent<ChatMessageUIComponent>();
            component.FirstPartText = string.Empty;
            component.SecondPartText = e.Message;
            component.SecondPartTextColor = chatUI.SystemMessageColor;
            RectTransform parent = chatContentGUINode.chatContentGUI.gameObject.GetComponent<RectTransform>();
            obj2.transform.SetParent(parent, false);
            parent.offsetMin = Vector2.zero;
            base.ScheduleEvent<ResizeBattleChatScrollViewEvent>(entity);
            chatUI.SendMessage("RefreshCurve", SendMessageOptions.DontRequireReceiver);
        }

        [OnEventFire]
        public void ShowReceivedTeamMessage(BattleChatUserMessageReceivedEvent e, UserNode userNode, TeamChatNode teamChatNode, [JoinByScreen] BattleChatGUINode battleChatGUINode, [JoinByScreen] ChatContentGUINode chatContentGUINode, [JoinAll] SelfUserTeamNode selfUserTeamNode, [JoinByTeam] TeamNode teamNode)
        {
            this.CreateMessage(chatContentGUINode, battleChatGUINode, userNode.Entity, e.Message, true, teamNode.colorInBattle.TeamColor);
        }

        public class BattleChatGUINode : Node
        {
            public ChatUIComponent chatUI;
            public ScreenGroupComponent screenGroup;
            public LazyScrollableVerticalListComponent lazyScrollableVerticalList;
        }

        [Not(typeof(BattleLobbyChatComponent))]
        public class BattleChatNode : ReceiveBattleMessageSystem.ChatNode
        {
        }

        public class BattleChatSystemMessageReceivedEvent : Event
        {
            public BattleChatSystemMessageReceivedEvent(string message)
            {
                this.Message = message;
            }

            public string Message { get; set; }
        }

        public class BattleChatUserMessageReceivedEvent : Event
        {
            public BattleChatUserMessageReceivedEvent(string message)
            {
                this.Message = message;
            }

            public string Message { get; set; }
        }

        public class BattleChatValidMessageReceivedEvent : Event
        {
            public BattleChatValidMessageReceivedEvent(string message, long userId)
            {
                this.Message = message;
                this.UserId = userId;
            }

            public string Message { get; set; }

            public long UserId { get; set; }
        }

        public class BattleLobbyChatNode : ReceiveBattleMessageSystem.ChatNode
        {
            public BattleLobbyChatComponent battleLobbyChat;
        }

        public class BattleLobbyUserNode : Node
        {
            public UserComponent user;
            public UserGroupComponent userGroup;
            public BattleLobbyGroupComponent battleLobbyGroup;
        }

        public class BattleUserNode : Node
        {
            public BattleUserComponent battleUser;
            public UserGroupComponent userGroup;
        }

        public class ChatContentGUINode : Node
        {
            public ChatContentGUIComponent chatContentGUI;
            public ScreenGroupComponent screenGroup;
        }

        public class ChatNode : Node
        {
            public ChatComponent chat;
            public ScreenGroupComponent screenGroup;
        }

        public class GeneralChatNode : ReceiveBattleMessageSystem.ChatNode
        {
            public GeneralBattleChatComponent generalBattleChat;
        }

        [Not(typeof(TeamGroupComponent))]
        public class NotTeamBattleUserNode : ReceiveBattleMessageSystem.BattleUserNode
        {
            public ColorInBattleComponent colorInBattle;
        }

        public class SelfBattleLobbyUserNode : ReceiveBattleMessageSystem.BattleLobbyUserNode
        {
            public SelfUserComponent selfUser;
        }

        public class SelfBattleUserNode : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public BattleGroupComponent battleGroup;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public BlackListComponent blackList;
        }

        public class SelfUserTeamNode : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public TeamGroupComponent teamGroup;
        }

        public class TeamBattleUserNode : ReceiveBattleMessageSystem.BattleUserNode
        {
            public TeamGroupComponent teamGroup;
        }

        public class TeamChatNode : ReceiveBattleMessageSystem.ChatNode
        {
            public TeamBattleChatComponent teamBattleChat;
        }

        public class TeamNode : Node
        {
            public TeamGroupComponent teamGroup;
            public ColorInBattleComponent colorInBattle;
        }

        public class UserNode : Node
        {
            public UserComponent user;
            public UserUidComponent userUid;
        }
    }
}

