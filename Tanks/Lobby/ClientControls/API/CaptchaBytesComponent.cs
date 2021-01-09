namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class CaptchaBytesComponent : Component
    {
        public byte[] bytes;

        public CaptchaBytesComponent()
        {
        }

        public CaptchaBytesComponent(byte[] bytes)
        {
            this.bytes = bytes;
        }
    }
}

