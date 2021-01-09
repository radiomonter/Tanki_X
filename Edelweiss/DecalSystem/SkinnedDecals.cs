namespace Edelweiss.DecalSystem
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class SkinnedDecals : GenericDecals<SkinnedDecals, SkinnedDecalProjectorBase, SkinnedDecalsMesh>
    {
        private List<SkinnedDecalsMeshRenderer> m_SkinnedDecalsMeshRenderers = new List<SkinnedDecalsMeshRenderer>();

        protected SkinnedDecals()
        {
        }

        protected abstract SkinnedDecalsMeshRenderer AddSkinnedDecalsMeshRendererComponentToGameObject(GameObject a_GameObject);
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
            foreach (SkinnedDecalsMeshRenderer renderer in this.m_SkinnedDecalsMeshRenderers)
            {
                if (Application.isPlaying)
                {
                    renderer.SkinnedMeshRenderer.material = currentMaterial;
                    continue;
                }
                renderer.SkinnedMeshRenderer.sharedMaterial = currentMaterial;
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
            foreach (SkinnedDecalsMeshRenderer renderer in this.m_SkinnedDecalsMeshRenderers)
            {
                renderer.gameObject.hideFlags = none;
            }
        }

        private void ApplyToSkinnedDecalsMeshRenderer(SkinnedDecalsMeshRenderer a_SkinnedDecalsMeshRenderer, SkinnedDecalsMesh a_SkinnedDecalsMesh)
        {
            Mesh mesh = this.MeshOfSkinnedDecalsMeshRenderer(a_SkinnedDecalsMeshRenderer);
            mesh.Clear();
            if (!Edition.IsDX11)
            {
                mesh.MarkDynamic();
            }
            if (a_SkinnedDecalsMesh.OriginalVertices.Count == 0)
            {
                mesh.vertices = new Vector3[1];
                if (base.CurrentNormalsMode != NormalsMode.None)
                {
                    mesh.normals = new Vector3[1];
                }
                if (base.CurrentTangentsMode != TangentsMode.None)
                {
                    mesh.tangents = new Vector4[1];
                }
                if (base.UseVertexColors)
                {
                    mesh.colors = new Color[1];
                }
                mesh.uv = new Vector2[1];
                if (base.CurrentUV2Mode != UV2Mode.None)
                {
                    mesh.uv2 = new Vector2[1];
                }
                mesh.boneWeights = new BoneWeight[1];
                mesh.bindposes = new Matrix4x4[1];
                a_SkinnedDecalsMeshRenderer.SkinnedMeshRenderer.bones = new Transform[] { a_SkinnedDecalsMeshRenderer.transform };
                a_SkinnedDecalsMeshRenderer.SkinnedMeshRenderer.localBounds = mesh.bounds;
                a_SkinnedDecalsMeshRenderer.SkinnedMeshRenderer.updateWhenOffscreen = false;
            }
            else
            {
                mesh.vertices = a_SkinnedDecalsMesh.OriginalVertices.ToArray();
                if (base.CurrentNormalsMode != NormalsMode.None)
                {
                    mesh.normals = a_SkinnedDecalsMesh.Normals.ToArray();
                }
                if (base.CurrentTangentsMode != TangentsMode.None)
                {
                    mesh.tangents = a_SkinnedDecalsMesh.Tangents.ToArray();
                }
                if (base.UseVertexColors)
                {
                    Color[] colorArray = a_SkinnedDecalsMesh.VertexColors.ToArray();
                    if (a_SkinnedDecalsMesh.PreserveVertexColorArrays)
                    {
                        a_SkinnedDecalsMesh.PreservedVertexColorArrays.Add(colorArray);
                    }
                    mesh.colors = colorArray;
                }
                Vector2[] item = a_SkinnedDecalsMesh.UVs.ToArray();
                if (a_SkinnedDecalsMesh.PreserveProjectedUVArrays)
                {
                    a_SkinnedDecalsMesh.PreservedProjectedUVArrays.Add(item);
                }
                mesh.uv = item;
                if (base.CurrentUV2Mode != UV2Mode.None)
                {
                    Vector2[] vectorArray2 = a_SkinnedDecalsMesh.UV2s.ToArray();
                    if (a_SkinnedDecalsMesh.PreserveProjectedUV2Arrays)
                    {
                        a_SkinnedDecalsMesh.PreservedProjectedUV2Arrays.Add(vectorArray2);
                    }
                    mesh.uv2 = vectorArray2;
                }
                mesh.boneWeights = a_SkinnedDecalsMesh.BoneWeights.ToArray();
                mesh.triangles = a_SkinnedDecalsMesh.Triangles.ToArray();
                mesh.bindposes = a_SkinnedDecalsMesh.BindPoses.ToArray();
                a_SkinnedDecalsMeshRenderer.SkinnedMeshRenderer.bones = a_SkinnedDecalsMesh.Bones.ToArray();
                a_SkinnedDecalsMeshRenderer.SkinnedMeshRenderer.localBounds = mesh.bounds;
                a_SkinnedDecalsMeshRenderer.SkinnedMeshRenderer.updateWhenOffscreen = true;
            }
        }

        private void ApplyToSkinnedDecalsMeshRenderer(SkinnedDecalsMeshRenderer a_SkinnedDecalsMeshRenderer, SkinnedDecalsMesh a_SkinnedDecalsMesh, GenericDecalProjectorBase a_FirstProjector, GenericDecalProjectorBase a_LastProjector)
        {
            int decalsMeshLowerVertexIndex = a_FirstProjector.DecalsMeshLowerVertexIndex;
            int decalsMeshUpperVertexIndex = a_LastProjector.DecalsMeshUpperVertexIndex;
            int decalsMeshLowerTriangleIndex = a_FirstProjector.DecalsMeshLowerTriangleIndex;
            int decalsMeshUpperTriangleIndex = a_LastProjector.DecalsMeshUpperTriangleIndex;
            Mesh mesh = this.MeshOfSkinnedDecalsMeshRenderer(a_SkinnedDecalsMeshRenderer);
            mesh.Clear();
            if (!Edition.IsDX11)
            {
                mesh.MarkDynamic();
            }
            Vector3[] vectorArray = new Vector3[(decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex) + 1];
            CopyListRangeToArray<Vector3>(ref vectorArray, a_SkinnedDecalsMesh.OriginalVertices, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
            mesh.vertices = vectorArray;
            BoneWeight[] weightArray = new BoneWeight[(decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex) + 1];
            CopyListRangeToArray<BoneWeight>(ref weightArray, a_SkinnedDecalsMesh.BoneWeights, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
            mesh.boneWeights = weightArray;
            int[] numArray = new int[(decalsMeshUpperTriangleIndex - decalsMeshLowerTriangleIndex) + 1];
            CopyListRangeToArray<int>(ref numArray, a_SkinnedDecalsMesh.Triangles, decalsMeshLowerTriangleIndex, decalsMeshUpperTriangleIndex);
            for (int i = 0; i < numArray.Length; i++)
            {
                numArray[i] -= decalsMeshLowerVertexIndex;
            }
            mesh.triangles = numArray;
            Vector2[] vectorArray2 = new Vector2[(decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex) + 1];
            CopyListRangeToArray<Vector2>(ref vectorArray2, a_SkinnedDecalsMesh.UVs, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
            if (a_SkinnedDecalsMesh.PreserveProjectedUVArrays)
            {
                a_SkinnedDecalsMesh.PreservedProjectedUVArrays.Add(vectorArray2);
            }
            mesh.uv = vectorArray2;
            if ((base.CurrentUV2Mode == UV2Mode.None) || (base.CurrentUV2Mode == UV2Mode.Lightmapping))
            {
                mesh.uv2 = null;
            }
            else
            {
                Vector2[] vectorArray3 = new Vector2[(decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex) + 1];
                CopyListRangeToArray<Vector2>(ref vectorArray3, a_SkinnedDecalsMesh.UV2s, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
                if (a_SkinnedDecalsMesh.PreserveProjectedUV2Arrays)
                {
                    a_SkinnedDecalsMesh.PreservedProjectedUV2Arrays.Add(vectorArray3);
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
                CopyListRangeToArray<Vector3>(ref vectorArray4, a_SkinnedDecalsMesh.Normals, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
                mesh.normals = vectorArray4;
            }
            if (base.CurrentTangentsMode == TangentsMode.None)
            {
                mesh.tangents = null;
            }
            else
            {
                Vector4[] vectorArray5 = new Vector4[(decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex) + 1];
                CopyListRangeToArray<Vector4>(ref vectorArray5, a_SkinnedDecalsMesh.Tangents, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
                mesh.tangents = vectorArray5;
            }
            if (!base.UseVertexColors)
            {
                mesh.colors = null;
            }
            else
            {
                Color[] colorArray = new Color[(decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex) + 1];
                CopyListRangeToArray<Color>(ref colorArray, a_SkinnedDecalsMesh.VertexColors, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
                if (a_SkinnedDecalsMesh.PreserveVertexColorArrays)
                {
                    a_SkinnedDecalsMesh.PreservedVertexColorArrays.Add(colorArray);
                }
                mesh.colors = colorArray;
            }
            mesh.bindposes = a_SkinnedDecalsMesh.BindPoses.ToArray();
            Transform[] transformArray = a_SkinnedDecalsMesh.Bones.ToArray();
            a_SkinnedDecalsMeshRenderer.SkinnedMeshRenderer.bones = transformArray;
            a_SkinnedDecalsMeshRenderer.SkinnedMeshRenderer.localBounds = mesh.bounds;
            a_SkinnedDecalsMeshRenderer.SkinnedMeshRenderer.updateWhenOffscreen = true;
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
            this.m_SkinnedDecalsMeshRenderers.Clear();
            Transform cachedTransform = base.CachedTransform;
            for (int i = 0; i < cachedTransform.childCount; i++)
            {
                SkinnedDecalsMeshRenderer component = cachedTransform.GetChild(i).GetComponent<SkinnedDecalsMeshRenderer>();
                if (component != null)
                {
                    this.m_SkinnedDecalsMeshRenderers.Add(component);
                }
            }
        }

        public bool IsSkinnedDecalsMeshRenderer(SkinnedMeshRenderer a_SkinnedMeshRenderer)
        {
            bool flag = false;
            foreach (SkinnedDecalsMeshRenderer renderer in this.m_SkinnedDecalsMeshRenderers)
            {
                if (a_SkinnedMeshRenderer == renderer.SkinnedMeshRenderer)
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        private Mesh MeshOfSkinnedDecalsMeshRenderer(SkinnedDecalsMeshRenderer a_SkinnedDecalsMeshRenderer)
        {
            Mesh sharedMesh;
            if (a_SkinnedDecalsMeshRenderer.SkinnedMeshRenderer.sharedMesh != null)
            {
                sharedMesh = a_SkinnedDecalsMeshRenderer.SkinnedMeshRenderer.sharedMesh;
                sharedMesh.Clear();
            }
            else
            {
                sharedMesh = new Mesh {
                    name = "Skinned Decal Mesh"
                };
                a_SkinnedDecalsMeshRenderer.SkinnedMeshRenderer.sharedMesh = sharedMesh;
            }
            return sharedMesh;
        }

        public override void OnEnable()
        {
            this.InitializeDecalsMeshRenderers();
            if (this.m_SkinnedDecalsMeshRenderers.Count == 0)
            {
                this.PushSkinnedDecalsMeshRenderer();
            }
        }

        public override void OptimizeDecalsMeshes()
        {
            base.OptimizeDecalsMeshes();
            foreach (SkinnedDecalsMeshRenderer renderer in this.m_SkinnedDecalsMeshRenderers)
            {
                if ((renderer.SkinnedMeshRenderer != null) && (renderer.SkinnedMeshRenderer.sharedMesh == null))
                {
                }
            }
        }

        private void PopSkinnedDecalsMeshRenderer()
        {
            SkinnedDecalsMeshRenderer renderer = this.m_SkinnedDecalsMeshRenderers[this.m_SkinnedDecalsMeshRenderers.Count - 1];
            if (Application.isPlaying)
            {
                Destroy(renderer.SkinnedMeshRenderer.sharedMesh);
                Destroy(renderer.gameObject);
            }
            this.m_SkinnedDecalsMeshRenderers.RemoveAt(this.m_SkinnedDecalsMeshRenderers.Count - 1);
        }

        private void PushSkinnedDecalsMeshRenderer()
        {
            GameObject obj2 = new GameObject("Decals Mesh Renderer");
            Transform transform = obj2.transform;
            transform.parent = base.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            SkinnedDecalsMeshRenderer item = this.AddSkinnedDecalsMeshRendererComponentToGameObject(obj2);
            item.SkinnedMeshRenderer.material = base.CurrentMaterial;
            this.m_SkinnedDecalsMeshRenderers.Add(item);
        }

        public override void UpdateDecalsMeshes(SkinnedDecalsMesh a_DecalsMesh)
        {
            base.UpdateDecalsMeshes(a_DecalsMesh);
            if (a_DecalsMesh.Vertices.Count <= 0xffff)
            {
                if (this.m_SkinnedDecalsMeshRenderers.Count == 0)
                {
                    this.PushSkinnedDecalsMeshRenderer();
                }
                else if (this.m_SkinnedDecalsMeshRenderers.Count > 1)
                {
                    while (this.m_SkinnedDecalsMeshRenderers.Count > 1)
                    {
                        this.PopSkinnedDecalsMeshRenderer();
                    }
                }
                SkinnedDecalsMeshRenderer renderer = this.m_SkinnedDecalsMeshRenderers[0];
                this.ApplyToSkinnedDecalsMeshRenderer(renderer, a_DecalsMesh);
            }
            else
            {
                int num = 0;
                int num2 = 0;
                while (true)
                {
                    if (num2 >= a_DecalsMesh.Projectors.Count)
                    {
                        while ((num + 1) < this.m_SkinnedDecalsMeshRenderers.Count)
                        {
                            this.PopSkinnedDecalsMeshRenderer();
                        }
                        break;
                    }
                    GenericDecalProjectorBase base2 = a_DecalsMesh.Projectors[num2];
                    GenericDecalProjectorBase base3 = a_DecalsMesh.Projectors[num2];
                    if (num >= this.m_SkinnedDecalsMeshRenderers.Count)
                    {
                        this.PushSkinnedDecalsMeshRenderer();
                    }
                    SkinnedDecalsMeshRenderer renderer2 = this.m_SkinnedDecalsMeshRenderers[num];
                    int num3 = 0;
                    int num4 = num2;
                    GenericDecalProjectorBase base4 = a_DecalsMesh.Projectors[num2];
                    while (true)
                    {
                        if ((num2 >= a_DecalsMesh.Projectors.Count) || ((num3 + base4.DecalsMeshVertexCount) > 0xffff))
                        {
                            if (num4 != num2)
                            {
                                this.ApplyToSkinnedDecalsMeshRenderer(renderer2, a_DecalsMesh, base2, base3);
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

        public override void UpdateProjectedUV2s(SkinnedDecalsMesh a_DecalsMesh)
        {
            base.UpdateProjectedUV2s(a_DecalsMesh);
            int num = 0;
            int num2 = 0;
            for (int i = 0; i < this.m_SkinnedDecalsMeshRenderers.Count; i++)
            {
                SkinnedDecalsMeshRenderer renderer = this.m_SkinnedDecalsMeshRenderers[i];
                Mesh sharedMesh = renderer.SkinnedMeshRenderer.sharedMesh;
                num2 = (num2 + sharedMesh.vertexCount) - 1;
                Vector2[] vectorArray = a_DecalsMesh.PreservedProjectedUV2Arrays[i];
                CopyListRangeToArray<Vector2>(ref vectorArray, a_DecalsMesh.UV2s, num, num2);
                sharedMesh.uv2 = vectorArray;
                num = num2;
            }
        }

        public override void UpdateProjectedUVs(SkinnedDecalsMesh a_DecalsMesh)
        {
            base.UpdateProjectedUVs(a_DecalsMesh);
            int num = 0;
            int num2 = 0;
            for (int i = 0; i < this.m_SkinnedDecalsMeshRenderers.Count; i++)
            {
                SkinnedDecalsMeshRenderer renderer = this.m_SkinnedDecalsMeshRenderers[i];
                Mesh sharedMesh = renderer.SkinnedMeshRenderer.sharedMesh;
                num2 = (num2 + sharedMesh.vertexCount) - 1;
                Vector2[] vectorArray = a_DecalsMesh.PreservedProjectedUVArrays[i];
                CopyListRangeToArray<Vector2>(ref vectorArray, a_DecalsMesh.UVs, num, num2);
                sharedMesh.uv = vectorArray;
                num = num2;
            }
        }

        [Obsolete("UpdateSkinnedDecalsMeshes is deprecated, please use UpdateDecalsMeshes instead.")]
        public void UpdateSkinnedDecalsMeshes(SkinnedDecalsMesh a_SkinnedDecalsMesh)
        {
            this.UpdateDecalsMeshes(a_SkinnedDecalsMesh);
        }

        public override void UpdateVertexColors(SkinnedDecalsMesh a_DecalsMesh)
        {
            base.UpdateVertexColors(a_DecalsMesh);
            int num = 0;
            int num2 = 0;
            for (int i = 0; i < this.m_SkinnedDecalsMeshRenderers.Count; i++)
            {
                SkinnedDecalsMeshRenderer renderer = this.m_SkinnedDecalsMeshRenderers[i];
                Mesh sharedMesh = renderer.SkinnedMeshRenderer.sharedMesh;
                num2 = (num2 + sharedMesh.vertexCount) - 1;
                Color[] colorArray = a_DecalsMesh.PreservedVertexColorArrays[i];
                CopyListRangeToArray<Color>(ref colorArray, a_DecalsMesh.VertexColors, num, num2);
                sharedMesh.colors = colorArray;
                num = num2;
            }
        }

        public override bool CastShadows
        {
            get => 
                this.SkinnedDecalsMeshRenderers[0].SkinnedMeshRenderer.castShadows;
            set
            {
                foreach (SkinnedDecalsMeshRenderer renderer in this.SkinnedDecalsMeshRenderers)
                {
                    renderer.SkinnedMeshRenderer.castShadows = value;
                }
            }
        }

        public override bool ReceiveShadows
        {
            get => 
                this.SkinnedDecalsMeshRenderers[0].SkinnedMeshRenderer.receiveShadows;
            set
            {
                foreach (SkinnedDecalsMeshRenderer renderer in this.SkinnedDecalsMeshRenderers)
                {
                    renderer.SkinnedMeshRenderer.receiveShadows = value;
                }
            }
        }

        public override bool UseLightProbes
        {
            get => 
                this.SkinnedDecalsMeshRenderers[0].SkinnedMeshRenderer.useLightProbes;
            set
            {
                foreach (SkinnedDecalsMeshRenderer renderer in this.SkinnedDecalsMeshRenderers)
                {
                    renderer.SkinnedMeshRenderer.useLightProbes = value;
                }
            }
        }

        public override Transform LightProbeAnchor
        {
            get => 
                this.SkinnedDecalsMeshRenderers[0].SkinnedMeshRenderer.probeAnchor;
            set
            {
                foreach (SkinnedDecalsMeshRenderer renderer in this.SkinnedDecalsMeshRenderers)
                {
                    renderer.SkinnedMeshRenderer.probeAnchor = value;
                }
            }
        }

        public SkinnedDecalsMeshRenderer[] SkinnedDecalsMeshRenderers =>
            this.m_SkinnedDecalsMeshRenderers.ToArray();
    }
}

