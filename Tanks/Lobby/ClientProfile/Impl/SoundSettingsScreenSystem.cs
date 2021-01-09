namespace Tanks.Lobby.ClientProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientSettings.API;

    public class SoundSettingsScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void InitMusicVolumeSliderBar(NodeAddedEvent e, MusicVolumeSliderBarNode musicVolumeSliderBarNode)
        {
            musicVolumeSliderBarNode.sliderBar.Value = SoundSettingsUtils.GetSavedVolume(SoundType.Music);
        }

        [OnEventFire]
        public void InitSFXVolumeSliderBar(NodeAddedEvent e, SFXVolumeSliderBarNode sfxVolumeSliderBarNode)
        {
            sfxVolumeSliderBarNode.sliderBar.Value = SoundSettingsUtils.GetSavedVolume(SoundType.SFX);
        }

        [OnEventFire]
        public void InitUIVolumeSliderBar(NodeAddedEvent e, UIVolumeSliderBarNode uiVolumeSliderBarNode)
        {
            uiVolumeSliderBarNode.sliderBar.Value = SoundSettingsUtils.GetSavedVolume(SoundType.UI);
        }

        public class MusicVolumeSliderBarNode : Node
        {
            public MusicVolumeSliderBarComponent musicVolumeSliderBar;
            public SliderBarComponent sliderBar;
        }

        public class SFXVolumeSliderBarNode : Node
        {
            public SFXVolumeSliderBarComponent sfxVolumeSliderBar;
            public SliderBarComponent sliderBar;
        }

        public class UIVolumeSliderBarNode : Node
        {
            public UIVolumeSliderBarComponent uiVolumeSliderBar;
            public SliderBarComponent sliderBar;
        }
    }
}

