namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.Impl;

    public class BattleSpectatorInputContextSystem : ECSSystem
    {
        [OnEventFire]
        public void ActivateBattleChatContext(PointEnterToBattleChatScrollViewEvent e, BattleChatSpectatorNotPressedOutOfFocusNode battleChatSpectator)
        {
            InputManager.ActivateContext(BattleChatContexts.BATTLE_CHAT);
            battleChatSpectator.Entity.AddComponent<BattleChatInFocusComponent>();
        }

        [OnEventFire]
        public void DeactivateBattleChatContext(PointExitFromBattleChatScrollViewEvent e, BattleChatSpectatorNotPressedInFocusNode battleChatSpectator)
        {
            InputManager.DeactivateContext(BattleChatContexts.BATTLE_CHAT);
            battleChatSpectator.Entity.RemoveComponent<BattleChatInFocusComponent>();
        }

        [OnEventFire]
        public void MouseButtonDownInChat(UpdateEvent evt, BattleChatSpectatorNotPressedInFocusNode battleChatSpectator)
        {
            if (InputManager.GetMouseButtonDown(UnityInputConstants.MOUSE_BUTTON_LEFT))
            {
                battleChatSpectator.Entity.AddComponent<BattleChatStartDraggingComponent>();
            }
        }

        [OnEventFire]
        public void MouseButtonUpInChat(UpdateEvent evt, BattleChatSpectatorPressedInFocusNode battleChatSpectator)
        {
            if (InputManager.GetMouseButtonUp(UnityInputConstants.MOUSE_BUTTON_LEFT))
            {
                battleChatSpectator.Entity.RemoveComponent<BattleChatStartDraggingComponent>();
            }
        }

        [OnEventFire]
        public void MouseButtonUpOutOfChat(UpdateEvent evt, BattleChatSpectatorPressedOutOfFocusNode battleChatSpectator)
        {
            if (InputManager.GetMouseButtonUp(UnityInputConstants.MOUSE_BUTTON_LEFT))
            {
                InputManager.DeactivateContext(BattleChatContexts.BATTLE_CHAT);
                battleChatSpectator.Entity.RemoveComponent<BattleChatStartDraggingComponent>();
            }
        }

        [OnEventFire]
        public void SetChatInFocus(PointEnterToBattleChatScrollViewEvent e, BattleChatSpectatorPressedOutOfFocusNode battleChatSpectator)
        {
            battleChatSpectator.Entity.AddComponent<BattleChatInFocusComponent>();
        }

        [OnEventFire]
        public void SetChatOutOfFocus(PointExitFromBattleChatScrollViewEvent e, BattleChatSpectatorPressedInFocusNode battleChatSpectator)
        {
            battleChatSpectator.Entity.RemoveComponent<BattleChatInFocusComponent>();
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class BattleChatSpectatorNode : Node
        {
            public BattleChatSpectatorComponent battleChatSpectator;
        }

        [Not(typeof(BattleChatStartDraggingComponent))]
        public class BattleChatSpectatorNotPressedInFocusNode : BattleSpectatorInputContextSystem.BattleChatSpectatorNode
        {
            public BattleChatInFocusComponent battleChatInFocus;
        }

        [Not(typeof(BattleChatInFocusComponent)), Not(typeof(BattleChatStartDraggingComponent))]
        public class BattleChatSpectatorNotPressedOutOfFocusNode : BattleSpectatorInputContextSystem.BattleChatSpectatorNode
        {
        }

        public class BattleChatSpectatorPressedInFocusNode : BattleSpectatorInputContextSystem.BattleChatSpectatorNode
        {
            public BattleChatInFocusComponent battleChatInFocus;
            public BattleChatStartDraggingComponent battleChatStartDragging;
        }

        [Not(typeof(BattleChatInFocusComponent))]
        public class BattleChatSpectatorPressedOutOfFocusNode : BattleSpectatorInputContextSystem.BattleChatSpectatorNode
        {
            public BattleChatStartDraggingComponent battleChatStartDragging;
        }
    }
}

