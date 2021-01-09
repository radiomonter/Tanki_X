namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x15e2d5c094cL)]
    public class TutorialConfigurationComponent : Component
    {
        public List<string> Tutorials { get; set; }
    }
}

