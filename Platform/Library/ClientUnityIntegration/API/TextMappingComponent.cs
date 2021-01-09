namespace Platform.Library.ClientUnityIntegration.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class TextMappingComponent : MonoBehaviour, Component
    {
        private UnityEngine.UI.Text text;

        private UnityEngine.UI.Text TextComponent
        {
            get
            {
                if (this.text == null)
                {
                    this.text = base.GetComponent<UnityEngine.UI.Text>();
                }
                return this.text;
            }
        }

        public virtual string Text
        {
            get => 
                this.TextComponent.text;
            set => 
                this.TextComponent.text = value;
        }
    }
}

