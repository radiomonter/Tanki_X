﻿namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, AddComponentMenu("Image Effects/Color Adjustments/Sepia Tone")]
    public class SepiaTone : ImageEffectBase
    {
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination, base.material);
        }
    }
}

