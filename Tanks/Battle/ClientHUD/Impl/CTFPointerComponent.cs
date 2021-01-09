namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class CTFPointerComponent : MonoBehaviour, Component
    {
        public RectTransform parentCanvasRect;
        public RectTransform selfRect;
        public CanvasGroup canvasGroup;
        public Text text;

        public void Hide()
        {
            this.canvasGroup.alpha = 0f;
        }

        private void OnDisable()
        {
            this.Hide();
        }

        public void Show()
        {
            this.canvasGroup.alpha = 1f;
        }
    }
}

