namespace Tanks.Lobby.ClientEntrance.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ChangeUserMoneyBufferEvent : Event
    {
        public int Crystals { get; set; }

        public int XCrystals { get; set; }
    }
}

