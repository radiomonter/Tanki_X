namespace Tanks.Battle.ClientGraphics.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class BattleSoundsSystem : ECSSystem
    {
        [CompilerGenerated]
        private static Comparison<RoundUserNode> <>f__am$cache0;

        [OnEventFire]
        public void CheckRemainingTimeInRound(NodeAddedEvent evt, SoundsListenerWithOldRoundRestartSoundsNode listener, SelfBattleUserNode battleUser, [JoinByBattle, Context] ActiveRoundStopTimeNode round, SingleNode<BattleSoundsAssetComponent> mapEffect)
        {
            if ((round.roundStopTime.StopTime.UnityTime - Date.Now.UnityTime) >= mapEffect.component.BattleSoundsBehaviour.MinRemainigRoundTimeSec)
            {
                listener.Entity.RemoveComponent<OldRoundRestartSoundsListenerComponent>();
                listener.Entity.AddComponent<MelodiesRoundRestartListenerComponent>();
            }
        }

        [OnEventFire]
        public void CheckRoundTimer(UpdateEvent e, SoundsListenerWithRoundRestartMelodiesNode listener, [JoinAll] SelfBattleUserNode battleUser, [JoinByBattle] ActiveRoundStopTimeNode round, [JoinAll] SingleNode<BattleSoundsAssetComponent> mapEffect)
        {
            if ((round.roundStopTime.StopTime.UnityTime - Date.Now.UnityTime) <= mapEffect.component.BattleSoundsBehaviour.MinRemainigRoundTimeSec)
            {
                Node[] nodes = new Node[] { battleUser, mapEffect, listener, round };
                base.NewEvent<DefineMelodyForRoundRestartEvent>().AttachAll(nodes).Schedule();
                listener.Entity.RemoveComponent<MelodiesRoundRestartListenerComponent>();
            }
        }

        [OnEventFire]
        public void CleanFirstRoundSpawnWhenExitBattle(NodeRemoveEvent evt, SingleNode<BattleSoundsAssetComponent> mapEffect, [JoinAll] SingleNode<RoundFirstSpawnPlayedComponent> listener)
        {
            listener.Entity.RemoveComponent<RoundFirstSpawnPlayedComponent>();
        }

        [OnEventFire]
        public void CleanFirstRoundSpawnWhenRoundFinished(NodeRemoveEvent evt, ActiveRoundNode round, [JoinByBattle] SelfBattleUserNode battleUser, [JoinAll] SingleNode<RoundFirstSpawnPlayedComponent> listener)
        {
            listener.Entity.RemoveComponent<RoundFirstSpawnPlayedComponent>();
        }

        [OnEventFire]
        public void CleanMelodiesMarkerWhenExitBattle(NodeRemoveEvent evt, SingleNode<BattleSoundsAssetComponent> mapEffect, [JoinAll] SingleNode<MelodiesRoundRestartListenerComponent> listener)
        {
            listener.Entity.RemoveComponent<MelodiesRoundRestartListenerComponent>();
        }

        [OnEventFire]
        public void CleanMelodiesMarkerWhenRoundRestart(NodeRemoveEvent evt, ActiveRoundNode activeRound, [JoinByBattle] SelfBattleUserNode battleUser, [JoinAll] SingleNode<MelodiesRoundRestartListenerComponent> listener)
        {
            listener.Entity.RemoveComponent<MelodiesRoundRestartListenerComponent>();
        }

        [OnEventFire]
        public void CleanOldSoundsMarkerWhenExitBattle(NodeRemoveEvent evt, SingleNode<BattleSoundsAssetComponent> mapEffect, [JoinAll] SingleNode<OldRoundRestartSoundsListenerComponent> listener)
        {
            listener.Entity.RemoveComponent<OldRoundRestartSoundsListenerComponent>();
        }

        [OnEventFire]
        public void CleanOldSoundsMarkerWhenRoundRestart(NodeRemoveEvent evt, ActiveRoundNode activeRound, [JoinByBattle] SelfBattleUserNode battleUser, [JoinAll] SingleNode<OldRoundRestartSoundsListenerComponent> listener)
        {
            listener.Entity.RemoveComponent<OldRoundRestartSoundsListenerComponent>();
        }

        [OnEventFire]
        public void CleanPlayingMelody(DefineMelodyForRoundRestartEvent evt, SingleNode<PlayingMelodyRoundRestartListenerComponent> listener)
        {
            listener.Entity.RemoveComponent<PlayingMelodyRoundRestartListenerComponent>();
        }

        [OnEventFire]
        public void FinalizeAmbientMapSoundEffect(LobbyAmbientSoundPlayEvent evt, SingleNode<PlayingMelodyRoundRestartListenerComponent> listener)
        {
            base.ScheduleEvent<StopBattleMelodyEvent>(listener);
        }

        [OnEventComplete]
        public void PlayMelodyWhenSelfBattleUserInCTF(DefineMelodyForRoundRestartEvent evt, SoundsListenerNode listener, SelfTankBattleUserInTeamNode battleUser, [JoinByTeam] TeamNode userTeam, [JoinByBattle] CTFBattleNode dm, [JoinByBattle] ICollection<TeamNode> teams, SingleNode<BattleSoundsAssetComponent> mapEffect)
        {
            this.PlayNonTeamMelodyInTeamMode(userTeam, teams, listener, mapEffect, mapEffect.component.BattleSoundsBehaviour.MinCtfScoreDiff);
        }

        [OnEventComplete]
        public void PlayMelodyWhenSelfBattleUserInDM(DefineMelodyForRoundRestartEvent evt, SoundsListenerNode listener, SelfTankBattleUserNode battleUser, [JoinByUser] RoundUserNode selfRoundUser, [JoinByBattle] DMBattleNode dm, [JoinByBattle] ICollection<RoundUserNode> players, SingleNode<BattleSoundsAssetComponent> mapEffect)
        {
            List<RoundUserNode> list = players.ToList<RoundUserNode>();
            if (list.Count == 1)
            {
                listener.Entity.AddComponent(new PlayingMelodyRoundRestartListenerComponent(mapEffect.component.BattleSoundsBehaviour.PlayNeutralMelody(listener.soundListener.transform, -1f)));
            }
            else
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = (r1, r2) => r1.roundUserStatistics.CompareTo(r2.roundUserStatistics);
                }
                list.Sort(<>f__am$cache0);
                RoundUserNode node = list[0];
                if (!node.Entity.Equals(selfRoundUser.Entity))
                {
                    listener.Entity.AddComponent(new PlayingMelodyRoundRestartListenerComponent(mapEffect.component.BattleSoundsBehaviour.PlayNeutralMelody(listener.soundListener.transform, -1f)));
                }
                else if (Mathf.Abs((int) (node.roundUserStatistics.ScoreWithoutBonuses - list[1].roundUserStatistics.ScoreWithoutBonuses)) < mapEffect.component.BattleSoundsBehaviour.MinDmScoreDiff)
                {
                    listener.Entity.AddComponent(new PlayingMelodyRoundRestartListenerComponent(mapEffect.component.BattleSoundsBehaviour.PlayNeutralMelody(listener.soundListener.transform, -1f)));
                }
                else
                {
                    listener.Entity.AddComponent(new PlayingMelodyRoundRestartListenerComponent(mapEffect.component.BattleSoundsBehaviour.PlayNonNeutralMelody(listener.soundListener.transform, true, -1f)));
                }
            }
        }

        [OnEventComplete]
        public void PlayMelodyWhenSelfBattleUserInTDM(DefineMelodyForRoundRestartEvent evt, SoundsListenerNode listener, SelfTankBattleUserInTeamNode battleUser, [JoinByTeam] TeamNode userTeam, [JoinByBattle] TDMBattleNode dm, [JoinByBattle] ICollection<TeamNode> teams, SingleNode<BattleSoundsAssetComponent> mapEffect)
        {
            this.PlayNonTeamMelodyInTeamMode(userTeam, teams, listener, mapEffect, mapEffect.component.BattleSoundsBehaviour.MinTdmScoreDiff);
        }

        [OnEventComplete]
        public void PlayNeutralMelodyWhenSpectator(DefineMelodyForRoundRestartEvent evt, SoundsListenerNode listener, SelfSpectatorBattleUser battleUser, SingleNode<BattleSoundsAssetComponent> mapEffect)
        {
            listener.Entity.AddComponent(new PlayingMelodyRoundRestartListenerComponent(mapEffect.component.BattleSoundsBehaviour.PlayNeutralMelody(listener.soundListener.transform, -1f)));
        }

        private void PlayNonTeamMelodyInTeamMode(TeamNode userTeam, ICollection<TeamNode> teams, SoundsListenerNode listener, SingleNode<BattleSoundsAssetComponent> mapEffect, int minScoreDiff)
        {
            <PlayNonTeamMelodyInTeamMode>c__AnonStorey0 storey = new <PlayNonTeamMelodyInTeamMode>c__AnonStorey0 {
                minScoreDiff = minScoreDiff,
                userTeamEntity = userTeam.Entity,
                userTeamScore = userTeam.teamScore.Score,
                isNearScores = false,
                winSound = true
            };
            teams.ForEach<TeamNode>(new Action<TeamNode>(storey.<>m__0));
            if (storey.isNearScores)
            {
                listener.Entity.AddComponent(new PlayingMelodyRoundRestartListenerComponent(mapEffect.component.BattleSoundsBehaviour.PlayNeutralMelody(listener.soundListener.transform, -1f)));
            }
            else
            {
                listener.Entity.AddComponent(new PlayingMelodyRoundRestartListenerComponent(mapEffect.component.BattleSoundsBehaviour.PlayNonNeutralMelody(listener.soundListener.transform, storey.winSound, -1f)));
            }
        }

        [OnEventFire]
        public void PlayNonTeamRestartingSoundWhenSpectator(DefineRoundRestartSoundEvent evt, SoundsListenerNode listener, SelfSpectatorBattleUser battleUser, SingleNode<BattleSoundsAssetComponent> mapEffect)
        {
            mapEffect.component.BattleSoundsBehaviour.PlayShortNeutralSound(listener.soundListener.transform, -1f);
        }

        [OnEventFire]
        public void PlayNonTeamRestartingSoundWhenTankBattleUserInDM(DefineRoundRestartSoundEvent evt, SoundsListenerNode listener, SelfTankBattleUserNode battleUser, DMBattleNode dm, SingleNode<BattleSoundsAssetComponent> mapEffect)
        {
            mapEffect.component.BattleSoundsBehaviour.PlayShortNeutralSound(listener.soundListener.transform, -1f);
        }

        [OnEventFire]
        public void PlayNonTeamRestartingSoundWhenTankBattleUserInTeamMode(DefineRoundRestartSoundEvent evt, SoundsListenerNode listener, SingleNode<BattleSoundsAssetComponent> mapEffect, SelfTankBattleUserInTeamNode battleUser, [JoinByTeam] TeamNode userTeam, TeamBattleNode teamBattle, [JoinByBattle] ICollection<TeamNode> teams)
        {
            <PlayNonTeamRestartingSoundWhenTankBattleUserInTeamMode>c__AnonStorey1 storey = new <PlayNonTeamRestartingSoundWhenTankBattleUserInTeamMode>c__AnonStorey1 {
                userTeamEntity = userTeam.Entity,
                userTeamScore = userTeam.teamScore.Score,
                isEqualScore = true,
                winSound = true
            };
            teams.ForEach<TeamNode>(new Action<TeamNode>(storey.<>m__0));
            if (storey.isEqualScore)
            {
                mapEffect.component.BattleSoundsBehaviour.PlayShortNeutralSound(listener.soundListener.transform, listener.soundListenerBattleMixerSnapshotTransition.TransitionTimeToSilentAfterRoundFinish);
            }
            else
            {
                mapEffect.component.BattleSoundsBehaviour.PlayShortNonNeutralSound(listener.soundListener.transform, storey.winSound, listener.soundListenerBattleMixerSnapshotTransition.TransitionTimeToSilentAfterRoundFinish);
            }
        }

        private void PlaySoundOnFirstSpawn(SoundsListenerNode listener, SingleNode<BattleSoundsAssetComponent> mapEffect)
        {
            if (!listener.Entity.HasComponent<RoundFirstSpawnPlayedComponent>())
            {
                listener.Entity.AddComponent<RoundFirstSpawnPlayedComponent>();
                mapEffect.component.BattleSoundsBehaviour.PlayStartSound(listener.soundListener.transform, -1f);
            }
        }

        [OnEventFire]
        public void PlaySoundOnFirstSpawn(NodeAddedEvent evt, BattleStateSoundsListenerNode listener, SingleNode<BattleSoundsAssetComponent> mapEffect, SelfBattleUserNode battleUser, [JoinByBattle, Context] ActiveRoundNode round)
        {
            this.PlaySoundOnFirstSpawn(listener, mapEffect);
        }

        [OnEventFire]
        public void PlaySoundOnFirstSpawn(NodeAddedEvent evt, SpawnSoundsListenerNode listener, SingleNode<BattleSoundsAssetComponent> mapEffect, SelfBattleUserNode battleUser, [JoinByBattle, Context] ActiveRoundNode round)
        {
            this.PlaySoundOnFirstSpawn(listener, mapEffect);
        }

        [OnEventFire]
        public void PlaySoundOnRoundFinished(NodeAddedEvent evt, BattleStateSoundsListenerNode listener, SoundsListenerWithOldRoundRestartSoundsNode listenerWithOldRoundRestartSounds, SingleNode<BattleSoundsAssetComponent> mapEffect, SelfBattleUserNode battleUser, [Context, JoinByBattle] RoundRestartingNode roundRestarting, [Context, JoinByBattle] BattleNode battle)
        {
            Node[] nodes = new Node[] { battleUser, battle, mapEffect, listener };
            base.NewEvent<DefineRoundRestartSoundEvent>().AttachAll(nodes).Schedule();
        }

        [OnEventFire]
        public void RoundBalanced(NodeRemoveEvent e, SingleNode<RoundDisbalancedComponent> round, [JoinAll] SoundsListenerNode listener, [JoinAll] SelfBattleUserNode battleUser, [JoinAll] SingleNode<BattleSoundsAssetComponent> mapEffect)
        {
            if (!listener.Entity.HasComponent<MelodiesRoundRestartListenerComponent>())
            {
                listener.Entity.AddComponent<MelodiesRoundRestartListenerComponent>();
            }
            base.ScheduleEvent<StopBattleMelodyWhenRoundBalancedEvent>(listener);
        }

        [OnEventFire]
        public void RoundDisbalanced(UpdateEvent e, SingleNode<RoundDisbalancedComponent> round, [JoinAll] SoundsListenerWithRoundRestartMelodiesNode listener, [JoinAll] SelfBattleUserNode battleUser, [JoinAll] SingleNode<BattleSoundsAssetComponent> mapEffect)
        {
            if ((round.component.FinishTime.UnityTime - Date.Now.UnityTime) <= mapEffect.component.BattleSoundsBehaviour.MinRemainigRoundTimeSec)
            {
                Node[] nodes = new Node[] { battleUser, mapEffect, listener, round };
                base.NewEvent<DefineMelodyForRoundRestartEvent>().AttachAll(nodes).Schedule();
                listener.Entity.RemoveComponent<MelodiesRoundRestartListenerComponent>();
            }
        }

        [OnEventFire]
        public void SetOldSoundsForListenerWhenEnterBattle(NodeAddedEvent evt, SoundsListenerNode listener, SelfBattleUserNode battleUser, [JoinByBattle, Context] ActiveRoundNode round, SingleNode<BattleSoundsAssetComponent> mapEffect)
        {
            listener.Entity.AddComponent<OldRoundRestartSoundsListenerComponent>();
        }

        [OnEventFire]
        public void StopMelody(NodeRemoveEvent evt, SingleNode<PlayingMelodyRoundRestartListenerComponent> listener)
        {
            listener.component.Melody.Stop();
        }

        [OnEventFire]
        public void StopMelody(StopBattleMelodyEvent evt, SingleNode<PlayingMelodyRoundRestartListenerComponent> listener)
        {
            listener.Entity.RemoveComponent<PlayingMelodyRoundRestartListenerComponent>();
        }

        [CompilerGenerated]
        private sealed class <PlayNonTeamMelodyInTeamMode>c__AnonStorey0
        {
            internal Entity userTeamEntity;
            internal int userTeamScore;
            internal int minScoreDiff;
            internal bool isNearScores;
            internal bool winSound;

            internal void <>m__0(BattleSoundsSystem.TeamNode t)
            {
                if (!ReferenceEquals(t.Entity, this.userTeamEntity))
                {
                    int score = t.teamScore.Score;
                    if (Mathf.Abs((int) (score - this.userTeamScore)) < this.minScoreDiff)
                    {
                        this.isNearScores = true;
                    }
                    if (score > this.userTeamScore)
                    {
                        this.winSound = false;
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <PlayNonTeamRestartingSoundWhenTankBattleUserInTeamMode>c__AnonStorey1
        {
            internal Entity userTeamEntity;
            internal int userTeamScore;
            internal bool isEqualScore;
            internal bool winSound;

            internal void <>m__0(BattleSoundsSystem.TeamNode t)
            {
                if (!ReferenceEquals(t.Entity, this.userTeamEntity))
                {
                    int score = t.teamScore.Score;
                    if (score != this.userTeamScore)
                    {
                        this.isEqualScore = false;
                    }
                    if (score > this.userTeamScore)
                    {
                        this.winSound = false;
                    }
                }
            }
        }

        public class ActiveRoundNode : BattleSoundsSystem.RoundNode
        {
            public RoundActiveStateComponent roundActiveState;
        }

        public class ActiveRoundStopTimeNode : BattleSoundsSystem.RoundNode
        {
            public RoundActiveStateComponent roundActiveState;
            public RoundStopTimeComponent roundStopTime;
        }

        public class BattleNode : Node
        {
            public BattleComponent battle;
            public BattleGroupComponent battleGroup;
            public MapGroupComponent mapGroup;
        }

        public class BattleStateSoundsListenerNode : BattleSoundsSystem.SoundsListenerNode
        {
            public SoundListenerBattleStateComponent soundListenerBattleState;
        }

        public class CTFBattleNode : BattleSoundsSystem.BattleNode
        {
            public CTFComponent ctf;
        }

        public class DMBattleNode : BattleSoundsSystem.BattleNode
        {
            public DMComponent dm;
        }

        public class RoundNode : Node
        {
            public RoundComponent round;
            public BattleGroupComponent battleGroup;
        }

        public class RoundRestartingNode : BattleSoundsSystem.RoundNode
        {
            public RoundRestartingStateComponent roundRestartingState;
        }

        public class RoundUserNode : Node
        {
            public RoundUserStatisticsComponent roundUserStatistics;
        }

        public class SelfBattleUserNode : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public BattleGroupComponent battleGroup;
        }

        public class SelfSpectatorBattleUser : BattleSoundsSystem.SelfBattleUserNode
        {
            public UserInBattleAsSpectatorComponent userInBattleAsSpectator;
        }

        public class SelfTankBattleUserInTeamNode : BattleSoundsSystem.SelfTankBattleUserNode
        {
            public TeamGroupComponent teamGroup;
        }

        public class SelfTankBattleUserNode : BattleSoundsSystem.SelfBattleUserNode
        {
            public UserInBattleAsTankComponent userInBattleAsTank;
        }

        public class SoundsListenerNode : Node
        {
            public SoundListenerComponent soundListener;
            public SoundListenerBattleMixerSnapshotTransitionComponent soundListenerBattleMixerSnapshotTransition;
        }

        public class SoundsListenerWithOldRoundRestartSoundsNode : BattleSoundsSystem.SoundsListenerNode
        {
            public OldRoundRestartSoundsListenerComponent oldRoundRestartSoundsListener;
        }

        public class SoundsListenerWithRoundRestartMelodiesNode : BattleSoundsSystem.SoundsListenerNode
        {
            public MelodiesRoundRestartListenerComponent melodiesRoundRestartListener;
        }

        public class SpawnSoundsListenerNode : BattleSoundsSystem.SoundsListenerNode
        {
            public SoundListenerSpawnStateComponent soundListenerSpawnState;
        }

        public class TDMBattleNode : BattleSoundsSystem.BattleNode
        {
            public TDMComponent tdm;
        }

        public class TeamBattleNode : BattleSoundsSystem.BattleNode
        {
            public TeamBattleComponent teamBattle;
        }

        public class TeamNode : Node
        {
            public TeamGroupComponent teamGroup;
            public TeamScoreComponent teamScore;
            public BattleGroupComponent battleGroup;
        }
    }
}

