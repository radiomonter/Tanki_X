namespace Edelweiss.DecalSystem
{
    using System;
    using UnityEngine;

    public class Edition
    {
        public static bool IsDecalSystemFree =>
            false;

        public static bool IsDX11 =>
            SystemInfo.graphicsShaderLevel >= 0x29;
    }
}

