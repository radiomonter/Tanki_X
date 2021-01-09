namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class CarouselCurrentItemIndexComponent : Component
    {
        public CarouselCurrentItemIndexComponent(int index)
        {
            this.Index = index;
        }

        public int Index { get; set; }
    }
}

