namespace Edelweiss.DecalSystem
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class SkinnedDecalsMesh : GenericDecalsMesh<SkinnedDecals, SkinnedDecalProjectorBase, SkinnedDecalsMesh>
    {
        private List<Vector3> m_OriginalVertices;
        private List<BoneWeight> m_BoneWeights;
        private List<Transform> l_Bones;
        private List<Matrix4x4> m_BindPoses;

        public SkinnedDecalsMesh()
        {
            this.m_OriginalVertices = new List<Vector3>();
            this.m_BoneWeights = new List<BoneWeight>();
            this.l_Bones = new List<Transform>();
            this.m_BindPoses = new List<Matrix4x4>();
        }

        public SkinnedDecalsMesh(SkinnedDecals a_Decals)
        {
            this.m_OriginalVertices = new List<Vector3>();
            this.m_BoneWeights = new List<BoneWeight>();
            this.l_Bones = new List<Transform>();
            this.m_BindPoses = new List<Matrix4x4>();
            base.m_Decals = a_Decals;
        }

        public void Add(Mesh a_Mesh, Transform[] a_Bones, SkinQuality a_SkinQuality, Matrix4x4 a_WorldToMeshMatrix, Matrix4x4 a_MeshToWorldMatrix)
        {
            SkinnedDecalProjectorBase activeDecalProjector = base.ActiveDecalProjector;
            if (activeDecalProjector == null)
            {
                throw new NullReferenceException("The active decal projector is not allowed to be null as mesh data should be added!");
            }
            if ((base.m_Decals.CurrentUVMode == UVMode.Project) || (base.m_Decals.CurrentUVMode == UVMode.ProjectWrapped))
            {
                if (Edition.IsDecalSystemFree && (base.m_Decals.CurrentTextureAtlasType == TextureAtlasType.Reference))
                {
                    throw new InvalidOperationException("Texture atlas references can only be used with Decal System Pro.");
                }
                if ((0 > activeDecalProjector.UV1RectangleIndex) || (activeDecalProjector.UV1RectangleIndex >= base.m_Decals.uvRectangles.Length))
                {
                    throw new IndexOutOfRangeException("The uv rectangle index of the active projector is not a valid index within the decals uv rectangles array!");
                }
            }
            if ((base.m_Decals.CurrentUV2Mode == UV2Mode.Project) || (base.m_Decals.CurrentUV2Mode == UV2Mode.ProjectWrapped))
            {
                if (Edition.IsDecalSystemFree && (base.m_Decals.CurrentTextureAtlasType == TextureAtlasType.Reference))
                {
                    throw new InvalidOperationException("Texture atlas references can only be used with Decal System Pro.");
                }
                if ((0 > activeDecalProjector.UV2RectangleIndex) || (activeDecalProjector.UV2RectangleIndex >= base.m_Decals.uv2Rectangles.Length))
                {
                    throw new IndexOutOfRangeException("The uv2 rectangle index of the active projector is not a valid index within the decals uv2 rectangles array!");
                }
            }
            if (a_Mesh == null)
            {
                throw new ArgumentNullException("The mesh parameter is not allowed to be null!");
            }
            if (!a_Mesh.isReadable)
            {
                throw new ArgumentException("The mesh is not readable!");
            }
            Vector3[] vertices = a_Mesh.vertices;
            Vector3[] normals = a_Mesh.normals;
            Vector4[] tangents = null;
            Color[] colors = null;
            Vector2[] uv = null;
            Vector2[] uv = null;
            BoneWeight[] boneWeights = a_Mesh.boneWeights;
            int[] triangles = a_Mesh.triangles;
            Transform[] transformArray = a_Bones;
            Matrix4x4[] bindposes = a_Mesh.bindposes;
            if (triangles == null)
            {
                throw new NullReferenceException("The triangles in the mesh are null!");
            }
            if (transformArray == null)
            {
                throw new NullReferenceException("The bones are null!");
            }
            if (bindposes == null)
            {
                throw new NullReferenceException("The bind poses in the mesh are null!");
            }
            if (transformArray.Length != bindposes.Length)
            {
                throw new FormatException("The number of bind poses in the mesh does not match the number of bones!");
            }
            if (vertices != null)
            {
                bool flag = false;
                if ((normals == null) || (normals.Length == 0))
                {
                    flag = true;
                    a_Mesh.RecalculateNormals();
                    normals = a_Mesh.normals;
                }
                else if (vertices.Length != normals.Length)
                {
                    throw new FormatException("The number of vertices in the mesh does not match the number of normals!");
                }
                if (base.m_Decals.CurrentTangentsMode == TangentsMode.Target)
                {
                    tangents = a_Mesh.tangents;
                    if (tangents == null)
                    {
                        throw new NullReferenceException("The tangents in the mesh are null!");
                    }
                    if (vertices.Length != tangents.Length)
                    {
                        throw new FormatException("The number of vertices in the mesh does not match the number of tangents!");
                    }
                }
                if (base.m_Decals.UseVertexColors)
                {
                    colors = a_Mesh.colors;
                    if ((colors != null) && ((colors.Length != 0) && (vertices.Length != colors.Length)))
                    {
                        throw new FormatException("The number of vertices in the mesh does not match the number of colors!");
                    }
                }
                if (base.m_Decals.CurrentUVMode == UVMode.TargetUV)
                {
                    uv = a_Mesh.uv;
                    if (uv == null)
                    {
                        throw new NullReferenceException("The uv's in the mesh are null!");
                    }
                    if (vertices.Length != uv.Length)
                    {
                        throw new FormatException("The number of vertices in the mesh does not match the number of uv's!");
                    }
                }
                else if (base.m_Decals.CurrentUVMode == UVMode.TargetUV2)
                {
                    uv = a_Mesh.uv2;
                    if (uv == null)
                    {
                        throw new NullReferenceException("The uv2's in the mesh are null!");
                    }
                    if (vertices.Length != uv.Length)
                    {
                        throw new FormatException("The number of vertices in the mesh does not match the number of uv2's!");
                    }
                }
                if (base.m_Decals.CurrentUV2Mode == UV2Mode.TargetUV)
                {
                    uv = a_Mesh.uv;
                    if (uv == null)
                    {
                        throw new NullReferenceException("The uv's in the mesh are null!");
                    }
                    if (vertices.Length != uv.Length)
                    {
                        throw new FormatException("The number of vertices in the mesh does not match the number of uv's!");
                    }
                }
                else if (base.m_Decals.CurrentUV2Mode == UV2Mode.TargetUV2)
                {
                    uv = a_Mesh.uv2;
                    if (uv == null)
                    {
                        throw new NullReferenceException("The uv2's in the mesh are null!");
                    }
                    if (vertices.Length != uv.Length)
                    {
                        throw new FormatException("The number of vertices in the mesh does not match the number of uv2's!");
                    }
                }
                this.Add(vertices, normals, tangents, uv, uv, boneWeights, triangles, transformArray, bindposes, a_SkinQuality, a_WorldToMeshMatrix, a_MeshToWorldMatrix);
                if (flag)
                {
                    a_Mesh.normals = null;
                }
            }
        }

        public void Add(Vector3[] a_Vertices, Vector3[] a_Normals, Vector4[] a_Tangents, Vector2[] a_UVs, Vector2[] a_UV2s, BoneWeight[] a_BoneWeights, int[] a_Triangles, Transform[] a_Bones, Matrix4x4[] a_BindPoses, SkinQuality a_SkinQuality, Matrix4x4 a_WorldToMeshMatrix, Matrix4x4 a_MeshToWorldMatrix)
        {
            try
            {
                this.Add(a_Vertices, a_Normals, a_Tangents, null, a_UVs, a_UV2s, a_BoneWeights, a_Triangles, a_Bones, a_BindPoses, a_SkinQuality, a_WorldToMeshMatrix, a_MeshToWorldMatrix);
            }
            catch
            {
                throw;
            }
        }

        public void Add(Vector3[] a_Vertices, Vector3[] a_Normals, Vector4[] a_Tangents, Color[] a_Colors, Vector2[] a_UVs, Vector2[] a_UV2s, BoneWeight[] a_BoneWeights, int[] a_Triangles, Transform[] a_Bones, Matrix4x4[] a_BindPoses, SkinQuality a_SkinQuality, Matrix4x4 a_WorldToMeshMatrix, Matrix4x4 a_MeshToWorldMatrix)
        {
            SkinnedDecalProjectorBase activeDecalProjector = base.ActiveDecalProjector;
            if (activeDecalProjector == null)
            {
                throw new NullReferenceException("The active decal projector is not allowed to be null as mesh data should be added!");
            }
            if ((base.m_Decals.CurrentUVMode == UVMode.Project) || (base.m_Decals.CurrentUVMode == UVMode.ProjectWrapped))
            {
                if (Edition.IsDecalSystemFree && (base.m_Decals.CurrentTextureAtlasType == TextureAtlasType.Reference))
                {
                    throw new InvalidOperationException("Texture atlas references can only be used with Decal System Pro.");
                }
                if ((0 > activeDecalProjector.UV1RectangleIndex) || (activeDecalProjector.UV1RectangleIndex >= base.m_Decals.uvRectangles.Length))
                {
                    throw new IndexOutOfRangeException("The uv rectangle index of the active projector is not a valid index within the decals uv rectangles array!");
                }
            }
            if ((base.m_Decals.CurrentUV2Mode == UV2Mode.Project) || (base.m_Decals.CurrentUV2Mode == UV2Mode.ProjectWrapped))
            {
                if (Edition.IsDecalSystemFree && (base.m_Decals.CurrentTextureAtlasType == TextureAtlasType.Reference))
                {
                    throw new InvalidOperationException("Texture atlas references can only be used with Decal System Pro.");
                }
                if ((0 > activeDecalProjector.UV2RectangleIndex) || (activeDecalProjector.UV2RectangleIndex >= base.m_Decals.uv2Rectangles.Length))
                {
                    throw new IndexOutOfRangeException("The uv2 rectangle index of the active projector is not a valid index within the decals uv2 rectangles array!");
                }
            }
            if (base.m_Decals.UseVertexColors && Edition.IsDecalSystemFree)
            {
                throw new InvalidOperationException("Vertex colors are only supported in Decal System Pro.");
            }
            if (a_Vertices == null)
            {
                throw new ArgumentNullException("The vertices parameter is not allowed to be null!");
            }
            if (a_Normals == null)
            {
                throw new ArgumentNullException("The normals parameter is not allowed to be null!");
            }
            if (a_Triangles == null)
            {
                throw new ArgumentNullException("The triangles parameter is not allowed to be null!");
            }
            if (a_Vertices.Length != a_Normals.Length)
            {
                throw new FormatException("The number of vertices does not match the number of normals!");
            }
            if (a_Bones == null)
            {
                throw new NullReferenceException("The bones are null!");
            }
            if (a_BindPoses == null)
            {
                throw new NullReferenceException("The bind poses are null!");
            }
            if (a_Bones.Length != a_BindPoses.Length)
            {
                throw new FormatException("The number of bind poses does not match the number of bones!");
            }
            if (base.m_Decals.CurrentTangentsMode == TangentsMode.Target)
            {
                if (a_Tangents == null)
                {
                    throw new ArgumentNullException("The tangents parameter is not allowed to be null!");
                }
                if (a_Vertices.Length != a_Tangents.Length)
                {
                    throw new FormatException("The number of vertices does not match the number of tangents!");
                }
            }
            if ((base.m_Decals.CurrentUVMode == UVMode.TargetUV) || (base.m_Decals.CurrentUVMode == UVMode.TargetUV2))
            {
                if (a_UVs == null)
                {
                    throw new ArgumentNullException("The uv parameter is not allowed to be null!");
                }
                if (a_Vertices.Length != a_UVs.Length)
                {
                    throw new FormatException("The number of vertices does not match the number of uv's!");
                }
            }
            else if ((base.m_Decals.CurrentUV2Mode == UV2Mode.TargetUV) || (base.m_Decals.CurrentUV2Mode == UV2Mode.TargetUV2))
            {
                if (a_UV2s == null)
                {
                    throw new NullReferenceException("The uv2 parameter is not allowed to be null!");
                }
                if (a_Vertices.Length != a_UV2s.Length)
                {
                    throw new FormatException("The number of vertices does not match the number of uv2's!");
                }
            }
            if (base.m_Decals.UseVertexColors && ((a_Colors != null) && ((a_Colors.Length != 0) && (a_Vertices.Length != a_Colors.Length))))
            {
                throw new FormatException("The number of vertices does not macht the number of colors!");
            }
            Vector3 v = new Vector3(0f, 1f, 0f);
            Matrix4x4 worldToLocalMatrix = base.m_Decals.CachedTransform.worldToLocalMatrix;
            v = (worldToLocalMatrix * activeDecalProjector.ProjectorToWorldMatrix).inverse.transpose.MultiplyVector(v).normalized;
            Matrix4x4 matrixx5 = worldToLocalMatrix * a_MeshToWorldMatrix;
            Matrix4x4 transpose = matrixx5.inverse.transpose;
            float num = Mathf.Cos(activeDecalProjector.CullingAngle * 0.01745329f);
            int count = base.m_Vertices.Count;
            int num3 = this.l_Bones.Count;
            for (int i = 0; i < a_Bones.Length; i++)
            {
                this.l_Bones.Add(a_Bones[i]);
                this.m_BindPoses.Add(a_BindPoses[i] * matrixx5.inverse);
            }
            this.InternalAdd(a_Vertices, a_Normals, a_Tangents, a_Colors, a_UVs, a_UV2s, a_BoneWeights, a_Triangles, num3, a_BindPoses, a_SkinQuality, v, num, worldToLocalMatrix, matrixx5, transpose);
            if (a_WorldToMeshMatrix.Determinant() >= 0f)
            {
                for (int j = 0; j < a_Triangles.Length; j += 3)
                {
                    int num7 = a_Triangles[j];
                    int num8 = a_Triangles[j + 1];
                    int num9 = a_Triangles[j + 2];
                    if (!base.m_RemovedIndices.IsRemovedIndex(num7) && (!base.m_RemovedIndices.IsRemovedIndex(num8) && !base.m_RemovedIndices.IsRemovedIndex(num9)))
                    {
                        num7 = count + base.m_RemovedIndices.AdjustedIndex(num7);
                        num8 = count + base.m_RemovedIndices.AdjustedIndex(num8);
                        num9 = count + base.m_RemovedIndices.AdjustedIndex(num9);
                        base.m_Triangles.Add(num7);
                        base.m_Triangles.Add(num8);
                        base.m_Triangles.Add(num9);
                    }
                }
            }
            else
            {
                for (int j = 0; j < a_Triangles.Length; j += 3)
                {
                    int num11 = a_Triangles[j];
                    int num12 = a_Triangles[j + 1];
                    int num13 = a_Triangles[j + 2];
                    if (!base.m_RemovedIndices.IsRemovedIndex(num11) && (!base.m_RemovedIndices.IsRemovedIndex(num12) && !base.m_RemovedIndices.IsRemovedIndex(num13)))
                    {
                        num11 = count + base.m_RemovedIndices.AdjustedIndex(num11);
                        num12 = count + base.m_RemovedIndices.AdjustedIndex(num12);
                        num13 = count + base.m_RemovedIndices.AdjustedIndex(num13);
                        base.m_Triangles.Add(num12);
                        base.m_Triangles.Add(num11);
                        base.m_Triangles.Add(num13);
                    }
                }
            }
            base.m_RemovedIndices.Clear();
            activeDecalProjector.DecalsMeshUpperVertexIndex = base.m_Vertices.Count - 1;
            activeDecalProjector.DecalsMeshUpperTriangleIndex = base.m_Triangles.Count - 1;
            activeDecalProjector.DecalsMeshUpperBonesIndex = this.l_Bones.Count - 1;
            activeDecalProjector.IsTangentProjectionCalculated = false;
            activeDecalProjector.IsUV1ProjectionCalculated = false;
            activeDecalProjector.IsUV2ProjectionCalculated = false;
        }

        private unsafe void AddSkinQuality1(Vector3[] a_Vertices, Vector3[] a_Normals, Vector4[] a_Tangents, Color[] a_Colors, Vector2[] a_UVs, Vector2[] a_UV2s, BoneWeight[] a_BoneWeights, int[] a_Triangles, int a_BoneIndexOffset, Matrix4x4[] a_BindPoses, Vector3 a_ReversedProjectionNormal, float a_CullingDotProduct, Matrix4x4 a_WorldToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrixInvertedTransposed)
        {
            Color vertexColorTint = base.m_Decals.VertexColorTint;
            Color color2 = (Color) ((1f - base.ActiveDecalProjector.VertexColorBlending) * base.ActiveDecalProjector.VertexColor);
            int index = 0;
            while (index < a_Vertices.Length)
            {
                BoneWeight item = a_BoneWeights[index];
                item.boneIndex0 += a_BoneIndexOffset;
                item.weight0 = 1f;
                item.weight1 = 0f;
                item.weight2 = 0f;
                item.weight3 = 0f;
                Matrix4x4 matrixx = this.l_Bones[item.boneIndex0].localToWorldMatrix * this.m_BindPoses[item.boneIndex0];
                Matrix4x4 matrixx2 = new Matrix4x4();
                int num2 = 0;
                while (true)
                {
                    if (num2 >= 0x10)
                    {
                        matrixx2 = (a_WorldToDecalsMatrix * matrixx2) * a_MeshToDecalsMatrix;
                        Matrix4x4 transpose = matrixx2.inverse.transpose;
                        Vector3 normalized = a_MeshToDecalsMatrixInvertedTransposed.MultiplyVector(a_Normals[index]).normalized;
                        Vector3 rhs = transpose.MultiplyVector(a_Normals[index]).normalized;
                        if (a_CullingDotProduct > Vector3.Dot(a_ReversedProjectionNormal, rhs))
                        {
                            base.m_RemovedIndices.AddRemovedIndex(index);
                        }
                        else
                        {
                            Vector3 vector5 = a_MeshToDecalsMatrix.MultiplyPoint3x4(a_Vertices[index]);
                            Vector3 vector6 = matrixx2.MultiplyPoint3x4(a_Vertices[index]);
                            base.m_Vertices.Add(vector6);
                            this.m_OriginalVertices.Add(vector5);
                            base.m_Normals.Add(normalized);
                            if (base.m_Decals.CurrentTangentsMode == TangentsMode.Target)
                            {
                                Vector4 vector7 = a_MeshToDecalsMatrixInvertedTransposed.MultiplyVector(*((Vector3*) &(a_Tangents[index]))).normalized;
                                base.m_Tangents.Add(vector7);
                            }
                            if (base.m_Decals.UseVertexColors)
                            {
                                Color white = Color.white;
                                if ((a_Colors != null) && (a_Colors.Length != 0))
                                {
                                    white = a_Colors[index];
                                }
                                base.m_TargetVertexColors.Add(white);
                                Color color4 = vertexColorTint * (color2 + (base.ActiveDecalProjector.VertexColorBlending * white));
                                base.m_VertexColors.Add(color4);
                            }
                            if ((base.m_Decals.CurrentUVMode == UVMode.TargetUV) || (base.m_Decals.CurrentUVMode == UVMode.TargetUV2))
                            {
                                base.m_UVs.Add(a_UVs[index]);
                            }
                            if ((base.m_Decals.CurrentUV2Mode == UV2Mode.TargetUV) || (base.m_Decals.CurrentUV2Mode == UV2Mode.TargetUV2))
                            {
                                base.m_UV2s.Add(a_UV2s[index]);
                            }
                            this.m_BoneWeights.Add(item);
                        }
                        index++;
                        break;
                    }
                    matrixx2[num2] = matrixx[num2] * item.weight0;
                    num2++;
                }
            }
        }

        private unsafe void AddSkinQuality2(Vector3[] a_Vertices, Vector3[] a_Normals, Vector4[] a_Tangents, Color[] a_Colors, Vector2[] a_UVs, Vector2[] a_UV2s, BoneWeight[] a_BoneWeights, int[] a_Triangles, int a_BoneIndexOffset, Matrix4x4[] a_BindPoses, Vector3 a_ReversedProjectionNormal, float a_CullingDotProduct, Matrix4x4 a_WorldToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrixInvertedTransposed)
        {
            Color vertexColorTint = base.m_Decals.VertexColorTint;
            Color color2 = (Color) ((1f - base.ActiveDecalProjector.VertexColorBlending) * base.ActiveDecalProjector.VertexColor);
            int index = 0;
            while (index < a_Vertices.Length)
            {
                BoneWeight item = a_BoneWeights[index];
                item.boneIndex0 += a_BoneIndexOffset;
                item.boneIndex1 += a_BoneIndexOffset;
                float num2 = 1f / (item.weight0 + item.weight1);
                item.weight0 *= num2;
                item.weight1 *= num2;
                item.weight2 = 0f;
                item.weight3 = 0f;
                Matrix4x4 matrixx = this.l_Bones[item.boneIndex0].localToWorldMatrix * this.m_BindPoses[item.boneIndex0];
                Matrix4x4 matrixx2 = this.l_Bones[item.boneIndex1].localToWorldMatrix * this.m_BindPoses[item.boneIndex1];
                Matrix4x4 matrixx3 = new Matrix4x4();
                int num3 = 0;
                while (true)
                {
                    if (num3 >= 0x10)
                    {
                        matrixx3 = (a_WorldToDecalsMatrix * matrixx3) * a_MeshToDecalsMatrix;
                        Matrix4x4 transpose = matrixx3.inverse.transpose;
                        Vector3 normalized = a_MeshToDecalsMatrixInvertedTransposed.MultiplyVector(a_Normals[index]).normalized;
                        Vector3 rhs = transpose.MultiplyVector(a_Normals[index]).normalized;
                        if (a_CullingDotProduct > Vector3.Dot(a_ReversedProjectionNormal, rhs))
                        {
                            base.m_RemovedIndices.AddRemovedIndex(index);
                        }
                        else
                        {
                            Vector3 vector5 = a_MeshToDecalsMatrix.MultiplyPoint3x4(a_Vertices[index]);
                            Vector3 vector6 = matrixx3.MultiplyPoint3x4(a_Vertices[index]);
                            base.m_Vertices.Add(vector6);
                            this.m_OriginalVertices.Add(vector5);
                            base.m_Normals.Add(normalized);
                            if (base.m_Decals.CurrentTangentsMode == TangentsMode.Target)
                            {
                                Vector4 vector7 = a_MeshToDecalsMatrixInvertedTransposed.MultiplyVector(*((Vector3*) &(a_Tangents[index]))).normalized;
                                base.m_Tangents.Add(vector7);
                            }
                            if (base.m_Decals.UseVertexColors)
                            {
                                Color white = Color.white;
                                if ((a_Colors != null) && (a_Colors.Length != 0))
                                {
                                    white = a_Colors[index];
                                }
                                base.m_TargetVertexColors.Add(white);
                                Color color4 = vertexColorTint * (color2 + (base.ActiveDecalProjector.VertexColorBlending * white));
                                base.m_VertexColors.Add(color4);
                            }
                            if ((base.m_Decals.CurrentUVMode == UVMode.TargetUV) || (base.m_Decals.CurrentUVMode == UVMode.TargetUV2))
                            {
                                base.m_UVs.Add(a_UVs[index]);
                            }
                            if ((base.m_Decals.CurrentUV2Mode == UV2Mode.TargetUV) || (base.m_Decals.CurrentUV2Mode == UV2Mode.TargetUV2))
                            {
                                base.m_UV2s.Add(a_UV2s[index]);
                            }
                            this.m_BoneWeights.Add(item);
                        }
                        index++;
                        break;
                    }
                    matrixx3[num3] = (matrixx[num3] * item.weight0) + (matrixx2[num3] * item.weight1);
                    num3++;
                }
            }
        }

        private unsafe void AddSkinQuality4(Vector3[] a_Vertices, Vector3[] a_Normals, Vector4[] a_Tangents, Color[] a_Colors, Vector2[] a_UVs, Vector2[] a_UV2s, BoneWeight[] a_BoneWeights, int[] a_Triangles, int a_BoneIndexOffset, Matrix4x4[] a_BindPoses, Vector3 a_ReversedProjectionNormal, float a_CullingDotProduct, Matrix4x4 a_WorldToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrixInvertedTransposed)
        {
            Color vertexColorTint = base.m_Decals.VertexColorTint;
            Color color2 = (Color) ((1f - base.ActiveDecalProjector.VertexColorBlending) * base.ActiveDecalProjector.VertexColor);
            int index = 0;
            while (index < a_Vertices.Length)
            {
                BoneWeight item = a_BoneWeights[index];
                item.boneIndex0 += a_BoneIndexOffset;
                item.boneIndex1 += a_BoneIndexOffset;
                item.boneIndex2 += a_BoneIndexOffset;
                item.boneIndex3 += a_BoneIndexOffset;
                Matrix4x4 matrixx = this.l_Bones[item.boneIndex0].localToWorldMatrix * this.m_BindPoses[item.boneIndex0];
                Matrix4x4 matrixx2 = this.l_Bones[item.boneIndex1].localToWorldMatrix * this.m_BindPoses[item.boneIndex1];
                Matrix4x4 matrixx3 = this.l_Bones[item.boneIndex2].localToWorldMatrix * this.m_BindPoses[item.boneIndex2];
                Matrix4x4 matrixx4 = this.l_Bones[item.boneIndex3].localToWorldMatrix * this.m_BindPoses[item.boneIndex3];
                Matrix4x4 matrixx5 = new Matrix4x4();
                int num2 = 0;
                while (true)
                {
                    if (num2 >= 0x10)
                    {
                        matrixx5 = (a_WorldToDecalsMatrix * matrixx5) * a_MeshToDecalsMatrix;
                        Matrix4x4 transpose = matrixx5.inverse.transpose;
                        Vector3 normalized = a_MeshToDecalsMatrixInvertedTransposed.MultiplyVector(a_Normals[index]).normalized;
                        Vector3 rhs = transpose.MultiplyVector(a_Normals[index]).normalized;
                        if (a_CullingDotProduct > Vector3.Dot(a_ReversedProjectionNormal, rhs))
                        {
                            base.m_RemovedIndices.AddRemovedIndex(index);
                        }
                        else
                        {
                            Vector3 vector5 = a_MeshToDecalsMatrix.MultiplyPoint3x4(a_Vertices[index]);
                            Vector3 vector6 = matrixx5.MultiplyPoint3x4(a_Vertices[index]);
                            base.m_Vertices.Add(vector6);
                            this.m_OriginalVertices.Add(vector5);
                            base.m_Normals.Add(normalized);
                            if (base.m_Decals.CurrentTangentsMode == TangentsMode.Target)
                            {
                                Vector4 vector7 = a_MeshToDecalsMatrixInvertedTransposed.MultiplyVector(*((Vector3*) &(a_Tangents[index]))).normalized;
                                base.m_Tangents.Add(vector7);
                            }
                            if (base.m_Decals.UseVertexColors)
                            {
                                Color white = Color.white;
                                if ((a_Colors != null) && (a_Colors.Length != 0))
                                {
                                    white = a_Colors[index];
                                }
                                base.m_TargetVertexColors.Add(white);
                                Color color4 = vertexColorTint * (color2 + (base.ActiveDecalProjector.VertexColorBlending * white));
                                base.m_VertexColors.Add(color4);
                            }
                            if ((base.m_Decals.CurrentUVMode == UVMode.TargetUV) || (base.m_Decals.CurrentUVMode == UVMode.TargetUV2))
                            {
                                base.m_UVs.Add(a_UVs[index]);
                            }
                            if ((base.m_Decals.CurrentUV2Mode == UV2Mode.TargetUV) || (base.m_Decals.CurrentUV2Mode == UV2Mode.TargetUV2))
                            {
                                base.m_UV2s.Add(a_UV2s[index]);
                            }
                            this.m_BoneWeights.Add(item);
                        }
                        index++;
                        break;
                    }
                    matrixx5[num2] = (((matrixx[num2] * item.weight0) + (matrixx2[num2] * item.weight1)) + (matrixx3[num2] * item.weight2)) + (matrixx4[num2] * item.weight3);
                    num2++;
                }
            }
        }

        private int AdjustedIndex(int a_Index, int a_UpperIndex, int a_Offset)
        {
            int num = a_Index;
            if (num > a_UpperIndex)
            {
                num -= a_Offset;
            }
            return num;
        }

        internal override void AdjustProjectorIndices(SkinnedDecalProjectorBase a_Projector, int a_VertexIndexOffset, int a_TriangleIndexOffset, int a_BoneIndexOffset)
        {
            base.AdjustProjectorIndices(a_Projector, a_VertexIndexOffset, a_TriangleIndexOffset, a_BoneIndexOffset);
            a_Projector.DecalsMeshLowerBonesIndex -= a_BoneIndexOffset;
            a_Projector.DecalsMeshUpperBonesIndex -= a_BoneIndexOffset;
        }

        internal override int BoneIndexOffset(SkinnedDecalProjectorBase a_Projector) => 
            a_Projector.DecalsMeshBonesCount;

        public override void ClearAll()
        {
            base.ClearAll();
            this.OriginalVertices.Clear();
            this.BoneWeights.Clear();
            this.Bones.Clear();
            this.BindPoses.Clear();
        }

        private void InternalAdd(Vector3[] a_Vertices, Vector3[] a_Normals, Vector4[] a_Tangents, Color[] a_Colors, Vector2[] a_UVs, Vector2[] a_UV2s, BoneWeight[] a_BoneWeights, int[] a_Triangles, int a_BoneIndexOffset, Matrix4x4[] a_BindPoses, SkinQuality a_SkinQuality, Vector3 a_ReversedProjectionNormal, float a_CullingDotProduct, Matrix4x4 a_WorldToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrixInvertedTransposed)
        {
            if ((a_SkinQuality == SkinQuality.Bone1) || ((a_SkinQuality == SkinQuality.Auto) && (QualitySettings.blendWeights == BlendWeights.OneBone)))
            {
                this.AddSkinQuality1(a_Vertices, a_Normals, a_Tangents, a_Colors, a_UVs, a_UV2s, a_BoneWeights, a_Triangles, a_BoneIndexOffset, a_BindPoses, a_ReversedProjectionNormal, a_CullingDotProduct, a_WorldToDecalsMatrix, a_MeshToDecalsMatrix, a_MeshToDecalsMatrixInvertedTransposed);
            }
            else if ((a_SkinQuality == SkinQuality.Bone2) || ((a_SkinQuality == SkinQuality.Auto) && (QualitySettings.blendWeights == BlendWeights.TwoBones)))
            {
                this.AddSkinQuality2(a_Vertices, a_Normals, a_Tangents, a_Colors, a_UVs, a_UV2s, a_BoneWeights, a_Triangles, a_BoneIndexOffset, a_BindPoses, a_ReversedProjectionNormal, a_CullingDotProduct, a_WorldToDecalsMatrix, a_MeshToDecalsMatrix, a_MeshToDecalsMatrixInvertedTransposed);
            }
            else if ((a_SkinQuality == SkinQuality.Bone4) || ((a_SkinQuality == SkinQuality.Auto) && (QualitySettings.blendWeights == BlendWeights.FourBones)))
            {
                this.AddSkinQuality4(a_Vertices, a_Normals, a_Tangents, a_Colors, a_UVs, a_UV2s, a_BoneWeights, a_Triangles, a_BoneIndexOffset, a_BindPoses, a_ReversedProjectionNormal, a_CullingDotProduct, a_WorldToDecalsMatrix, a_MeshToDecalsMatrix, a_MeshToDecalsMatrixInvertedTransposed);
            }
        }

        public override void OffsetActiveProjectorVertices()
        {
            SkinnedDecalProjectorBase activeDecalProjector = base.ActiveDecalProjector;
            if (activeDecalProjector == null)
            {
                throw new InvalidOperationException("There is no active projector.");
            }
            float meshOffset = activeDecalProjector.MeshOffset;
            if (!Mathf.Approximately(meshOffset, 0f))
            {
                Matrix4x4 worldToLocalMatrix = base.m_Decals.CachedTransform.worldToLocalMatrix;
                Matrix4x4 transpose = worldToLocalMatrix.inverse.transpose;
                int decalsMeshUpperVertexIndex = activeDecalProjector.DecalsMeshUpperVertexIndex;
                for (int i = activeDecalProjector.DecalsMeshLowerVertexIndex; i <= decalsMeshUpperVertexIndex; i++)
                {
                    Vector3 vector2 = transpose.MultiplyVector(base.m_Normals[i]);
                    Vector3 vector = worldToLocalMatrix.MultiplyVector(vector2.normalized * meshOffset);
                    this.m_OriginalVertices[i] += vector;
                }
            }
        }

        internal override void RemoveBonesAndAdjustBoneWeightIndices(SkinnedDecalProjectorBase a_Projector)
        {
            int decalsMeshLowerBonesIndex = a_Projector.DecalsMeshLowerBonesIndex;
            int decalsMeshUpperBonesIndex = a_Projector.DecalsMeshUpperBonesIndex;
            int decalsMeshBonesCount = a_Projector.DecalsMeshBonesCount;
            if (decalsMeshBonesCount > 0)
            {
                this.l_Bones.RemoveRange(decalsMeshLowerBonesIndex, decalsMeshBonesCount);
                this.m_BindPoses.RemoveRange(decalsMeshLowerBonesIndex, decalsMeshBonesCount);
            }
            for (int i = a_Projector.DecalsMeshLowerVertexIndex; i < this.m_BoneWeights.Count; i++)
            {
                BoneWeight weight = this.m_BoneWeights[i];
                weight.boneIndex0 = this.AdjustedIndex(weight.boneIndex0, decalsMeshUpperBonesIndex, decalsMeshBonesCount);
                weight.boneIndex1 = this.AdjustedIndex(weight.boneIndex1, decalsMeshUpperBonesIndex, decalsMeshBonesCount);
                weight.boneIndex2 = this.AdjustedIndex(weight.boneIndex2, decalsMeshUpperBonesIndex, decalsMeshBonesCount);
                weight.boneIndex3 = this.AdjustedIndex(weight.boneIndex3, decalsMeshUpperBonesIndex, decalsMeshBonesCount);
                this.m_BoneWeights[i] = weight;
            }
        }

        internal override void RemoveRange(int a_StartIndex, int a_Count)
        {
            base.m_Vertices.RemoveRange(a_StartIndex, a_Count);
            base.m_Normals.RemoveRange(a_StartIndex, a_Count);
            this.m_OriginalVertices.RemoveRange(a_StartIndex, a_Count);
            this.m_BoneWeights.RemoveRange(a_StartIndex, a_Count);
            if (base.m_Decals.CurrentTangentsMode == TangentsMode.Target)
            {
                base.m_Tangents.RemoveRange(a_StartIndex, a_Count);
            }
            if (base.m_Decals.UseVertexColors)
            {
                base.m_TargetVertexColors.RemoveRange(a_StartIndex, a_Count);
                base.m_VertexColors.RemoveRange(a_StartIndex, a_Count);
            }
            if ((base.m_Decals.CurrentUVMode == UVMode.TargetUV) || (base.m_Decals.CurrentUVMode == UVMode.TargetUV2))
            {
                base.m_UVs.RemoveRange(a_StartIndex, a_Count);
            }
            if ((base.m_Decals.CurrentUV2Mode == UV2Mode.TargetUV) || (base.m_Decals.CurrentUV2Mode == UV2Mode.TargetUV2))
            {
                base.m_UV2s.RemoveRange(a_StartIndex, a_Count);
            }
        }

        internal override void RemoveRangesOfVertexAlignedLists(int a_LowerIndex, int a_Count)
        {
            base.RemoveRangesOfVertexAlignedLists(a_LowerIndex, a_Count);
            this.m_OriginalVertices.RemoveRange(a_LowerIndex, a_Count);
            this.m_BoneWeights.RemoveRange(a_LowerIndex, a_Count);
        }

        internal override void SetRangesForAddedProjector(SkinnedDecalProjectorBase a_Projector)
        {
            base.SetRangesForAddedProjector(a_Projector);
            a_Projector.DecalsMeshLowerBonesIndex = this.l_Bones.Count;
            a_Projector.DecalsMeshUpperBonesIndex = this.l_Bones.Count - 1;
        }

        public List<Vector3> OriginalVertices =>
            this.m_OriginalVertices;

        public List<BoneWeight> BoneWeights =>
            this.m_BoneWeights;

        public List<Transform> Bones =>
            this.l_Bones;

        public List<Matrix4x4> BindPoses =>
            this.m_BindPoses;
    }
}

