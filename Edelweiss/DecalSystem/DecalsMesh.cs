namespace Edelweiss.DecalSystem
{
    using System;
    using UnityEngine;

    public class DecalsMesh : GenericDecalsMesh<Decals, DecalProjectorBase, DecalsMesh>
    {
        public DecalsMesh()
        {
        }

        public DecalsMesh(Decals a_Decals)
        {
            base.m_Decals = a_Decals;
        }

        public void Add(Terrain a_Terrain, Matrix4x4 a_TerrainToDecalsMatrix)
        {
            DecalProjectorBase activeDecalProjector = base.ActiveDecalProjector;
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
                if ((0 > activeDecalProjector.UV1RectangleIndex) || ((base.m_Decals.CurrentUvRectangles == null) || (activeDecalProjector.UV1RectangleIndex >= base.m_Decals.CurrentUvRectangles.Length)))
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
                if ((0 > activeDecalProjector.UV2RectangleIndex) || ((base.m_Decals.CurrentUv2Rectangles == null) || (activeDecalProjector.UV2RectangleIndex >= base.m_Decals.CurrentUv2Rectangles.Length)))
                {
                    throw new IndexOutOfRangeException("The uv2 rectangle index of the active projector is not a valid index within the decals uv2 rectangles array!");
                }
            }
            if (a_Terrain == null)
            {
                throw new ArgumentNullException("The terrain parameter is not allowed to be null!");
            }
            if (base.m_Decals.CurrentTangentsMode == TangentsMode.Target)
            {
                throw new InvalidOperationException("Terrains don't have tangents!");
            }
            if ((base.m_Decals.CurrentUVMode == UVMode.TargetUV) || (base.m_Decals.CurrentUVMode == UVMode.TargetUV2))
            {
                throw new InvalidOperationException("Terrains don't have uv's!");
            }
            if ((base.m_Decals.CurrentUV2Mode == UV2Mode.TargetUV) || (base.m_Decals.CurrentUV2Mode == UV2Mode.TargetUV2))
            {
                throw new InvalidOperationException("Terrains don't have uv2's!");
            }
            Bounds bounds = activeDecalProjector.WorldBounds();
            bounds.center -= a_Terrain.transform.position;
            TerrainData terrainData = a_Terrain.terrainData;
            Matrix4x4 transpose = a_TerrainToDecalsMatrix.inverse.transpose;
            if (terrainData != null)
            {
                Vector3 min = bounds.min;
                Vector3 max = bounds.max;
                if (min.x > max.x)
                {
                    min.x = max.x;
                    max.x = min.x;
                }
                if (min.z > max.z)
                {
                    min.z = max.z;
                    max.z = min.z;
                }
                Vector3 size = terrainData.size;
                min.x = Mathf.Clamp(min.x, 0f, size.x);
                max.x = Mathf.Clamp(max.x, 0f, size.x);
                min.z = Mathf.Clamp(min.z, 0f, size.z);
                max.z = Mathf.Clamp(max.z, 0f, size.z);
                Vector3 heightmapScale = terrainData.heightmapScale;
                int num3 = Mathf.FloorToInt(min.x / heightmapScale.x);
                int num4 = Mathf.FloorToInt(min.z / heightmapScale.z);
                int num5 = Mathf.CeilToInt(max.x / heightmapScale.x);
                int num6 = Mathf.CeilToInt(max.z / heightmapScale.z);
                int count = base.Vertices.Count;
                int num8 = base.Triangles.Count;
                if ((num3 < num5) && (num4 < num6))
                {
                    Color item = base.m_Decals.VertexColorTint * (((1f - base.ActiveDecalProjector.VertexColorBlending) * base.ActiveDecalProjector.VertexColor) + (base.ActiveDecalProjector.VertexColorBlending * Color.white));
                    float num9 = 1f / ((float) (terrainData.heightmapWidth - 1));
                    float num10 = 1f / ((float) (terrainData.heightmapHeight - 1));
                    int x = num3;
                    while (true)
                    {
                        if (x > num5)
                        {
                            int num17 = (num6 - num4) + 1;
                            int num18 = ((num5 - num3) + 1) - 1;
                            int num19 = num17 - 1;
                            int num20 = 0;
                            while (num20 < num18)
                            {
                                int num21 = 0;
                                while (true)
                                {
                                    if (num21 >= num19)
                                    {
                                        num20++;
                                        break;
                                    }
                                    int num22 = (count + num21) + (num20 * num17);
                                    int num23 = num22 + 1;
                                    int num24 = num22 + num17;
                                    int num25 = num24 + 1;
                                    base.m_Triangles.Add(num22);
                                    base.m_Triangles.Add(num23);
                                    base.m_Triangles.Add(num25);
                                    base.m_Triangles.Add(num22);
                                    base.m_Triangles.Add(num25);
                                    base.m_Triangles.Add(num24);
                                    num21++;
                                }
                            }
                            break;
                        }
                        float num12 = x * heightmapScale.x;
                        int y = num4;
                        while (true)
                        {
                            if (y > num6)
                            {
                                x++;
                                break;
                            }
                            float height = terrainData.GetHeight(x, y);
                            float z = y * heightmapScale.z;
                            Vector3 vector5 = a_TerrainToDecalsMatrix.MultiplyPoint3x4(new Vector3(num12, height, z));
                            Vector3 vector6 = transpose.MultiplyVector(terrainData.GetInterpolatedNormal(x * num9, y * num10));
                            vector6.Normalize();
                            base.m_Vertices.Add(vector5);
                            base.m_Normals.Add(vector6);
                            if (base.m_Decals.UseVertexColors)
                            {
                                base.m_TargetVertexColors.Add(Color.white);
                                base.m_VertexColors.Add(item);
                            }
                            y++;
                        }
                    }
                }
                float a = Mathf.Cos(activeDecalProjector.CullingAngle * 0.01745329f);
                if (!Mathf.Approximately(a, -1f))
                {
                    Vector3 v = new Vector3(0f, 1f, 0f);
                    v = (base.m_Decals.CachedTransform.worldToLocalMatrix * activeDecalProjector.ProjectorToWorldMatrix).inverse.transpose.MultiplyVector(v).normalized;
                    int num27 = count;
                    while (true)
                    {
                        if (num27 >= base.m_Vertices.Count)
                        {
                            int num28 = base.m_Triangles.Count - 1;
                            while (true)
                            {
                                if (num28 < num8)
                                {
                                    base.RemoveIndices(base.m_RemovedIndices);
                                    base.m_RemovedIndices.Clear();
                                    break;
                                }
                                int num29 = base.m_Triangles[num28];
                                int num30 = base.m_Triangles[num28 - 1];
                                int num31 = base.m_Triangles[num28 - 2];
                                if (base.m_RemovedIndices.IsRemovedIndex(num29) || (base.m_RemovedIndices.IsRemovedIndex(num30) || base.m_RemovedIndices.IsRemovedIndex(num31)))
                                {
                                    base.m_Triangles.RemoveRange(num28 - 2, 3);
                                }
                                else
                                {
                                    int num33 = base.m_RemovedIndices.AdjustedIndex(num30);
                                    int num34 = base.m_RemovedIndices.AdjustedIndex(num31);
                                    base.m_Triangles[num28] = base.m_RemovedIndices.AdjustedIndex(num29);
                                    base.m_Triangles[num28 - 1] = num33;
                                    base.m_Triangles[num28 - 2] = num34;
                                }
                                num28 -= 3;
                            }
                            break;
                        }
                        Vector3 rhs = base.m_Normals[num27];
                        if (a > Vector3.Dot(v, rhs))
                        {
                            base.m_RemovedIndices.AddRemovedIndex(num27);
                        }
                        num27++;
                    }
                }
                activeDecalProjector.DecalsMeshUpperVertexIndex = base.m_Vertices.Count - 1;
                activeDecalProjector.DecalsMeshUpperTriangleIndex = base.m_Triangles.Count - 1;
                activeDecalProjector.IsUV1ProjectionCalculated = false;
                activeDecalProjector.IsUV2ProjectionCalculated = false;
                activeDecalProjector.IsTangentProjectionCalculated = false;
            }
        }

        public void Add(Mesh a_Mesh, Matrix4x4 a_WorldToMeshMatrix, Matrix4x4 a_MeshToWorldMatrix)
        {
            DecalProjectorBase activeDecalProjector = base.ActiveDecalProjector;
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
                if ((0 > activeDecalProjector.UV1RectangleIndex) || ((base.m_Decals.CurrentUvRectangles == null) || (activeDecalProjector.UV1RectangleIndex >= base.m_Decals.CurrentUvRectangles.Length)))
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
                if ((0 > activeDecalProjector.UV2RectangleIndex) || ((base.m_Decals.CurrentUv2Rectangles == null) || (activeDecalProjector.UV2RectangleIndex >= base.m_Decals.CurrentUv2Rectangles.Length)))
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
                throw new ArgumentException("The mesh is not readable! Make sure the mesh can be read in the import settings and no static batching is used for it.");
            }
            Vector3[] vertices = a_Mesh.vertices;
            Vector3[] normals = a_Mesh.normals;
            Vector4[] tangents = null;
            Color[] colors = null;
            Vector2[] uv = null;
            Vector2[] uv = null;
            int[] triangles = a_Mesh.triangles;
            if (triangles == null)
            {
                throw new NullReferenceException("The triangles in the mesh are null!");
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
                this.Add(vertices, normals, tangents, colors, uv, uv, triangles, a_WorldToMeshMatrix, a_MeshToWorldMatrix);
                if (flag)
                {
                    a_Mesh.normals = null;
                }
            }
        }

        public void Add(Terrain a_Terrain, float a_Density, Matrix4x4 a_TerrainToDecalsMatrix)
        {
            int count;
            int num8;
            DecalProjectorBase activeDecalProjector = base.ActiveDecalProjector;
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
                if ((0 > activeDecalProjector.UV1RectangleIndex) || ((base.m_Decals.CurrentUvRectangles == null) || (activeDecalProjector.UV1RectangleIndex >= base.m_Decals.CurrentUvRectangles.Length)))
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
                if ((0 > activeDecalProjector.UV2RectangleIndex) || ((base.m_Decals.CurrentUv2Rectangles == null) || (activeDecalProjector.UV2RectangleIndex >= base.m_Decals.CurrentUv2Rectangles.Length)))
                {
                    throw new IndexOutOfRangeException("The uv2 rectangle index of the active projector is not a valid index within the decals uv2 rectangles array!");
                }
            }
            if (a_Terrain == null)
            {
                throw new ArgumentNullException("The terrain parameter is not allowed to be null!");
            }
            if (base.m_Decals.CurrentTangentsMode == TangentsMode.Target)
            {
                throw new InvalidOperationException("Terrains don't have tangents!");
            }
            if ((base.m_Decals.CurrentUVMode == UVMode.TargetUV) || (base.m_Decals.CurrentUVMode == UVMode.TargetUV2))
            {
                throw new InvalidOperationException("Terrains don't have uv's!");
            }
            if ((base.m_Decals.CurrentUV2Mode == UV2Mode.TargetUV) || (base.m_Decals.CurrentUV2Mode == UV2Mode.TargetUV2))
            {
                throw new InvalidOperationException("Terrains don't have uv2's!");
            }
            if ((a_Density < 0f) || (a_Density > 1f))
            {
                throw new ArgumentOutOfRangeException("The density has to be in the range [0.0f, 1.0f].");
            }
            Bounds bounds = activeDecalProjector.WorldBounds();
            bounds.center -= a_Terrain.transform.position;
            TerrainData terrainData = a_Terrain.terrainData;
            Matrix4x4 transpose = a_TerrainToDecalsMatrix.inverse.transpose;
            if (terrainData == null)
            {
                return;
            }
            else
            {
                Vector3 min = bounds.min;
                Vector3 max = bounds.max;
                if (min.x > max.x)
                {
                    min.x = max.x;
                    max.x = min.x;
                }
                if (min.z > max.z)
                {
                    min.z = max.z;
                    max.z = min.z;
                }
                Vector3 size = terrainData.size;
                min.x = Mathf.Clamp(min.x, 0f, size.x);
                max.x = Mathf.Clamp(max.x, 0f, size.x);
                min.z = Mathf.Clamp(min.z, 0f, size.z);
                max.z = Mathf.Clamp(max.z, 0f, size.z);
                Vector3 heightmapScale = terrainData.heightmapScale;
                int num3 = Mathf.FloorToInt(min.x / heightmapScale.x);
                int num4 = Mathf.FloorToInt(min.z / heightmapScale.z);
                int num5 = Mathf.CeilToInt(max.x / heightmapScale.x);
                int num6 = Mathf.CeilToInt(max.z / heightmapScale.z);
                count = base.Vertices.Count;
                num8 = base.Triangles.Count;
                if ((num3 < num5) && (num4 < num6))
                {
                    Color item = base.m_Decals.VertexColorTint * (((1f - base.ActiveDecalProjector.VertexColorBlending) * base.ActiveDecalProjector.VertexColor) + (base.ActiveDecalProjector.VertexColorBlending * Color.white));
                    float t = -Mathf.Pow(2f, -7f * a_Density) + 1f;
                    int num11 = Mathf.RoundToInt(Mathf.Lerp((float) (num5 - num3), 1f, t));
                    int num12 = Mathf.RoundToInt(Mathf.Lerp((float) (num6 - num4), 1f, t));
                    float num13 = 1f / ((float) (terrainData.heightmapWidth - 1));
                    float num14 = 1f / ((float) (terrainData.heightmapHeight - 1));
                    int num15 = 0;
                    int num16 = 0;
                    int x = num3;
                    while (true)
                    {
                        bool flag = false;
                        if (x >= num5)
                        {
                            x = num5;
                            flag = true;
                        }
                        float num18 = x * heightmapScale.x;
                        int y = num4;
                        while (true)
                        {
                            bool flag2 = false;
                            if (y >= num6)
                            {
                                y = num6;
                                flag2 = true;
                            }
                            float height = terrainData.GetHeight(x, y);
                            float z = y * heightmapScale.z;
                            Vector3 vector5 = a_TerrainToDecalsMatrix.MultiplyPoint3x4(new Vector3(num18, height, z));
                            Vector3 vector6 = transpose.MultiplyVector(terrainData.GetInterpolatedNormal(x * num13, y * num14));
                            vector6.Normalize();
                            base.m_Vertices.Add(vector5);
                            base.m_Normals.Add(vector6);
                            if (base.m_Decals.UseVertexColors)
                            {
                                base.m_TargetVertexColors.Add(Color.white);
                                base.m_VertexColors.Add(item);
                            }
                            if (num15 == 0)
                            {
                                num16++;
                            }
                            if (!flag2)
                            {
                                y += num12;
                                continue;
                            }
                            num15++;
                            if (!flag)
                            {
                                x += num11;
                            }
                            else
                            {
                                int num22 = num15 - 1;
                                int num23 = num16 - 1;
                                x = 0;
                                while (x < num22)
                                {
                                    int num24 = 0;
                                    while (true)
                                    {
                                        if (num24 >= num23)
                                        {
                                            x++;
                                            break;
                                        }
                                        int num25 = (count + num24) + (x * num16);
                                        int num26 = num25 + 1;
                                        int num27 = num25 + num16;
                                        int num28 = num27 + 1;
                                        base.m_Triangles.Add(num25);
                                        base.m_Triangles.Add(num26);
                                        base.m_Triangles.Add(num28);
                                        base.m_Triangles.Add(num25);
                                        base.m_Triangles.Add(num28);
                                        base.m_Triangles.Add(num27);
                                        num24++;
                                    }
                                }
                                break;
                            }
                            break;
                        }
                    }
                }
            }
            float a = Mathf.Cos(activeDecalProjector.CullingAngle * 0.01745329f);
            if (!Mathf.Approximately(a, -1f))
            {
                Vector3 v = new Vector3(0f, 1f, 0f);
                v = (base.m_Decals.CachedTransform.worldToLocalMatrix * activeDecalProjector.ProjectorToWorldMatrix).inverse.transpose.MultiplyVector(v).normalized;
                int num30 = count;
                while (true)
                {
                    if (num30 >= base.m_Vertices.Count)
                    {
                        int num31 = base.m_Triangles.Count - 1;
                        while (true)
                        {
                            if (num31 < num8)
                            {
                                base.RemoveIndices(base.m_RemovedIndices);
                                base.m_RemovedIndices.Clear();
                                break;
                            }
                            int num32 = base.m_Triangles[num31];
                            int num33 = base.m_Triangles[num31 - 1];
                            int num34 = base.m_Triangles[num31 - 2];
                            if (base.m_RemovedIndices.IsRemovedIndex(num32) || (base.m_RemovedIndices.IsRemovedIndex(num33) || base.m_RemovedIndices.IsRemovedIndex(num34)))
                            {
                                base.m_Triangles.RemoveRange(num31 - 2, 3);
                            }
                            else
                            {
                                int num36 = base.m_RemovedIndices.AdjustedIndex(num33);
                                int num37 = base.m_RemovedIndices.AdjustedIndex(num34);
                                base.m_Triangles[num31] = base.m_RemovedIndices.AdjustedIndex(num32);
                                base.m_Triangles[num31 - 1] = num36;
                                base.m_Triangles[num31 - 2] = num37;
                            }
                            num31 -= 3;
                        }
                        break;
                    }
                    Vector3 rhs = base.m_Normals[num30];
                    if (a > Vector3.Dot(v, rhs))
                    {
                        base.m_RemovedIndices.AddRemovedIndex(num30);
                    }
                    num30++;
                }
            }
            activeDecalProjector.DecalsMeshUpperVertexIndex = base.m_Vertices.Count - 1;
            activeDecalProjector.DecalsMeshUpperTriangleIndex = base.m_Triangles.Count - 1;
            activeDecalProjector.IsUV1ProjectionCalculated = false;
            activeDecalProjector.IsUV2ProjectionCalculated = false;
            activeDecalProjector.IsTangentProjectionCalculated = false;
        }

        public void Add(Vector3[] a_Vertices, Vector3[] a_Normals, Vector4[] a_Tangents, Vector2[] a_UVs, Vector2[] a_UV2s, int[] a_Triangles, Matrix4x4 a_WorldToMeshMatrix, Matrix4x4 a_MeshToWorldMatrix)
        {
            try
            {
                this.Add(a_Vertices, a_Normals, a_Tangents, null, a_UVs, a_UV2s, a_Triangles, a_WorldToMeshMatrix, a_MeshToWorldMatrix);
            }
            catch
            {
                throw;
            }
        }

        public void Add(Vector3[] a_Vertices, Vector3[] a_Normals, Vector4[] a_Tangents, Color[] a_Colors, Vector2[] a_UVs, Vector2[] a_UV2s, int[] a_Triangles, Matrix4x4 a_WorldToMeshMatrix, Matrix4x4 a_MeshToWorldMatrix)
        {
            DecalProjectorBase activeDecalProjector = base.ActiveDecalProjector;
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
                if ((0 > activeDecalProjector.UV1RectangleIndex) || ((base.m_Decals.CurrentUvRectangles == null) || (activeDecalProjector.UV1RectangleIndex >= base.m_Decals.CurrentUvRectangles.Length)))
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
                if ((0 > activeDecalProjector.UV2RectangleIndex) || ((base.m_Decals.CurrentUv2Rectangles == null) || (activeDecalProjector.UV2RectangleIndex >= base.m_Decals.CurrentUv2Rectangles.Length)))
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
            int count = base.m_Vertices.Count;
            this.InternalAdd(a_Vertices, a_Normals, a_Tangents, a_Colors, a_UVs, a_UV2s, v, Mathf.Cos(activeDecalProjector.CullingAngle * 0.01745329f), matrixx5, matrixx5.inverse.transpose);
            if (a_WorldToMeshMatrix.Determinant() >= 0f)
            {
                for (int i = 0; i < a_Triangles.Length; i += 3)
                {
                    int num5 = a_Triangles[i];
                    int num6 = a_Triangles[i + 1];
                    int num7 = a_Triangles[i + 2];
                    if (!base.m_RemovedIndices.IsRemovedIndex(num5) && (!base.m_RemovedIndices.IsRemovedIndex(num6) && !base.m_RemovedIndices.IsRemovedIndex(num7)))
                    {
                        num5 = count + base.m_RemovedIndices.AdjustedIndex(num5);
                        num6 = count + base.m_RemovedIndices.AdjustedIndex(num6);
                        num7 = count + base.m_RemovedIndices.AdjustedIndex(num7);
                        base.m_Triangles.Add(num5);
                        base.m_Triangles.Add(num6);
                        base.m_Triangles.Add(num7);
                    }
                }
            }
            else
            {
                for (int i = 0; i < a_Triangles.Length; i += 3)
                {
                    int num9 = a_Triangles[i];
                    int num10 = a_Triangles[i + 1];
                    int num11 = a_Triangles[i + 2];
                    if (!base.m_RemovedIndices.IsRemovedIndex(num9) && (!base.m_RemovedIndices.IsRemovedIndex(num10) && !base.m_RemovedIndices.IsRemovedIndex(num11)))
                    {
                        num9 = count + base.m_RemovedIndices.AdjustedIndex(num9);
                        num10 = count + base.m_RemovedIndices.AdjustedIndex(num10);
                        num11 = count + base.m_RemovedIndices.AdjustedIndex(num11);
                        base.m_Triangles.Add(num10);
                        base.m_Triangles.Add(num9);
                        base.m_Triangles.Add(num11);
                    }
                }
            }
            base.m_RemovedIndices.Clear();
            activeDecalProjector.DecalsMeshUpperVertexIndex = base.m_Vertices.Count - 1;
            activeDecalProjector.DecalsMeshUpperTriangleIndex = base.m_Triangles.Count - 1;
            activeDecalProjector.IsTangentProjectionCalculated = false;
            activeDecalProjector.IsUV1ProjectionCalculated = false;
            activeDecalProjector.IsUV2ProjectionCalculated = false;
        }

        private unsafe void InternalAdd(Vector3[] a_Vertices, Vector3[] a_Normals, Vector4[] a_Tangents, Color[] a_Colors, Vector2[] a_UVs, Vector2[] a_UV2s, Vector3 a_ReversedProjectionNormal, float a_CullingDotProduct, Matrix4x4 a_MeshToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrixInvertedTransposed)
        {
            Color vertexColorTint = base.m_Decals.VertexColorTint;
            Color color2 = (Color) ((1f - base.ActiveDecalProjector.VertexColorBlending) * base.ActiveDecalProjector.VertexColor);
            for (int i = 0; i < a_Vertices.Length; i++)
            {
                Vector3 vector2 = a_MeshToDecalsMatrixInvertedTransposed.MultiplyVector(a_Normals[i]);
                Vector3 normalized = vector2.normalized;
                if (a_CullingDotProduct > Vector3.Dot(a_ReversedProjectionNormal, normalized))
                {
                    base.m_RemovedIndices.AddRemovedIndex(i);
                }
                else
                {
                    Vector3 item = a_MeshToDecalsMatrix.MultiplyPoint3x4(a_Vertices[i]);
                    base.m_Vertices.Add(item);
                    base.m_Normals.Add(normalized);
                    if (base.m_Decals.CurrentTangentsMode == TangentsMode.Target)
                    {
                        Vector4 vector4 = a_MeshToDecalsMatrixInvertedTransposed.MultiplyVector(*((Vector3*) &(a_Tangents[i]))).normalized;
                        base.m_Tangents.Add(vector4);
                    }
                    if (base.m_Decals.UseVertexColors)
                    {
                        Color white = Color.white;
                        if ((a_Colors != null) && (a_Colors.Length != 0))
                        {
                            white = a_Colors[i];
                        }
                        base.m_TargetVertexColors.Add(white);
                        Color color4 = vertexColorTint * (color2 + (base.ActiveDecalProjector.VertexColorBlending * white));
                        base.m_VertexColors.Add(color4);
                    }
                    if ((base.m_Decals.CurrentUVMode == UVMode.TargetUV) || (base.m_Decals.CurrentUVMode == UVMode.TargetUV2))
                    {
                        base.m_UVs.Add(a_UVs[i]);
                    }
                    if ((base.m_Decals.CurrentUV2Mode == UV2Mode.TargetUV) || (base.m_Decals.CurrentUV2Mode == UV2Mode.TargetUV2))
                    {
                        base.m_UV2s.Add(a_UV2s[i]);
                    }
                }
            }
        }

        public override void OffsetActiveProjectorVertices()
        {
            DecalProjectorBase activeDecalProjector = base.ActiveDecalProjector;
            if (activeDecalProjector == null)
            {
                throw new InvalidOperationException("There is no active projector.");
            }
            float meshOffset = activeDecalProjector.MeshOffset;
            if (!Mathf.Approximately(meshOffset, 0f))
            {
                Matrix4x4 worldToLocalMatrix = base.m_Decals.CachedTransform.worldToLocalMatrix;
                Matrix4x4 transpose = worldToLocalMatrix.transpose;
                int decalsMeshUpperVertexIndex = activeDecalProjector.DecalsMeshUpperVertexIndex;
                for (int i = activeDecalProjector.DecalsMeshLowerVertexIndex; i <= decalsMeshUpperVertexIndex; i++)
                {
                    Vector3 vector2 = transpose.MultiplyVector(base.m_Normals[i]);
                    Vector3 vector = worldToLocalMatrix.MultiplyVector(vector2.normalized * meshOffset);
                    base.m_Vertices[i] += vector;
                }
            }
        }

        internal override void RemoveRange(int a_StartIndex, int a_Count)
        {
            base.m_Vertices.RemoveRange(a_StartIndex, a_Count);
            base.m_Normals.RemoveRange(a_StartIndex, a_Count);
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
    }
}

