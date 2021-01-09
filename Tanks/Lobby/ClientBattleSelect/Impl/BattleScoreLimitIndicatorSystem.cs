namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientBattleSelect.API;

    public class BattleScoreLimitIndicatorSystem : ECSSystem
    {
        [OnEventFire]
        public void SetScoreLimit(NodeAddedEvent e, BattleScoreLimitIndicatorNode battleScoreLimitIndicator, [Context, JoinByBattle] BattleLimitNode battleLimit)
        {
            battleScoreLimitIndicator.battleScoreLimitIndicator.ScoreLimit = battleLimit.scoreLimit.ScoreLimit;
        }

        public class BattleLimitNode : Node
        {
            public BattleGroupComponent battleGroup;
            public ScoreLimitComponent scoreLimit;
        }

        public class BattleScoreLimitIndicatorNode : Node
        {
            public BattleScoreLimitIndicatorComponent battleScoreLimitIndicator;
            public BattleGroupComponent battleGroup;
        }
    }
}

