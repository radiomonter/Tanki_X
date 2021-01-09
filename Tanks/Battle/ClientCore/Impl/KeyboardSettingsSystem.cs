namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;

    public class KeyboardSettingsSystem : ECSSystem
    {
        [OnEventFire]
        public void ResetToDefault(ButtonClickEvent e, DefaultButtonNode button, [JoinByScreen] ScreenNode screen)
        {
            InputManager.ResetToDefaultActions();
            base.ScheduleEvent<SetDefaultControlSettingsEvent>(button);
            foreach (KeyboardSettingsInputComponent component in Object.FindObjectsOfType<KeyboardSettingsInputComponent>())
            {
                component.SetText();
            }
            screen.keyboardSettingsScreen.CheckForOneKeyOnFewActions();
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class DefaultButtonNode : Node
        {
            public DefaultButtonComponent defaultButton;
            public ScreenGroupComponent screenGroup;
        }

        public class ScreenNode : Node
        {
            public KeyboardSettingsScreenComponent keyboardSettingsScreen;
            public ScreenGroupComponent screenGroup;
        }
    }
}

