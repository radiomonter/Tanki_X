namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class EmailConfirmationNotificationComponent : Component
    {
        public string ConfirmationMessageTemplate { get; set; }

        public string ChangeEmailMessageTemplate { get; set; }
    }
}

