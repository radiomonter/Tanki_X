namespace UnityEngine.PostProcessing
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;

    [ImageEffectAllowedInSceneView, RequireComponent(typeof(Camera)), DisallowMultipleComponent, ExecuteInEditMode, AddComponentMenu("Effects/Post-Processing Behaviour", -1)]
    public class PostProcessingBehaviour : MonoBehaviour
    {
        public PostProcessingProfile profile;
        public Func<Vector2, Matrix4x4> jitteredMatrixFunc;
        private Dictionary<Type, KeyValuePair<CameraEvent, CommandBuffer>> m_CommandBuffers;
        private List<PostProcessingComponentBase> m_Components;
        private Dictionary<PostProcessingComponentBase, bool> m_ComponentStates;
        private MaterialFactory m_MaterialFactory;
        private RenderTextureFactory m_RenderTextureFactory;
        private PostProcessingContext m_Context;
        private Camera m_Camera;
        private PostProcessingProfile m_PreviousProfile;
        private bool m_RenderingInSceneView;
        private BuiltinDebugViewsComponent m_DebugViews;
        private AmbientOcclusionComponent m_AmbientOcclusion;
        private ScreenSpaceReflectionComponent m_ScreenSpaceReflection;
        private FogComponent m_FogComponent;
        private MotionBlurComponent m_MotionBlur;
        private TaaComponent m_Taa;
        private EyeAdaptationComponent m_EyeAdaptation;
        private DepthOfFieldComponent m_DepthOfField;
        private BloomComponent m_Bloom;
        private ChromaticAberrationComponent m_ChromaticAberration;
        private ColorGradingComponent m_ColorGrading;
        private UserLutComponent m_UserLut;
        private GrainComponent m_Grain;
        private VignetteComponent m_Vignette;
        private DitheringComponent m_Dithering;
        private FxaaComponent m_Fxaa;
        private List<PostProcessingComponentBase> m_ComponentsToEnable = new List<PostProcessingComponentBase>();
        private List<PostProcessingComponentBase> m_ComponentsToDisable = new List<PostProcessingComponentBase>();

        private CommandBuffer AddCommandBuffer<T>(CameraEvent evt, string name) where T: PostProcessingModel
        {
            CommandBuffer buffer = new CommandBuffer {
                name = name
            };
            KeyValuePair<CameraEvent, CommandBuffer> pair = new KeyValuePair<CameraEvent, CommandBuffer>(evt, buffer);
            this.m_CommandBuffers.Add(typeof(T), pair);
            this.m_Camera.AddCommandBuffer(evt, pair.Value);
            return pair.Value;
        }

        private T AddComponent<T>(T component) where T: PostProcessingComponentBase
        {
            this.m_Components.Add(component);
            return component;
        }

        private void CheckObservers()
        {
            foreach (KeyValuePair<PostProcessingComponentBase, bool> pair in this.m_ComponentStates)
            {
                PostProcessingComponentBase key = pair.Key;
                bool enabled = key.GetModel().enabled;
                if (enabled != pair.Value)
                {
                    if (enabled)
                    {
                        this.m_ComponentsToEnable.Add(key);
                        continue;
                    }
                    this.m_ComponentsToDisable.Add(key);
                }
            }
            for (int i = 0; i < this.m_ComponentsToDisable.Count; i++)
            {
                PostProcessingComponentBase base3 = this.m_ComponentsToDisable[i];
                this.m_ComponentStates[base3] = false;
                base3.OnDisable();
            }
            for (int j = 0; j < this.m_ComponentsToEnable.Count; j++)
            {
                PostProcessingComponentBase base4 = this.m_ComponentsToEnable[j];
                this.m_ComponentStates[base4] = true;
                base4.OnEnable();
            }
            this.m_ComponentsToDisable.Clear();
            this.m_ComponentsToEnable.Clear();
        }

        private void DisableComponents()
        {
            foreach (PostProcessingComponentBase base2 in this.m_Components)
            {
                PostProcessingModel model = base2.GetModel();
                if ((model != null) && model.enabled)
                {
                    base2.OnDisable();
                }
            }
        }

        private CommandBuffer GetCommandBuffer<T>(CameraEvent evt, string name) where T: PostProcessingModel
        {
            CommandBuffer buffer;
            KeyValuePair<CameraEvent, CommandBuffer> pair;
            if (!this.m_CommandBuffers.TryGetValue(typeof(T), out pair))
            {
                buffer = this.AddCommandBuffer<T>(evt, name);
            }
            else if (((CameraEvent) pair.Key) == evt)
            {
                buffer = pair.Value;
            }
            else
            {
                this.RemoveCommandBuffer<T>();
                buffer = this.AddCommandBuffer<T>(evt, name);
            }
            return buffer;
        }

        private void OnDisable()
        {
            foreach (KeyValuePair<CameraEvent, CommandBuffer> pair in this.m_CommandBuffers.Values)
            {
                this.m_Camera.RemoveCommandBuffer(pair.Key, pair.Value);
                pair.Value.Dispose();
            }
            this.m_CommandBuffers.Clear();
            if (this.profile != null)
            {
                this.DisableComponents();
            }
            this.m_Components.Clear();
            if (this.m_Camera != null)
            {
                this.m_Camera.depthTextureMode = DepthTextureMode.None;
            }
            this.m_MaterialFactory.Dispose();
            this.m_RenderTextureFactory.Dispose();
            GraphicsUtils.Dispose();
        }

        private void OnEnable()
        {
            this.m_CommandBuffers = new Dictionary<Type, KeyValuePair<CameraEvent, CommandBuffer>>();
            this.m_MaterialFactory = new MaterialFactory();
            this.m_RenderTextureFactory = new RenderTextureFactory();
            this.m_Context = new PostProcessingContext();
            this.m_Components = new List<PostProcessingComponentBase>();
            this.m_DebugViews = this.AddComponent<BuiltinDebugViewsComponent>(new BuiltinDebugViewsComponent());
            this.m_AmbientOcclusion = this.AddComponent<AmbientOcclusionComponent>(new AmbientOcclusionComponent());
            this.m_ScreenSpaceReflection = this.AddComponent<ScreenSpaceReflectionComponent>(new ScreenSpaceReflectionComponent());
            this.m_FogComponent = this.AddComponent<FogComponent>(new FogComponent());
            this.m_MotionBlur = this.AddComponent<MotionBlurComponent>(new MotionBlurComponent());
            this.m_Taa = this.AddComponent<TaaComponent>(new TaaComponent());
            this.m_EyeAdaptation = this.AddComponent<EyeAdaptationComponent>(new EyeAdaptationComponent());
            this.m_DepthOfField = this.AddComponent<DepthOfFieldComponent>(new DepthOfFieldComponent());
            this.m_Bloom = this.AddComponent<BloomComponent>(new BloomComponent());
            this.m_ChromaticAberration = this.AddComponent<ChromaticAberrationComponent>(new ChromaticAberrationComponent());
            this.m_ColorGrading = this.AddComponent<ColorGradingComponent>(new ColorGradingComponent());
            this.m_UserLut = this.AddComponent<UserLutComponent>(new UserLutComponent());
            this.m_Grain = this.AddComponent<GrainComponent>(new GrainComponent());
            this.m_Vignette = this.AddComponent<VignetteComponent>(new VignetteComponent());
            this.m_Dithering = this.AddComponent<DitheringComponent>(new DitheringComponent());
            this.m_Fxaa = this.AddComponent<FxaaComponent>(new FxaaComponent());
            this.m_ComponentStates = new Dictionary<PostProcessingComponentBase, bool>();
            foreach (PostProcessingComponentBase base2 in this.m_Components)
            {
                this.m_ComponentStates.Add(base2, false);
            }
            base.useGUILayout = false;
        }

        private void OnGUI()
        {
            if ((Event.current.type == EventType.Repaint) && ((this.profile != null) && (this.m_Camera != null)))
            {
                if (this.m_EyeAdaptation.active && this.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.EyeAdaptation))
                {
                    this.m_EyeAdaptation.OnGUI();
                }
                else if (this.m_ColorGrading.active && this.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.LogLut))
                {
                    this.m_ColorGrading.OnGUI();
                }
                else if (this.m_UserLut.active && this.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.UserLut))
                {
                    this.m_UserLut.OnGUI();
                }
            }
        }

        private void OnPostRender()
        {
            if (((this.profile != null) && (this.m_Camera != null)) && (!this.m_RenderingInSceneView && (this.m_Taa.active && !this.profile.debugViews.willInterrupt)))
            {
                this.m_Context.camera.ResetProjectionMatrix();
            }
        }

        private void OnPreCull()
        {
            this.m_Camera = base.GetComponent<Camera>();
            if ((this.profile != null) && (this.m_Camera != null))
            {
                PostProcessingContext pcontext = this.m_Context.Reset();
                pcontext.profile = this.profile;
                pcontext.renderTextureFactory = this.m_RenderTextureFactory;
                pcontext.materialFactory = this.m_MaterialFactory;
                pcontext.camera = this.m_Camera;
                this.m_DebugViews.Init(pcontext, this.profile.debugViews);
                this.m_AmbientOcclusion.Init(pcontext, this.profile.ambientOcclusion);
                this.m_ScreenSpaceReflection.Init(pcontext, this.profile.screenSpaceReflection);
                this.m_FogComponent.Init(pcontext, this.profile.fog);
                this.m_MotionBlur.Init(pcontext, this.profile.motionBlur);
                this.m_Taa.Init(pcontext, this.profile.antialiasing);
                this.m_EyeAdaptation.Init(pcontext, this.profile.eyeAdaptation);
                this.m_DepthOfField.Init(pcontext, this.profile.depthOfField);
                this.m_Bloom.Init(pcontext, this.profile.bloom);
                this.m_ChromaticAberration.Init(pcontext, this.profile.chromaticAberration);
                this.m_ColorGrading.Init(pcontext, this.profile.colorGrading);
                this.m_UserLut.Init(pcontext, this.profile.userLut);
                this.m_Grain.Init(pcontext, this.profile.grain);
                this.m_Vignette.Init(pcontext, this.profile.vignette);
                this.m_Dithering.Init(pcontext, this.profile.dithering);
                this.m_Fxaa.Init(pcontext, this.profile.antialiasing);
                if (this.m_PreviousProfile != this.profile)
                {
                    this.DisableComponents();
                    this.m_PreviousProfile = this.profile;
                }
                this.CheckObservers();
                DepthTextureMode none = DepthTextureMode.None;
                foreach (PostProcessingComponentBase base2 in this.m_Components)
                {
                    if (base2.active)
                    {
                        none |= base2.GetCameraFlags();
                    }
                }
                pcontext.camera.depthTextureMode = none;
                if (!this.m_RenderingInSceneView && (this.m_Taa.active && !this.profile.debugViews.willInterrupt))
                {
                    this.m_Taa.SetProjectionMatrix(this.jitteredMatrixFunc);
                }
            }
        }

        private void OnPreRender()
        {
            if (this.profile != null)
            {
                this.TryExecuteCommandBuffer<BuiltinDebugViewsModel>(this.m_DebugViews);
                this.TryExecuteCommandBuffer<AmbientOcclusionModel>(this.m_AmbientOcclusion);
                this.TryExecuteCommandBuffer<ScreenSpaceReflectionModel>(this.m_ScreenSpaceReflection);
                this.TryExecuteCommandBuffer<FogModel>(this.m_FogComponent);
                if (!this.m_RenderingInSceneView)
                {
                    this.TryExecuteCommandBuffer<MotionBlurModel>(this.m_MotionBlur);
                }
            }
        }

        [ImageEffectTransformsToLDR]
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if ((this.profile == null) || (this.m_Camera == null))
            {
                Graphics.Blit(source, destination);
            }
            else
            {
                bool flag = false;
                bool active = this.m_Fxaa.active;
                bool antialiasCoC = this.m_Taa.active && !this.m_RenderingInSceneView;
                bool flag4 = this.m_DepthOfField.active && !this.m_RenderingInSceneView;
                Material uberMaterial = this.m_MaterialFactory.Get("Hidden/Post FX/Uber Shader");
                uberMaterial.shaderKeywords = null;
                RenderTexture baseRenderTexture = source;
                RenderTexture dest = destination;
                if (antialiasCoC)
                {
                    RenderTexture texture3 = this.m_RenderTextureFactory.Get(baseRenderTexture);
                    this.m_Taa.Render(baseRenderTexture, texture3);
                    baseRenderTexture = texture3;
                }
                Texture whiteTexture = GraphicsUtils.whiteTexture;
                if (this.m_EyeAdaptation.active)
                {
                    flag = true;
                    whiteTexture = this.m_EyeAdaptation.Prepare(baseRenderTexture, uberMaterial);
                }
                uberMaterial.SetTexture("_AutoExposure", whiteTexture);
                if (flag4)
                {
                    flag = true;
                    this.m_DepthOfField.Prepare(baseRenderTexture, uberMaterial, antialiasCoC, this.m_Taa.jitterVector, this.m_Taa.model.settings.taaSettings.motionBlending);
                }
                if (this.m_Bloom.active)
                {
                    flag = true;
                    this.m_Bloom.Prepare(baseRenderTexture, uberMaterial, whiteTexture);
                }
                flag = (((flag | this.TryPrepareUberImageEffect<ChromaticAberrationModel>(this.m_ChromaticAberration, uberMaterial)) | this.TryPrepareUberImageEffect<ColorGradingModel>(this.m_ColorGrading, uberMaterial)) | this.TryPrepareUberImageEffect<VignetteModel>(this.m_Vignette, uberMaterial)) | this.TryPrepareUberImageEffect<UserLutModel>(this.m_UserLut, uberMaterial);
                Material material = !active ? null : this.m_MaterialFactory.Get("Hidden/Post FX/FXAA");
                if (!active)
                {
                    flag = (flag | this.TryPrepareUberImageEffect<GrainModel>(this.m_Grain, uberMaterial)) | this.TryPrepareUberImageEffect<DitheringModel>(this.m_Dithering, uberMaterial);
                    if (flag)
                    {
                        if (!GraphicsUtils.isLinearColorSpace)
                        {
                            uberMaterial.EnableKeyword("UNITY_COLORSPACE_GAMMA");
                        }
                        Graphics.Blit(baseRenderTexture, dest, uberMaterial, 0);
                    }
                }
                else
                {
                    material.shaderKeywords = null;
                    this.TryPrepareUberImageEffect<GrainModel>(this.m_Grain, material);
                    this.TryPrepareUberImageEffect<DitheringModel>(this.m_Dithering, material);
                    if (flag)
                    {
                        RenderTexture texture5 = this.m_RenderTextureFactory.Get(baseRenderTexture);
                        Graphics.Blit(baseRenderTexture, texture5, uberMaterial, 0);
                        baseRenderTexture = texture5;
                    }
                    this.m_Fxaa.Render(baseRenderTexture, dest);
                }
                if (!flag && !active)
                {
                    Graphics.Blit(baseRenderTexture, dest);
                }
                this.m_RenderTextureFactory.ReleaseAll();
            }
        }

        private void RemoveCommandBuffer<T>() where T: PostProcessingModel
        {
            KeyValuePair<CameraEvent, CommandBuffer> pair;
            Type key = typeof(T);
            if (this.m_CommandBuffers.TryGetValue(key, out pair))
            {
                this.m_Camera.RemoveCommandBuffer(pair.Key, pair.Value);
                this.m_CommandBuffers.Remove(key);
                pair.Value.Dispose();
            }
        }

        public void ResetTemporalEffects()
        {
            this.m_Taa.ResetHistory();
            this.m_MotionBlur.ResetHistory();
            this.m_EyeAdaptation.ResetHistory();
        }

        private void TryExecuteCommandBuffer<T>(PostProcessingComponentCommandBuffer<T> component) where T: PostProcessingModel
        {
            if (!component.active)
            {
                this.RemoveCommandBuffer<T>();
            }
            else
            {
                CommandBuffer commandBuffer = this.GetCommandBuffer<T>(component.GetCameraEvent(), component.GetName());
                commandBuffer.Clear();
                component.PopulateCommandBuffer(commandBuffer);
            }
        }

        private bool TryPrepareUberImageEffect<T>(PostProcessingComponentRenderTexture<T> component, Material material) where T: PostProcessingModel
        {
            if (!component.active)
            {
                return false;
            }
            component.Prepare(material);
            return true;
        }
    }
}

