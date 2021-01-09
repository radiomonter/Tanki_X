namespace Tanks.Lobby.ClientNavigation.API
{
    using System;
    using System.Collections.Generic;

    public class HomeScreenHistoryCleaner : ScreenHistoryCleaner
    {
        public override void ClearHistory(Stack<ShowScreenData> history)
        {
            foreach (ShowScreenData data in history)
            {
                data.FreeContext();
            }
            history.Clear();
        }
    }
}

