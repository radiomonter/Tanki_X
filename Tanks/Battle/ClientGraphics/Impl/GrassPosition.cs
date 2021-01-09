namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct GrassPosition
    {
        public Vector3 position;
        public Vector2 lightmapUV;
        public int lightmapId;
    }
}

