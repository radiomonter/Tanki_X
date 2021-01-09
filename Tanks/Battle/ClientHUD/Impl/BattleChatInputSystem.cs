namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientCommunicator.API;
    using Tanks.Lobby.ClientCommunicator.Impl;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientNavigation.API;

    public class BattleChatInputSystem : ECSSystem
    {
        [OnEventFire]
        public void CheckBattleChatInputFocus(BattleChatFocusCheckEvent e, Node any, [JoinAll, Combine] InputFieldNode inputField)
        {
            if (inputField.inputField.InputField.isFocused)
            {
                e.InputIsFocused = true;
            }
        }

        [OnEventFire]
        public void SaveInputMessage(InputFieldValueChangedEvent e, InputFieldNode inputFieldNode, [JoinByScreen] BattleChatGUINode battleChatGUINode)
        {
            if (inputFieldNode.inputField.InputField.isFocused)
            {
                battleChatGUINode.chatUI.SavedInputMessage = inputFieldNode.inputField.Input;
            }
        }

        [OnEventFire]
        public void SetMaxMessageLength(NodeAddedEvent e, InputFieldNode inputFieldNode, [JoinByScreen, Combine] ChatNode chatNode)
        {
            inputFieldNode.inputField.InputField.characterLimit = chatNode.chatConfig.MaxMessageLength;
        }

        public class BattleChatGUINode : Node
        {
            public ChatUIComponent chatUI;
            public ScreenGroupComponent screenGroup;
        }

        public class ChatNode : Node
        {
            public ChatComponent chat;
            public ChatConfigComponent chatConfig;
            public ScreenGroupComponent screenGroup;
        }

        public class InputFieldNode : Node
        {
            public InputFieldComponent inputField;
            public ChatMessageInputComponent chatMessageInput;
            public ScreenGroupComponent screenGroup;
        }
    }
}

