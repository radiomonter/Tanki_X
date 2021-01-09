namespace Tanks.Battle.ClientCore.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class PauseSystem : ECSSystem
    {
        private void EnterPause(Entity user)
        {
            base.Log.Info("EnterPause");
            base.ScheduleEvent<PauseEvent>(user);
        }

        [OnEventFire]
        public void ForcePauseActiveTankUserUser(NodeAddedEvent e, ActiveTankNode tank, [JoinByUser] UnpausedUnfocusedUserNode user)
        {
            if (this.ShouldPauseOnFocusLoss())
            {
                this.EnterPause(user.Entity);
            }
        }

        [OnEventFire]
        public void ForcePauseSemiActiveTankUser(NodeAddedEvent e, SemiActiveSelfTankNode tank, [JoinByUser] UnpausedUnfocusedUserNode user)
        {
            if (this.ShouldPauseOnFocusLoss())
            {
                this.EnterPause(user.Entity);
            }
        }

        private bool IsAnyKeyPressed() => 
            InputManager.IsAnyKey();

        private bool IsMouseMovement() => 
            ((InputManager.GetAxis(CameraRotationActions.MOUSEX_ROTATE, false) != 0f) || (InputManager.GetAxis(CameraRotationActions.MOUSEY_ROTATE, false) != 0f)) || (InputManager.GetAxis(CameraRotationActions.MOUSEY_MOVE_SHAFT_AIM, false) != 0f);

        private bool IsNearZero(float value) => 
            Math.Abs(value) < 0.001;

        private void LeavePause(Entity user)
        {
            base.Log.Info("LeavePause");
            base.ScheduleEvent<UnpauseEvent>(user);
        }

        [OnEventFire]
        public void MarkUserOnApplicationFocusLost(ApplicationFocusEvent e, FocusedUserNode user)
        {
            if (!e.IsFocused)
            {
                user.Entity.AddComponent<UnfocusedUserComponent>();
            }
        }

        [OnEventFire]
        public void OnApplicationFocusEvent(ApplicationFocusEvent e, PausedUserNode user)
        {
            if (e.IsFocused)
            {
                this.LeavePause(user.Entity);
            }
        }

        [OnEventFire]
        public void OnApplicationFocusLost(ApplicationFocusEvent e, UnpausedUserNode user, [JoinByUser] ActiveTankNode tank)
        {
            if (!e.IsFocused && this.ShouldPauseOnFocusLoss())
            {
                this.EnterPause(user.Entity);
            }
        }

        [OnEventFire]
        public void OnUpdate(UpdateEvent e, PausedUserNode user)
        {
            if (this.IsAnyKeyPressed() || this.IsMouseMovement())
            {
                this.LeavePause(user.Entity);
            }
        }

        [OnEventFire]
        public void OnUpdate(UpdateEvent e, UnpausedUserNode user, [JoinByUser] ActiveTankNode tank, [JoinByTank] WeaponNode weaponNode)
        {
            if (InputManager.GetActionKeyDown(BattleActions.PAUSE) && (this.IsNearZero(weaponNode.weaponRotationControl.Control) && (this.IsNearZero(tank.chassis.MoveAxis) && this.IsNearZero(tank.chassis.TurnAxis))))
            {
                this.EnterPause(user.Entity);
            }
        }

        private bool ShouldPauseOnFocusLoss() => 
            !Application.isEditor;

        [OnEventFire]
        public void UnmarkUserOnApplicationFocusReturns(ApplicationFocusEvent e, SingleNode<UnfocusedUserComponent> user)
        {
            if (e.IsFocused)
            {
                user.Entity.RemoveComponent<UnfocusedUserComponent>();
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        [Not(typeof(SelfDestructionComponent))]
        public class ActiveTankNode : Node
        {
            public TankComponent tank;
            public SelfTankComponent selfTank;
            public UserGroupComponent userGroup;
            public TankActiveStateComponent tankActiveState;
            public TankGroupComponent tankGroup;
            public ChassisComponent chassis;
        }

        [Not(typeof(UnfocusedUserComponent))]
        public class FocusedUserNode : Node
        {
            public BattleUserComponent battleUser;
        }

        public class PausedUserNode : Node
        {
            public BattleUserComponent battleUser;
            public SelfBattleUserComponent selfBattleUser;
            public UserGroupComponent userGroup;
            public PauseComponent pause;
        }

        public class SemiActiveSelfTankNode : Node
        {
            public TankGroupComponent tankGroup;
            public TankSemiActiveStateComponent tankSemiActiveState;
            public SelfTankComponent selfTank;
        }

        [Not(typeof(PauseComponent))]
        public class UnpausedUnfocusedUserNode : Node
        {
            public BattleUserComponent battleUser;
            public UnfocusedUserComponent unfocusedUser;
        }

        [Not(typeof(PauseComponent))]
        public class UnpausedUserNode : Node
        {
            public BattleUserComponent battleUser;
            public SelfBattleUserComponent selfBattleUser;
            public UserGroupComponent userGroup;
        }

        public class WeaponNode : Node
        {
            public TankGroupComponent tankGroup;
            public WeaponRotationControlComponent weaponRotationControl;
        }
    }
}

