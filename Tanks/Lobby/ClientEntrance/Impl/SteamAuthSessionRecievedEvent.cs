namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x15def1520d5L)]
    public class SteamAuthSessionRecievedEvent : Event
    {
        public bool GoToPayment { get; set; }
    }
}

