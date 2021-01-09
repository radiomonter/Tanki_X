namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class HangarCameraDecelerationRotateComponent : Component
    {
        public float Speed { get; set; }

        public int LastUpdateFrame { get; set; }
    }
}

