namespace Tanks.Battle.ClientHUD.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientBattleSelect.Impl;

    public class TeamBattleScoreTableSystem : ECSSystem
    {
        private void AddRow(InitedScoreTable scoreTable, RoundUserNode roundUser)
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
        public void AddUserToBattleTable(NodeAddedEvent e, [Combine] InitedScoreTable scoreTable, [Context, JoinByBattle, Combine] RoundUserNode roundUser, [JoinByTeam] TeamUIColorNode teamNode, [JoinAll] SingleNode<BattleScreenComponent> screen)
        {
            if (scoreTable.uiTeam.TeamColor == teamNode.colorInBattle.TeamColor)
            {
                this.AddRow(scoreTable, roundUser);
            }
        }

        [OnEventFire]
        public void InitRowsCache(NodeAddedEvent e, [Combine] TeamScoreTableNode scoreTable, [Context, JoinByBattle] BattleNode battle, [JoinAll] SingleNode<BattleScreenComponent> screen)
        {
            scoreTable.scoreTable.InitRowsCache(battle.userLimit.TeamLimit, scoreTable.scoreTableUserRowIndicators.indicators);
            scoreTable.Entity.AddComponent<ScoreTableRowsCacheInitedComponent>();
        }

        public class BattleNode : Node
        {
            public BattleGroupComponent battleGroup;
            public UserLimitComponent userLimit;
            public BattleComponent battle;
        }

        public class InitedScoreTable : TeamBattleScoreTableSystem.TeamScoreTableNode
        {
            public ScoreTableRowsCacheInitedComponent scoreTableRowsCacheInited;
        }

        public class RoundUserNode : Node
        {
            public RoundUserComponent roundUser;
            public UserGroupComponent userGroup;
            public TeamGroupComponent teamGroup;
            public BattleGroupComponent battleGroup;
        }

        public class TeamScoreTableNode : Node
        {
            public ScoreTableComponent scoreTable;
            public BattleGroupComponent battleGroup;
            public ScoreTableGroupComponent scoreTableGroup;
            public UITeamComponent uiTeam;
            public ScoreTableUserRowIndicatorsComponent scoreTableUserRowIndicators;
        }

        public class TeamUIColorNode : Node
        {
            public TeamGroupComponent teamGroup;
            public ColorInBattleComponent colorInBattle;
        }
    }
}

