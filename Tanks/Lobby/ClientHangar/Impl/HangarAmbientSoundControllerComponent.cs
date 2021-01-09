namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class HangarAmbientSoundControllerComponent : Component
    {
        public HangarAmbientSoundControllerComponent(Tanks.Lobby.ClientHangar.Impl.HangarAmbientSoundController hangarAmbientSoundController)
        {
            this.HangarAmbientSoundController = hangarAmbientSoundController;
        }

        public Tanks.Lobby.ClientHangar.Impl.HangarAmbientSoundController HangarAmbientSoundController { get; set; }
    }
}

