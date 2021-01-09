namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, AddComponentMenu("Image Effects/Displacement/Twirl")]
    public class Twirl : ImageEffectBase
    {
        public Vector2 radius = new Vector2(0.3f, 0.3f);
        public float angle = 50f;
        public Vector2 center = new Vector2(0.5f, 0.5f);

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            UnityStandardAssets.ImageEffects.ImageEffects.RenderDistortion(base.material, source, destination, this.angle, this.center, this.radius);
        }
    }
}

