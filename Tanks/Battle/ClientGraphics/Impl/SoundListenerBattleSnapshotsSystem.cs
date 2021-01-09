namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine.Audio;

    public class SoundListenerBattleSnapshotsSystem : ECSSystem
    {
        private void Switch(SoundListenerNode listener, AudioMixerSnapshot snapshot, float transition)
        {
            AudioMixerSnapshot[] snapshots = new AudioMixerSnapshot[] { snapshot };
            float[] weights = new float[] { 1f };
            listener.soundListenerResources.Resources.SfxMixer.TransitionToSnapshots(snapshots, weights, transition);
        }

        private void SwitchToLoud(SoundListenerNode listener, float transitionTime)
        {
            this.Switch(listener, listener.soundListenerResources.Resources.SfxMixerSnapshots[listener.soundListenerBattleMixerSnapshots.LoudSnapshotIndex], transitionTime);
        }

        [OnEventFire]
        public void SwitchToLoudFromUser(NodeRemoveEvent e, SingleNode<SoundListenerSelfRankRewardStateComponent> selfRankReward, [JoinSelf] SingleNode<SoundListenerBattleStateComponent> battleState, [JoinSelf] SoundListenerNode listener, [JoinAll] SingleNode<MapInstanceComponent> map, [JoinAll] SingleNode<RoundActiveStateComponent> round)
        {
            this.Switch(listener, listener.soundListenerResources.Resources.SfxMixerSnapshots[listener.soundListenerBattleMixerSnapshots.LoudSnapshotIndex], listener.soundListenerBattleMixerSnapshotTransition.TransitionToLoudTimeInSelfUserMode);
        }

        [OnEventFire]
        public void SwitchToLoudWhenBattleState(NodeAddedEvent e, SoundListenerBattleStateNode listener)
        {
            this.SwitchToLoud(listener, listener.soundListenerBattleMixerSnapshotTransition.TransitionToLoudTimeInBattle);
        }

        [OnEventComplete]
        public void SwitchToLoudWhenBattleState(StopBattleMelodyWhenRoundBalancedEvent e, SoundListenerBattleStateNode listener)
        {
            this.SwitchToLoud(listener, listener.soundListenerBattleMixerSnapshotTransition.TransitionTimeToMelodySilent);
        }

        [OnEventFire]
        public void SwitchToLoudWhenNewRoundInBattle(NodeAddedEvent e, ActiveRoundNode round, [Context, JoinByBattle] SelfBattleUserNode battleUser, [JoinAll] SoundListenerBattleStateNode listener)
        {
            this.SwitchToLoud(listener, listener.soundListenerBattleMixerSnapshotTransition.TransitionToLoudTimeInBattle);
        }

        private void SwitchToMelodySilent(SoundListenerNode listener, float transitionTime)
        {
            this.Switch(listener, listener.soundListenerResources.Resources.SfxMixerSnapshots[listener.soundListenerBattleMixerSnapshots.MelodySilentSnapshotIndex], transitionTime);
        }

        [OnEventFire]
        public void SwitchToMelodySilentWhenRoundFinish(NodeAddedEvent e, SoundListenerBattleFinishStateNode listener)
        {
            this.SwitchToMelodySilent(listener, listener.soundListenerBattleMixerSnapshotTransition.TransitionTimeToMelodySilent);
        }

        [OnEventFire]
        public void SwitchToSelfUserSnapshot(NodeAddedEvent e, SoundListenerSelfRankRewardStateNode listener)
        {
            this.Switch(listener, listener.soundListenerResources.Resources.SfxMixerSnapshots[listener.soundListenerBattleMixerSnapshots.SelfUserSnapshotIndex], 0f);
        }

        private void SwitchToSilent(SoundListenerNode listener, float transitionTime)
        {
            this.Switch(listener, listener.soundListenerResources.Resources.SfxMixerSnapshots[listener.soundListenerBattleMixerSnapshots.SilentSnapshotIndex], transitionTime);
        }

        [OnEventFire]
        public void SwitchToSilentWhenExitBattle(ExitBattleEvent e, Node node, [JoinAll] SoundListenerNode listener)
        {
            this.SwitchToSilent(listener, listener.soundListenerBattleMixerSnapshotTransition.TransitionTimeToSilentAfterExitBattle);
        }

        [OnEventFire]
        public void SwitchToSilentWhenRoundFinish(NodeRemoveEvent e, SingleNode<RoundActiveStateComponent> roundActive, [JoinSelf] RoundNode round, [JoinByBattle] SelfBattleUserNode battleUser, [JoinAll] SoundListenerNode listener)
        {
            this.SwitchToSilent(listener, listener.soundListenerBattleMixerSnapshotTransition.TransitionTimeToSilentAfterRoundFinish);
        }

        [OnEventFire]
        public void SwitchToSilentWhenSpawnState(NodeAddedEvent e, SoundListenerSpawnStateNode listener)
        {
            this.SwitchToSilent(listener, 0f);
        }

        public class ActiveRoundNode : SoundListenerBattleSnapshotsSystem.RoundNode
        {
            public RoundActiveStateComponent roundActiveState;
        }

        public class RoundNode : Node
        {
            public RoundComponent round;
            public BattleGroupComponent battleGroup;
        }

        public class SelfBattleUserNode : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public BattleUserComponent battleUser;
            public BattleGroupComponent battleGroup;
        }

        public class SoundListenerBattleFinishStateNode : SoundListenerBattleSnapshotsSystem.SoundListenerNode
        {
            public SoundListenerBattleFinishStateComponent soundListenerBattleFinishState;
        }

        public class SoundListenerBattleStateNode : SoundListenerBattleSnapshotsSystem.SoundListenerNode
        {
            public SoundListenerBattleStateComponent soundListenerBattleState;
        }

        public class SoundListenerNode : Node
        {
            public SoundListenerComponent soundListener;
            public SoundListenerBattleMixerSnapshotTransitionComponent soundListenerBattleMixerSnapshotTransition;
            public SoundListenerResourcesComponent soundListenerResources;
            public SoundListenerBattleMixerSnapshotsComponent soundListenerBattleMixerSnapshots;
        }

        public class SoundListenerSelfRankRewardStateNode : SoundListenerBattleSnapshotsSystem.SoundListenerNode
        {
            public SoundListenerSelfRankRewardStateComponent soundListenerSelfRankRewardState;
        }

        public class SoundListenerSpawnStateNode : SoundListenerBattleSnapshotsSystem.SoundListenerNode
        {
            public SoundListenerSpawnStateComponent soundListenerSpawnState;
        }
    }
}

