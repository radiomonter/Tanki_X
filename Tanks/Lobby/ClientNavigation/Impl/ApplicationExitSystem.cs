namespace Tanks.Lobby.ClientNavigation.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class ApplicationExitSystem : ECSSystem
    {
        [OnEventFire]
        public void ExitApplication(ButtonClickEvent e, SingleNode<ExitButtonComponent> node)
        {
            Application.Quit();
        }
    }
}

