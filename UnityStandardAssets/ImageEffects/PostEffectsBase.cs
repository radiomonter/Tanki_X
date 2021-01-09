namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, RequireComponent(typeof(Camera))]
    public class PostEffectsBase : MonoBehaviour
    {
        protected bool supportHDRTextures = true;
        protected bool supportDX11;
        protected bool isSupported = true;

        public virtual bool CheckResources()
        {
            Debug.LogWarning("CheckResources () for " + this.ToString() + " should be overwritten.");
            return this.isSupported;
        }

        private bool CheckShader(Shader s)
        {
            string[] textArray1 = new string[] { "The shader ", s.ToString(), " on effect ", this.ToString(), " is not part of the Unity 3.2+ effects suite anymore. For best performance and quality, please ensure you are using the latest Standard Assets Image Effects (Pro only) package." };
            Debug.Log(string.Concat(textArray1));
            if (!s.isSupported)
            {
                this.NotSupported();
            }
            return false;
        }

        protected Material CheckShaderAndCreateMaterial(Shader s, Material m2Create)
        {
            if (!s)
            {
                Debug.Log("Missing shader in " + this.ToString());
                base.enabled = false;
                return null;
            }
            if (s.isSupported && (m2Create && (m2Create.shader == s)))
            {
                return m2Create;
            }
            if (s.isSupported)
            {
                m2Create = new Material(s);
                m2Create.hideFlags = HideFlags.DontSave;
                return (!m2Create ? null : m2Create);
            }
            this.NotSupported();
            string[] textArray1 = new string[] { "The shader ", s.ToString(), " on effect ", this.ToString(), " is not supported on this platform!" };
            Debug.Log(string.Concat(textArray1));
            return null;
        }

        protected bool CheckSupport() => 
            this.CheckSupport(false);

        protected bool CheckSupport(bool needDepth)
        {
            this.isSupported = true;
            this.supportHDRTextures = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf);
            this.supportDX11 = (SystemInfo.graphicsShaderLevel >= 50) && SystemInfo.supportsComputeShaders;
            if (!SystemInfo.supportsImageEffects || !SystemInfo.supportsRenderTextures)
            {
                this.NotSupported();
                return false;
            }
            if (needDepth && !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
            {
                this.NotSupported();
                return false;
            }
            if (needDepth)
            {
                Camera component = base.GetComponent<Camera>();
                component.depthTextureMode |= DepthTextureMode.Depth;
            }
            return true;
        }

        protected bool CheckSupport(bool needDepth, bool needHdr)
        {
            if (this.CheckSupport(needDepth))
            {
                if (!needHdr || this.supportHDRTextures)
                {
                    return true;
                }
                this.NotSupported();
            }
            return false;
        }

        protected Material CreateMaterial(Shader s, Material m2Create)
        {
            if (!s)
            {
                Debug.Log("Missing shader in " + this.ToString());
                return null;
            }
            if (m2Create && ((m2Create.shader == s) && s.isSupported))
            {
                return m2Create;
            }
            if (!s.isSupported)
            {
                return null;
            }
            m2Create = new Material(s);
            m2Create.hideFlags = HideFlags.DontSave;
            return (!m2Create ? null : m2Create);
        }

        protected void DrawBorder(RenderTexture dest, Material material)
        {
            RenderTexture.active = dest;
            bool flag = true;
            GL.PushMatrix();
            GL.LoadOrtho();
            for (int i = 0; i < material.passCount; i++)
            {
                float num6;
                float num7;
                material.SetPass(i);
                if (flag)
                {
                    num6 = 1f;
                    num7 = 0f;
                }
                else
                {
                    num6 = 0f;
                    num7 = 1f;
                }
                float x = 0f;
                float num2 = 1f / (dest.width * 1f);
                float y = 0f;
                float num4 = 1f;
                GL.Begin(7);
                GL.TexCoord2(0f, num6);
                GL.Vertex3(x, y, 0.1f);
                GL.TexCoord2(1f, num6);
                GL.Vertex3(num2, y, 0.1f);
                GL.TexCoord2(1f, num7);
                GL.Vertex3(num2, num4, 0.1f);
                GL.TexCoord2(0f, num7);
                GL.Vertex3(x, num4, 0.1f);
                x = 1f - (1f / (dest.width * 1f));
                num2 = 1f;
                y = 0f;
                num4 = 1f;
                GL.TexCoord2(0f, num6);
                GL.Vertex3(x, y, 0.1f);
                GL.TexCoord2(1f, num6);
                GL.Vertex3(num2, y, 0.1f);
                GL.TexCoord2(1f, num7);
                GL.Vertex3(num2, num4, 0.1f);
                GL.TexCoord2(0f, num7);
                GL.Vertex3(x, num4, 0.1f);
                x = 0f;
                num2 = 1f;
                y = 0f;
                num4 = 1f / (dest.height * 1f);
                GL.TexCoord2(0f, num6);
                GL.Vertex3(x, y, 0.1f);
                GL.TexCoord2(1f, num6);
                GL.Vertex3(num2, y, 0.1f);
                GL.TexCoord2(1f, num7);
                GL.Vertex3(num2, num4, 0.1f);
                GL.TexCoord2(0f, num7);
                GL.Vertex3(x, num4, 0.1f);
                x = 0f;
                num2 = 1f;
                y = 1f - (1f / (dest.height * 1f));
                num4 = 1f;
                GL.TexCoord2(0f, num6);
                GL.Vertex3(x, y, 0.1f);
                GL.TexCoord2(1f, num6);
                GL.Vertex3(num2, y, 0.1f);
                GL.TexCoord2(1f, num7);
                GL.Vertex3(num2, num4, 0.1f);
                GL.TexCoord2(0f, num7);
                GL.Vertex3(x, num4, 0.1f);
                GL.End();
            }
            GL.PopMatrix();
        }

        public bool Dx11Support() => 
            this.supportDX11;

        protected void NotSupported()
        {
            base.enabled = false;
            this.isSupported = false;
        }

        private void OnEnable()
        {
            this.isSupported = true;
        }

        protected void ReportAutoDisable()
        {
            Debug.LogWarning("The image effect " + this.ToString() + " has been disabled as it's not supported on the current platform.");
        }

        protected void Start()
        {
            this.CheckResources();
        }
    }
}

