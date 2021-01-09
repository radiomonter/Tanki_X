﻿namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class SetPriceEvent : Event
    {
        public long OldPrice { get; set; }

        public long Price { get; set; }

        public long OldXPrice { get; set; }

        public long XPrice { get; set; }
    }
}

