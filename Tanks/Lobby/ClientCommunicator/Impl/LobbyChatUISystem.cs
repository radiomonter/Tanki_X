namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;

    public class LobbyChatUISystem : ECSSystem
    {
        [OnEventFire]
        public void OnChannelActivate(NodeAddedEvent e, ActiveChannel activeChannel, Dialog dialog)
        {
            dialog.chatDialog.SetHeader(activeChannel.chatChannelUI.GetSpriteUid(), activeChannel.chatChannelUI.Name, activeChannel.chatChannel.ChatType == ChatType.PERSONAL);
            dialog.chatDialog.SelectChannel(activeChannel.chatChannel.ChatType, activeChannel.chatChannel.Messages);
        }

        [OnEventFire]
        public void OnChannelActivate(NodeAddedEvent e, ActiveNotGeneralChannel activeChannel, Dialog dialog)
        {
            int unread = activeChannel.chatChannelUI.Unread;
            dialog.chatDialog.Unread -= unread;
            activeChannel.chatChannelUI.Unread = 0;
        }

        [OnEventFire]
        public void OnChannelActivate(ChatMaximazedEvent e, Node any, [JoinAll] Dialog dialog, [JoinAll] ActiveNotGeneralChannel activeChannel)
        {
            int unread = activeChannel.chatChannelUI.Unread;
            dialog.chatDialog.Unread -= unread;
            activeChannel.chatChannelUI.Unread = 0;
        }

        [OnEventFire]
        public void OnChannelClose(NodeRemoveEvent e, SingleNode<ChatChannelUIComponent> channel, [JoinAll] Dialog dialog)
        {
            dialog.chatDialog.Unread -= channel.component.Unread;
        }

        [OnEventFire]
        public void OnRecievedMessage(RecievedLobbyChatMessageEvent e, ActiveNotGeneralChannel activeChannel, [JoinAll] Dialog dialog)
        {
            if (!dialog.chatDialog.IsOpen())
            {
                activeChannel.chatChannelUI.Unread++;
                dialog.chatDialog.Unread++;
            }
        }

        [OnEventComplete]
        public void OnRecievedMessage(RecievedLobbyChatMessageEvent e, InactiveNotGeneralChannel inactiveChannel, [JoinAll] Dialog dialog)
        {
            inactiveChannel.chatChannelUI.Unread++;
            dialog.chatDialog.Unread++;
        }

        [OnEventFire]
        public void OnRecievedMessage(RecievedLobbyChatMessageEvent e, Node any, [JoinAll] ActiveOverallChannel activeOverallChannel, [JoinAll] Dialog dialog)
        {
            if (!e.Message.System)
            {
                dialog.chatDialog.AddUIMessage(e.Message);
            }
        }

        [OnEventFire]
        public void SelectChannel(SelectChannelEvent e, ChannelUI selectedChannel, [JoinAll] Optional<ActiveChannel> activeChannel)
        {
            if (activeChannel.IsPresent())
            {
                if (ReferenceEquals(selectedChannel.Entity, activeChannel.Get().Entity))
                {
                    return;
                }
                activeChannel.Get().chatChannelUI.Deselect();
                activeChannel.Get().Entity.RemoveComponent<ActiveChannelComponent>();
            }
            selectedChannel.chatChannelUI.Select();
            selectedChannel.Entity.AddComponent<ActiveChannelComponent>();
        }

        [OnEventComplete]
        public void ShowReceivedMessage(NodeAddedEvent e, ActiveOverallChannel activeOverallChannel, Dialog dialog)
        {
            dialog.chatDialog.SetInputInteractable(false);
        }

        [OnEventComplete]
        public void ShowReceivedMessage(NodeRemoveEvent e, ActiveOverallChannel activeOverallChannel, [JoinAll] Dialog dialog)
        {
            dialog.chatDialog.SetInputInteractable(true);
        }

        public class ActiveChannel : LobbyChatUISystem.ChannelUI
        {
            public ActiveChannelComponent activeChannel;
        }

        [Not(typeof(GeneralChatComponent))]
        public class ActiveNotGeneralChannel : LobbyChatUISystem.ActiveChannel
        {
        }

        public class ActiveOverallChannel : LobbyChatUISystem.ActiveChannel
        {
            public OverallChannelComponent overallChannel;
        }

        public class Channel : Node
        {
            public ChatChannelComponent chatChannel;
        }

        public class ChannelUI : LobbyChatUISystem.Channel
        {
            public ChatChannelUIComponent chatChannelUI;
        }

        public class Dialog : Node
        {
            public ChatDialogComponent chatDialog;
        }

        [Not(typeof(ActiveChannelComponent))]
        public class InactiveChannel : LobbyChatUISystem.ChannelUI
        {
        }

        [Not(typeof(GeneralChatComponent))]
        public class InactiveNotGeneralChannel : LobbyChatUISystem.InactiveChannel
        {
        }

        [Not(typeof(OverallChannelComponent))]
        public class NotOverallChannel : LobbyChatUISystem.ChannelUI
        {
        }

        public class Overall : LobbyChatUISystem.Channel
        {
            public OverallChannelComponent overallChannel;
        }
    }
}

