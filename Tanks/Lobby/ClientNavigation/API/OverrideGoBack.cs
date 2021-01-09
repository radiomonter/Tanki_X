namespace Tanks.Lobby.ClientNavigation.API
{
    using System;
    using UnityEngine;

    public abstract class OverrideGoBack : MonoBehaviour
    {
        protected OverrideGoBack()
        {
        }

        public abstract ShowScreenData ScreenData { get; }
    }
}

