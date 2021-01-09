namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class TutorialRequiredCompletedTutorialsComponent : Component
    {
        public List<long> TutorialsIds { get; set; }
    }
}

