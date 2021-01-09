namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientEntrance.API;

    public class IdleKickSystem : ECSSystem
    {
        [OnEventFire]
        public void InputListner(UpdateEvent e, BattleUserNode user)
        {
            int checkPeriodicTimeSec = user.idleKickConfig.CheckPeriodicTimeSec;
            if (InputManager.IsAnyKey() || this.IsMouseMovement())
            {
                Date now = Date.Now;
                if ((now - user.idleKickCheckLastTime.CheckLastTime) > checkPeriodicTimeSec)
                {
                    base.ScheduleEvent<ResetIdleKickTimeEvent>(user);
                    user.idleKickCheckLastTime.CheckLastTime = now;
                }
            }
        }

        private bool IsMouseMovement() => 
            ((InputManager.GetAxis(CameraRotationActions.MOUSEX_ROTATE, false) != 0f) || (InputManager.GetAxis(CameraRotationActions.MOUSEY_ROTATE, false) != 0f)) || (InputManager.GetAxis(CameraRotationActions.MOUSEY_MOVE_SHAFT_AIM, false) != 0f);

        [OnEventFire]
        public void Sync(IdleBeginTimeSyncEvent e, BattleUserNode battleUser)
        {
            battleUser.idleBeginTime.IdleBeginTime = new Date?(e.IdleBeginTime);
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class BattleUserNode : Node
        {
            public SelfComponent self;
            public BattleUserComponent battleUser;
            public IdleCounterComponent idleCounter;
            public IdleBeginTimeComponent idleBeginTime;
            public IdleKickConfigComponent idleKickConfig;
            public IdleKickCheckLastTimeComponent idleKickCheckLastTime;
        }
    }
}

