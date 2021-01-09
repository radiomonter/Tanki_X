namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class TutorialHighlightTankStepDataComponent : Component
    {
        public bool HighlightHull { get; set; }

        public bool HighlightWeapon { get; set; }
    }
}

