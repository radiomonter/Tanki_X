namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.API;

    public class CheckMountedModuleEvent : Event
    {
        public long StepId { get; set; }

        public long ItemId { get; set; }

        public Slot MountSlot { get; set; }

        public bool ModuleMounted { get; set; }
    }
}

