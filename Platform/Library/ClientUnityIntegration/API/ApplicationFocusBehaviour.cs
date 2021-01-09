namespace Platform.Library.ClientUnityIntegration.API
{
    using System;
    using UnityEngine;

    public class ApplicationFocusBehaviour : MonoBehaviour
    {
        public static ApplicationFocusBehaviour INSTANCE;
        private bool focused = true;

        private void Awake()
        {
            INSTANCE = this;
        }

        private void OnApplicationFocus(bool focused)
        {
            this.focused = focused;
        }

        public bool Focused =>
            this.focused;
    }
}

