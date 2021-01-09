namespace CurvedUI
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [StructLayout(LayoutKind.Sequential)]
    public struct ViveInputArgs
    {
        public uint controllerIndex;
        public float buttonPressure;
        public Vector2 touchpadAxis;
    }
}

