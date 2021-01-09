namespace Tanks.Lobby.ClientEntrance.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class RanksNamesComponent : Component
    {
        public string[] Names { get; set; }
    }
}

