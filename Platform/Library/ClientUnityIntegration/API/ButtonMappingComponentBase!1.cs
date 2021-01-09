namespace Platform.Library.ClientUnityIntegration.API
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(UnityEngine.UI.Button))]
    public abstract class ButtonMappingComponentBase<T> : EventMappingComponent where T: Event, new()
    {
        private UnityEngine.UI.Button button;

        protected ButtonMappingComponentBase()
        {
        }

        protected override void Subscribe()
        {
            this.Button.onClick.AddListener(() => this.SendEvent<T>());
        }

        public UnityEngine.UI.Button Button
        {
            get
            {
                if (this.button == null)
                {
                    this.button = base.GetComponent<UnityEngine.UI.Button>();
                }
                return this.button;
            }
        }
    }
}

