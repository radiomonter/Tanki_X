namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class CheckForSkipTutorial : Event
    {
        public bool canSkipTutorial { get; set; }
    }
}

