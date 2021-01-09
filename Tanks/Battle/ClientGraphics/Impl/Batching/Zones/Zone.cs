namespace Tanks.Battle.ClientGraphics.Impl.Batching.Zones
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public sealed class Zone
    {
        public List<Submesh> contents;
        public Bounds bounds;

        public Material material =>
            this.contents[0].material;

        public int lightmapIndex =>
            this.contents[0].lightmapIndex;
    }
}

