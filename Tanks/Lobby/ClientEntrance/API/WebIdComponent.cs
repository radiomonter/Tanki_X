namespace Tanks.Lobby.ClientEntrance.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x1588c2c1a9cL)]
    public class WebIdComponent : Component
    {
        public string WebId { get; set; }

        public string Utm { get; set; }

        public string GoogleAnalyticsId { get; set; }

        public string WebIdUid { get; set; }
    }
}

