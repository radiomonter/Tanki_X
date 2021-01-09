namespace Edelweiss.DecalSystem
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class GenericDecalsMesh<D, P, DM> : GenericDecalsMeshBase where D: GenericDecals<D, P, DM> where P: GenericDecalProjector<D, P, DM> where DM: GenericDecalsMesh<D, P, DM>
    {
        protected D m_Decals;
        private List<P> m_Projectors;
        private List<Color[]> m_PreservedVertexColorArrays;
        private bool m_PreserveVertexColorArrays;
        private List<Vector2[]> m_PreservedProjectedUVArrays;
        private bool m_PreserveProjectedUVArrays;
        private List<Vector2[]> m_PreservedProjectedUV2Arrays;
        private bool m_PreserveProjectedUV2Arrays;

        protected GenericDecalsMesh()
        {
            this.m_Projectors = new List<P>();
            this.m_PreservedVertexColorArrays = new List<Color[]>();
            this.m_PreservedProjectedUVArrays = new List<Vector2[]>();
            this.m_PreservedProjectedUV2Arrays = new List<Vector2[]>();
        }

        public void AddProjector(P a_Projector)
        {
            if (a_Projector == null)
            {
                throw new ArgumentNullException("Projector parameter is not allowed to be null!");
            }
            if (a_Projector.DecalsMesh != null)
            {
                throw new InvalidOperationException("Projector is already used in this or another instance!");
            }
            if (this.m_Decals == null)
            {
                throw new NullReferenceException("Projectors can only be added if the decals is not null!");
            }
            if ((this.m_Decals.LinkedDecalsMesh != null) && !ReferenceEquals(this.m_Decals.LinkedDecalsMesh, this))
            {
                throw new InvalidOperationException("The decals instance is already linked to another decals mesh.");
            }
            P activeDecalProjector = this.ActiveDecalProjector;
            if (activeDecalProjector != null)
            {
                activeDecalProjector.IsActiveProjector = false;
            }
            this.m_Projectors.Add(a_Projector);
            a_Projector.DecalsMesh = this as DM;
            a_Projector.IsActiveProjector = true;
            this.SetRangesForAddedProjector(a_Projector);
            this.m_Decals.LinkedDecalsMesh = this;
        }

        internal virtual void AdjustProjectorIndices(P a_Projector, int a_VertexIndexOffset, int a_TriangleIndexOffset, int a_BoneIndexOffset)
        {
            a_Projector.DecalsMeshLowerVertexIndex -= a_VertexIndexOffset;
            a_Projector.DecalsMeshUpperVertexIndex -= a_VertexIndexOffset;
            a_Projector.DecalsMeshLowerTriangleIndex -= a_TriangleIndexOffset;
            a_Projector.DecalsMeshUpperTriangleIndex -= a_TriangleIndexOffset;
        }

        private void AdjustTriangleIndices(int a_LowerTriangleIndex, RemovedIndices a_RemovedIndices)
        {
            for (int i = a_LowerTriangleIndex; i < base.m_Triangles.Count; i++)
            {
                base.m_Triangles[i] = a_RemovedIndices.AdjustedIndex(base.m_Triangles[i]);
            }
            this.ActiveDecalProjector.DecalsMeshUpperTriangleIndex = base.m_Triangles.Count - 1;
        }

        internal virtual int BoneIndexOffset(P a_Projector) => 
            0;

        private void CalculateProjectedTangents(GenericDecalProjectorBase a_Projector)
        {
            while (base.m_Tangents.Count < a_Projector.DecalsMeshLowerVertexIndex)
            {
                base.m_Tangents.Add(Vector4.zero);
            }
            Matrix4x4 transpose = (a_Projector.WorldToProjectorMatrix * this.m_Decals.CachedTransform.localToWorldMatrix).inverse.transpose;
            Matrix4x4 matrixx4 = (this.m_Decals.CachedTransform.worldToLocalMatrix * a_Projector.ProjectorToWorldMatrix).inverse.transpose;
            for (int i = a_Projector.DecalsMeshLowerVertexIndex; i <= a_Projector.DecalsMeshUpperVertexIndex; i++)
            {
                Vector3 v = transpose.MultiplyVector(base.Normals[i]);
                v.z = 0f;
                if (Mathf.Approximately(v.x, 0f) && Mathf.Approximately(v.y, 0f))
                {
                    v = new Vector3(0f, 1f, 0f);
                }
                v = new Vector3(v.y, -v.x, v.z);
                v = matrixx4.MultiplyVector(v);
                v.Normalize();
                Vector4 item = new Vector4(v.x, v.y, v.z, -1f);
                if (i < base.m_Tangents.Count)
                {
                    base.m_Tangents[i] = item;
                }
                else
                {
                    base.m_Tangents.Add(item);
                }
            }
        }

        private void CalculateProjectedUV(GenericDecalProjectorBase a_Projector)
        {
            Matrix4x4 matrixx = a_Projector.WorldToProjectorMatrix * this.m_Decals.CachedTransform.localToWorldMatrix;
            UVRectangle rectangle = this.m_Decals.CurrentUvRectangles[a_Projector.UV1RectangleIndex];
            List<Vector2> uVs = base.m_UVs;
            int decalsMeshLowerVertexIndex = a_Projector.DecalsMeshLowerVertexIndex;
            int decalsMeshUpperVertexIndex = a_Projector.DecalsMeshUpperVertexIndex;
            if (this.m_Decals.CurrentUVMode == UVMode.Project)
            {
                this.CalculateProjectedUV(matrixx, rectangle, uVs, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
            }
            else
            {
                this.CalculateWrappedProjectionUV(matrixx, rectangle, uVs, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
            }
            a_Projector.IsUV1ProjectionCalculated = true;
        }

        private void CalculateProjectedUV(Matrix4x4 a_DecalsToProjectorMatrix, UVRectangle a_UVRectangle, List<Vector2> a_UVs, int a_LowerIndex, int a_UpperIndex)
        {
            Vector2 lowerLeftUV = a_UVRectangle.lowerLeftUV;
            Vector2 size = a_UVRectangle.Size;
            while (a_UVs.Count < a_LowerIndex)
            {
                a_UVs.Add(Vector2.zero);
            }
            for (int i = a_LowerIndex; i <= a_UpperIndex; i++)
            {
                Vector3 v = base.Vertices[i];
                v = a_DecalsToProjectorMatrix.MultiplyPoint3x4(v);
                Vector2 item = new Vector2(v.x, v.z);
                item.x = lowerLeftUV.x + ((item.x + 0.5f) * size.x);
                item.y = lowerLeftUV.y + ((item.y + 0.5f) * size.y);
                if (i < a_UVs.Count)
                {
                    a_UVs[i] = item;
                }
                else
                {
                    a_UVs.Add(item);
                }
            }
        }

        public void CalculateProjectedUV1ForActiveProjector()
        {
            this.CalculateProjectedUV(this.ActiveDecalProjector);
        }

        private void CalculateProjectedUV2(GenericDecalProjectorBase a_Projector)
        {
            Matrix4x4 matrixx = a_Projector.WorldToProjectorMatrix * this.m_Decals.CachedTransform.localToWorldMatrix;
            UVRectangle rectangle = this.m_Decals.CurrentUv2Rectangles[a_Projector.UV2RectangleIndex];
            List<Vector2> list = base.m_UV2s;
            int decalsMeshLowerVertexIndex = a_Projector.DecalsMeshLowerVertexIndex;
            int decalsMeshUpperVertexIndex = a_Projector.DecalsMeshUpperVertexIndex;
            if (this.m_Decals.CurrentUVMode == UVMode.Project)
            {
                this.CalculateProjectedUV(matrixx, rectangle, list, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
            }
            else
            {
                this.CalculateWrappedProjectionUV(matrixx, rectangle, list, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
            }
            a_Projector.IsUV2ProjectionCalculated = true;
        }

        public void CalculateProjectedUV2ForActiveProjector()
        {
            this.CalculateProjectedUV2(this.ActiveDecalProjector);
        }

        private void CalculateWrappedProjectionUV(Matrix4x4 a_DecalsToProjectorMatrix, UVRectangle a_UVRectangle, List<Vector2> a_UVs, int a_LowerIndex, int a_UpperIndex)
        {
            Vector2 lowerLeftUV = a_UVRectangle.lowerLeftUV;
            Vector2 size = a_UVRectangle.Size;
            while (a_UVs.Count < a_LowerIndex)
            {
                a_UVs.Add(Vector2.zero);
            }
            for (int i = a_LowerIndex; i <= a_UpperIndex; i++)
            {
                Vector3 vector3 = a_DecalsToProjectorMatrix.MultiplyPoint3x4(base.Vertices[i]);
                Vector3 vector4 = a_DecalsToProjectorMatrix.MultiplyVector(base.Normals[i]);
                Vector2 item = new Vector2(vector3.x, vector3.z);
                item -= vector3.y * new Vector2(vector4.x, vector4.z);
                item.x = Mathf.Clamp(item.x, -0.5f, 0.5f);
                item.y = Mathf.Clamp(item.y, -0.5f, 0.5f);
                item.x = lowerLeftUV.x + ((item.x + 0.5f) * size.x);
                item.y = lowerLeftUV.y + ((item.y + 0.5f) * size.y);
                if (i < a_UVs.Count)
                {
                    a_UVs[i] = item;
                }
                else
                {
                    a_UVs.Add(item);
                }
            }
        }

        public virtual void ClearAll()
        {
            foreach (P local in this.m_Projectors)
            {
                local.ResetDecalsMesh();
            }
            this.m_Projectors.Clear();
            base.Vertices.Clear();
            base.Normals.Clear();
            base.Tangents.Clear();
            base.TargetVertexColors.Clear();
            base.VertexColors.Clear();
            base.UVs.Clear();
            base.UV2s.Clear();
            base.Triangles.Clear();
            if (this.m_Decals != null)
            {
                this.m_Decals.LinkedDecalsMesh = null;
            }
        }

        public bool ContainsProjector(P a_Projector) => 
            this.m_Projectors.Contains(a_Projector);

        protected override bool HasUV2LightmappingMode()
        {
            bool flag = false;
            if ((this.m_Decals != null) && (this.m_Decals.CurrentUV2Mode == UV2Mode.Lightmapping))
            {
                flag = true;
            }
            return flag;
        }

        public void Initialize(D a_Decals)
        {
            this.m_Decals = a_Decals;
            this.PreserveVertexColorArrays = false;
            this.PreserveProjectedUVArrays = false;
            this.PreserveProjectedUV2Arrays = false;
            this.ClearAll();
        }

        public abstract void OffsetActiveProjectorVertices();
        protected override void RecalculateTangents()
        {
            if ((this.m_Decals != null) && (this.m_Decals.CurrentTangentsMode == TangentsMode.Project))
            {
                while (true)
                {
                    if (base.m_Tangents.Count <= base.m_Vertices.Count)
                    {
                        foreach (GenericDecalProjectorBase base2 in this.m_Projectors)
                        {
                            if (!base2.IsTangentProjectionCalculated)
                            {
                                this.CalculateProjectedTangents(base2);
                            }
                        }
                        break;
                    }
                    base.m_Tangents.RemoveAt(base.m_Tangents.Count - 1);
                }
            }
        }

        protected override void RecalculateUV2s()
        {
            if ((this.m_Decals != null) && ((this.m_Decals.CurrentUV2Mode == UV2Mode.Project) || (this.m_Decals.CurrentUV2Mode == UV2Mode.ProjectWrapped)))
            {
                while (true)
                {
                    if (base.m_UV2s.Count <= base.m_Vertices.Count)
                    {
                        foreach (GenericDecalProjectorBase base2 in this.m_Projectors)
                        {
                            if (!base2.IsUV2ProjectionCalculated)
                            {
                                this.CalculateProjectedUV2(base2);
                            }
                        }
                        break;
                    }
                    base.m_UV2s.RemoveAt(base.m_UV2s.Count - 1);
                }
            }
        }

        protected override void RecalculateUVs()
        {
            if ((this.m_Decals != null) && ((this.m_Decals.CurrentUVMode == UVMode.Project) || (this.m_Decals.CurrentUVMode == UVMode.ProjectWrapped)))
            {
                while (true)
                {
                    if (base.m_UVs.Count <= base.m_Vertices.Count)
                    {
                        foreach (P local in this.m_Projectors)
                        {
                            if (!local.IsUV1ProjectionCalculated)
                            {
                                this.CalculateProjectedUV(local);
                            }
                        }
                        break;
                    }
                    base.m_UVs.RemoveAt(base.m_UVs.Count - 1);
                }
            }
        }

        internal void RemoveAndAdjustIndices(int a_LowerTriangleIndex, RemovedIndices a_RemovedIndices)
        {
            this.AdjustTriangleIndices(a_LowerTriangleIndex, a_RemovedIndices);
            this.RemoveIndices(a_RemovedIndices);
        }

        internal virtual void RemoveBonesAndAdjustBoneWeightIndices(P a_Projector)
        {
        }

        internal void RemoveIndices(RemovedIndices a_RemovedIndices)
        {
            int num = -1;
            int num2 = 0;
            for (int i = base.m_Vertices.Count - 1; i >= 0; i--)
            {
                bool flag = a_RemovedIndices.IsRemovedIndex(i);
                if (flag)
                {
                    if (num == -1)
                    {
                        num = i;
                        num2 = 1;
                    }
                    else
                    {
                        num = i;
                        num2++;
                    }
                }
                if ((!flag || (i == 0)) && (num != -1))
                {
                    this.RemoveRange(num, num2);
                    num = -1;
                    num2 = 0;
                }
            }
            this.ActiveDecalProjector.DecalsMeshUpperVertexIndex = base.m_Vertices.Count - 1;
        }

        public void RemoveProjector(P a_Projector)
        {
            if (a_Projector.DecalsMesh != this)
            {
                throw new InvalidOperationException("Unable to remove a projector that is not part of this instance.");
            }
            int decalsMeshLowerVertexIndex = a_Projector.DecalsMeshLowerVertexIndex;
            int decalsMeshUpperVertexIndex = a_Projector.DecalsMeshUpperVertexIndex;
            int decalsMeshVertexCount = a_Projector.DecalsMeshVertexCount;
            int decalsMeshLowerTriangleIndex = a_Projector.DecalsMeshLowerTriangleIndex;
            int decalsMeshTriangleCount = a_Projector.DecalsMeshTriangleCount;
            if (decalsMeshTriangleCount > 0)
            {
                base.m_Triangles.RemoveRange(decalsMeshLowerTriangleIndex, decalsMeshTriangleCount);
            }
            for (int i = decalsMeshLowerTriangleIndex; i < base.m_Triangles.Count; i++)
            {
                int num7 = base.m_Triangles[i];
                if (num7 > decalsMeshUpperVertexIndex)
                {
                    base.m_Triangles[i] = num7 - decalsMeshVertexCount;
                }
            }
            this.RemoveRangesOfVertexAlignedLists(decalsMeshLowerVertexIndex, decalsMeshVertexCount);
            int num8 = this.BoneIndexOffset(a_Projector);
            this.RemoveBonesAndAdjustBoneWeightIndices(a_Projector);
            if (a_Projector.IsActiveProjector)
            {
                this.Projectors.RemoveAt(this.Projectors.Count - 1);
            }
            else
            {
                int index = this.Projectors.IndexOf(a_Projector);
                int num10 = index + 1;
                while (true)
                {
                    if (num10 >= this.Projectors.Count)
                    {
                        this.Projectors.RemoveAt(index);
                        break;
                    }
                    P local = this.Projectors[num10];
                    this.AdjustProjectorIndices(local, decalsMeshVertexCount, decalsMeshTriangleCount, num8);
                    num10++;
                }
            }
            a_Projector.ResetDecalsMesh();
            if (this.m_Projectors.Count == 0)
            {
                this.m_Decals.LinkedDecalsMesh = null;
            }
        }

        internal abstract void RemoveRange(int a_StartIndex, int a_Count);
        internal virtual void RemoveRangesOfVertexAlignedLists(int a_LowerIndex, int a_Count)
        {
            base.m_Vertices.RemoveRange(a_LowerIndex, a_Count);
            if (a_LowerIndex < base.m_Normals.Count)
            {
                base.m_Normals.RemoveRange(a_LowerIndex, a_Count);
            }
            if (a_LowerIndex < base.m_UVs.Count)
            {
                base.m_UVs.RemoveRange(a_LowerIndex, a_Count);
            }
            if (a_LowerIndex < base.m_UV2s.Count)
            {
                base.m_UV2s.RemoveRange(a_LowerIndex, a_Count);
            }
            if (a_LowerIndex < base.m_Tangents.Count)
            {
                base.m_Tangents.RemoveRange(a_LowerIndex, a_Count);
            }
            if (a_LowerIndex < base.m_TargetVertexColors.Count)
            {
                base.m_TargetVertexColors.RemoveRange(a_LowerIndex, a_Count);
            }
            if (a_LowerIndex < base.m_VertexColors.Count)
            {
                base.m_VertexColors.RemoveRange(a_LowerIndex, a_Count);
            }
        }

        internal void RemoveTriangleAt(int a_Index)
        {
            base.m_Triangles.RemoveRange(a_Index, 3);
        }

        internal void RemoveTrianglesAt(int a_Index, int a_Count)
        {
            base.m_Triangles.RemoveRange(a_Index, 3 * a_Count);
        }

        internal virtual void SetRangesForAddedProjector(P a_Projector)
        {
            a_Projector.DecalsMeshLowerVertexIndex = base.m_Vertices.Count;
            a_Projector.DecalsMeshUpperVertexIndex = base.m_Vertices.Count - 1;
            a_Projector.DecalsMeshLowerTriangleIndex = base.m_Triangles.Count;
            a_Projector.DecalsMeshUpperTriangleIndex = base.m_Triangles.Count - 1;
        }

        public void SmoothActiveProjectorNormals(float a_SmoothFactor)
        {
            P activeDecalProjector = this.ActiveDecalProjector;
            if (activeDecalProjector == null)
            {
                throw new InvalidOperationException("There is no active projector.");
            }
            if ((a_SmoothFactor < 0f) || (a_SmoothFactor > 1f))
            {
                throw new ArgumentOutOfRangeException("The smooth factor has to be in [0.0f, 1.0f].");
            }
            int decalsMeshUpperVertexIndex = activeDecalProjector.DecalsMeshUpperVertexIndex;
            Vector3 b = (Vector3) (activeDecalProjector.Rotation * Vector3.up);
            for (int i = activeDecalProjector.DecalsMeshLowerVertexIndex; i <= decalsMeshUpperVertexIndex; i++)
            {
                Vector3 vector2 = Vector3.Lerp(base.m_Normals[i], b, a_SmoothFactor);
                vector2.Normalize();
                base.m_Normals[i] = vector2;
            }
        }

        public void UpdateProjectedUV(P a_Projector)
        {
            if (this.m_Decals == null)
            {
                throw new InvalidOperationException("Decals mesh is not initialized with a decals instance.");
            }
            if (this.m_Decals.CurrentUvRectangles == null)
            {
                throw new InvalidOperationException("Empty uv rectangles.");
            }
            if ((this.m_Decals.CurrentUVMode != UVMode.Project) && (this.m_Decals.CurrentUVMode != UVMode.ProjectWrapped))
            {
                throw new InvalidOperationException("UV mode of the decals is not projected!");
            }
            if (!this.m_Projectors.Contains(a_Projector))
            {
                throw new ArgumentException("Projector is not part of the decals mesh.");
            }
            this.CalculateProjectedUV(a_Projector);
        }

        public void UpdateProjectedUV2(P a_Projector)
        {
            if (this.m_Decals == null)
            {
                throw new InvalidOperationException("Decals mesh is not initialized with a decals instance.");
            }
            if (this.m_Decals.CurrentUvRectangles == null)
            {
                throw new InvalidOperationException("Empty uv rectangles.");
            }
            if ((this.m_Decals.CurrentUV2Mode != UV2Mode.Project) && (this.m_Decals.CurrentUV2Mode != UV2Mode.ProjectWrapped))
            {
                throw new InvalidOperationException("UV2 mode of the decals is not projected!");
            }
            if (!this.m_Projectors.Contains(a_Projector))
            {
                throw new ArgumentException("Projector is not part of the decals mesh.");
            }
            this.CalculateProjectedUV2(a_Projector);
        }

        public void UpdateVertexColors(P a_Projector)
        {
            if (this.m_Decals == null)
            {
                throw new InvalidOperationException("Decals mesh is not initialized with a decals instance.");
            }
            if (!this.m_Projectors.Contains(a_Projector))
            {
                throw new ArgumentException("Projector is not part of the decals mesh.");
            }
            Color vertexColorTint = this.m_Decals.VertexColorTint;
            Color color2 = (Color) ((1f - a_Projector.VertexColorBlending) * a_Projector.VertexColor);
            for (int i = a_Projector.DecalsMeshLowerVertexIndex; i <= a_Projector.DecalsMeshUpperVertexIndex; i++)
            {
                base.m_VertexColors[i] = vertexColorTint * (color2 + (a_Projector.VertexColorBlending * base.m_TargetVertexColors[i]));
            }
        }

        public D Decals =>
            this.m_Decals;

        internal List<P> Projectors =>
            this.m_Projectors;

        public P ActiveDecalProjector
        {
            get
            {
                P local = null;
                if (this.m_Projectors.Count != 0)
                {
                    local = this.m_Projectors[this.m_Projectors.Count - 1];
                }
                return local;
            }
        }

        internal List<Color[]> PreservedVertexColorArrays =>
            this.m_PreservedVertexColorArrays;

        public bool PreserveVertexColorArrays
        {
            get => 
                this.m_PreserveVertexColorArrays;
            set
            {
                if (!value)
                {
                    this.m_PreserveVertexColorArrays = value;
                    this.m_PreservedVertexColorArrays.Clear();
                }
                else
                {
                    if (this.m_Decals == null)
                    {
                        throw new InvalidOperationException("Unable to set preserving value while no valid decals instance is set.");
                    }
                    if (Edition.IsDecalSystemFree)
                    {
                        throw new InvalidOperationException("Preserving vertex color arrays is only supported in Decal System Pro.");
                    }
                    if (!this.m_Decals.UseVertexColors)
                    {
                        throw new InvalidOperationException("Unable to preserve vertex color arrays for a decals instance that does not use them.");
                    }
                    this.m_PreserveVertexColorArrays = value;
                }
            }
        }

        internal List<Vector2[]> PreservedProjectedUVArrays =>
            this.m_PreservedProjectedUVArrays;

        public bool PreserveProjectedUVArrays
        {
            get => 
                this.m_PreserveProjectedUVArrays;
            set
            {
                if (!value)
                {
                    this.m_PreserveProjectedUVArrays = value;
                    this.m_PreservedProjectedUVArrays.Clear();
                }
                else
                {
                    if (this.m_Decals == null)
                    {
                        throw new InvalidOperationException("Unable to set preserving value while no valid decals instance is set.");
                    }
                    if (Edition.IsDecalSystemFree)
                    {
                        throw new InvalidOperationException("Preserving uv arrays is only supported in Decal System Pro.");
                    }
                    if ((this.m_Decals.CurrentUVMode != UVMode.Project) && (this.m_Decals.CurrentUVMode != UVMode.ProjectWrapped))
                    {
                        throw new InvalidOperationException("Unalble to preserve uv arrays for a decals instance that does not use them.");
                    }
                    this.m_PreserveProjectedUVArrays = value;
                }
            }
        }

        internal List<Vector2[]> PreservedProjectedUV2Arrays =>
            this.m_PreservedProjectedUV2Arrays;

        public bool PreserveProjectedUV2Arrays
        {
            get => 
                this.m_PreserveProjectedUV2Arrays;
            set
            {
                if (!value)
                {
                    this.m_PreserveProjectedUV2Arrays = value;
                    this.m_PreservedProjectedUV2Arrays.Clear();
                }
                else
                {
                    if (this.m_Decals == null)
                    {
                        throw new InvalidOperationException("Unable to set preserving value while no valid decals instance is set.");
                    }
                    if (Edition.IsDecalSystemFree)
                    {
                        throw new InvalidOperationException("Preserving uv arrays is only supported in Decal System Pro.");
                    }
                    if ((this.m_Decals.CurrentUV2Mode != UV2Mode.Project) && (this.m_Decals.CurrentUV2Mode != UV2Mode.ProjectWrapped))
                    {
                        throw new InvalidOperationException("Unable to preserve uv arrays for a decals instance that does not use them.");
                    }
                    this.m_PreserveProjectedUV2Arrays = value;
                }
            }
        }
    }
}

