namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientBattleSelect.Impl;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;
    using UnityEngine.UI;

    public class BattleResultsScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void InitDMScreen(NodeAddedEvent e, DMScoreTableNode scoreTable, BattleResultsNode battleResults, SingleNode<DMBattleResultsScreenComponent> screen, [JoinAll] SelfUserNode selfUser, [JoinAll] SingleNode<FriendsComponent> friends)
        {
            ScrollRect component = scoreTable.dmScoreTable.gameObject.GetComponent<ScrollRect>();
            Vector2 vector = new Vector2();
            component.content.anchoredPosition = vector;
            PlayerStatInfoUI rowPrefab = scoreTable.battleResultsScoreTable.rowPrefab;
            int selfScore = 0;
            int maxScore = 0;
            foreach (UserResult result in battleResults.battleResults.ResultForClient.DmUsers)
            {
                bool isSelf = result.UserId == selfUser.userGroup.Key;
                bool containerLeft = false;
                bool isFriend = friends.component.AcceptedFriendsIds.Contains(result.UserId);
                int leagueIndex = result.League.GetComponent<LeagueConfigComponent>().LeagueIndex;
                bool isDm = true;
                if (isSelf)
                {
                    selfScore = result.ScoreWithoutPremium;
                }
                PlayerStatInfoUI oui2 = Object.Instantiate<PlayerStatInfoUI>(rowPrefab);
                oui2.Init(leagueIndex, result.Uid, result.Kills, result.Deaths, result.KillAssists, result.ScoreWithoutPremium, !isSelf ? Color.white : scoreTable.scoreTableRowColor.selfRowColor, result.HullId, result.WeaponId, result.UserId, battleResults.battleResults.ResultForClient.BattleId, result.AvatarId, isSelf, isDm, isFriend, containerLeft, false);
                oui2.transform.SetParent(component.content, false);
                if (result.ScoreWithoutPremium > maxScore)
                {
                    maxScore = result.ScoreWithoutPremium;
                }
            }
            screen.component.Init(selfScore, maxScore, Flow.Current.EntityRegistry.GetEntity(battleResults.battleResults.ResultForClient.MapId).GetComponent<DescriptionItemComponent>().Name);
        }

        [OnEventFire]
        public void InitTeamScoreTables(NodeAddedEvent e, [Combine] TeamScoreTableNode scoreTable, BattleResultsNode battleResults, SingleNode<TeamBattleResultsScreenComponent> screen, [JoinAll] SelfUserNode selfUser, [JoinAll] SingleNode<FriendsComponent> friends)
        {
            BattleResultForClient resultForClient = battleResults.battleResults.ResultForClient;
            ScrollRect component = scoreTable.uiTeam.gameObject.GetComponent<ScrollRect>();
            Vector2 vector = new Vector2();
            component.content.anchoredPosition = vector;
            PlayerStatInfoUI rowPrefab = scoreTable.battleResultsScoreTable.rowPrefab;
            ICollection<UserResult> is2 = null;
            TeamColor teamColor = scoreTable.uiTeam.TeamColor;
            if (teamColor == TeamColor.BLUE)
            {
                is2 = (resultForClient.Spectator || (resultForClient.PersonalResult.UserTeamColor == TeamColor.BLUE)) ? resultForClient.BlueUsers : resultForClient.RedUsers;
            }
            else if (teamColor == TeamColor.RED)
            {
                is2 = (resultForClient.Spectator || (resultForClient.PersonalResult.UserTeamColor == TeamColor.BLUE)) ? resultForClient.RedUsers : resultForClient.BlueUsers;
            }
            foreach (UserResult result in is2)
            {
                PlayerStatInfoUI oui2 = Object.Instantiate<PlayerStatInfoUI>(rowPrefab);
                bool isSelf = result.UserId == selfUser.userGroup.Key;
                bool isFriend = friends.component.AcceptedFriendsIds.Contains(result.UserId);
                bool containerLeft = false;
                int leagueIndex = result.League.GetComponent<LeagueConfigComponent>().LeagueIndex;
                bool isDm = false;
                oui2.Init(leagueIndex, result.Uid, result.Kills, result.Deaths, result.KillAssists, result.ScoreWithoutPremium, !isSelf ? Color.white : scoreTable.scoreTableRowColor.selfRowColor, result.HullId, result.WeaponId, result.UserId, battleResults.battleResults.ResultForClient.BattleId, result.AvatarId, isSelf, isDm, isFriend, containerLeft, false);
                oui2.transform.SetParent(component.content, false);
            }
        }

        [OnEventFire]
        public void InitTeamScreen(NodeAddedEvent e, BattleResultsNode battleResults, SingleNode<TeamBattleResultsScreenComponent> screen)
        {
            BattleResultForClient resultForClient = battleResults.battleResults.ResultForClient;
            if (resultForClient.BattleMode != BattleMode.DM)
            {
                string name = Flow.Current.EntityRegistry.GetEntity(resultForClient.MapId).GetComponent<DescriptionItemComponent>().Name;
                if (resultForClient.Spectator)
                {
                    screen.component.Init(resultForClient.BattleMode.ToString(), resultForClient.BlueTeamScore, resultForClient.RedTeamScore, name, true);
                }
                else if (resultForClient.PersonalResult.UserTeamColor == TeamColor.BLUE)
                {
                    screen.component.Init(resultForClient.BattleMode.ToString(), resultForClient.BlueTeamScore, resultForClient.RedTeamScore, name, false);
                }
                else
                {
                    screen.component.Init(resultForClient.BattleMode.ToString(), resultForClient.RedTeamScore, resultForClient.BlueTeamScore, name, false);
                }
            }
        }

        public class BattleResultsNode : Node
        {
            public BattleResultsComponent battleResults;
        }

        public class DMScoreTableNode : Node
        {
            public DMScoreTableComponent dmScoreTable;
            public BattleResultsScoreTableComponent battleResultsScoreTable;
            public ScoreTableRowColorComponent scoreTableRowColor;
        }

        public class SelfUserNode : Node
        {
            public UserGroupComponent userGroup;
            public SelfUserComponent selfUser;
            public UserExperienceComponent userExperience;
        }

        public class TeamScoreTableNode : Node
        {
            public UITeamComponent uiTeam;
            public BattleResultsScoreTableComponent battleResultsScoreTable;
            public ScoreTableRowColorComponent scoreTableRowColor;
        }
    }
}

