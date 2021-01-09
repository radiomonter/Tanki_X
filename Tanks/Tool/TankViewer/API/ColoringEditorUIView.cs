namespace Tanks.Tool.TankViewer.API
{
    using System;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public class ColoringEditorUIView : MonoBehaviour
    {
        public CreatorView creatorView;
        public ViewerView viewerView;

        public void SwitchToEditor(ColoringComponent coloringComponent)
        {
            this.creatorView.colorView.SetColor((Color) coloringComponent.color);
            this.creatorView.textureView.SetAlphaMode(coloringComponent.coloringTextureAlphaMode);
            this.creatorView.textureView.textureDropdown.value = 0;
            this.creatorView.normalMapView.SetNormalScale(coloringComponent.coloringNormalScale);
            this.creatorView.normalMapView.normalMapDropdown.value = 0;
            this.creatorView.intensityThresholdView.SetUseIntensityThreshold(coloringComponent.useColoringIntensityThreshold);
            this.creatorView.intensityThresholdView.SetIntensityThreshold(coloringComponent.coloringMaskThreshold);
            this.creatorView.smoothnessView.SetOverrideSmoothness(coloringComponent.overwriteSmoothness);
            this.creatorView.smoothnessView.SetSmoothnessStrenght(coloringComponent.smoothnessStrength);
            this.creatorView.metallicView.SetFloat(coloringComponent.metallic);
            this.creatorView.gameObject.SetActive(true);
            this.viewerView.gameObject.SetActive(false);
        }

        public void SwitchToViewer()
        {
            this.creatorView.gameObject.SetActive(false);
            this.viewerView.gameObject.SetActive(true);
        }
    }
}

