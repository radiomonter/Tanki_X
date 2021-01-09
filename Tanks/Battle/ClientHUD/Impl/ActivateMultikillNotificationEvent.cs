namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ActivateMultikillNotificationEvent : Event
    {
        public int Score { get; set; }

        public int Kills { get; set; }

        public string UserName { get; set; }
    }
}

