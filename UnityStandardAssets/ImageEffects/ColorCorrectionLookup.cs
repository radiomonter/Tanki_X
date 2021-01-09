namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, AddComponentMenu("Image Effects/Color Adjustments/Color Correction (3D Lookup Texture)")]
    public class ColorCorrectionLookup : PostEffectsBase
    {
        public Shader shader;
        private Material material;
        public Texture3D converted3DLut;
        public string basedOnTempTex = string.Empty;

        public override bool CheckResources()
        {
            base.CheckSupport(false);
            this.material = base.CheckShaderAndCreateMaterial(this.shader, this.material);
            if (!base.isSupported || !SystemInfo.supports3DTextures)
            {
                base.ReportAutoDisable();
            }
            return base.isSupported;
        }

        public void Convert(Texture2D temp2DTex, string path)
        {
            if (!temp2DTex)
            {
                Debug.LogError("Couldn't color correct with 3D LUT texture. Image Effect will be disabled.");
            }
            else
            {
                int width = temp2DTex.width * temp2DTex.height;
                width = temp2DTex.height;
                if (!this.ValidDimensions(temp2DTex))
                {
                    Debug.LogWarning("The given 2D texture " + temp2DTex.name + " cannot be used as a 3D LUT.");
                    this.basedOnTempTex = string.Empty;
                }
                else
                {
                    Color[] pixels = temp2DTex.GetPixels();
                    Color[] colors = new Color[pixels.Length];
                    int num2 = 0;
                    while (true)
                    {
                        if (num2 >= width)
                        {
                            if (this.converted3DLut)
                            {
                                DestroyImmediate(this.converted3DLut);
                            }
                            this.converted3DLut = new Texture3D(width, width, width, TextureFormat.ARGB32, false);
                            this.converted3DLut.SetPixels(colors);
                            this.converted3DLut.Apply();
                            this.basedOnTempTex = path;
                            break;
                        }
                        int num3 = 0;
                        while (true)
                        {
                            if (num3 >= width)
                            {
                                num2++;
                                break;
                            }
                            int num4 = 0;
                            while (true)
                            {
                                if (num4 >= width)
                                {
                                    num3++;
                                    break;
                                }
                                int num5 = (width - num3) - 1;
                                colors[(num2 + (num3 * width)) + ((num4 * width) * width)] = pixels[((num4 * width) + num2) + ((num5 * width) * width)];
                                num4++;
                            }
                        }
                    }
                }
            }
        }

        private void OnDestroy()
        {
            if (this.converted3DLut)
            {
                DestroyImmediate(this.converted3DLut);
            }
            this.converted3DLut = null;
        }

        private void OnDisable()
        {
            if (this.material)
            {
                DestroyImmediate(this.material);
                this.material = null;
            }
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!this.CheckResources() || !SystemInfo.supports3DTextures)
            {
                Graphics.Blit(source, destination);
            }
            else
            {
                if (this.converted3DLut == null)
                {
                    this.SetIdentityLut();
                }
                int width = this.converted3DLut.width;
                this.converted3DLut.wrapMode = TextureWrapMode.Clamp;
                this.material.SetFloat("_Scale", ((float) (width - 1)) / (1f * width));
                this.material.SetFloat("_Offset", 1f / (2f * width));
                this.material.SetTexture("_ClutTex", this.converted3DLut);
                Graphics.Blit(source, destination, this.material, (QualitySettings.activeColorSpace != ColorSpace.Linear) ? 0 : 1);
            }
        }

        public void SetIdentityLut()
        {
            int width = 0x10;
            Color[] colors = new Color[(width * width) * width];
            float num2 = 1f / ((1f * width) - 1f);
            int num3 = 0;
            while (num3 < width)
            {
                int num4 = 0;
                while (true)
                {
                    if (num4 >= width)
                    {
                        num3++;
                        break;
                    }
                    int num5 = 0;
                    while (true)
                    {
                        if (num5 >= width)
                        {
                            num4++;
                            break;
                        }
                        colors[(num3 + (num4 * width)) + ((num5 * width) * width)] = new Color((num3 * 1f) * num2, (num4 * 1f) * num2, (num5 * 1f) * num2, 1f);
                        num5++;
                    }
                }
            }
            if (this.converted3DLut)
            {
                DestroyImmediate(this.converted3DLut);
            }
            this.converted3DLut = new Texture3D(width, width, width, TextureFormat.ARGB32, false);
            this.converted3DLut.SetPixels(colors);
            this.converted3DLut.Apply();
            this.basedOnTempTex = string.Empty;
        }

        public bool ValidDimensions(Texture2D tex2d) => 
            tex2d ? (tex2d.height == Mathf.FloorToInt(Mathf.Sqrt((float) tex2d.width))) : false;
    }
}

