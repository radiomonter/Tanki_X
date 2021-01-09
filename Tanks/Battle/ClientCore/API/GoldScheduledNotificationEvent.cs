namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x14cfede1f2fL)]
    public class GoldScheduledNotificationEvent : Event
    {
        public string Sender { get; set; }
    }
}

