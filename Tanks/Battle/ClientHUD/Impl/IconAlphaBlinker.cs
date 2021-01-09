namespace Tanks.Battle.ClientHUD.Impl
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(CanvasGroup))]
    public class IconAlphaBlinker : MonoBehaviour
    {
        private CanvasGroup icon;

        private void Awake()
        {
            this.icon = base.GetComponent<CanvasGroup>();
        }

        public void OnBlink(float value)
        {
            this.icon.alpha = value;
        }
    }
}

