﻿namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class LocalizedTextComponent : Component
    {
        public string Text { get; set; }
    }
}

