namespace Tanks.Tool.TankViewer.API
{
    using System;
    using UnityEngine;

    public class MaskImageEffect : MonoBehaviour
    {
        public Shader shader;
        private Material material;

        private void Awake()
        {
            this.material = new Material(this.shader);
        }

        [ImageEffectOpaque]
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination, this.material);
        }
    }
}

