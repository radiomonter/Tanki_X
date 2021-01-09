namespace tanks.modules.tool.TankViewer.Scripts.API.ColoringEditor.ParamView
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class MetallicView : MonoBehaviour
    {
        public Slider slider;

        public float GetFloat() => 
            this.slider.value;

        public void SetFloat(float value)
        {
            this.slider.value = value;
        }
    }
}

