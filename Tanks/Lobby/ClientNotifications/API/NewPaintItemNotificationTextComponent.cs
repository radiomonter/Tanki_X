namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class NewPaintItemNotificationTextComponent : Component
    {
        public string CoverText { get; set; }

        public string PaintText { get; set; }
    }
}

