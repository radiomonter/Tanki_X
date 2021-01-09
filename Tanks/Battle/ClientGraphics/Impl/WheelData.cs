namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [StructLayout(LayoutKind.Sequential)]
    public struct WheelData
    {
        public Vector3 position;
        public Vector3 right;
    }
}

