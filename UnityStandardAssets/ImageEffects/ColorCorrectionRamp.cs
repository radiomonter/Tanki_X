﻿namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, AddComponentMenu("Image Effects/Color Adjustments/Color Correction (Ramp)")]
    public class ColorCorrectionRamp : ImageEffectBase
    {
        public Texture textureRamp;

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            base.material.SetTexture("_RampTex", this.textureRamp);
            Graphics.Blit(source, destination, base.material);
        }
    }
}

