namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, AddComponentMenu("Image Effects/Blur/Blur")]
    public class Blur : MonoBehaviour
    {
        public int iterations = 3;
        public float blurSpread = 0.6f;
        public Shader blurShader;
        private static Material m_Material;

        private void DownSample4x(RenderTexture source, RenderTexture dest)
        {
            float y = 1f;
            Vector2[] offsets = new Vector2[] { new Vector2(-y, -y), new Vector2(-y, y), new Vector2(y, y), new Vector2(y, -y) };
            Graphics.BlitMultiTap(source, dest, this.material, offsets);
        }

        public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
        {
            float y = 0.5f + (iteration * this.blurSpread);
            Vector2[] offsets = new Vector2[] { new Vector2(-y, -y), new Vector2(-y, y), new Vector2(y, y), new Vector2(y, -y) };
            Graphics.BlitMultiTap(source, dest, this.material, offsets);
        }

        protected void OnDisable()
        {
            if (m_Material)
            {
                DestroyImmediate(m_Material);
            }
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            int width = source.width / 4;
            int height = source.height / 4;
            RenderTexture dest = RenderTexture.GetTemporary(width, height, 0);
            this.DownSample4x(source, dest);
            for (int i = 0; i < this.iterations; i++)
            {
                RenderTexture texture2 = RenderTexture.GetTemporary(width, height, 0);
                this.FourTapCone(dest, texture2, i);
                RenderTexture.ReleaseTemporary(dest);
                dest = texture2;
            }
            Graphics.Blit(dest, destination);
            RenderTexture.ReleaseTemporary(dest);
        }

        protected void Start()
        {
            if (!SystemInfo.supportsImageEffects)
            {
                base.enabled = false;
            }
            else if (!this.blurShader || !this.material.shader.isSupported)
            {
                base.enabled = false;
            }
        }

        protected Material material
        {
            get
            {
                if (m_Material == null)
                {
                    m_Material = new Material(this.blurShader);
                    m_Material.hideFlags = HideFlags.DontSave;
                }
                return m_Material;
            }
        }
    }
}

