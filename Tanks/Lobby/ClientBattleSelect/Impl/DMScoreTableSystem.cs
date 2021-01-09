namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientBattleSelect.API;

    public class DMScoreTableSystem : ECSSystem
    {
        [OnEventFire]
        public void AddUser(NodeAddedEvent e, [Combine] InitedScoreTable scoreTable, [Context, JoinByBattle, Combine] RoundUserNode roundUser)
        {
            ScoreTableRowComponent component = scoreTable.scoreTable.AddRow();
            Entity entity = component.GetComponent<EntityBehaviour>().Entity;
            entity.AddComponent(new ScoreTableGroupComponent(scoreTable.scoreTableGroup.Key));
            entity.AddComponent(new UserGroupComponent(roundUser.userGroup.Key));
            foreach (ScoreTableRowIndicator indicator in component.indicators.Values)
            {
                EntityBehaviour behaviour = indicator.GetComponent<EntityBehaviour>();
                if (behaviour != null)
                {
                    behaviour.BuildEntity(entity);
                }
            }
        }

        [OnEventFire]
        public void InitRowsCache(NodeAddedEvent e, [Combine] DMScoreTableNode scoreTable, [Context, JoinByBattle] DMBattleNode battle)
        {
            scoreTable.scoreTable.InitRowsCache(battle.userLimit.UserLimit, scoreTable.scoreTableUserRowIndicators.indicators);
            scoreTable.Entity.AddComponent<ScoreTableRowsCacheInitedComponent>();
        }

        public class DMBattleNode : Node
        {
            public BattleGroupComponent battleGroup;
            public UserLimitComponent userLimit;
            public DMComponent dm;
        }

        public class DMScoreTableNode : Node
        {
            public ScoreTableComponent scoreTable;
            public BattleGroupComponent battleGroup;
            public ScoreTableGroupComponent scoreTableGroup;
            public DMScoreTableComponent dmScoreTable;
            public ScoreTableUserRowIndicatorsComponent scoreTableUserRowIndicators;
        }

        public class InitedScoreTable : DMScoreTableSystem.DMScoreTableNode
        {
            public ScoreTableRowsCacheInitedComponent scoreTableRowsCacheInited;
        }

        public class RoundUserNode : Node
        {
            public RoundUserStatisticsComponent roundUserStatistics;
            public UserGroupComponent userGroup;
            public BattleGroupComponent battleGroup;
        }
    }
}

