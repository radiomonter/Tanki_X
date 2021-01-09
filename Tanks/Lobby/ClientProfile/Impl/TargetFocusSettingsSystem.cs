namespace Tanks.Lobby.ClientProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class TargetFocusSettingsSystem : ECSSystem
    {
        public static readonly string TARGET_FOCUS_ENABLED_KEY = "TARGET_FOCUS_ENABLED";
        public static readonly int TARGET_FOCUS_ENABLED_DEFAULT_VALUE = 1;

        [OnEventFire]
        public void GameSettingsChanged(SettingsChangedEvent<TargetFocusSettingsComponent> e, SingleNode<TargetFocusSettingsComponent> settings)
        {
            PlayerPrefs.SetInt(TARGET_FOCUS_ENABLED_KEY, !settings.component.Enabled ? 0 : 1);
        }

        [OnEventFire]
        public void InitGameSettings(NodeAddedEvent evt, SingleNode<TargetFocusSettingsComponent> gameSettings)
        {
            gameSettings.component.Enabled = this.IsTargetFocusEnabledInPlayerPrefs();
        }

        private bool IsTargetFocusEnabledInPlayerPrefs()
        {
            if (PlayerPrefs.HasKey(TARGET_FOCUS_ENABLED_KEY))
            {
                return (PlayerPrefs.GetInt(TARGET_FOCUS_ENABLED_KEY) > 0);
            }
            PlayerPrefs.SetInt(TARGET_FOCUS_ENABLED_KEY, TARGET_FOCUS_ENABLED_DEFAULT_VALUE);
            return true;
        }
    }
}

