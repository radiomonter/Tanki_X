namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientCommunicator.Impl;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class ResizeBattleChatScrollViewSystem : ECSSystem
    {
        private void ChangeScrollBarActivity(ChatUIComponent chatUI, bool chatIsActive, int messagesCount, int maxMessagesCount)
        {
            chatUI.ScrollBarActivity = chatIsActive && ((messagesCount > maxMessagesCount) || (chatUI.ScrollViewPosY >= 0f));
        }

        [OnEventFire]
        public void ResizeChatOnActionsState(NodeAddedEvent e, SingleNode<BattleActionsStateComponent> battleActionsState, [Combine] BattleChatGUINode battleChatGUINode, [Combine, JoinByScreen] ChatContentGUINode chatContentGUINode)
        {
            chatContentGUINode.chatContentGUI.gameObject.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            this.ResizeScrollView(battleChatGUINode, false);
        }

        [OnEventFire]
        public void ResizeChatOnChatState(NodeAddedEvent e, SingleNode<BattleChatStateComponent> battleChatState, [Combine] BattleChatGUINode battleChatGUINode)
        {
            this.ResizeScrollView(battleChatGUINode, true);
        }

        [OnEventFire]
        public void ResizeChatOnMessageResized(ResizeBattleChatScrollViewEvent e, Node anyNode, [JoinAll] SingleNode<BattleActionsStateComponent> battleActionsState, [JoinAll, Combine] BattleChatGUINode battleChatGUINode)
        {
            this.ResizeScrollView(battleChatGUINode, false);
        }

        [OnEventFire]
        public void ResizeChatOnMessageResized(ResizeBattleChatScrollViewEvent e, Node anyNode, [JoinAll] SingleNode<BattleChatStateComponent> battleChatState, [JoinAll, Combine] BattleChatGUINode battleChatGUINode)
        {
            this.ResizeScrollView(battleChatGUINode, true);
        }

        private void ResizeScrollView(BattleChatGUINode battleChatGUINode, bool chatIsActive)
        {
            battleChatGUINode.lazyScrollableVerticalList.AdjustPlaceholdersSiblingIndices();
            ChatUIComponent chatUI = battleChatGUINode.chatUI;
            LayoutRebuilder.ForceRebuildLayoutImmediate(chatUI.MessagesContainer.gameObject.GetComponent<RectTransform>());
            int a = chatUI.MessagesContainer.transform.childCount - 2;
            if (a != 0)
            {
                int b = !chatIsActive ? chatUI.MaxVisibleMessagesInPassiveState : chatUI.MaxVisibleMessagesInActiveState;
                int num3 = Mathf.Min(a, b);
                int index = a;
                float num5 = 0f;
                while (num3 > 0)
                {
                    Vector2 sizeDelta = chatUI.MessagesContainer.transform.GetChild(index).GetComponent<RectTransform>().sizeDelta;
                    num5 += sizeDelta.y;
                    index--;
                    num3--;
                }
                chatUI.ScrollViewHeight = num5;
                this.ChangeScrollBarActivity(chatUI, chatIsActive, a, b);
            }
        }

        [OnEventFire]
        public void ResizeSpectatorChatOnMessageResized(ResizeBattleChatScrollViewEvent e, Node anyNode, [JoinAll] BattleChatSpectatorGUINode battleChatSpectatorGUINode)
        {
            this.ResizeScrollView(battleChatSpectatorGUINode, false);
        }

        public class BattleChatGUINode : Node
        {
            public ChatUIComponent chatUI;
            public ScreenGroupComponent screenGroup;
            public LazyScrollableVerticalListComponent lazyScrollableVerticalList;
        }

        public class BattleChatSpectatorGUINode : ResizeBattleChatScrollViewSystem.BattleChatGUINode
        {
            public BattleChatSpectatorComponent battleChatSpectator;
        }

        public class ChatContentGUINode : Node
        {
            public ChatContentGUIComponent chatContentGUI;
            public ScreenGroupComponent screenGroup;
        }
    }
}

