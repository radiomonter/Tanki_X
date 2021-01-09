namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Battle.ClientGraphics.Impl;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;
    using UnityEngine.Audio;

    public class HangarModuleSoundsSystem : ECSSystem
    {
        [CompilerGenerated]
        private static Action<HangarModuleSoundBehaviour> <>f__am$cache0;

        [OnEventFire]
        public void Cancel(NodeRemoveEvent e, SingleNode<ModuleGarageSoundWaitForFinishComponent> node)
        {
            node.component.ScheduledEvent.Cancel();
        }

        [OnEventComplete]
        public void Cancel(ModuleGarageSoundFinishEvent e, SingleNode<ModuleGarageSoundWaitForFinishComponent> node)
        {
            node.Entity.RemoveComponent<ModuleGarageSoundWaitForFinishComponent>();
        }

        [OnEventFire]
        public void CleanUp(MapAmbientSoundPlayEvent e, SoundListenerNode listener)
        {
            this.CleanUpAllGarageModuleSound(listener.soundListener.transform);
        }

        private void CleanUpAllGarageModuleSound(Transform listenerTransform)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = delegate (HangarModuleSoundBehaviour s) {
                    SoundController component = s.GetComponent<SoundController>();
                    component.FadeOut();
                    Object.DestroyObject(s.gameObject, component.FadeOutTimeSec + 0.2f);
                };
            }
            listenerTransform.gameObject.GetComponentsInChildren<HangarModuleSoundBehaviour>().ForEach<HangarModuleSoundBehaviour>(<>f__am$cache0);
        }

        [OnEventFire]
        public void PlayModuleActivationSound(ModuleAssembledEvent e, SingleNode<UserComponent> node, [JoinAll] SoundListenerNode listener)
        {
            this.PlayModuleSound(listener.soundListenerResources.Resources.ModuleActivation, listener);
        }

        private void PlayModuleSound(GameObject source, SoundListenerNode listener)
        {
            Transform listenerTransform = listener.soundListener.transform;
            this.CleanUpAllGarageModuleSound(listenerTransform);
            SoundController component = Object.Instantiate<GameObject>(source, listenerTransform.position, listenerTransform.rotation, listenerTransform).GetComponent<SoundController>();
            this.SwitchMusicMixerToSnapshot(listener.soundListenerResources.Resources.MusicMixerSnapshots[listener.soundListenerMusicSnapshots.GarageModuleMusicSnapshot], listener.soundListenerMusicTransitions.TransitionModuleTheme, listener);
            component.SetSoundActive();
            Object.DestroyObject(component.gameObject, component.Source.clip.length + 0.1f);
            listener.Entity.RemoveComponentIfPresent<CardsContainerOpeningSoundWaitForFinishComponent>();
            listener.Entity.RemoveComponentIfPresent<ModuleGarageSoundWaitForFinishComponent>();
            listener.Entity.AddComponent(new ModuleGarageSoundWaitForFinishComponent(base.NewEvent<ModuleGarageSoundFinishEvent>().Attach(listener).ScheduleDelayed(component.Source.clip.length).Manager()));
        }

        [OnEventFire]
        public void PlayModuleUpgradeSound(ModuleUpgradedEvent e, SingleNode<UserItemComponent> node, [JoinAll] SoundListenerNode listener)
        {
            this.PlayModuleSound(listener.soundListenerResources.Resources.ModuleUpgrade, listener);
        }

        private void SwitchMusicMixerToSnapshot(AudioMixerSnapshot snapshot, float transition, SoundListenerNode listener)
        {
            AudioMixerSnapshot[] snapshots = new AudioMixerSnapshot[] { snapshot };
            float[] weights = new float[] { 1f };
            listener.soundListenerResources.Resources.MusicMixer.TransitionToSnapshots(snapshots, weights, transition);
        }

        [OnEventFire]
        public void SwitchSnapshot(ModuleGarageSoundFinishEvent e, SoundListenerNode listener)
        {
            this.SwitchMusicMixerToSnapshot(listener.soundListenerResources.Resources.MusicMixerSnapshots[listener.soundListenerMusicSnapshots.HymnLoopSnapshot], listener.soundListenerMusicTransitions.TransitionModuleTheme, listener);
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

