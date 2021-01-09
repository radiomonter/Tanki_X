namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(Camera)), AddComponentMenu("")]
    public class ImageEffectBase : MonoBehaviour
    {
        public Shader shader;
        private Material m_Material;

        protected virtual void OnDisable()
        {
            if (this.m_Material)
            {
                DestroyImmediate(this.m_Material);
            }
        }

        protected virtual void Start()
        {
            if (!SystemInfo.supportsImageEffects)
            {
                base.enabled = false;
            }
            else if (!this.shader || !this.shader.isSupported)
            {
                base.enabled = false;
            }
        }

        protected Material material
        {
            get
            {
                if (this.m_Material == null)
                {
                    this.m_Material = new Material(this.shader);
                    this.m_Material.hideFlags = HideFlags.HideAndDontSave;
                }
                return this.m_Material;
            }
        }
    }
}

