namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class RegisterScreensEvent : Event
    {
        public IEnumerable<GameObject> Screens { get; set; }
    }
}

