namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class SetCarouselItemIndexEvent : Event
    {
        public SetCarouselItemIndexEvent(int index)
        {
            this.Index = index;
        }

        public int Index { get; set; }
    }
}

