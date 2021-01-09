namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ScreenRangeChangedEvent : Event
    {
        public ScreenRangeChangedEvent(IndexRange range)
        {
            this.Range = range;
        }

        public IndexRange Range { get; set; }
    }
}

