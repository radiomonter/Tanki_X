namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientBattleSelect.Impl;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientLoading.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;
    using UnityEngine.Audio;

    public class HangarAmbientSoundSystem : ECSSystem
    {
        [OnEventFire]
        public void FilterLobbyAmbientSoundPlayEvent(BeforeLobbyAmbientSoundPlayEvent evt, SoundListenerNode listener, [JoinAll] SingleNode<HangarInstanceComponent> hangar, [JoinAll] PlayLobbySoundScreenNode screen, [JoinAll] Optional<SingleNode<MapInstanceComponent>> map)
        {
            if (!map.IsPresent())
            {
                base.ScheduleEvent(new LobbyAmbientSoundPlayEvent(evt.HymnMode), listener);
            }
        }

        [OnEventFire]
        public void FilterMapAmbientSoundPlayEvent(BeforeMapAmbientSoundPlayEvent evt, SoundListenerNode soundListener, [JoinAll] SingleNode<MapInstanceComponent> map, [JoinAll] BattleScreenNode battleScreen)
        {
            base.ScheduleEvent<MapAmbientSoundPlayEvent>(soundListener);
        }

        [OnEventFire]
        public void FinalizeAmbientSoundEffect(MapAmbientSoundPlayEvent evt, ReadyNonSilentAmbientSoundNode soundListener)
        {
            this.Stop(soundListener);
        }

        [OnEventFire]
        public void FinalizeAmbientSoundEffect(MapAmbientSoundPlayEvent evt, ReadySilentAmbientSoundNode soundListener)
        {
            this.Stop(soundListener);
            soundListener.Entity.RemoveComponent<HangarAmbientSoundSilenceComponent>();
        }

        [OnEventFire]
        public void FinalizeAmbientSoundEffect(NodeAddedEvent evt, BattleScreenNode battleScreen, [JoinAll] SoundListenerNode soundListener)
        {
            base.NewEvent<BeforeMapAmbientSoundPlayEvent>().Attach(soundListener).ScheduleDelayed(soundListener.soundListener.DelayForBattleEnterState);
        }

        private void Play(ReadySilentAmbientSoundNode soundListener, bool playWithNitro)
        {
            Entity entity = soundListener.Entity;
            soundListener.hangarAmbientSoundController.HangarAmbientSoundController.Play(playWithNitro);
            entity.RemoveComponent<HangarAmbientSoundSilenceComponent>();
        }

        [OnEventComplete]
        public void PlayAmbientSoundEffect(LobbyAmbientSoundPlayEvent evt, ReadySilentNotPlayedAmbientSoundNode soundListener)
        {
            soundListener.Entity.AddComponent<HangarAmbientSoundAlreadyPlayedComponent>();
            this.Play(soundListener, true);
        }

        [OnEventComplete]
        public void PlayAmbientSoundEffect(LobbyAmbientSoundPlayEvent evt, ReadySilentPlayedAmbientSoundNode soundListener)
        {
            this.Play(soundListener, false);
        }

        [OnEventFire]
        public void PlayAmbientSoundEffect(NodeAddedEvent evt, ReadySilentAmbientSoundNode soundListener, BattleResultsScreenNode screen)
        {
            base.NewEvent(new BeforeLobbyAmbientSoundPlayEvent(false)).Attach(soundListener).ScheduleDelayed(soundListener.soundListener.DelayForLobbyState);
        }

        [OnEventFire]
        public void PlayAmbientSoundEffect(NodeAddedEvent evt, ReadySilentAmbientSoundNode soundListener, BattleSelectScreenNode screen)
        {
            base.NewEvent(new BeforeLobbyAmbientSoundPlayEvent(true)).Attach(soundListener).ScheduleDelayed(soundListener.soundListener.DelayForLobbyState);
        }

        [OnEventFire]
        public void PlayAmbientSoundEffect(NodeAddedEvent evt, ReadySilentAmbientSoundNode soundListener, HomeScreenNode screen)
        {
            base.NewEvent(new BeforeLobbyAmbientSoundPlayEvent(true)).Attach(soundListener).ScheduleDelayed(soundListener.soundListener.DelayForLobbyState);
        }

        [OnEventFire]
        public void PrepareAmbientSoundEffect(NodeAddedEvent evt, InitialHangarAmbientSoundNode hangar, NotHangarAmbientSoundListenerNode soundListener)
        {
            Entity entity = soundListener.Entity;
            entity.AddComponent(new HangarAmbientSoundControllerComponent(this.PrepareNewEffect(hangar, soundListener)));
            entity.AddComponent<HangarAmbientSoundSilenceComponent>();
        }

        private HangarAmbientSoundController PrepareNewEffect(InitialHangarAmbientSoundNode hangar, SoundListenerNode soundListener)
        {
            HangarAmbientSoundController controller2 = Object.Instantiate<HangarAmbientSoundController>(hangar.hangarAmbientSoundPrefab.HangarAmbientSoundController);
            Transform transform = controller2.transform;
            transform.parent = soundListener.soundListener.transform;
            transform.localRotation = Quaternion.identity;
            transform.localPosition = Vector3.zero;
            return controller2;
        }

        [OnEventFire]
        public void SetBattleResultSnapshot(LobbyAmbientSoundPlayEvent evt, ReadySilentPlayedAmbientSoundNode soundListener)
        {
            this.SwitchMusicMixerToSnapshot(soundListener.soundListenerResources.Resources.MusicMixerSnapshots[!evt.HymnMode ? soundListener.soundListenerMusicSnapshots.BattleResultMusicSnapshot : soundListener.soundListenerMusicSnapshots.HymnLoopSnapshot], 0f, soundListener);
        }

        private void Stop(HangarAmbientSoundListenerNode soundListener)
        {
            HangarAmbientSoundController hangarAmbientSoundController = soundListener.hangarAmbientSoundController.HangarAmbientSoundController;
            soundListener.Entity.RemoveComponent<HangarAmbientSoundControllerComponent>();
            hangarAmbientSoundController.Stop();
        }

        private void SwitchMusicMixerToSnapshot(AudioMixerSnapshot snapshot, float transition, SoundListenerNode listener)
        {
            AudioMixerSnapshot[] snapshots = new AudioMixerSnapshot[] { snapshot };
            float[] weights = new float[] { 1f };
            listener.soundListenerResources.Resources.MusicMixer.TransitionToSnapshots(snapshots, weights, transition);
        }

        [OnEventFire]
        public void SwitchToHymmSnapshot(NodeAddedEvent evt, HomeScreenNode screen, [JoinAll] ReadyNonSilentAmbientSoundNode listener)
        {
            this.SwitchMusicMixerToSnapshot(listener.soundListenerResources.Resources.MusicMixerSnapshots[listener.soundListenerMusicSnapshots.HymnLoopSnapshot], listener.soundListenerMusicTransitions.MusicTransitionSec, listener);
        }

        public class BattleResultsScreenNode : Node
        {
            public BattleResultScreenComponent battleResultScreen;
            public ActiveScreenComponent activeScreen;
        }

        public class BattleScreenNode : Node
        {
            public BattleScreenComponent battleScreen;
            public ActiveScreenComponent activeScreen;
        }

        public class BattleSelectScreenNode : Node
        {
            public ActiveScreenComponent activeScreen;
            public BattleSelectScreenComponent battleSelectScreen;
        }

        public class HangarAmbientSoundListenerNode : HangarAmbientSoundSystem.SoundListenerNode
        {
            public HangarAmbientSoundControllerComponent hangarAmbientSoundController;
        }

        public class HomeScreenNode : Node
        {
            public MainScreenComponent mainScreen;
            public ActiveScreenComponent activeScreen;
        }

        public class InitialHangarAmbientSoundNode : Node
        {
            public HangarAmbientSoundPrefabComponent hangarAmbientSoundPrefab;
        }

        [Not(typeof(HangarAmbientSoundControllerComponent)), Not(typeof(HangarAmbientSoundSilenceComponent))]
        public class NotHangarAmbientSoundListenerNode : HangarAmbientSoundSystem.SoundListenerNode
        {
        }

        [Not(typeof(BattleLoadScreenComponent)), Not(typeof(BattleScreenComponent))]
        public class PlayLobbySoundScreenNode : Node
        {
            public ActiveScreenComponent activeScreen;
            public ScreenComponent screen;
        }

        [Not(typeof(HangarAmbientSoundSilenceComponent))]
        public class ReadyNonSilentAmbientSoundNode : HangarAmbientSoundSystem.HangarAmbientSoundListenerNode
        {
        }

        public class ReadySilentAmbientSoundNode : HangarAmbientSoundSystem.HangarAmbientSoundListenerNode
        {
            public HangarAmbientSoundSilenceComponent hangarAmbientSoundSilence;
        }

        [Not(typeof(HangarAmbientSoundAlreadyPlayedComponent))]
        public class ReadySilentNotPlayedAmbientSoundNode : HangarAmbientSoundSystem.ReadySilentAmbientSoundNode
        {
        }

        public class ReadySilentPlayedAmbientSoundNode : HangarAmbientSoundSystem.ReadySilentAmbientSoundNode
        {
            public HangarAmbientSoundAlreadyPlayedComponent hangarAmbientSoundAlreadyPlayed;
        }

        public class SoundListenerNode : Node
        {
            public SoundListenerComponent soundListener;
            public SoundListenerResourcesComponent soundListenerResources;
            public SoundListenerMusicSnapshotsComponent soundListenerMusicSnapshots;
            public SoundListenerMusicTransitionsComponent soundListenerMusicTransitions;
        }
    }
}

