namespace Edelweiss.DecalSystem
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class Decals : GenericDecals<Decals, DecalProjectorBase, DecalsMesh>
    {
        [SerializeField]
        private MeshMinimizerMode m_MeshMinimizerMode;
        [SerializeField]
        private float m_MeshMinimizerMaximumAbsoluteError = 0.0001f;
        [SerializeField]
        private float m_MeshMinimizerMaximumRelativeError = 0.0001f;
        private List<DecalsMeshRenderer> m_DecalsMeshRenderers = new List<DecalsMeshRenderer>();

        protected Decals()
        {
        }

        protected abstract DecalsMeshRenderer AddDecalsMeshRendererComponentToGameObject(GameObject a_GameObject);
        public override void ApplyMaterialToMeshRenderers()
        {
            base.ApplyMaterialToMeshRenderers();
            Material currentMaterial = null;
            if (base.CurrentTextureAtlasType == TextureAtlasType.Builtin)
            {
                currentMaterial = base.CurrentMaterial;
            }
            else if ((base.CurrentTextureAtlasType == TextureAtlasType.Reference) && (base.CurrentTextureAtlasAsset != null))
            {
                currentMaterial = base.CurrentTextureAtlasAsset.material;
            }
            foreach (DecalsMeshRenderer renderer in this.m_DecalsMeshRenderers)
            {
                renderer.MeshRenderer.material = currentMaterial;
            }
        }

        public override void ApplyRenderersEditability()
        {
            base.ApplyRenderersEditability();
            HideFlags none = HideFlags.None;
            if (!base.AreRenderersEditable)
            {
                none = HideFlags.NotEditable;
            }
            foreach (DecalsMeshRenderer renderer in this.m_DecalsMeshRenderers)
            {
                renderer.gameObject.hideFlags = none;
            }
        }

        private void ApplyToDecalsMeshRenderer(DecalsMeshRenderer a_DecalsMeshRenderer, DecalsMesh a_DecalsMesh)
        {
            Mesh mesh = this.MeshOfDecalsMeshRenderer(a_DecalsMeshRenderer);
            mesh.Clear();
            if (!Edition.IsDX11)
            {
                mesh.MarkDynamic();
            }
            mesh.vertices = a_DecalsMesh.Vertices.ToArray();
            mesh.normals = (base.CurrentNormalsMode == NormalsMode.None) ? null : a_DecalsMesh.Normals.ToArray();
            mesh.tangents = (base.CurrentTangentsMode == TangentsMode.None) ? null : a_DecalsMesh.Tangents.ToArray();
            if (!base.UseVertexColors)
            {
                mesh.colors = null;
            }
            else
            {
                Color[] colorArray = a_DecalsMesh.VertexColors.ToArray();
                if (a_DecalsMesh.PreserveVertexColorArrays)
                {
                    a_DecalsMesh.PreservedVertexColorArrays.Add(colorArray);
                }
                mesh.colors = colorArray;
            }
            Vector2[] item = a_DecalsMesh.UVs.ToArray();
            if (a_DecalsMesh.PreserveProjectedUVArrays)
            {
                a_DecalsMesh.PreservedProjectedUVArrays.Add(item);
            }
            mesh.uv = item;
            if (base.CurrentUV2Mode == UV2Mode.None)
            {
                mesh.uv2 = null;
            }
            else
            {
                Vector2[] vectorArray2 = a_DecalsMesh.UV2s.ToArray();
                if (a_DecalsMesh.PreserveProjectedUV2Arrays)
                {
                    a_DecalsMesh.PreservedProjectedUV2Arrays.Add(vectorArray2);
                }
                mesh.uv2 = vectorArray2;
            }
            mesh.triangles = a_DecalsMesh.Triangles.ToArray();
        }

        private void ApplyToDecalsMeshRenderer(DecalsMeshRenderer a_DecalsMeshRenderer, DecalsMesh a_DecalsMesh, GenericDecalProjectorBase a_FirstProjector, GenericDecalProjectorBase a_LastProjector)
        {
            int decalsMeshLowerVertexIndex = a_FirstProjector.DecalsMeshLowerVertexIndex;
            int decalsMeshUpperVertexIndex = a_LastProjector.DecalsMeshUpperVertexIndex;
            int decalsMeshLowerTriangleIndex = a_FirstProjector.DecalsMeshLowerTriangleIndex;
            int decalsMeshUpperTriangleIndex = a_LastProjector.DecalsMeshUpperTriangleIndex;
            Mesh mesh = this.MeshOfDecalsMeshRenderer(a_DecalsMeshRenderer);
            mesh.Clear();
            if (!Edition.IsDX11)
            {
                mesh.MarkDynamic();
            }
            Vector3[] vectorArray = new Vector3[(decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex) + 1];
            CopyListRangeToArray<Vector3>(ref vectorArray, a_DecalsMesh.Vertices, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
            mesh.vertices = vectorArray;
            int[] numArray = new int[(decalsMeshUpperTriangleIndex - decalsMeshLowerTriangleIndex) + 1];
            CopyListRangeToArray<int>(ref numArray, a_DecalsMesh.Triangles, decalsMeshLowerTriangleIndex, decalsMeshUpperTriangleIndex);
            for (int i = 0; i < numArray.Length; i++)
            {
                numArray[i] -= decalsMeshLowerVertexIndex;
            }
            mesh.triangles = numArray;
            Vector2[] vectorArray2 = new Vector2[(decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex) + 1];
            CopyListRangeToArray<Vector2>(ref vectorArray2, a_DecalsMesh.UVs, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
            if (a_DecalsMesh.PreserveProjectedUVArrays)
            {
                a_DecalsMesh.PreservedProjectedUVArrays.Add(vectorArray2);
            }
            mesh.uv = vectorArray2;
            if ((base.CurrentUV2Mode == UV2Mode.None) || (base.CurrentUV2Mode == UV2Mode.Lightmapping))
            {
                mesh.uv2 = null;
            }
            else
            {
                Vector2[] vectorArray3 = new Vector2[(decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex) + 1];
                CopyListRangeToArray<Vector2>(ref vectorArray3, a_DecalsMesh.UV2s, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
                if (a_DecalsMesh.PreserveProjectedUV2Arrays)
                {
                    a_DecalsMesh.PreservedProjectedUV2Arrays.Add(vectorArray3);
                }
                mesh.uv2 = vectorArray3;
            }
            if (base.CurrentNormalsMode == NormalsMode.None)
            {
                mesh.normals = null;
            }
            else
            {
                Vector3[] vectorArray4 = new Vector3[(decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex) + 1];
                CopyListRangeToArray<Vector3>(ref vectorArray4, a_DecalsMesh.Normals, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
                mesh.normals = vectorArray4;
            }
            if (base.CurrentTangentsMode == TangentsMode.None)
            {
                mesh.tangents = null;
            }
            else
            {
                Vector4[] vectorArray5 = new Vector4[(decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex) + 1];
                CopyListRangeToArray<Vector4>(ref vectorArray5, a_DecalsMesh.Tangents, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
                mesh.tangents = vectorArray5;
            }
            if (!base.UseVertexColors)
            {
                mesh.colors = null;
            }
            else
            {
                Color[] colorArray = new Color[(decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex) + 1];
                CopyListRangeToArray<Color>(ref colorArray, a_DecalsMesh.VertexColors, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
                if (a_DecalsMesh.PreserveVertexColorArrays)
                {
                    a_DecalsMesh.PreservedVertexColorArrays.Add(colorArray);
                }
                mesh.colors = colorArray;
            }
        }

        private static void CopyListRangeToArray<T>(ref T[] a_Array, List<T> a_List, int a_LowerListIndex, int a_UpperListIndex)
        {
            int index = 0;
            for (int i = a_LowerListIndex; i <= a_UpperListIndex; i++)
            {
                a_Array[index] = a_List[i];
                index++;
            }
        }

        public override void InitializeDecalsMeshRenderers()
        {
            this.m_DecalsMeshRenderers.Clear();
            Transform cachedTransform = base.CachedTransform;
            for (int i = 0; i < cachedTransform.childCount; i++)
            {
                DecalsMeshRenderer component = cachedTransform.GetChild(i).GetComponent<DecalsMeshRenderer>();
                if (component != null)
                {
                    this.m_DecalsMeshRenderers.Add(component);
                }
            }
        }

        public bool IsDecalsMeshRenderer(MeshRenderer a_MeshRenderer)
        {
            bool flag = false;
            foreach (DecalsMeshRenderer renderer in this.m_DecalsMeshRenderers)
            {
                if (a_MeshRenderer == renderer.MeshRenderer)
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        private Mesh MeshOfDecalsMeshRenderer(DecalsMeshRenderer a_DecalsMeshRenderer)
        {
            Mesh sharedMesh;
            if (Application.isPlaying)
            {
                if (a_DecalsMeshRenderer.MeshFilter.mesh != null)
                {
                    sharedMesh = a_DecalsMeshRenderer.MeshFilter.mesh;
                    sharedMesh.Clear();
                }
                else
                {
                    sharedMesh = new Mesh {
                        name = "Decal Mesh"
                    };
                    a_DecalsMeshRenderer.MeshFilter.mesh = sharedMesh;
                }
            }
            else if (a_DecalsMeshRenderer.MeshFilter.sharedMesh != null)
            {
                sharedMesh = a_DecalsMeshRenderer.MeshFilter.sharedMesh;
                sharedMesh.Clear();
            }
            else
            {
                sharedMesh = new Mesh {
                    name = "Decal Mesh"
                };
                a_DecalsMeshRenderer.MeshFilter.sharedMesh = sharedMesh;
            }
            return sharedMesh;
        }

        public override void OnEnable()
        {
            this.InitializeDecalsMeshRenderers();
            if (this.m_DecalsMeshRenderers.Count == 0)
            {
                this.PushDecalsMeshRenderer();
            }
        }

        public override void OptimizeDecalsMeshes()
        {
            base.OptimizeDecalsMeshes();
            foreach (DecalsMeshRenderer renderer in this.m_DecalsMeshRenderers)
            {
                if (Application.isPlaying)
                {
                    if ((renderer.MeshFilter != null) && (renderer.MeshFilter.mesh == null))
                    {
                    }
                    continue;
                }
                if ((renderer.MeshFilter != null) && (renderer.MeshFilter.sharedMesh == null))
                {
                }
            }
        }

        private void PopDecalsMeshRenderer()
        {
            DecalsMeshRenderer renderer = this.m_DecalsMeshRenderers[this.m_DecalsMeshRenderers.Count - 1];
            if (Application.isPlaying)
            {
                Destroy(renderer.MeshFilter.mesh);
                Destroy(renderer.gameObject);
            }
            this.m_DecalsMeshRenderers.RemoveAt(this.m_DecalsMeshRenderers.Count - 1);
        }

        private void PushDecalsMeshRenderer()
        {
            GameObject obj2 = new GameObject("Decals Mesh Renderer");
            Transform transform = obj2.transform;
            transform.parent = base.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            DecalsMeshRenderer item = this.AddDecalsMeshRendererComponentToGameObject(obj2);
            item.MeshRenderer.material = base.CurrentMaterial;
            this.m_DecalsMeshRenderers.Add(item);
        }

        public override void UpdateDecalsMeshes(DecalsMesh a_DecalsMesh)
        {
            base.UpdateDecalsMeshes(a_DecalsMesh);
            if (a_DecalsMesh.Vertices.Count <= 0xffff)
            {
                if (this.m_DecalsMeshRenderers.Count == 0)
                {
                    this.PushDecalsMeshRenderer();
                }
                else if (this.m_DecalsMeshRenderers.Count > 1)
                {
                    while (this.m_DecalsMeshRenderers.Count > 1)
                    {
                        this.PopDecalsMeshRenderer();
                    }
                }
                DecalsMeshRenderer renderer = this.m_DecalsMeshRenderers[0];
                this.ApplyToDecalsMeshRenderer(renderer, a_DecalsMesh);
            }
            else
            {
                int num = 0;
                int num2 = 0;
                while (true)
                {
                    if (num2 >= a_DecalsMesh.Projectors.Count)
                    {
                        while ((num + 1) < this.m_DecalsMeshRenderers.Count)
                        {
                            this.PopDecalsMeshRenderer();
                        }
                        break;
                    }
                    GenericDecalProjectorBase base2 = a_DecalsMesh.Projectors[num2];
                    GenericDecalProjectorBase base3 = a_DecalsMesh.Projectors[num2];
                    if (num >= this.m_DecalsMeshRenderers.Count)
                    {
                        this.PushDecalsMeshRenderer();
                    }
                    DecalsMeshRenderer renderer2 = this.m_DecalsMeshRenderers[num];
                    int num3 = 0;
                    int num4 = num2;
                    GenericDecalProjectorBase base4 = a_DecalsMesh.Projectors[num2];
                    while (true)
                    {
                        if ((num2 >= a_DecalsMesh.Projectors.Count) || ((num3 + base4.DecalsMeshVertexCount) > 0xffff))
                        {
                            if (num4 != num2)
                            {
                                this.ApplyToDecalsMeshRenderer(renderer2, a_DecalsMesh, base2, base3);
                                num++;
                            }
                            num2++;
                            break;
                        }
                        base3 = base4;
                        num3 += base4.DecalsMeshVertexCount;
                        num2++;
                        if (num2 < a_DecalsMesh.Projectors.Count)
                        {
                            base4 = a_DecalsMesh.Projectors[num2];
                        }
                    }
                }
            }
            base.SetDecalsMeshesAreNotOptimized();
        }

        public override void UpdateProjectedUV2s(DecalsMesh a_DecalsMesh)
        {
            base.UpdateProjectedUV2s(a_DecalsMesh);
            int num = 0;
            int num2 = 0;
            for (int i = 0; i < this.m_DecalsMeshRenderers.Count; i++)
            {
                DecalsMeshRenderer renderer = this.m_DecalsMeshRenderers[i];
                Mesh mesh = !Application.isPlaying ? renderer.MeshFilter.sharedMesh : renderer.MeshFilter.mesh;
                num2 = (num2 + mesh.vertexCount) - 1;
                Vector2[] vectorArray = a_DecalsMesh.PreservedProjectedUV2Arrays[i];
                CopyListRangeToArray<Vector2>(ref vectorArray, a_DecalsMesh.UV2s, num, num2);
                mesh.uv2 = vectorArray;
                num = num2;
            }
        }

        public override void UpdateProjectedUVs(DecalsMesh a_DecalsMesh)
        {
            base.UpdateProjectedUVs(a_DecalsMesh);
            int num = 0;
            int num2 = 0;
            for (int i = 0; i < this.m_DecalsMeshRenderers.Count; i++)
            {
                DecalsMeshRenderer renderer = this.m_DecalsMeshRenderers[i];
                Mesh mesh = !Application.isPlaying ? renderer.MeshFilter.sharedMesh : renderer.MeshFilter.mesh;
                num2 = (num2 + mesh.vertexCount) - 1;
                Vector2[] vectorArray = a_DecalsMesh.PreservedProjectedUVArrays[i];
                CopyListRangeToArray<Vector2>(ref vectorArray, a_DecalsMesh.UVs, num, num2);
                mesh.uv = vectorArray;
                num = num2;
            }
        }

        public override void UpdateVertexColors(DecalsMesh a_DecalsMesh)
        {
            base.UpdateVertexColors(a_DecalsMesh);
            int num = 0;
            int num2 = 0;
            for (int i = 0; i < this.m_DecalsMeshRenderers.Count; i++)
            {
                DecalsMeshRenderer renderer = this.m_DecalsMeshRenderers[i];
                Mesh mesh = !Application.isPlaying ? renderer.MeshFilter.sharedMesh : renderer.MeshFilter.mesh;
                num2 = (num2 + mesh.vertexCount) - 1;
                Color[] colorArray = a_DecalsMesh.PreservedVertexColorArrays[i];
                CopyListRangeToArray<Color>(ref colorArray, a_DecalsMesh.VertexColors, num, num2);
                mesh.colors = colorArray;
                num = num2;
            }
        }

        public MeshMinimizerMode CurrentMeshMinimizerMode
        {
            get => 
                this.m_MeshMinimizerMode;
            set => 
                this.m_MeshMinimizerMode = value;
        }

        public float MeshMinimizerMaximumAbsoluteError
        {
            get => 
                this.m_MeshMinimizerMaximumAbsoluteError;
            set
            {
                if (value < 0f)
                {
                    throw new ArgumentOutOfRangeException("The maximum absolute error has to be greater than zero.");
                }
                this.m_MeshMinimizerMaximumAbsoluteError = value;
            }
        }

        public float MeshMinimizerMaximumRelativeError
        {
            get => 
                this.m_MeshMinimizerMaximumRelativeError;
            set
            {
                if ((value < 0f) || (value > 1f))
                {
                    throw new ArgumentOutOfRangeException("The maximum relative error has to be within [0.0f, 1.0f].");
                }
                this.m_MeshMinimizerMaximumRelativeError = value;
            }
        }

        public override bool CastShadows
        {
            get => 
                this.DecalsMeshRenderers[0].MeshRenderer.castShadows;
            set
            {
                foreach (DecalsMeshRenderer renderer in this.DecalsMeshRenderers)
                {
                    renderer.MeshRenderer.castShadows = value;
                }
            }
        }

        public override bool ReceiveShadows
        {
            get => 
                this.DecalsMeshRenderers[0].MeshRenderer.receiveShadows;
            set
            {
                foreach (DecalsMeshRenderer renderer in this.DecalsMeshRenderers)
                {
                    renderer.MeshRenderer.receiveShadows = value;
                }
            }
        }

        public override bool UseLightProbes
        {
            get => 
                this.DecalsMeshRenderers[0].MeshRenderer.useLightProbes;
            set
            {
                foreach (DecalsMeshRenderer renderer in this.DecalsMeshRenderers)
                {
                    renderer.MeshRenderer.useLightProbes = value;
                }
            }
        }

        public override Transform LightProbeAnchor
        {
            get => 
                this.DecalsMeshRenderers[0].MeshRenderer.probeAnchor;
            set
            {
                foreach (DecalsMeshRenderer renderer in this.DecalsMeshRenderers)
                {
                    renderer.MeshRenderer.probeAnchor = value;
                }
            }
        }

        public DecalsMeshRenderer[] DecalsMeshRenderers =>
            this.m_DecalsMeshRenderers.ToArray();
    }
}

