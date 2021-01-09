namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;
    using UnityEngine.Rendering;

    public class CharacterShadowCommonSettingsComponent : MonoBehaviour, Component
    {
        public Shader blurShader;
        public Shader casterShader;
        public Shader receiverShader;
        public LayerMask ignoreLayers = 0;
        public int maxShadowMapAtlasSize = 0x800;
        public int textureSize = 0x80;
        public int blurSize = 5;
        public Transform virtualLight;
        public RenderTexture shadowMap;
        private UnityEngine.Camera camera;
        private Material horizontalBlurMaterial;
        private Material verticalBlurMaterial;
        private UnityEngine.Rendering.CommandBuffer commandBuffer;
        private int rawShadowMapNameId;
        private RenderTargetIdentifier rawShadowMapId;
        private int blurredShadowMapNameId;
        private RenderTargetIdentifier blurredShadowMapId;

        public void Awake()
        {
            if (!this.ShadowsSupported())
            {
                Destroy(base.gameObject);
            }
            else
            {
                this.commandBuffer = new UnityEngine.Rendering.CommandBuffer();
                this.rawShadowMapNameId = Shader.PropertyToID("rawShadowMapNameId");
                this.rawShadowMapId = new RenderTargetIdentifier(this.rawShadowMapNameId);
                this.blurredShadowMapNameId = Shader.PropertyToID("blurredShadowMapNameId");
                this.blurredShadowMapId = new RenderTargetIdentifier(this.blurredShadowMapNameId);
                this.horizontalBlurMaterial = new Material(this.blurShader);
                this.verticalBlurMaterial = new Material(this.blurShader);
                this.horizontalBlurMaterial.SetVector("Direction", new Vector2(1f, 0f));
                this.verticalBlurMaterial.SetVector("Direction", new Vector2(0f, 1f));
                this.camera = this.CreateCamera();
                this.camera.AddCommandBuffer(CameraEvent.AfterForwardOpaque, this.commandBuffer);
            }
        }

        private UnityEngine.Camera CreateCamera()
        {
            UnityEngine.Camera camera = new GameObject("CharacterShadowCamera") { transform = { parent = base.transform } }.AddComponent<UnityEngine.Camera>();
            camera.depth = -10f;
            camera.renderingPath = RenderingPath.VertexLit;
            camera.clearFlags = CameraClearFlags.Nothing;
            camera.backgroundColor = new Color(1f, 1f, 1f, 1f);
            camera.cullingMask = 0;
            camera.orthographic = true;
            camera.enabled = false;
            return camera;
        }

        public bool ShadowsSupported() => 
            SystemInfo.supportsRenderTextures && (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R8) ? SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB32) : true);

        public RenderTextureFormat ShadowMapTextureFormat =>
            !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R8) ? RenderTextureFormat.ARGB32 : RenderTextureFormat.R8;

        public UnityEngine.Camera Camera =>
            this.camera;

        public Material HorizontalBlurMaterial =>
            this.horizontalBlurMaterial;

        public Material VerticalBlurMaterial =>
            this.verticalBlurMaterial;

        public UnityEngine.Rendering.CommandBuffer CommandBuffer =>
            this.commandBuffer;

        public int RawShadowMapNameId =>
            this.rawShadowMapNameId;

        public RenderTargetIdentifier RawShadowMapId =>
            this.rawShadowMapId;

        public int BlurredShadowMapNameId =>
            this.blurredShadowMapNameId;

        public RenderTargetIdentifier BlurredShadowMapId =>
            this.blurredShadowMapId;

        public int MaxCharactersCountInAtlas
        {
            get
            {
                int num = this.maxShadowMapAtlasSize / this.textureSize;
                return (num * num);
            }
        }
    }
}

