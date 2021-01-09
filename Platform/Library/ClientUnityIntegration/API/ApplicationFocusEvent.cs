﻿namespace Platform.Library.ClientUnityIntegration.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ApplicationFocusEvent : Event
    {
        public bool IsFocused { get; set; }
    }
}

