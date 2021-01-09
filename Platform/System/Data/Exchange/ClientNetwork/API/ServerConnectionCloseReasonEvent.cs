namespace Platform.System.Data.Exchange.ClientNetwork.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ServerConnectionCloseReasonEvent : Event
    {
        public string Reason { get; set; }
    }
}

