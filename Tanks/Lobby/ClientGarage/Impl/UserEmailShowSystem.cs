namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class UserEmailShowSystem : ECSSystem
    {
        [OnEventFire]
        public void InitButton(NodeAddedEvent e, SingleNode<UserEmailShowButtonComponent> button, SingleNode<UserEmailUIComponent> userEmailUiComponent)
        {
            PlayerPrefs.SetInt("UserEmailIsVisibile", 0);
            button.component.SetEyeColor(this.UserEmailIsVisibile());
            userEmailUiComponent.component.EmailIsVisible = this.UserEmailIsVisibile();
        }

        [OnEventFire]
        public void SwitchEmailVisibility(ButtonClickEvent e, SingleNode<UserEmailShowButtonComponent> button, [JoinAll] SingleNode<UserEmailUIComponent> userEmailUiComponent)
        {
            PlayerPrefs.SetInt("UserEmailIsVisibile", !this.UserEmailIsVisibile() ? 1 : 0);
            button.component.SetEyeColor(this.UserEmailIsVisibile());
            userEmailUiComponent.component.EmailIsVisible = this.UserEmailIsVisibile();
        }

        private bool UserEmailIsVisibile() => 
            PlayerPrefs.GetInt("UserEmailIsVisibile", 1) == 1;
    }
}

