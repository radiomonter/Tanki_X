namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientEntrance.API;
    using UnityEngine.UI;

    public class CommonScoreTableSystem : ECSSystem
    {
        [OnEventFire]
        public void ClearScoreTable(NodeRemoveEvent e, ScoreTableNode scoreTable)
        {
            scoreTable.scoreTable.Clear();
        }

        [OnEventFire]
        public void ClearScoreTableHeaders(NodeRemoveEvent e, ScoreTableWithHeaderNode headers)
        {
            headers.scoreTableHeader.Clear();
        }

        [OnEventFire]
        public void ColorizeRemoteUserRow(NodeAddedEvent e, UserRowNode userRow, [JoinByScoreTable] ScoreTableColorNode scoreTableColor, [Context] UserRowNode userRow1, [JoinByUser] RemoteUserNode remoteUser)
        {
            userRow.scoreTableRow.Color = !remoteUser.Entity.HasComponent<AcceptedFriendComponent>() ? scoreTableColor.scoreTableRowColor.rowColor : scoreTableColor.scoreTableRowColor.friendRowColor;
        }

        [OnEventFire]
        public void ColorizeSelfUserRow(NodeAddedEvent e, UserRowNode userRow, [JoinByScoreTable] ScoreTableColorNode scoreTableColor, UserRowNode userRow1, [JoinByUser] SelfUserNode selfUser)
        {
            userRow.scoreTableRow.Color = scoreTableColor.scoreTableRowColor.selfRowColor;
        }

        [OnEventFire]
        public void InitPosition(NodeAddedEvent e, RoundUserNode roundUser, [Context, JoinByUser] UserRowNode userRow)
        {
            base.Log.DebugFormat("InitPosition roundUser={0} position={1}", roundUser.Entity.Id, roundUser.roundUserStatistics.Place);
            userRow.scoreTableRow.HidePosition();
            userRow.scoreTableRow.Position = roundUser.roundUserStatistics.Place;
            userRow.scoreTableRow.SetLayoutDirty();
        }

        [OnEventFire]
        public void InitScoreTableHeaders(NodeAddedEvent e, ScoreTableWithHeaderNode scoreTableHeader)
        {
            scoreTableHeader.Entity.AddComponent(new ScoreTableGroupComponent(scoreTableHeader.Entity));
            foreach (ScoreTableRowIndicator indicator in scoreTableHeader.scoreTableHeader.headers)
            {
                scoreTableHeader.scoreTableHeader.AddHeader(indicator);
            }
            LayoutRebuilder.MarkLayoutForRebuild(scoreTableHeader.scoreTableHeader.GetComponent<RectTransform>());
            scoreTableHeader.scoreTable.SetHeaderDirty();
        }

        [OnEventFire]
        public void RemoveRow(NodeRemoveEvent e, UserNode user, [JoinByUser, Combine] UserRowNode row, [JoinByScoreTable, Mandatory] ScoreTableNode scoreTable)
        {
            scoreTable.scoreTable.RemoveRow(row.scoreTableRow);
        }

        [OnEventFire]
        public void SetPosition(SetScoreTablePositionEvent e, RoundUserNode roundUser, [JoinByUser] UserRowNode userRow)
        {
            base.Log.DebugFormat("SetPosition roundUser={0} position={1}", roundUser.Entity.Id, e.Position);
            userRow.scoreTableRow.Position = e.Position;
        }

        [Not(typeof(SelfUserComponent))]
        public class RemoteUserNode : Node
        {
            public UserComponent user;
            public UserGroupComponent userGroup;
        }

        public class RoundUserNode : Node, IComparable<CommonScoreTableSystem.RoundUserNode>
        {
            public RoundUserStatisticsComponent roundUserStatistics;
            public UserGroupComponent userGroup;
            public BattleGroupComponent battleGroup;

            public int CompareTo(CommonScoreTableSystem.RoundUserNode other) => 
                this.roundUserStatistics.CompareTo(other.roundUserStatistics);
        }

        public class RowNode : Node
        {
            public ScoreTableRowComponent scoreTableRow;
            public ScoreTableGroupComponent scoreTableGroup;
        }

        public class ScoreTableColorNode : Node
        {
            public ScoreTableGroupComponent scoreTableGroup;
            public ScoreTableRowColorComponent scoreTableRowColor;
        }

        public class ScoreTableNode : Node
        {
            public ScoreTableComponent scoreTable;
            public ScoreTableGroupComponent scoreTableGroup;
            public ScoreTableUserRowIndicatorsComponent scoreTableUserRowIndicators;
        }

        public class ScoreTableWithHeaderNode : Node
        {
            public ScoreTableComponent scoreTable;
            public ScoreTableHeaderComponent scoreTableHeader;
        }

        public class SelfUserNode : Node
        {
            public UserComponent user;
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
        }

        public class UserNode : Node
        {
            public RoundUserComponent roundUser;
            public UserGroupComponent userGroup;
            public BattleGroupComponent battleGroup;
        }

        public class UserRowNode : Node
        {
            public ScoreTableRowComponent scoreTableRow;
            public ScoreTableGroupComponent scoreTableGroup;
            public UserGroupComponent userGroup;
        }
    }
}

