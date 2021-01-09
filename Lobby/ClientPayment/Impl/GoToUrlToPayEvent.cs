namespace Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15281c7455bL)]
    public class GoToUrlToPayEvent : Event
    {
        public string Url { get; set; }
    }
}

