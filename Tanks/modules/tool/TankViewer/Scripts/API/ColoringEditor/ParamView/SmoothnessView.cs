namespace tanks.modules.tool.TankViewer.Scripts.API.ColoringEditor.ParamView
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class SmoothnessView : MonoBehaviour
    {
        public Toggle toggle;
        public Slider slider;

        public void Disable()
        {
            this.toggle.interactable = false;
            this.slider.interactable = false;
        }

        public void Enable()
        {
            this.toggle.interactable = true;
            this.slider.interactable = this.toggle.isOn;
        }

        public bool GetOverrideSmoothness() => 
            this.toggle.isOn;

        public float GetSmoothnessStrenght() => 
            this.slider.value;

        public void OnToggleChanged()
        {
            this.Enable();
        }

        public void SetOverrideSmoothness(bool value)
        {
            this.toggle.isOn = value;
            if (this.toggle.interactable)
            {
                this.Enable();
            }
        }

        public void SetSmoothnessStrenght(float value)
        {
            this.slider.value = value;
        }
    }
}

