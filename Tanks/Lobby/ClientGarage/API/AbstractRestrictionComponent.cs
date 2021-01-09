namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public abstract class AbstractRestrictionComponent : Component
    {
        protected AbstractRestrictionComponent()
        {
        }

        public int RestrictionValue { get; set; }
    }
}

