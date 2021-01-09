﻿namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, AddComponentMenu("Image Effects/Color Adjustments/Grayscale")]
    public class Grayscale : ImageEffectBase
    {
        public Texture textureRamp;
        public float rampOffset;

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            base.material.SetTexture("_RampTex", this.textureRamp);
            base.material.SetFloat("_RampOffset", this.rampOffset);
            Graphics.Blit(source, destination, base.material);
        }
    }
}

