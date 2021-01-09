namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class UpdateRankEffectDistortionMobile : MonoBehaviour
    {
        public float TextureScale = 1f;
        public UnityEngine.RenderTextureFormat RenderTextureFormat;
        public UnityEngine.FilterMode FilterMode;
        public LayerMask CullingMask = -17;
        public UnityEngine.RenderingPath RenderingPath;
        public int FPSWhenMoveCamera = 40;
        public int FPSWhenStaticCamera = 20;
        private RenderTexture renderTexture;
        private Camera cameraInstance;
        private GameObject goCamera;
        private Vector3 oldPosition;
        private Quaternion oldRotation;
        private Transform instanceCameraTransform;
        private bool canUpdateCamera;
        private bool isStaticUpdate;
        private WaitForSeconds fpsMove;
        private WaitForSeconds fpsStatic;
        private const int dropedFrames = 50;
        private int frameCountWhenCameraIsStatic;
        private bool isInitialized;

        private void Initialize()
        {
            this.goCamera = new GameObject("RenderTextureCamera");
            this.cameraInstance = this.goCamera.AddComponent<Camera>();
            Camera main = Camera.main;
            this.cameraInstance.CopyFrom(main);
            this.cameraInstance.depth++;
            this.cameraInstance.cullingMask = (int) this.CullingMask;
            this.cameraInstance.renderingPath = this.RenderingPath;
            this.goCamera.transform.parent = main.transform;
            this.renderTexture = new RenderTexture(Mathf.RoundToInt(Screen.width * this.TextureScale), Mathf.RoundToInt(Screen.height * this.TextureScale), 0x10, this.RenderTextureFormat);
            this.renderTexture.DiscardContents();
            this.renderTexture.filterMode = this.FilterMode;
            this.cameraInstance.targetTexture = this.renderTexture;
            this.instanceCameraTransform = this.cameraInstance.transform;
            this.oldPosition = this.instanceCameraTransform.position;
            Shader.SetGlobalTexture("_GrabTextureMobile", this.renderTexture);
            this.isInitialized = true;
        }

        private void OnBecameInvisible()
        {
            if (this.goCamera != null)
            {
                this.goCamera.SetActive(false);
            }
        }

        private void OnBecameVisible()
        {
            if (this.goCamera != null)
            {
                this.goCamera.SetActive(true);
            }
        }

        private void OnDisable()
        {
            if (this.goCamera)
            {
                DestroyImmediate(this.goCamera);
                this.goCamera = null;
            }
            if (this.renderTexture)
            {
                DestroyImmediate(this.renderTexture);
                this.renderTexture = null;
            }
            this.isInitialized = false;
        }

        private void OnEnable()
        {
            this.fpsMove = new WaitForSeconds(1f / ((float) this.FPSWhenMoveCamera));
            this.fpsStatic = new WaitForSeconds(1f / ((float) this.FPSWhenStaticCamera));
        }

        [DebuggerHidden]
        private IEnumerator RepeatCameraMove() => 
            new <RepeatCameraMove>c__Iterator0 { $this = this };

        [DebuggerHidden]
        private IEnumerator RepeatCameraStatic() => 
            new <RepeatCameraStatic>c__Iterator1 { $this = this };

        private void Update()
        {
            if (!this.isInitialized)
            {
                this.Initialize();
                base.StartCoroutine(this.RepeatCameraMove());
                base.StartCoroutine(this.RepeatCameraStatic());
            }
            if ((Vector3.SqrMagnitude(this.instanceCameraTransform.position - this.oldPosition) > 1E-05f) || !(this.instanceCameraTransform.rotation == this.oldRotation))
            {
                this.frameCountWhenCameraIsStatic = 0;
                this.isStaticUpdate = false;
            }
            else
            {
                this.frameCountWhenCameraIsStatic++;
                if (this.frameCountWhenCameraIsStatic >= 50)
                {
                    this.isStaticUpdate = true;
                }
            }
            this.oldPosition = this.instanceCameraTransform.position;
            this.oldRotation = this.instanceCameraTransform.rotation;
            if (this.cameraInstance == null)
            {
                this.canUpdateCamera = false;
            }
            else if (this.canUpdateCamera)
            {
                this.cameraInstance.enabled = true;
                this.canUpdateCamera = false;
            }
            else if (this.cameraInstance.enabled)
            {
                this.cameraInstance.enabled = false;
            }
        }

        [CompilerGenerated]
        private sealed class <RepeatCameraMove>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal UpdateRankEffectDistortionMobile $this;
            internal object $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                    case 1:
                        if (!this.$this.isStaticUpdate)
                        {
                            this.$this.canUpdateCamera = true;
                        }
                        this.$current = this.$this.fpsMove;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }

        [CompilerGenerated]
        private sealed class <RepeatCameraStatic>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal UpdateRankEffectDistortionMobile $this;
            internal object $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                    case 1:
                        if (this.$this.isStaticUpdate)
                        {
                            this.$this.canUpdateCamera = true;
                        }
                        this.$current = this.$this.fpsStatic;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

