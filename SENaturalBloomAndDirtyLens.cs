using System;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(Camera)), AddComponentMenu("Image Effects/Sonic Ether/SE Natural Bloom and Dirty Lens")]
public class SENaturalBloomAndDirtyLens : MonoBehaviour
{
    [Range(0f, 0.4f)]
    public float bloomIntensity = 0.05f;
    public Shader shader;
    private Material material;
    public Texture2D lensDirtTexture;
    [Range(0f, 0.95f)]
    public float lensDirtIntensity = 0.05f;
    private bool isSupported;
    private float blurSize = 4f;
    public bool inputIsHDR;

    private void OnDisable()
    {
        if (this.material)
        {
            DestroyImmediate(this.material);
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!this.isSupported)
        {
            Graphics.Blit(source, destination);
        }
        else
        {
            if (!this.material)
            {
                this.material = new Material(this.shader);
            }
            this.material.hideFlags = HideFlags.HideAndDontSave;
            this.material.SetFloat("_BloomIntensity", Mathf.Exp(this.bloomIntensity) - 1f);
            this.material.SetFloat("_LensDirtIntensity", Mathf.Exp(this.lensDirtIntensity) - 1f);
            source.filterMode = FilterMode.Bilinear;
            RenderTexture dest = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
            Graphics.Blit(source, dest, this.material, 4);
            int width = source.width / 2;
            int height = source.height / 2;
            RenderTexture texture2 = dest;
            float num3 = 1f;
            int num4 = 2;
            int num5 = 0;
            while (num5 < 6)
            {
                RenderTexture texture3 = RenderTexture.GetTemporary(width, height, 0, source.format);
                texture3.filterMode = FilterMode.Bilinear;
                Graphics.Blit(texture2, texture3, this.material, 1);
                texture2 = texture3;
                num3 = (num5 <= 1) ? 0.5f : 1f;
                if (num5 == 2)
                {
                    num3 = 0.75f;
                }
                int num6 = 0;
                while (true)
                {
                    if (num6 >= num4)
                    {
                        switch (num5)
                        {
                            case 0:
                                this.material.SetTexture("_Bloom0", texture3);
                                break;

                            case 1:
                                this.material.SetTexture("_Bloom1", texture3);
                                break;

                            case 2:
                                this.material.SetTexture("_Bloom2", texture3);
                                break;

                            case 3:
                                this.material.SetTexture("_Bloom3", texture3);
                                break;

                            case 4:
                                this.material.SetTexture("_Bloom4", texture3);
                                break;

                            case 5:
                                this.material.SetTexture("_Bloom5", texture3);
                                break;

                            default:
                                break;
                        }
                        RenderTexture.ReleaseTemporary(texture3);
                        width /= 2;
                        height /= 2;
                        num5++;
                        break;
                    }
                    this.material.SetFloat("_BlurSize", ((this.blurSize * 0.5f) + num6) * num3);
                    RenderTexture texture4 = RenderTexture.GetTemporary(width, height, 0, source.format);
                    texture4.filterMode = FilterMode.Bilinear;
                    Graphics.Blit(texture3, texture4, this.material, 2);
                    RenderTexture.ReleaseTemporary(texture3);
                    texture3 = texture4;
                    texture4 = RenderTexture.GetTemporary(width, height, 0, source.format);
                    texture4.filterMode = FilterMode.Bilinear;
                    Graphics.Blit(texture3, texture4, this.material, 3);
                    RenderTexture.ReleaseTemporary(texture3);
                    texture3 = texture4;
                    num6++;
                }
            }
            this.material.SetTexture("_LensDirt", this.lensDirtTexture);
            Graphics.Blit(dest, destination, this.material, 0);
            RenderTexture.ReleaseTemporary(dest);
        }
    }

    private void Start()
    {
        this.isSupported = true;
        if (!this.material)
        {
            this.material = new Material(this.shader);
        }
        if (!SystemInfo.supportsImageEffects || (!SystemInfo.supportsRenderTextures || !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf)))
        {
            this.isSupported = false;
        }
    }
}

