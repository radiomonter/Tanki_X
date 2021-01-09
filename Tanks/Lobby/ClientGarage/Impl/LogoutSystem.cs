namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;

    public class LogoutSystem : ECSSystem
    {
        [OnEventFire]
        public void Logout(DialogConfirmEvent e, LogoutWindowNode node)
        {
            PlayerPrefs.DeleteKey("TOToken");
            PlayerPrefs.SetInt("SteamAuthentication", 1);
            PlayerPrefs.SetInt("RemeberMeFlag", 0);
            base.ScheduleEvent<SwitchToEntranceSceneEvent>(node);
        }

        [OnEventFire]
        public void ShowLogoutConfirmDialog(ButtonClickEvent e, SingleNode<LogoutButtonComponent> button, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            LogoutConfirmWindowComponent component = dialogs.component.Get<LogoutConfirmWindowComponent>();
            List<Animator> animators = new List<Animator>();
            if (screens.IsPresent())
            {
                animators = screens.Get().component.Animators;
            }
            component.Show(animators);
        }

        public class LogoutWindowNode : Node
        {
            public LogoutConfirmWindowComponent logoutConfirmWindow;
        }

        public class ProfileAccountSectionNode : Node
        {
            public ProfileAccountSectionUIComponent profileAccountSectionUI;
        }
    }
}

