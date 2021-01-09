using System;
using System.Collections.Generic;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;
using UnityEngine.Rendering;
using UnityStandardAssets.ImageEffects;

[RequireComponent(typeof(Bloom))]
public class BurningTargetBloom : MonoBehaviour
{
    [HideInInspector]
    public Shader bloomMaskShader;
    [HideInInspector]
    public Shader brightPassFilterShader;
    public List<Renderer> targets;
    private CommandBuffer commandBuffer;
    private Material bloomMaskMaterial;
    private RenderTexture bloomMask;
    private Bloom bloom;
    private Material bloomFilterMaterial;

    private CommandBuffer CreateCommandBuffer() => 
        new CommandBuffer { name = "HotBloomCommandBuffer" };

    private void OnDisable()
    {
        if (this.commandBuffer != null)
        {
            base.GetComponent<Camera>().RemoveCommandBuffer(CameraEvent.AfterDepthTexture, this.commandBuffer);
            this.commandBuffer.Dispose();
            this.bloomMask.Release();
        }
    }

    private void OnEnable()
    {
        this.commandBuffer = this.CreateCommandBuffer();
        base.GetComponent<Camera>().AddCommandBuffer(CameraEvent.AfterDepthTexture, this.commandBuffer);
        this.bloomMask = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
        this.bloomMaskMaterial = new Material(this.bloomMaskShader);
        this.bloom = base.GetComponent<Bloom>();
        this.bloom.enabled = false;
        this.bloom.brightPassFilterShader = this.brightPassFilterShader;
        Material material = new Material(this.brightPassFilterShader) {
            hideFlags = HideFlags.DontSave
        };
        this.bloom.brightPassFilterMaterial = material;
    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        this.bloom.brightPassFilterMaterial.SetTexture("_TargetBloomMask", this.bloomMask);
        this.bloom.OnRenderImage(source, destination);
    }

    private void Reset()
    {
        this.bloomMaskShader = Shader.Find("Alternativa/PostEffects/TargetBloom/BloomMask");
        this.brightPassFilterShader = Shader.Find("Alternativa/PostEffects/TargetBloom/BrightPassFilter");
    }

    private void Update()
    {
        this.commandBuffer.Clear();
        this.commandBuffer.SetRenderTarget(this.bloomMask);
        this.commandBuffer.ClearRenderTarget(true, true, Color.black, 1f);
        foreach (Renderer renderer in this.targets)
        {
            if (renderer != null)
            {
                Material[] materials = renderer.materials;
                for (int i = 0; i < materials.Length; i++)
                {
                    if (!materials[i].HasProperty(TankMaterialPropertyNames.COLORING_MAP_ALPHA_DEF_ID))
                    {
                        this.commandBuffer.DrawRenderer(renderer, this.bloomMaskMaterial, i);
                    }
                    else if (materials[i].GetFloat(TankMaterialPropertyNames.COLORING_MAP_ALPHA_DEF_ID).Equals((float) 1f))
                    {
                        this.commandBuffer.DrawRenderer(renderer, this.bloomMaskMaterial, i);
                    }
                    else if (materials[i].HasProperty(TankMaterialPropertyNames.TEMPERATURE_ID) && (materials[i].GetFloat(TankMaterialPropertyNames.TEMPERATURE_ID) > 0f))
                    {
                        this.commandBuffer.DrawRenderer(renderer, this.bloomMaskMaterial, i);
                    }
                }
            }
        }
    }
}

