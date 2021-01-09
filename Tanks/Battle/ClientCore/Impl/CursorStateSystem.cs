namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class CursorStateSystem : ECSSystem
    {
        [OnEventFire]
        public void HideCursor(BattleFullyLoadedEvent e, UserAsTank selfBattleUserAsTank)
        {
            if (selfBattleUserAsTank.mouseControlStateHolder.MouseControlAllowed)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        private bool IsErrorScreen() => 
            SceneManager.GetActiveScene() == SceneManager.GetSceneByName(SceneNames.FATAL_ERROR);

        [OnEventFire]
        public void OnFocus(ApplicationFocusEvent e, Spectator spectator)
        {
            if (e.IsFocused && !this.IsErrorScreen())
            {
                Cursor.visible = spectator.cursorState.CursorVisible;
                Cursor.lockState = spectator.cursorState.CursorLockState;
            }
        }

        [OnEventFire]
        public void OnFocus(ApplicationFocusEvent e, UserAsTank userAsTank)
        {
            if (e.IsFocused && (userAsTank.mouseControlStateHolder.MouseControlAllowed && !this.IsErrorScreen()))
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        [OnEventFire]
        public void SetBattleChatState(NodeAddedEvent e, SingleNode<BattleChatStateComponent> battleChatState)
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
        }

        [OnEventFire]
        public void SetCursor(NodeAddedEvent e, SingleNode<SelfBattleUserComponent> selfBattleUser)
        {
            CursorStateComponent component = new CursorStateComponent();
            selfBattleUser.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void SetCursorWhenCloseChat(NodeRemoveEvent e, SingleNode<BattleChatStateComponent> battleChatState, Spectator spectator)
        {
            Cursor.visible = spectator.cursorState.CursorVisible;
            Cursor.lockState = spectator.cursorState.CursorLockState;
        }

        [OnEventFire]
        public void SetCursorWhenCloseChat(NodeRemoveEvent e, SingleNode<BattleChatStateComponent> battleChatState, UserAsTank userAsTank)
        {
            if (userAsTank.mouseControlStateHolder.MouseControlAllowed)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        [OnEventFire]
        public void ShowCursorOnExitFromBattle(NodeRemoveEvent e, SingleNode<SelfBattleUserComponent> selfBattleUser)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        [OnEventFire]
        public void SwitchCursorState(UpdateEvent evt, Spectator spectator)
        {
            if (InputManager.GetActionKeyDown(BattleActions.CURSOR))
            {
                Cursor.visible = !Cursor.visible;
                Cursor.lockState = !(Cursor.visible || !spectator.mouseControlStateHolder.MouseControlAllowed) ? CursorLockMode.Locked : CursorLockMode.None;
                spectator.cursorState.CursorVisible = Cursor.visible;
                spectator.cursorState.CursorLockState = Cursor.lockState;
            }
        }

        [OnEventFire]
        public void Update(UpdateEvent e, Spectator spectator)
        {
            if (InputManager.GetActionKeyDown(BattleActions.SHOW_SCORE))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            if (InputManager.GetActionKeyUp(BattleActions.SHOW_SCORE))
            {
                Cursor.lockState = spectator.cursorState.CursorLockState;
                Cursor.visible = spectator.cursorState.CursorVisible;
            }
        }

        [OnEventFire]
        public void Update(UpdateEvent e, UserAsTank userAsTank)
        {
            if (userAsTank.mouseControlStateHolder.MouseControlAllowed)
            {
                if (InputManager.GetActionKeyDown(BattleActions.SHOW_SCORE))
                {
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                }
                if (InputManager.GetActionKeyUp(BattleActions.SHOW_SCORE))
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class SelfBattleUserNode : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public MouseControlStateHolderComponent mouseControlStateHolder;
        }

        public class Spectator : CursorStateSystem.SelfBattleUserNode
        {
            public SelfBattleUserComponent selfBattleUser;
            public UserInBattleAsSpectatorComponent userInBattleAsSpectator;
            public CursorStateComponent cursorState;
        }

        public class UserAsTank : CursorStateSystem.SelfBattleUserNode
        {
            public SelfBattleUserComponent selfBattleUser;
            public UserInBattleAsTankComponent userInBattleAsTank;
        }
    }
}

