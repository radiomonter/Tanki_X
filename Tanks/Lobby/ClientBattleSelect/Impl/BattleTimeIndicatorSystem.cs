namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientControls.API;

    public class BattleTimeIndicatorSystem : ECSSystem
    {
        [OnEventFire]
        public void UpdateTime(UpdateEvent e, BattleTimeIndicatorNode battleTimeIndicator, [JoinByBattle] BattleTimeNode battleTime, [JoinByBattle] RoundNode round)
        {
            float progress = 0f;
            long timeLimitSec = battleTime.timeLimit.TimeLimitSec;
            float timeSeconds = timeLimitSec;
            if (battleTime.Entity.HasComponent<BattleStartTimeComponent>() && !round.Entity.HasComponent<RoundWarmingUpStateComponent>())
            {
                Date roundStartTime = battleTime.Entity.GetComponent<BattleStartTimeComponent>().RoundStartTime;
                timeSeconds -= Date.Now - roundStartTime;
                progress = Date.Now.GetProgress(roundStartTime, roundStartTime + timeLimitSec);
            }
            string timerText = TimerUtils.GetTimerText(timeSeconds);
            battleTimeIndicator.battleTimeIndicator.Progress = 1f - progress;
            battleTimeIndicator.battleTimeIndicator.Time = timerText;
        }

        public class BattleTimeIndicatorNode : Node
        {
            public BattleGroupComponent battleGroup;
            public BattleTimeIndicatorComponent battleTimeIndicator;
        }

        public class BattleTimeNode : Node
        {
            public TimeLimitComponent timeLimit;
            public BattleGroupComponent battleGroup;
        }

        public class RoundNode : Node
        {
            public BattleGroupComponent battleGroup;
            public RoundComponent round;
        }
    }
}

