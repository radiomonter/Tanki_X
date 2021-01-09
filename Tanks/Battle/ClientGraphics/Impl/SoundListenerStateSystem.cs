namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientSettings.API;

    public class SoundListenerStateSystem : ECSSystem
    {
        [OnEventFire]
        public void InitSoundListenerESM(NodeAddedEvent evt, SoundListenerNode listener)
        {
            SoundListenerESMComponent component = new SoundListenerESMComponent();
            EntityStateMachine esm = component.Esm;
            esm.AddState<SoundListenerStates.SoundListenerSpawnState>();
            esm.AddState<SoundListenerStates.SoundListenerBattleState>();
            esm.AddState<SoundListenerStates.SoundListenerLobbyState>();
            esm.AddState<SoundListenerStates.SoundListenerBattleFinishState>();
            esm.AddState<SoundListenerStates.SoundListenerSelfRankRewardState>();
            listener.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void SwitchSoundListenerToBattleFinishState(DefineMelodyForRoundRestartEvent e, SoundListenerNode listener, [JoinAll] SingleNode<MapInstanceComponent> map)
        {
            base.ScheduleEvent<SwitchSoundListenerStateEvent<SoundListenerStates.SoundListenerBattleFinishState>>(listener);
        }

        [OnEventFire]
        public void SwitchSoundListenerToBattleFinishState(StopBattleMelodyWhenRoundBalancedEvent e, SoundListenerNode listener, [JoinAll] SingleNode<MapInstanceComponent> map)
        {
            base.ScheduleEvent<SwitchSoundListenerStateEvent<SoundListenerStates.SoundListenerBattleState>>(listener);
        }

        [OnEventFire]
        public void SwitchSoundListenerToBattleState(SwitchSoundListenerStateEvent evt, SoundListenerESMNode soundListener)
        {
            soundListener.soundListenerEsm.Esm.ChangeState(evt.StateType);
        }

        [OnEventFire]
        public void SwitchSoundListenerToBattleState(NodeAddedEvent evt, ContainersScreenNode screen, SingleNode<SoundListenerComponent> listener)
        {
            base.ScheduleEvent<SwitchSoundListenerStateEvent<SoundListenerStates.SoundListenerBattleState>>(listener);
        }

        [OnEventFire]
        public void SwitchSoundListenerToBattleState(SoundListenerInitBattleStateEvent e, SoundListenerESMNode soundListener, [JoinSelf] SingleNode<SoundListenerSpawnStateComponent> spawn)
        {
            base.ScheduleEvent<SwitchSoundListenerStateEvent<SoundListenerStates.SoundListenerBattleState>>(soundListener);
        }

        [OnEventFire]
        public void SwitchSoundListenerToLobbyState(NodeRemoveEvent evt, ContainersScreenNode screen, [JoinAll] SingleNode<SoundListenerComponent> listener)
        {
            base.ScheduleEvent<SwitchSoundListenerStateEvent<SoundListenerStates.SoundListenerLobbyState>>(listener);
        }

        [OnEventFire]
        public void SwitchSoundListenerToLobbyState(LobbyAmbientSoundPlayEvent evt, SoundListenerESMNode soundListener, [JoinAll] Optional<SingleNode<MapInstanceComponent>> map)
        {
            if (!map.IsPresent())
            {
                base.ScheduleEvent<SwitchSoundListenerStateEvent<SoundListenerStates.SoundListenerLobbyState>>(soundListener);
            }
        }

        [OnEventFire]
        public void SwitchSoundListenerToSpawnState(MapAmbientSoundPlayEvent evt, SoundListenerNotBattleStateESMNode soundListener, [JoinAll] SingleNode<MapInstanceComponent> map)
        {
            soundListener.soundListenerEsm.Esm.ChangeState<SoundListenerStates.SoundListenerSpawnState>();
            base.NewEvent<SoundListenerInitBattleStateEvent>().Attach(soundListener).ScheduleDelayed(soundListener.soundListener.DelayForBattleState);
        }

        [OnEventFire]
        public void SwitchToBattleState(NodeRemoveEvent e, SingleNode<SelfUserRankSoundEffectInstanceComponent> effect, [JoinAll] SoundListenerNotBattleFinishStateNode listener, [JoinAll] SingleNode<MapInstanceComponent> map, [JoinAll] SingleNode<RoundActiveStateComponent> round, [JoinAll] ICollection<SingleNode<SelfUserRankSoundEffectInstanceComponent>> effects)
        {
            if (effects.Count <= 1)
            {
                base.ScheduleEvent<SwitchSoundListenerStateEvent<SoundListenerStates.SoundListenerBattleState>>(listener);
            }
        }

        [OnEventFire]
        public void SwitchToSelfRankRewardState(NodeAddedEvent e, SingleNode<SelfUserRankSoundEffectInstanceComponent> effect, [JoinAll] SoundListenerNotBattleFinishStateNode listener, [JoinAll] SingleNode<MapInstanceComponent> map, [JoinAll] SingleNode<RoundActiveStateComponent> round)
        {
            base.ScheduleEvent<SwitchSoundListenerStateEvent<SoundListenerStates.SoundListenerSelfRankRewardState>>(listener);
        }

        public class ContainersScreenNode : Node
        {
            public ContainersScreenComponent containersScreen;
            public ActiveScreenComponent activeScreen;
        }

        public class SoundListenerESMNode : SoundListenerStateSystem.SoundListenerNode
        {
            public SoundListenerESMComponent soundListenerEsm;
        }

        public class SoundListenerNode : Node
        {
            public SoundListenerComponent soundListener;
        }

        [Not(typeof(SoundListenerBattleFinishStateComponent))]
        public class SoundListenerNotBattleFinishStateNode : Node
        {
            public SoundListenerBattleStateComponent soundListenerBattleState;
        }

        [Not(typeof(SoundListenerBattleStateComponent))]
        public class SoundListenerNotBattleStateESMNode : SoundListenerStateSystem.SoundListenerESMNode
        {
        }
    }
}

