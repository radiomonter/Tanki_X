namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ModuleTypesImagesComponent : Component
    {
        public Dictionary<ModuleBehaviourType, string> moduleType2image { get; set; }

        public Dictionary<ModuleBehaviourType, string> moduleType2color { get; set; }
    }
}

