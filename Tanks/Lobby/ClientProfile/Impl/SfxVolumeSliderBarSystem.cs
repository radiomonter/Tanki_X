namespace Tanks.Lobby.ClientProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;

    public class SfxVolumeSliderBarSystem : ECSSystem
    {
        [OnEventFire]
        public void FinalizeVolumeNotifier(NodeRemoveEvent e, SfxVolumeSliderBarNotifierNode slider)
        {
            Object.DestroyObject(slider.volumeChangedNotifier.AudioSource.gameObject, slider.volumeChangedNotifier.AudioSource.clip.length);
            Object.Destroy(slider.volumeChangedNotifier);
        }

        [OnEventFire]
        public void InitVolumeNotifier(NodeAddedEvent e, SfxVolumeSliderBarNode slider, [Context, JoinByScreen] SoundSettingsScreenNode screen, SingleNode<SoundListenerResourcesComponent> listener)
        {
            GameObject gameObject = slider.sfxVolumeSliderBar.gameObject;
            VolumeChangedNotifierComponent component = gameObject.AddComponent<VolumeChangedNotifierComponent>();
            component.Slider = gameObject.GetComponent<Slider>();
            component.AudioSource = Object.Instantiate<AudioSource>(listener.component.Resources.SfxSourcePreview);
            slider.Entity.AddComponent(component);
        }

        public class SfxVolumeSliderBarNode : Node
        {
            public SFXVolumeSliderBarComponent sfxVolumeSliderBar;
            public ScreenGroupComponent screenGroup;
        }

        public class SfxVolumeSliderBarNotifierNode : SfxVolumeSliderBarSystem.SfxVolumeSliderBarNode
        {
            public VolumeChangedNotifierComponent volumeChangedNotifier;
        }

        public class SoundSettingsScreenNode : Node
        {
            public SoundSettingsScreenComponent soundSettingsScreen;
            public ScreenGroupComponent screenGroup;
        }
    }
}

