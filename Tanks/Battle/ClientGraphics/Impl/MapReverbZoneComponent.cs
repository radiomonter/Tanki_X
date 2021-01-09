namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class MapReverbZoneComponent : Component
    {
        public MapReverbZoneComponent(GameObject reverbZoneRoot)
        {
            this.ReverbZoneRoot = reverbZoneRoot;
        }

        public GameObject ReverbZoneRoot { get; set; }
    }
}

