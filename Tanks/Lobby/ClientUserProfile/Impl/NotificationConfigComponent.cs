namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d3864a1fd41798L)]
    public class NotificationConfigComponent : Component
    {
        public float ShowDuration { get; set; }

        public float ShowDelay { get; set; }

        public int Order { get; set; }

        public bool IsFullScreen { get; set; }
    }
}

