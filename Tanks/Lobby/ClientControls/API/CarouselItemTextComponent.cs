﻿namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class CarouselItemTextComponent : Component
    {
        public string LocalizedCaption { get; set; }
    }
}
