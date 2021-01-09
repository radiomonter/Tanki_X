namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientCommunicator.Impl;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;

    public class BattleHUDStateControllerSystem : ECSSystem
    {
        private void SetBattleActionsState(NotSpectatorBattleChatGUINode battleChatGUINode, HUDScreenNode hudScreenNode)
        {
            battleChatGUINode.chatUI.InputPanelActivity = false;
            hudScreenNode.battleHUDESM.Esm.ChangeState<BattleHUDStates.ActionsState>();
        }

        [OnEventFire]
        public void SetBattleActionsStateOnEnter(NodeAddedEvent e, NotSpectatorBattleChatGUINode battleChatGUINode, HUDScreenNode hudScreenNode)
        {
            this.SetBattleActionsState(battleChatGUINode, hudScreenNode);
        }

        [OnEventComplete]
        public void SetBattleActionsStateOnEnterPressed(UpdateEvent e, SingleNode<BattleChatStateComponent> battleChatState, [JoinAll] NotSpectatorBattleChatGUINode battleChatGUINode, [JoinByScreen] HUDScreenNode hudScreenNode)
        {
            if (InputManager.GetKeyDown(KeyCode.Return) || InputManager.GetKeyDown(KeyCode.KeypadEnter))
            {
                this.SetBattleActionsState(battleChatGUINode, hudScreenNode);
            }
        }

        [OnEventComplete]
        public void SetBattleActionsStateOnEscPressed(UpdateEvent e, NotSpectatorBattleChatGUINode battleChatGUINode, [JoinByScreen] HUDScreenNode hudScreenNode)
        {
            if (InputManager.GetActionKeyDown(BattleChatActions.CLOSE_CHAT))
            {
                this.SetBattleActionsState(battleChatGUINode, hudScreenNode);
            }
        }

        [OnEventFire]
        public void SetBattleActionsStateOnExit(GoBackFromBattleEvent e, Node any, [JoinAll] NotSpectatorBattleChatGUINode battleChatGUINode, [JoinByScreen] HUDScreenNode hudScreenNode)
        {
            this.SetBattleActionsState(battleChatGUINode, hudScreenNode);
        }

        [OnEventFire]
        public void SetBattleActionsStateOnSendMessageButtonClick(ButtonClickEvent e, SingleNode<SendMessageButtonComponent> button, [JoinAll] NotSpectatorBattleChatGUINode battleChatGUINode, [JoinByScreen] HUDScreenNode hudScreenNode)
        {
            this.SetBattleActionsState(battleChatGUINode, hudScreenNode);
        }

        private void SetBattleChatState(NotSpectatorBattleChatGUINode battleChatGUINode, HUDScreenNode hudScreenNode)
        {
            battleChatGUINode.chatUI.InputPanelActivity = true;
            hudScreenNode.battleHUDESM.Esm.ChangeState<BattleHUDStates.ChatState>();
        }

        [OnEventComplete]
        public void SetBattleChatStateOnEnterPressed(UpdateEvent e, NotSpectatorBattleChatGUINode battleChatGUINode, [JoinByScreen] HUDScreenNode hudScreenNode)
        {
            if (InputManager.GetActionKeyDown(BattleActions.SHOW_CHAT))
            {
                this.SetBattleChatState(battleChatGUINode, hudScreenNode);
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class HUDScreenNode : Node
        {
            public BattleScreenComponent battleScreen;
            public BattleHUDESMComponent battleHUDESM;
            public ScreenGroupComponent screenGroup;
        }

        [Not(typeof(BattleChatSpectatorComponent))]
        public class NotSpectatorBattleChatGUINode : Node
        {
            public BattleChatUIComponent battleChatUI;
            public ChatUIComponent chatUI;
            public ScreenGroupComponent screenGroup;
        }
    }
}

