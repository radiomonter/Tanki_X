namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [StructLayout(LayoutKind.Sequential)]
    public struct DecalEntry
    {
        public GameObject gameObject;
        public Material material;
        public float timeToDestroy;
        public DecalEntryType type;
    }
}

