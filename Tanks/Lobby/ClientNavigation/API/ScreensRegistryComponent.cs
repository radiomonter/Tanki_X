namespace Tanks.Lobby.ClientNavigation.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;

    public class ScreensRegistryComponent : Component
    {
        public List<GameObject> screens = new List<GameObject>();
    }
}

