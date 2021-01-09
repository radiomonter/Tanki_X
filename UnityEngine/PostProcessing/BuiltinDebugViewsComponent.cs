namespace UnityEngine.PostProcessing
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Rendering;

    public sealed class BuiltinDebugViewsComponent : PostProcessingComponentCommandBuffer<BuiltinDebugViewsModel>
    {
        private const string k_ShaderString = "Hidden/Post FX/Builtin Debug Views";
        private ArrowArray m_Arrows;

        private void DepthNormalsPass(CommandBuffer cb)
        {
            Material mat = base.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
            cb.Blit((Texture) null, 2, mat, 1);
        }

        private void DepthPass(CommandBuffer cb)
        {
            Material mat = base.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
            BuiltinDebugViewsModel.DepthSettings depth = base.model.settings.depth;
            cb.SetGlobalFloat(Uniforms._DepthScale, 1f / depth.scale);
            cb.Blit((Texture) null, 2, mat, 0);
        }

        public override CameraEvent GetCameraEvent() => 
            (base.model.settings.mode != BuiltinDebugViewsModel.Mode.MotionVectors) ? CameraEvent.BeforeImageEffectsOpaque : CameraEvent.BeforeImageEffects;

        public override DepthTextureMode GetCameraFlags()
        {
            BuiltinDebugViewsModel.Mode mode = base.model.settings.mode;
            DepthTextureMode none = DepthTextureMode.None;
            if (mode == BuiltinDebugViewsModel.Mode.Normals)
            {
                none |= DepthTextureMode.DepthNormals;
            }
            else if (mode == BuiltinDebugViewsModel.Mode.MotionVectors)
            {
                none |= DepthTextureMode.MotionVectors | DepthTextureMode.Depth;
            }
            else if (mode == BuiltinDebugViewsModel.Mode.Depth)
            {
                none |= DepthTextureMode.Depth;
            }
            return none;
        }

        public override string GetName() => 
            "Builtin Debug Views";

        private void MotionVectorsPass(CommandBuffer cb)
        {
            Material mat = base.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
            BuiltinDebugViewsModel.MotionVectorsSettings motionVectors = base.model.settings.motionVectors;
            int nameID = Uniforms._TempRT;
            cb.GetTemporaryRT(nameID, base.context.width, base.context.height, 0, FilterMode.Bilinear);
            cb.SetGlobalFloat(Uniforms._Opacity, motionVectors.sourceOpacity);
            cb.SetGlobalTexture(Uniforms._MainTex, 2);
            cb.Blit(2, nameID, mat, 2);
            if ((motionVectors.motionImageOpacity > 0f) && (motionVectors.motionImageAmplitude > 0f))
            {
                int num2 = Uniforms._TempRT2;
                cb.GetTemporaryRT(num2, base.context.width, base.context.height, 0, FilterMode.Bilinear);
                cb.SetGlobalFloat(Uniforms._Opacity, motionVectors.motionImageOpacity);
                cb.SetGlobalFloat(Uniforms._Amplitude, motionVectors.motionImageAmplitude);
                cb.SetGlobalTexture(Uniforms._MainTex, nameID);
                cb.Blit(nameID, num2, mat, 3);
                cb.ReleaseTemporaryRT(nameID);
                nameID = num2;
            }
            if ((motionVectors.motionVectorsOpacity > 0f) && (motionVectors.motionVectorsAmplitude > 0f))
            {
                this.PrepareArrows();
                float y = 1f / ((float) motionVectors.motionVectorsResolution);
                float x = (y * base.context.height) / ((float) base.context.width);
                cb.SetGlobalVector(Uniforms._Scale, new Vector2(x, y));
                cb.SetGlobalFloat(Uniforms._Opacity, motionVectors.motionVectorsOpacity);
                cb.SetGlobalFloat(Uniforms._Amplitude, motionVectors.motionVectorsAmplitude);
                cb.DrawMesh(this.m_Arrows.mesh, Matrix4x4.identity, mat, 0, 4);
            }
            cb.SetGlobalTexture(Uniforms._MainTex, nameID);
            cb.Blit(nameID, 2);
            cb.ReleaseTemporaryRT(nameID);
        }

        public override void OnDisable()
        {
            if (this.m_Arrows != null)
            {
                this.m_Arrows.Release();
            }
            this.m_Arrows = null;
        }

        public override void PopulateCommandBuffer(CommandBuffer cb)
        {
            BuiltinDebugViewsModel.Settings settings = base.model.settings;
            Material material = base.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
            material.shaderKeywords = null;
            if (base.context.isGBufferAvailable)
            {
                material.EnableKeyword("SOURCE_GBUFFER");
            }
            BuiltinDebugViewsModel.Mode mode = settings.mode;
            if (mode == BuiltinDebugViewsModel.Mode.Depth)
            {
                this.DepthPass(cb);
            }
            else if (mode == BuiltinDebugViewsModel.Mode.Normals)
            {
                this.DepthNormalsPass(cb);
            }
            else if (mode == BuiltinDebugViewsModel.Mode.MotionVectors)
            {
                this.MotionVectorsPass(cb);
            }
            base.context.Interrupt();
        }

        private void PrepareArrows()
        {
            int motionVectorsResolution = base.model.settings.motionVectors.motionVectorsResolution;
            int columns = (motionVectorsResolution * Screen.width) / Screen.height;
            this.m_Arrows ??= new ArrowArray();
            if ((this.m_Arrows.columnCount != columns) || (this.m_Arrows.rowCount != motionVectorsResolution))
            {
                this.m_Arrows.Release();
                this.m_Arrows.BuildMesh(columns, motionVectorsResolution);
            }
        }

        public override bool active =>
            (base.model.IsModeActive(BuiltinDebugViewsModel.Mode.Depth) || base.model.IsModeActive(BuiltinDebugViewsModel.Mode.Normals)) || base.model.IsModeActive(BuiltinDebugViewsModel.Mode.MotionVectors);

        private class ArrowArray
        {
            public void BuildMesh(int columns, int rows)
            {
                Vector3[] vectorArray = new Vector3[] { new Vector3(0f, 0f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 1f, 0f), new Vector3(-1f, 1f, 0f), new Vector3(0f, 1f, 0f), new Vector3(1f, 1f, 0f) };
                int capacity = (6 * columns) * rows;
                List<Vector3> inVertices = new List<Vector3>(capacity);
                List<Vector2> uvs = new List<Vector2>(capacity);
                int num2 = 0;
                while (num2 < rows)
                {
                    int num3 = 0;
                    while (true)
                    {
                        if (num3 >= columns)
                        {
                            num2++;
                            break;
                        }
                        Vector2 item = new Vector2((0.5f + num3) / ((float) columns), (0.5f + num2) / ((float) rows));
                        int index = 0;
                        while (true)
                        {
                            if (index >= 6)
                            {
                                num3++;
                                break;
                            }
                            inVertices.Add(vectorArray[index]);
                            uvs.Add(item);
                            index++;
                        }
                    }
                }
                int[] indices = new int[capacity];
                for (int i = 0; i < capacity; i++)
                {
                    indices[i] = i;
                }
                Mesh mesh = new Mesh {
                    hideFlags = HideFlags.DontSave
                };
                this.mesh = mesh;
                this.mesh.SetVertices(inVertices);
                this.mesh.SetUVs(0, uvs);
                this.mesh.SetIndices(indices, MeshTopology.Lines, 0);
                this.mesh.UploadMeshData(true);
                this.columnCount = columns;
                this.rowCount = rows;
            }

            public void Release()
            {
                GraphicsUtils.Destroy(this.mesh);
                this.mesh = null;
            }

            public Mesh mesh { get; private set; }

            public int columnCount { get; private set; }

            public int rowCount { get; private set; }
        }

        private enum Pass
        {
            Depth,
            Normals,
            MovecOpacity,
            MovecImaging,
            MovecArrows
        }

        private static class Uniforms
        {
            internal static readonly int _DepthScale = Shader.PropertyToID("_DepthScale");
            internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");
            internal static readonly int _Opacity = Shader.PropertyToID("_Opacity");
            internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");
            internal static readonly int _TempRT2 = Shader.PropertyToID("_TempRT2");
            internal static readonly int _Amplitude = Shader.PropertyToID("_Amplitude");
            internal static readonly int _Scale = Shader.PropertyToID("_Scale");
        }
    }
}

