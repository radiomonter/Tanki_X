namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class SlotsTextsComponent : Component
    {
        public Dictionary<ModuleBehaviourType, string> Slot2modules { get; set; }
    }
}

