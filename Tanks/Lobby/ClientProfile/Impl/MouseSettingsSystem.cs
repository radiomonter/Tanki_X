namespace Tanks.Lobby.ClientProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class MouseSettingsSystem : ECSSystem
    {
        private static readonly string MOUSE_CONTROL_ALLOWED_PP_KEY = "MOUSE_CONTROL_ALLOWED";
        private static readonly int MOUSE_CONTROL_ALLOWED_DEFAULT_VALUE = 1;
        private static readonly string MOUSE_VERTICAL_INVERTED_PP_KEY = "MOUSE_VERTICAL_INVERTED";
        private static readonly int MOUSE_VERTICAL_INVERTED_DEFAULT_VALUE;
        private static readonly string MOUSE_SENSIVITY_PP_KEY = "MOUSE_SENSIVITY";
        private static readonly float MOUSE_SENSIVITY_DEFAULT_VALUE = 0.5f;

        [OnEventFire]
        public void GameSettingsChanged(SettingsChangedEvent<GameMouseSettingsComponent> e, Node any)
        {
            PlayerPrefs.SetInt(MOUSE_CONTROL_ALLOWED_PP_KEY, !e.Data.MouseControlAllowed ? 0 : 1);
            PlayerPrefs.SetInt(MOUSE_VERTICAL_INVERTED_PP_KEY, !e.Data.MouseVerticalInverted ? 0 : 1);
            PlayerPrefs.SetFloat(MOUSE_SENSIVITY_PP_KEY, e.Data.MouseSensivity);
        }

        [OnEventFire]
        public void InitGameSettings(NodeAddedEvent e, SingleNode<GameMouseSettingsComponent> gameSettings)
        {
            gameSettings.component.MouseControlAllowed = PlayerPrefs.GetInt(MOUSE_CONTROL_ALLOWED_PP_KEY, MOUSE_CONTROL_ALLOWED_DEFAULT_VALUE) > 0;
            gameSettings.component.MouseVerticalInverted = PlayerPrefs.GetInt(MOUSE_VERTICAL_INVERTED_PP_KEY, MOUSE_VERTICAL_INVERTED_DEFAULT_VALUE) > 0;
            gameSettings.component.MouseSensivity = PlayerPrefs.GetFloat(MOUSE_SENSIVITY_PP_KEY, MOUSE_SENSIVITY_DEFAULT_VALUE);
        }

        [OnEventFire]
        public void SetDefaultMouseSettings(SetDefaultControlSettingsEvent e, Node any, [JoinAll] SingleNode<GameMouseSettingsComponent> settings)
        {
            settings.component.MouseControlAllowed = MOUSE_CONTROL_ALLOWED_DEFAULT_VALUE == 1;
            settings.component.MouseVerticalInverted = MOUSE_VERTICAL_INVERTED_DEFAULT_VALUE == 1;
            settings.component.MouseSensivity = MOUSE_SENSIVITY_DEFAULT_VALUE;
            base.ScheduleEvent(new SettingsChangedEvent<GameMouseSettingsComponent>(settings.component), settings);
        }
    }
}

