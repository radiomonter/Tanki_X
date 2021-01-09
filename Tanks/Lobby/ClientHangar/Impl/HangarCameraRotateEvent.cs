namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class HangarCameraRotateEvent : Event
    {
        public HangarCameraRotateEvent()
        {
        }

        public HangarCameraRotateEvent(float angle)
        {
            this.Angle = angle;
        }

        public float Angle { get; set; }
    }
}

