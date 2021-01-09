namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, RequireComponent(typeof(Camera)), AddComponentMenu("Image Effects/Edge Detection/Edge Detection")]
    public class EdgeDetection : PostEffectsBase
    {
        public EdgeDetectMode mode = EdgeDetectMode.SobelDepthThin;
        public float sensitivityDepth = 1f;
        public float sensitivityNormals = 1f;
        public float lumThreshold = 0.2f;
        public float edgeExp = 1f;
        public float sampleDist = 1f;
        public float edgesOnly;
        public Color edgesOnlyBgColor = Color.white;
        public Shader edgeDetectShader;
        private Material edgeDetectMaterial;
        private EdgeDetectMode oldMode = EdgeDetectMode.SobelDepthThin;

        public override bool CheckResources()
        {
            base.CheckSupport(true);
            this.edgeDetectMaterial = base.CheckShaderAndCreateMaterial(this.edgeDetectShader, this.edgeDetectMaterial);
            if (this.mode != this.oldMode)
            {
                this.SetCameraFlag();
            }
            this.oldMode = this.mode;
            if (!base.isSupported)
            {
                base.ReportAutoDisable();
            }
            return base.isSupported;
        }

        private void OnEnable()
        {
            this.SetCameraFlag();
        }

        [ImageEffectOpaque]
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!this.CheckResources())
            {
                Graphics.Blit(source, destination);
            }
            else
            {
                Vector2 vector = new Vector2(this.sensitivityDepth, this.sensitivityNormals);
                this.edgeDetectMaterial.SetVector("_Sensitivity", new Vector4(vector.x, vector.y, 1f, vector.y));
                this.edgeDetectMaterial.SetFloat("_BgFade", this.edgesOnly);
                this.edgeDetectMaterial.SetFloat("_SampleDistance", this.sampleDist);
                this.edgeDetectMaterial.SetVector("_BgColor", (Vector4) this.edgesOnlyBgColor);
                this.edgeDetectMaterial.SetFloat("_Exponent", this.edgeExp);
                this.edgeDetectMaterial.SetFloat("_Threshold", this.lumThreshold);
                Graphics.Blit(source, destination, this.edgeDetectMaterial, (int) this.mode);
            }
        }

        private void SetCameraFlag()
        {
            if ((this.mode == EdgeDetectMode.SobelDepth) || (this.mode == EdgeDetectMode.SobelDepthThin))
            {
                Camera component = base.GetComponent<Camera>();
                component.depthTextureMode |= DepthTextureMode.Depth;
            }
            else if ((this.mode == EdgeDetectMode.TriangleDepthNormals) || (this.mode == EdgeDetectMode.RobertsCrossDepthNormals))
            {
                Camera component = base.GetComponent<Camera>();
                component.depthTextureMode |= DepthTextureMode.DepthNormals;
            }
        }

        private void Start()
        {
            this.oldMode = this.mode;
        }

        public enum EdgeDetectMode
        {
            TriangleDepthNormals,
            RobertsCrossDepthNormals,
            SobelDepth,
            SobelDepthThin,
            TriangleLuminance
        }
    }
}

