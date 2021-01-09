namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class Slots2ModuleBehaviourTypesConfigComponent : Component
    {
        public Dictionary<Slot, ModuleBehaviourType> Slots { get; set; }
    }
}

