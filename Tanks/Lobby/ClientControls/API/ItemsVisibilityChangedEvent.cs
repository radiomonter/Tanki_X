namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ItemsVisibilityChangedEvent : Event
    {
        public ItemsVisibilityChangedEvent(IndexRange prevRange, IndexRange range)
        {
            this.PrevRange = prevRange;
            this.Range = range;
        }

        public IndexRange PrevRange { get; set; }

        public IndexRange Range { get; set; }
    }
}

