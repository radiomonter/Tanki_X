﻿namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class CheckForQuickGameEvent : Event
    {
        public bool IsQuickGame { get; set; }
    }
}

