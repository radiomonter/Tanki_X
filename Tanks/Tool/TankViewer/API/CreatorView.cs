namespace Tanks.Tool.TankViewer.API
{
    using System;
    using System.Collections.Generic;
    using tanks.modules.tool.TankViewer.Scripts.API.ColoringEditor.ParamView;
    using Tanks.Tool.TankViewer.API.Params;
    using UnityEngine;
    using UnityEngine.UI;

    public class CreatorView : MonoBehaviour
    {
        public ColorView colorView;
        public TextureView textureView;
        public NormalMapView normalMapView;
        public MetallicView metallicView;
        public SmoothnessView smoothnessView;
        public IntensityThresholdView intensityThresholdView;
        public Button saveButton;
        public Button cancelButton;
        public Button updateTexturesButton;

        public void Disable()
        {
            foreach (Selectable selectable in this.GetUIElements())
            {
                selectable.interactable = false;
            }
            this.normalMapView.Disable();
            this.textureView.Disable();
            this.smoothnessView.Disable();
            this.intensityThresholdView.Disable();
        }

        public void Enable()
        {
            foreach (Selectable selectable in this.GetUIElements())
            {
                selectable.interactable = true;
            }
            this.normalMapView.Enable();
            this.textureView.Enable();
            this.smoothnessView.Enable();
            this.intensityThresholdView.Enable();
        }

        private List<Selectable> GetUIElements() => 
            new List<Selectable> { 
                this.colorView.colorInput,
                this.metallicView.slider,
                this.saveButton,
                this.cancelButton,
                this.updateTexturesButton
            };
    }
}

