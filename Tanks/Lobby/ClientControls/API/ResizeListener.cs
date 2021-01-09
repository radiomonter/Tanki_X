namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;

    public abstract class ResizeListener : MonoBehaviour
    {
        protected ResizeListener()
        {
        }

        public abstract void OnResize(RectTransform source);
    }
}

