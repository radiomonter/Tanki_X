namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [StructLayout(LayoutKind.Sequential)]
    public struct TrackSector
    {
        public Vector3 startPosition;
        public Vector3 startForward;
        public Vector3 endPosition;
        public Vector3 endForward;
        public Vector3 normal;
        public float width;
        public float textureWidth;
        public float rotationCos;
        public bool contiguous;
    }
}

