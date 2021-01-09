namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;

    public class BattleInputContextSystem : ECSSystem
    {
        [OnEventFire]
        public void CheckMouseState(CheckMouseEvent e, SelfBattleUser selfBattleUser, Optional<SingleNode<SpectatorCameraComponent>> spectator)
        {
            if (spectator.IsPresent())
            {
                InputManager.DeactivateContext(BasicContexts.MOUSE_CONTEXT);
            }
            else if (selfBattleUser.mouseControlStateHolder.MouseControlAllowed)
            {
                InputManager.ActivateContext(BasicContexts.MOUSE_CONTEXT);
            }
        }

        [OnEventComplete]
        public void CheckMouseState(NodeAddedEvent e, SelfBattleUser selfBattleUser, SingleNode<BattleActionsStateComponent> battleActionsState, Optional<SingleNode<SpectatorCameraComponent>> spectator)
        {
            if (spectator.IsPresent())
            {
                InputManager.DeactivateContext(BasicContexts.MOUSE_CONTEXT);
            }
            else if (selfBattleUser.mouseControlStateHolder.MouseControlAllowed && !InputManager.CheckAction(BattleActions.SHOW_SCORE))
            {
                InputManager.ActivateContext(BasicContexts.MOUSE_CONTEXT);
            }
        }

        [OnEventFire]
        public void DeactivateMouseInSpectator(NodeAddedEvent e, SingleNode<SpectatorCameraComponent> spectator)
        {
            InputManager.DeactivateContext(BasicContexts.MOUSE_CONTEXT);
        }

        [OnEventFire]
        public void OnExitBattle(NodeRemoveEvent e, SingleNode<SelfBattleUserComponent> selfBattleUser)
        {
            this.SwitchContextToActions();
            InputManager.DeactivateContext(BasicContexts.MOUSE_CONTEXT);
        }

        [OnEventFire]
        public void OnFocus(ApplicationFocusEvent e, SingleNode<SelfBattleUserComponent> selfBattleUser, [JoinAll] SingleNode<BattleActionsStateComponent> battleActionsState)
        {
            if (e.IsFocused)
            {
                this.SwitchContextToActions();
                base.ScheduleEvent(new CheckMouseEvent(), selfBattleUser.Entity);
            }
        }

        [OnEventFire]
        public void SetBattleActionsState(NodeAddedEvent e, SingleNode<BattleActionsStateComponent> battleActionsState)
        {
            this.SwitchContextToActions();
        }

        [OnEventFire]
        public void SetBattleChatState(NodeAddedEvent e, SingleNode<BattleChatStateComponent> battleChatState)
        {
            this.SwitchContextToChat();
        }

        private void SwitchContextToActions()
        {
            InputManager.DeactivateContext(BattleChatContexts.BATTLE_CHAT);
            InputManager.ActivateContext(BasicContexts.BATTLE_CONTEXT);
        }

        private void SwitchContextToChat()
        {
            InputManager.DeactivateContext(BasicContexts.BATTLE_CONTEXT);
            InputManager.ActivateContext(BattleChatContexts.BATTLE_CHAT);
            InputManager.DeactivateContext(BasicContexts.MOUSE_CONTEXT);
        }

        [OnEventComplete]
        public void Update(UpdateEvent e, SingleNode<SelfBattleUserComponent> selfBattleUser, [JoinAll] SingleNode<BattleActionsStateComponent> battleActionsState)
        {
            if (InputManager.GetActionKeyUp(BattleActions.SHOW_SCORE))
            {
                base.ScheduleEvent(new CheckMouseEvent(), selfBattleUser.Entity);
            }
        }

        [OnEventComplete]
        public void Update(UpdateEvent e, SingleNode<SelfBattleUserComponent> selfBattleUser, [JoinAll] SingleNode<BattleShaftAimingStateComponent> battleShaftAimingState)
        {
            if (InputManager.GetActionKeyUp(BattleActions.SHOW_SCORE))
            {
                base.ScheduleEvent(new CheckMouseEvent(), selfBattleUser.Entity);
            }
        }

        [OnEventFire]
        public void UpdateToScore(UpdateEvent e, SingleNode<SelfBattleUserComponent> selfBattleUser, [JoinAll] SingleNode<BattleActionsStateComponent> battleActionsState)
        {
            if (InputManager.GetActionKeyDown(BattleActions.SHOW_SCORE))
            {
                InputManager.DeactivateContext(BasicContexts.MOUSE_CONTEXT);
            }
        }

        [OnEventFire]
        public void UpdateToScore(UpdateEvent e, SingleNode<SelfBattleUserComponent> selfBattleUser, [JoinAll] SingleNode<BattleShaftAimingStateComponent> battleShaftAimingState)
        {
            if (InputManager.GetActionKeyDown(BattleActions.SHOW_SCORE))
            {
                InputManager.DeactivateContext(BasicContexts.MOUSE_CONTEXT);
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class CheckMouseEvent : Event
        {
        }

        public class SelfBattleUser : Node
        {
            public MouseControlStateHolderComponent mouseControlStateHolder;
            public SelfBattleUserComponent selfBattleUser;
        }
    }
}

