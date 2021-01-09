﻿namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientBattleSelect.API;
    using UnityEngine;

    public class TeamBattleScoreIndicatorSystem : ECSSystem
    {
        [OnEventFire]
        public void HideTeamScoreIndicator(NodeRemoveEvent e, SingleNode<BattleComponent> battle, [Context, JoinByBattle] ScoreIndicatorContainserNode indicatorContainer)
        {
            indicatorContainer.teamBattleScoreIndicatorContainer.TdmScoreIndicator.SetActive(false);
            indicatorContainer.teamBattleScoreIndicatorContainer.CtfScoreIndicator.SetActive(false);
        }

        [OnEventFire]
        public void InitIndicator(NodeAddedEvent e, BattleWithLimitNode battleWithLimit, [Context, JoinByBattle] RoundNode round, [Context, JoinByBattle] IndicatorNode indicator)
        {
            indicator.teamBattleScoreIndicator.UpdateScore(battleWithLimit.battleScore.ScoreBlue, battleWithLimit.battleScore.ScoreRed, battleWithLimit.scoreLimit.ScoreLimit);
        }

        [OnEventFire]
        public void InitIndicator(NodeAddedEvent e, BattleWithoutLimitNode battleWithoutLimit, [Context, JoinByBattle] RoundNode round, [Context, JoinByBattle] IndicatorNode indicator)
        {
            int scoreRed = battleWithoutLimit.battleScore.ScoreRed;
            int scoreBlue = battleWithoutLimit.battleScore.ScoreBlue;
            indicator.teamBattleScoreIndicator.UpdateScore(scoreBlue, scoreRed, Mathf.Max(scoreRed, scoreBlue));
        }

        [OnEventComplete]
        public void ScoreUpdate(RoundScoreUpdatedEvent e, RoundNode node, [JoinByBattle] BattleWithLimitNode battleWithLimit, [JoinByBattle] IndicatorNode indicator, [JoinByBattle] ICollection<TeamNode> teams)
        {
            int redScore = 0;
            int blueScore = 0;
            foreach (TeamNode node2 in teams)
            {
                TeamColor teamColor = node2.teamColor.TeamColor;
                if (teamColor == TeamColor.RED)
                {
                    redScore = node2.teamScore.Score;
                    continue;
                }
                if (teamColor == TeamColor.BLUE)
                {
                    blueScore = node2.teamScore.Score;
                }
            }
            indicator.teamBattleScoreIndicator.UpdateScore(blueScore, redScore, battleWithLimit.scoreLimit.ScoreLimit);
        }

        [OnEventComplete]
        public void ScoreUpdate(RoundScoreUpdatedEvent e, RoundNode node, [JoinByBattle] BattleWithoutLimitNode battleWithoutLimit, [JoinByBattle] IndicatorNode indicator, [JoinByBattle] ICollection<TeamNode> teams)
        {
            int redScore = 0;
            int blueScore = 0;
            foreach (TeamNode node2 in teams)
            {
                TeamColor teamColor = node2.teamColor.TeamColor;
                if (teamColor == TeamColor.RED)
                {
                    redScore = node2.teamScore.Score;
                    continue;
                }
                if (teamColor == TeamColor.BLUE)
                {
                    blueScore = node2.teamScore.Score;
                }
            }
            indicator.teamBattleScoreIndicator.UpdateScore(blueScore, redScore, Mathf.Max(blueScore, redScore));
        }

        [OnEventFire]
        public void ShowCTFScoreIndicator(NodeAddedEvent e, SingleNode<CTFComponent> ctfBattle, [Context, JoinByBattle] ScoreIndicatorContainserNode indicatorContainer)
        {
            indicatorContainer.teamBattleScoreIndicatorContainer.CtfScoreIndicator.SetActive(true);
        }

        [OnEventFire]
        public void ShowTDMScoreIndicator(NodeAddedEvent e, SingleNode<TDMComponent> tdmBattle, [Context, JoinByBattle] ScoreIndicatorContainserNode indicatorContainer)
        {
            indicatorContainer.teamBattleScoreIndicatorContainer.TdmScoreIndicator.SetActive(true);
        }

        public class BattleWithLimitNode : TeamBattleScoreIndicatorSystem.ScoreBattleNode
        {
            public ScoreLimitComponent scoreLimit;
        }

        [Not(typeof(ScoreLimitComponent))]
        public class BattleWithoutLimitNode : TeamBattleScoreIndicatorSystem.ScoreBattleNode
        {
        }

        public class IndicatorNode : Node
        {
            public TeamBattleScoreIndicatorComponent teamBattleScoreIndicator;
            public BattleGroupComponent battleGroup;
        }

        public class RoundNode : Node
        {
            public BattleGroupComponent battleGroup;
            public RoundComponent round;
        }

        public class ScoreBattleNode : Node
        {
            public BattleGroupComponent battleGroup;
            public TeamBattleComponent teamBattle;
            public BattleScoreComponent battleScore;
        }

        public class ScoreIndicatorContainserNode : Node
        {
            public TeamBattleScoreIndicatorContainerComponent teamBattleScoreIndicatorContainer;
            public BattleGroupComponent battleGroup;
        }

        public class TeamNode : Node
        {
            public TeamComponent team;
            public TeamGroupComponent teamGroup;
            public TeamColorComponent teamColor;
            public TeamScoreComponent teamScore;
        }
    }
}

