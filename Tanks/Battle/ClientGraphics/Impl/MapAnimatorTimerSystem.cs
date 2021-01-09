namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;

    public class MapAnimatorTimerSystem : ECSSystem
    {
        [OnEventFire]
        public void CheckRoundTimer(UpdateEvent e, MapAnimatorTimerNode mapAnimatorTimer, [JoinAll] SelfBattleUserNode battleUser, [JoinByBattle] ActiveRoundStopTimeNode round, [JoinAll] SingleNode<AnimatorTimerComponent> animatorTimer)
        {
            if ((round.roundStopTime.StopTime.UnityTime - Date.Now.UnityTime) <= animatorTimer.component.timer)
            {
                animatorTimer.component.animator.SetTrigger(animatorTimer.component.triggerName);
            }
        }

        public class ActiveRoundStopTimeNode : MapAnimatorTimerSystem.RoundNode
        {
            public RoundActiveStateComponent roundActiveState;
            public RoundStopTimeComponent roundStopTime;
        }

        public class MapAnimatorTimerNode : Node
        {
            public MapAnimatorTimerComponent mapAnimatorTimer;
        }

        public class RoundNode : Node
        {
            public RoundComponent round;
            public BattleGroupComponent battleGroup;
        }

        public class SelfBattleUserNode : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public BattleGroupComponent battleGroup;
        }
    }
}

