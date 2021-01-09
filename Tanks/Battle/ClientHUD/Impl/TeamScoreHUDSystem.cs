namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientBattleSelect.API;

    public class TeamScoreHUDSystem : ECSSystem
    {
        [OnEventFire]
        public void DisableFlags(NodeRemoveEvent e, CTFBattleNode battle, SingleNode<FlagsHUDComponent> hud)
        {
            hud.component.gameObject.SetActive(false);
        }

        [OnEventFire]
        public void InitFlags(NodeAddedEvent e, CTFBattleNode battle, [Context, JoinByBattle] RoundNode round, [Context, JoinByBattle] HUDNodes.SelfBattleUserNode self, [Context] SingleNode<FlagsHUDComponent> hud)
        {
            hud.component.BlueFlagNormalizedPosition = 0f;
            hud.component.RedFlagNormalizedPosition = 0f;
            hud.component.gameObject.SetActive(true);
        }

        [OnEventFire]
        public void InitIndicator(NodeAddedEvent e, ScoreBattleNode battle, [Context, JoinByBattle] RoundNode round, [Context, JoinByBattle] HUDNodes.SelfBattleUserNode self, [JoinByBattle] ICollection<TeamNode> teams, [Context] SingleNode<TeamScoreHUDComponent> hud)
        {
            hud.component.gameObject.SetActive(true);
            this.SetScore(teams, hud.component);
        }

        [OnEventComplete]
        public void ScoreUpdate(RoundScoreUpdatedEvent e, RoundNode node, [JoinByBattle] ScoreBattleNode battle, [JoinByBattle] HUDNodes.SelfBattleUserNode self, [JoinByBattle] ICollection<TeamNode> teams, [JoinAll] SingleNode<TeamScoreHUDComponent> hud)
        {
            this.SetScore(teams, hud.component);
        }

        [OnEventFire]
        public void SetCTFMessage(NodeAddedEvent e, HUDNode hud, HUDNodes.SelfTankNode tank, SingleNode<CTFHUDMessagesComponent> messageNode, [JoinByBattle] CTFBattleNode battle)
        {
            hud.mainHUD.ShowMessageWithPriority(messageNode.component.CaptureFlagMessage, 0);
            hud.mainHUD.SetMessageCTFPosition();
        }

        private void SetScore(ICollection<TeamNode> teams, TeamScoreHUDComponent hud)
        {
            int score = 0;
            int score = 0;
            foreach (TeamNode node in teams)
            {
                TeamColor teamColor = node.colorInBattle.TeamColor;
                if (teamColor == TeamColor.RED)
                {
                    score = node.teamScore.Score;
                    continue;
                }
                if (teamColor == TeamColor.BLUE)
                {
                    score = node.teamScore.Score;
                }
            }
            hud.BlueScore = score;
            hud.RedScore = score;
        }

        [OnEventFire]
        public void SetScoresCTFPosition(NodeAddedEvent e, SingleNode<TeamScoreHUDComponent> hud, HUDNodes.SelfTankNode tank, [JoinByBattle] CTFBattleNode battle)
        {
            hud.component.SetCtfMode();
        }

        [OnEventFire]
        public void SetScoresTDMPosition(NodeAddedEvent e, SingleNode<TeamScoreHUDComponent> hud, HUDNodes.SelfTankNode tank, [JoinByBattle] TDMBattleNode battle)
        {
            hud.component.SetTdmMode();
        }

        [OnEventFire]
        public void SetTDMMessage(NodeAddedEvent e, HUDNode hud, HUDNodes.SelfTankNode tank, SingleNode<TDMHUDMessagesComponent> messageNode, [JoinByBattle] TDMBattleNode battle)
        {
            hud.mainHUD.ShowMessageWithPriority(messageNode.component.MainMessage, 0);
            hud.mainHUD.SetMessageTDMPosition();
        }

        public class CTFBattleNode : TeamScoreHUDSystem.ScoreBattleNode
        {
            public CTFComponent ctf;
        }

        public class HUDNode : Node
        {
            public MainHUDComponent mainHUD;
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

        public class TDMBattleNode : TeamScoreHUDSystem.ScoreBattleNode
        {
            public TDMComponent tdm;
        }

        public class TeamNode : Node
        {
            public TeamComponent team;
            public TeamGroupComponent teamGroup;
            public ColorInBattleComponent colorInBattle;
            public TeamScoreComponent teamScore;
        }
    }
}

