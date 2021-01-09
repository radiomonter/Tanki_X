namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15f9d614c25L)]
    public class ShareEnergyEvent : Event
    {
        public long ReceiverId { get; set; }
    }
}

