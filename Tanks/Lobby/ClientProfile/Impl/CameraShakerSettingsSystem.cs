namespace Tanks.Lobby.ClientProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class CameraShakerSettingsSystem : ECSSystem
    {
        public static readonly string CAMERA_SHAKER_ENABLED_KEY = "CAMERA_SHAKER_ENABLED";
        public static readonly int CAMERA_SHAKER_ENABLED_DEFAULT_VALUE = 1;

        [OnEventFire]
        public void GameSettingsChanged(SettingsChangedEvent<GameCameraShakerSettingsComponent> e, SingleNode<GameCameraShakerSettingsComponent> settings)
        {
            PlayerPrefs.SetInt(CAMERA_SHAKER_ENABLED_KEY, !settings.component.Enabled ? 0 : 1);
        }

        [OnEventFire]
        public void InitGameSettings(NodeAddedEvent evt, SingleNode<GameCameraShakerSettingsComponent> gameSettings)
        {
            gameSettings.component.Enabled = this.IsCameraShakerEnabledInPlayerPrefs();
        }

        private bool IsCameraShakerEnabledInPlayerPrefs()
        {
            if (PlayerPrefs.HasKey(CAMERA_SHAKER_ENABLED_KEY))
            {
                return (PlayerPrefs.GetInt(CAMERA_SHAKER_ENABLED_KEY) > 0);
            }
            PlayerPrefs.SetInt(CAMERA_SHAKER_ENABLED_KEY, CAMERA_SHAKER_ENABLED_DEFAULT_VALUE);
            return true;
        }
    }
}

