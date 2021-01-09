namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class MapNativeSoundsComponent : Component
    {
        public MapNativeSoundsComponent(MapNativeSoundsBehaviour mapNativeSounds)
        {
            this.MapNativeSounds = mapNativeSounds;
        }

        public MapNativeSoundsBehaviour MapNativeSounds { get; set; }
    }
}

