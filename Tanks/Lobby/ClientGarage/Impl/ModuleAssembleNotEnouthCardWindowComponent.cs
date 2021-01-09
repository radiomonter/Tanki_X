namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ModuleAssembleNotEnouthCardWindowComponent : Component
    {
        public ModuleAssembleNotEnouthCardWindowComponent(int tier)
        {
            this.Tier = tier;
        }

        public int Tier { get; private set; }
    }
}

