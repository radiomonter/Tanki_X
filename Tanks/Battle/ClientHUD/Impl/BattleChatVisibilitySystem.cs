namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientHUD.API;
    using Tanks.Lobby.ClientCommunicator.Impl;
    using Tanks.Lobby.ClientNavigation.API;

    public class BattleChatVisibilitySystem : ECSSystem
    {
        private static readonly string BATTLE_CHAT_SHOW_PREREQUISITE = "BATTLE_CHAT_SHOW_PREREQUISITE";
        private static readonly string BATTLE_CHAT_SHOW_MESSAGES_PREREQUISITE = "BATTLE_CHAT_SHOW_MESSAGES_PREREQUISITE";

        [OnEventFire]
        public void DisableHideMessagesOnChatState(NodeAddedEvent e, BattleChatStateNode battleChatStateNode, [JoinByScreen] ChatContentWithSheduleNode chatContentWithSheduleNode)
        {
            this.DisableHideMessagesSchedule(chatContentWithSheduleNode);
        }

        [OnEventFire]
        public void DisableHideMessagesOnMessageReceived(NodeAddedEvent e, MessageNode battleChatMessageGUI, [JoinByScreen] SingleNode<BattleChatUIComponent> battleChat, [JoinByScreen] ChatContentNode chatContentNode, [JoinByScreen] ChatContentWithSheduleNode chatContentWithSheduleNode, [JoinAll] Optional<SingleNode<BattleActionsStateComponent>> battleActionsState, [JoinAll] Optional<SingleNode<BattleShaftAimingStateComponent>> battleAimState)
        {
            if (battleActionsState.IsPresent() || battleAimState.IsPresent())
            {
                this.DisableHideMessagesSchedule(chatContentWithSheduleNode);
            }
        }

        private void DisableHideMessagesSchedule(ChatContentWithSheduleNode chatContentWithSheduleNode)
        {
            chatContentWithSheduleNode.hideBattleChatMessagesShedule.ScheduledEvent.Manager().Cancel();
            chatContentWithSheduleNode.Entity.RemoveComponent<HideBattleChatMessagesSheduleComponent>();
        }

        [OnEventFire]
        public void HideBattleChatElements(NodeAddedEvent e, SingleNode<BattleActionsStateComponent> battleActionsState, [Combine] BattleChatElementNode battleChatElementNode)
        {
            battleChatElementNode.visibilityPrerequisites.RemoveShowPrerequisite(BATTLE_CHAT_SHOW_PREREQUISITE, false);
        }

        [OnEventFire]
        public void HideBattleChatMessages(NodeRemoveEvent e, BattleChatStateNode battleChatStateNode, [JoinByScreen] ChatContentNode chatContentNode)
        {
            this.HideMessagesDelayed(chatContentNode);
        }

        [OnEventFire]
        public void HideBattleChatMessages(StopVisiblePeriodEvent e, ChatContentWithSheduleNode chatContentWithSheduleNode, [JoinAll] Optional<SingleNode<BattleActionsStateComponent>> battleActionsState, [JoinAll] Optional<SingleNode<BattleShaftAimingStateComponent>> battleAimState)
        {
            if (battleActionsState.IsPresent() || battleAimState.IsPresent())
            {
                chatContentWithSheduleNode.visibilityPrerequisites.RemoveShowPrerequisite(BATTLE_CHAT_SHOW_MESSAGES_PREREQUISITE, false);
                chatContentWithSheduleNode.Entity.RemoveComponent<HideBattleChatMessagesSheduleComponent>();
            }
        }

        private void HideMessagesDelayed(ChatContentNode chatContentNode)
        {
            ScheduledEvent scheduledEvent = base.NewEvent<StopVisiblePeriodEvent>().Attach(chatContentNode).ScheduleDelayed((float) chatContentNode.visibilityInterval.intervalInSec);
            chatContentNode.Entity.AddComponent(new HideBattleChatMessagesSheduleComponent(scheduledEvent));
        }

        [OnEventFire]
        public void HideMessagesOnShowScore(UpdateEvent e, ChatContentWithSheduleNode chatContentWithSheduleNode, [JoinAll] SingleNode<BattleActionsStateComponent> battleActionsState)
        {
            if (InputManager.CheckAction(BattleActions.SHOW_SCORE))
            {
                this.DisableHideMessagesSchedule(chatContentWithSheduleNode);
                chatContentWithSheduleNode.visibilityPrerequisites.RemoveShowPrerequisite(BATTLE_CHAT_SHOW_MESSAGES_PREREQUISITE, false);
            }
        }

        [OnEventFire]
        public void ShowBattleChatElements(NodeAddedEvent e, SingleNode<BattleChatStateComponent> battleChatState, [Combine] BattleChatElementNode battleChatElementNode)
        {
            battleChatElementNode.visibilityPrerequisites.AddShowPrerequisite(BATTLE_CHAT_SHOW_PREREQUISITE, false);
        }

        [OnEventFire]
        public void ShowBattleChatMessagesOnChatState(NodeAddedEvent e, BattleChatStateNode battleChatStateNode, [JoinByScreen] ChatContentNode chatContentNode)
        {
            chatContentNode.visibilityPrerequisites.AddShowPrerequisite(BATTLE_CHAT_SHOW_MESSAGES_PREREQUISITE, false);
        }

        [OnEventComplete]
        public void ShowBattleChatMessagesOnMessageReceived(NodeAddedEvent e, MessageNode battleChatMessageGUI, [JoinByScreen] SingleNode<BattleChatUIComponent> battleChat, [JoinByScreen] ChatContentNode chatContentNode, [JoinAll] Optional<SingleNode<BattleActionsStateComponent>> battleActionsState, [JoinAll] Optional<SingleNode<BattleShaftAimingStateComponent>> battleAimState)
        {
            if ((battleActionsState.IsPresent() || battleAimState.IsPresent()) && !battleChatMessageGUI.chatMessageUI.showed)
            {
                battleChatMessageGUI.chatMessageUI.showed = true;
                chatContentNode.visibilityPrerequisites.AddShowPrerequisite(BATTLE_CHAT_SHOW_MESSAGES_PREREQUISITE, false);
                this.HideMessagesDelayed(chatContentNode);
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class BattleChatElementNode : Node
        {
            public ShowWhileBattleChatIsActiveComponent showWhileBattleChatIsActive;
            public VisibilityPrerequisitesComponent visibilityPrerequisites;
        }

        public class BattleChatStateNode : Node
        {
            public BattleChatStateComponent battleChatState;
            public ScreenGroupComponent screenGroup;
        }

        public class ChatContentNode : Node
        {
            public ChatContentGUIComponent chatContentGui;
            public VisibilityPrerequisitesComponent visibilityPrerequisites;
            public VisibilityIntervalComponent visibilityInterval;
            public ScreenGroupComponent screenGroup;
        }

        public class ChatContentWithSheduleNode : BattleChatVisibilitySystem.ChatContentNode
        {
            public HideBattleChatMessagesSheduleComponent hideBattleChatMessagesShedule;
        }

        public class MessageNode : Node
        {
            public ChatMessageUIComponent chatMessageUI;
            public ScreenGroupComponent screenGroup;
        }
    }
}

