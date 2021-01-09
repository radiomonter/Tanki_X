namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x154f16bff00L), Shared]
    public class NotificationComponent : Component, IComparable<NotificationComponent>
    {
        public int CompareTo(NotificationComponent other)
        {
            int num = other.Priority.CompareTo(this.Priority);
            return ((num != 0) ? num : this.TimeCreation.CompareTo(other.TimeCreation));
        }

        public NotificationPriority Priority { get; set; }

        public Date TimeCreation { get; set; }
    }
}

