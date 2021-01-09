namespace Edelweiss.DecalSystem
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [StructLayout(LayoutKind.Sequential)]
    internal struct DecalRay
    {
        public Vector3 origin;
        public Vector3 direction;
        public DecalRay(Vector3 a_Origin, Vector3 a_Direction)
        {
            this.origin = a_Origin;
            this.direction = a_Direction;
        }
    }
}

