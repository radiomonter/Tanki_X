namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;
    using UnityEngine.PostProcessing;

    public class CameraComponent : Component
    {
        public CameraComponent(Camera unityCamera)
        {
            this.UnityCamera = unityCamera;
            this.PostProcessingBehaviour = unityCamera.GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>();
            this.PostEffectsSets = unityCamera.GetComponents<PostEffectsSet>();
        }

        public Camera UnityCamera { get; set; }

        public bool Enabled
        {
            get => 
                this.UnityCamera.enabled;
            set => 
                this.UnityCamera.enabled = value;
        }

        public float FOV
        {
            get => 
                this.UnityCamera.fieldOfView;
            set => 
                this.UnityCamera.fieldOfView = value;
        }

        public UnityEngine.DepthTextureMode DepthTextureMode
        {
            get => 
                this.UnityCamera.depthTextureMode;
            set => 
                this.UnityCamera.depthTextureMode = value;
        }

        public Matrix4x4 ProjectionMatrix =>
            this.UnityCamera.projectionMatrix;

        public Matrix4x4 WorldToCameraMatrix =>
            this.UnityCamera.worldToCameraMatrix;

        public PostEffectsSet[] PostEffectsSets { get; private set; }

        public Tanks.Battle.ClientGraphics.API.SetPostProcessing SetPostProcessing { get; set; }

        public UnityEngine.PostProcessing.PostProcessingBehaviour PostProcessingBehaviour { get; set; }
    }
}

