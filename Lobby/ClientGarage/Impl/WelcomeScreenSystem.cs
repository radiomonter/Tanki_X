namespace Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;

    public class WelcomeScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void ButtonClick(ButtonClickEvent e, SingleNode<WelcomeScreenCloseButton> closeButton, [JoinAll] SingleNode<Dialogs60Component> dialogs)
        {
            dialogs.component.Get<WelcomeScreenDialog>().Hide();
        }
    }
}

