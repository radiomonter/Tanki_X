namespace Tanks.Lobby.ClientProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class TanksSettingsSystem : ECSSystem
    {
        private const string MOVEMENT_CONTROL_INVERTED_PP_KEY = "MOVEMENT_CONTROL_INVERTED";
        private const int MOVEMENT_CONTROL_INVERTED_DEFAULT_VALUE = 0;
        private const string DAMAGE_INFO_ENABLED_PP_KEY = "DAMAGE_INFO_ENABLED";
        private const int DAMAGE_INFO_ENABLED_DEFAULT_VALUE = 1;
        private const string HEALTH_FEEDBACK_ENABLED_PP_KEY = "LOW_HEALTH_FEEDBACK_ENABLED";
        private const int HEALTH_FEEDBACK_ENABLED_DEFAULT_VALUE = 1;
        private const string SELF_TARGET_HIT_FEEDBACK_ENABLED_PP_KEY = "SELF_TARGET_HIT_FEEDBACK_ENABLED";
        private const int SELF_TARGET_HIT_FEEDBACK_ENABLED_DEFAULT_VALUE = 1;

        [OnEventFire]
        public void GameSettingsChanged(SettingsChangedEvent<GameTankSettingsComponent> e, Node any, [JoinAll] SingleNode<FeedbackGraphicsRestrictionsComponent> quality)
        {
            PlayerPrefs.SetInt("MOVEMENT_CONTROL_INVERTED", !e.Data.MovementControlsInverted ? 0 : 1);
            PlayerPrefs.SetInt("DAMAGE_INFO_ENABLED", !e.Data.DamageInfoEnabled ? 0 : 1);
            if (quality.component.SelfTargetHitFeedbackAllowed)
            {
                PlayerPrefs.SetInt("SELF_TARGET_HIT_FEEDBACK_ENABLED", !e.Data.SelfTargetHitFeedbackEnabled ? 0 : 1);
            }
            if (quality.component.HealthFeedbackAllowed)
            {
                PlayerPrefs.SetInt("LOW_HEALTH_FEEDBACK_ENABLED", !e.Data.HealthFeedbackEnabled ? 0 : 1);
            }
        }

        [OnEventFire]
        public void InitGameSettings(NodeAddedEvent e, SingleNode<GameTankSettingsComponent> gameSettings, SingleNode<FeedbackGraphicsRestrictionsComponent> qualitySettings)
        {
            gameSettings.component.MovementControlsInverted = PlayerPrefs.GetInt("MOVEMENT_CONTROL_INVERTED", 0) > 0;
            gameSettings.component.DamageInfoEnabled = PlayerPrefs.GetInt("DAMAGE_INFO_ENABLED", 1) > 0;
            gameSettings.component.HealthFeedbackEnabled = (PlayerPrefs.GetInt("LOW_HEALTH_FEEDBACK_ENABLED", 1) > 0) && qualitySettings.component.HealthFeedbackAllowed;
            gameSettings.component.SelfTargetHitFeedbackEnabled = (PlayerPrefs.GetInt("SELF_TARGET_HIT_FEEDBACK_ENABLED", 1) > 0) && qualitySettings.component.SelfTargetHitFeedbackAllowed;
        }

        [OnEventFire]
        public void SetDefaultMouseSettings(SetDefaultControlSettingsEvent e, Node any, [JoinAll] SingleNode<GameTankSettingsComponent> settings)
        {
            settings.component.MovementControlsInverted = false;
            base.ScheduleEvent(new SettingsChangedEvent<GameTankSettingsComponent>(settings.component), settings);
        }
    }
}

