namespace Tanks.Lobby.ClientNavigation.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using UnityEngine;

    public class ScreensLayerComponent : MonoBehaviour, Component
    {
        public RectTransform selfRectTransform;
        public RectTransform dialogsLayer;
        public RectTransform dialogs60Layer;
        public RectTransform screensLayer;
        public RectTransform screens60Layer;
    }
}

