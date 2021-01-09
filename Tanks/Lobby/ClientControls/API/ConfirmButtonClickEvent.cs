namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ConfirmButtonClickEvent : Event
    {
        public long MissingCrystalsAmount { get; set; }

        public long MissingXCrystalsAmount { get; set; }
    }
}

