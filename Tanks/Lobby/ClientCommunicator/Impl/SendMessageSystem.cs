namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientCommunicator.API;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;

    public class SendMessageSystem : ECSSystem
    {
        [OnEventFire]
        public void OpenChat(ChatMessageClickEvent e, Node any, [JoinAll] ActiveLobbyChat activeChannel, [JoinAll] SingleNode<SelfUserComponent> self)
        {
            OpenPersonalChannelEvent eventInstance = new OpenPersonalChannelEvent {
                UserUid = e.Link
            };
            base.NewEvent(eventInstance).Attach(self).Attach(activeChannel).Schedule();
        }

        [OnEventFire]
        public void OpenChat(OpenPersonalChatFromContextMenuEvent e, SingleNode<UserUidComponent> friend, [JoinAll] SingleNode<SelfUserComponent> self, [JoinAll] ActiveLobbyChat activeChannel, [JoinAll] SingleNode<ChatDialogComponent> dialog)
        {
            if (!dialog.component.IsOpen() && !dialog.component.IsHidden())
            {
                dialog.component.Maximaze();
            }
            OpenPersonalChannelEvent eventInstance = new OpenPersonalChannelEvent {
                UserUid = friend.component.Uid.Replace("Deserter ", string.Empty)
            };
            base.NewEvent(eventInstance).Attach(self).Attach(activeChannel).Schedule();
        }

        [Mandatory, OnEventFire]
        public void SendLobbyMessage(SendMessageEvent e, Node any, [JoinAll] ActiveLobbyChat activeChannel, [JoinAll] SingleNode<SelfUserComponent> self)
        {
            Event event2;
            if (ChatCommands.IsCommand(e.Message, out event2))
            {
                base.NewEvent(event2).Attach(activeChannel).Attach(self).Schedule();
            }
            else
            {
                base.ScheduleEvent(new SendChatMessageEvent(e.Message), activeChannel);
            }
        }

        private void SendMessage(Entity chat, ChatUINode chatUI, InputFieldNode inputFieldNode)
        {
            string[] tags = new string[] { RichTextTags.COLOR, RichTextTags.SIZE };
            string str = ChatMessageUtil.RemoveTags(ChatMessageUtil.RemoveWhiteSpaces(inputFieldNode.inputField.Input), tags);
            if (!string.IsNullOrEmpty(str))
            {
                base.ScheduleEvent(new SendChatMessageEvent(str), chat);
                inputFieldNode.inputField.Input = string.Empty;
                inputFieldNode.inputField.InputField.Select();
                inputFieldNode.inputField.InputField.ActivateInputField();
                chatUI.chatUI.SavedInputMessage = string.Empty;
            }
        }

        [OnEventFire]
        public void SendMessageOnButtonClick(ButtonClickEvent e, SingleNode<SendMessageButtonComponent> sendMessageButton, [JoinByScreen] InputFieldNode inputFieldNode, [JoinByScreen] ChatUINode chatUI, [JoinByScreen] ChatNode chat)
        {
            this.SendMessage(chat.Entity, chatUI, inputFieldNode);
        }

        [OnEventFire]
        public void SendMessageOnEnterPressed(UpdateEvent e, InputFieldNode inputFieldNode, [JoinByScreen] ChatUINode chatUI, [JoinByScreen] ChatNode chat)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                this.SendMessage(chat.Entity, chatUI, inputFieldNode);
            }
        }

        [OnEventFire]
        public void SetMessageLength(InputFieldValueChangedEvent e, InputFieldNode input, [JoinByScreen] ChatNode chat)
        {
            string str = input.inputField.Input;
            int maxMessageLength = chat.chatConfig.MaxMessageLength;
            if (str.Length > maxMessageLength)
            {
                input.inputField.Input = str.Remove(maxMessageLength);
            }
        }

        public class ActiveLobbyChat : Node
        {
            public ActiveChannelComponent activeChannel;
            public ChatChannelComponent chatChannel;
        }

        public class ChatNode : Node
        {
            public ChatComponent chat;
            public ChatConfigComponent chatConfig;
            public ActiveBattleChannelComponent activeBattleChannel;
        }

        public class ChatUINode : Node
        {
            public ChatUIComponent chatUI;
        }

        public class InputFieldNode : Node
        {
            public InputFieldComponent inputField;
            public ChatMessageInputComponent chatMessageInput;
        }
    }
}

