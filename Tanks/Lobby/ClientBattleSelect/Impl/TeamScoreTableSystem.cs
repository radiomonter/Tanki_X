namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientBattleSelect.API;

    public class TeamScoreTableSystem : ECSSystem
    {
        private void AddRow(TeamScoreTableNode scoreTable, RoundUserNode roundUser)
        {
            Entity entity = scoreTable.scoreTable.AddRow().GetComponent<EntityBehaviour>().Entity;
            entity.AddComponent(new ScoreTableGroupComponent(scoreTable.scoreTableGroup.Key));
            entity.AddComponent(new UserGroupComponent(roundUser.userGroup.Key));
        }

        [OnEventFire]
        public void AddUserToBattleSelectTable(NodeAddedEvent e, [Combine] TeamScoreTableNode scoreTable, [Context, JoinByBattle, Combine] RoundUserNode roundUser, [JoinByTeam] TeamNode teamNode, [JoinAll] SingleNode<BattleSelectScreenComponent> screen)
        {
            if (scoreTable.uiTeam.TeamColor == teamNode.teamColor.TeamColor)
            {
                this.AddRow(scoreTable, roundUser);
            }
        }

        public class RoundUserNode : Node
        {
            public RoundUserComponent roundUser;
            public UserGroupComponent userGroup;
            public TeamGroupComponent teamGroup;
            public BattleGroupComponent battleGroup;
        }

        public class TeamNode : Node
        {
            public TeamGroupComponent teamGroup;
            public TeamColorComponent teamColor;
        }

        public class TeamScoreTableNode : Node
        {
            public ScoreTableComponent scoreTable;
            public BattleGroupComponent battleGroup;
            public ScoreTableGroupComponent scoreTableGroup;
            public UITeamComponent uiTeam;
            public ScoreTableUserRowIndicatorsComponent scoreTableUserRowIndicators;
        }
    }
}

