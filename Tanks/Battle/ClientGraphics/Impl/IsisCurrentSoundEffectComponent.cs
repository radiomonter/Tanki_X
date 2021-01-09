namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d2e6e11856ae4aL)]
    public class IsisCurrentSoundEffectComponent : Component
    {
        public IsisCurrentSoundEffectComponent()
        {
            this.WasStarted = false;
            this.WasStopped = false;
        }

        public Tanks.Battle.ClientGraphics.Impl.SoundController SoundController { get; set; }

        public bool WasStarted { get; set; }

        public bool WasStopped { get; set; }
    }
}

