namespace Tanks.Lobby.ClientNavigation.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;

    public class HistoryComponent : Component
    {
        public Stack<ShowScreenData> screens = new Stack<ShowScreenData>();
    }
}

