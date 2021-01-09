namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x1619372c1abL)]
    public class EnergyCompensationNotificationComponent : Component
    {
        public long Charges { get; set; }

        public long Crys { get; set; }
    }
}

