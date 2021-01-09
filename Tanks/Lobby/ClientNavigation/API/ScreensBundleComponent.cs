namespace Tanks.Lobby.ClientNavigation.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class ScreensBundleComponent : BehaviourComponent
    {
        [HideInInspector]
        private ScreenComponent[] screens;

        private void Awake()
        {
            foreach (ScreenComponent component in this.Screens)
            {
                if (component.gameObject.activeSelf)
                {
                    Debug.LogError("Screen is Active " + component.name + ". Disable it in scene!");
                    component.gameObject.SetActive(false);
                }
            }
        }

        public IEnumerable<ScreenComponent> Screens
        {
            get
            {
                this.screens ??= base.GetComponentsInChildren<ScreenComponent>(true);
                return this.screens;
            }
        }

        public Dialogs60Component Dialogs60 =>
            base.GetComponentInChildren<Dialogs60Component>(true);
    }
}

