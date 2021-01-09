namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;

    public class ChatSystem : ECSSystem
    {
        [OnEventFire]
        public void ActivateInputField(NodeAddedEvent e, InputFieldNode inputFieldNode, [JoinByScreen] ChatUINode chatUI)
        {
            inputFieldNode.inputField.Input = string.Empty;
            inputFieldNode.inputField.InputField.Select();
            inputFieldNode.inputField.InputField.ActivateInputField();
            chatUI.chatUI.SendMessage("RefreshCurve", SendMessageOptions.DontRequireReceiver);
        }

        [OnEventFire]
        public void CheckoInputFieldFocus(UpdateEvent e, BattleInputFieldNode inputFieldNode)
        {
            if (!inputFieldNode.inputField.InputField.isFocused)
            {
                inputFieldNode.inputField.InputField.Select();
                inputFieldNode.inputField.InputField.ActivateInputField();
            }
        }

        [OnEventFire]
        public void ClearChatOnExit(NodeRemoveEvent e, ChatUINode chatUI)
        {
            ChatUIComponent component = chatUI.chatUI;
            component.InputHintText = string.Empty;
            component.BottomLineColor = component.CommonTextColor;
            component.SavedInputMessage = string.Empty;
            component.MessagesContainer.GetComponent<ChatContentGUIComponent>().ClearMessages();
        }

        public class BattleInputFieldNode : ChatSystem.InputFieldNode
        {
            public BattleChatMessageInputComponent battleChatMessageInput;
        }

        public class ChatUINode : Node
        {
            public ChatUIComponent chatUI;
        }

        public class InputFieldNode : Node
        {
            public InputFieldComponent inputField;
            public ScreenGroupComponent screenGroup;
            public ChatMessageInputComponent chatMessageInput;
        }
    }
}

