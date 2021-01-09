namespace tanks.modules.tool.TankViewer.Scripts.API.ColoringEditor.ParamView
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class IntensityThresholdView : MonoBehaviour
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

        public float GetIntensityThreshold() => 
            this.slider.value;

        public bool GetUseIntensityThreshold() => 
            this.toggle.isOn;

        public void OnToggleChanged()
        {
            this.Enable();
        }

        public void SetIntensityThreshold(float value)
        {
            this.slider.value = value;
        }

        public void SetUseIntensityThreshold(bool value)
        {
            this.toggle.isOn = value;
        }
    }
}

