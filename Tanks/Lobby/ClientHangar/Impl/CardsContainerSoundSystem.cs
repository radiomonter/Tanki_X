namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;
    using UnityEngine.Audio;

    public class CardsContainerSoundSystem : ECSSystem
    {
        [CompilerGenerated]
        private static Action<CardsSoundBehaviour> <>f__am$cache0;

        [OnEventFire]
        public void Cancel(NodeRemoveEvent e, SingleNode<CardsContainerOpeningSoundWaitForFinishComponent> node)
        {
            node.component.ScheduledEvent.Cancel();
        }

        [OnEventComplete]
        public void Cancel(CardsContainerOpeningSoundFinishEvent e, SingleNode<CardsContainerOpeningSoundWaitForFinishComponent> node)
        {
            node.Entity.RemoveComponent<CardsContainerOpeningSoundWaitForFinishComponent>();
        }

        private void CleanUpAllOpenCardsContainerSound(Transform listenerTransform)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = delegate (CardsSoundBehaviour s) {
                    s.OpenCardsContainerSource.FadeOut();
                    Object.DestroyObject(s.gameObject, s.OpenCardsContainerSource.FadeOutTimeSec + 0.5f);
                };
            }
            listenerTransform.gameObject.GetComponentsInChildren<CardsSoundBehaviour>().ForEach<CardsSoundBehaviour>(<>f__am$cache0);
        }

        [OnEventFire]
        public void CleanUpAllOpenCardsContainerSound(MapAmbientSoundPlayEvent e, SoundListenerNode listener)
        {
            this.CleanUpAllOpenCardsContainerSound(listener.soundListener.transform);
        }

        [OnEventFire]
        public void PlayOpenSound(OpenVisualContainerEvent e, Node any, [JoinAll] SingleNode<CardsContainerSoundsComponent> container, [JoinAll] SoundListenerNode listener)
        {
            Transform listenerTransform = listener.soundListener.transform;
            this.CleanUpAllOpenCardsContainerSound(listenerTransform);
            CardsSoundBehaviour behaviour = Object.Instantiate<CardsSoundBehaviour>(container.component.CardsSounds);
            behaviour.transform.SetParent(listenerTransform);
            behaviour.transform.localRotation = Quaternion.identity;
            behaviour.transform.localPosition = Vector3.zero;
            this.SwitchMusicMixerToSnapshot(listener.soundListenerResources.Resources.MusicMixerSnapshots[listener.soundListenerMusicSnapshots.CardsContainerMusicSnapshot], listener.soundListenerMusicTransitions.TransitionCardsContainerTheme, listener);
            behaviour.OpenCardsContainerSource.FadeIn();
            AudioClip clip = behaviour.OpenCardsContainerSource.Source.clip;
            Object.DestroyObject(behaviour.gameObject, clip.length + 0.5f);
            listener.Entity.RemoveComponentIfPresent<CardsContainerOpeningSoundWaitForFinishComponent>();
            listener.Entity.RemoveComponentIfPresent<ModuleGarageSoundWaitForFinishComponent>();
            listener.Entity.AddComponent(new CardsContainerOpeningSoundWaitForFinishComponent(base.NewEvent<CardsContainerOpeningSoundFinishEvent>().Attach(listener).ScheduleDelayed(clip.length).Manager()));
        }

        private void SwitchMusicMixerToSnapshot(AudioMixerSnapshot snapshot, float transition, SoundListenerNode listener)
        {
            AudioMixerSnapshot[] snapshots = new AudioMixerSnapshot[] { snapshot };
            float[] weights = new float[] { 1f };
            listener.soundListenerResources.Resources.MusicMixer.TransitionToSnapshots(snapshots, weights, transition);
        }

        [OnEventFire]
        public void SwitchSnapshot(CardsContainerOpeningSoundFinishEvent e, SoundListenerNode listener)
        {
            this.SwitchMusicMixerToSnapshot(listener.soundListenerResources.Resources.MusicMixerSnapshots[listener.soundListenerMusicSnapshots.HymnLoopSnapshot], listener.soundListenerMusicTransitions.TransitionCardsContainerTheme, listener);
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

