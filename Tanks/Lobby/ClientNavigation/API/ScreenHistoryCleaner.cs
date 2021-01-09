namespace Tanks.Lobby.ClientNavigation.API
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class ScreenHistoryCleaner : MonoBehaviour
    {
        protected ScreenHistoryCleaner()
        {
        }

        public abstract void ClearHistory(Stack<ShowScreenData> history);
    }
}

