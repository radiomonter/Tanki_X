namespace Edelweiss.DecalSystem
{
    using System;
    using UnityEngine;

    public class DecalsMeshCutter : GenericDecalsMeshCutter<Decals, DecalProjectorBase, DecalsMesh>
    {
        private int CutEdgeOptimizedVerticesNormals(int a_RelativeVertexLocationsOffset, int a_IndexA, int a_IndexB, Vector3 a_VertexA, Vector3 a_VertexB, bool a_IsVertexAInside, float a_IntersectionFactorAB)
        {
            CutEdge edge = new CutEdge(a_IndexA, a_IndexB);
            int count = base.m_DecalsMesh.Vertices.Count;
            base.m_DecalsMesh.Vertices.Add(Vector3.Lerp(a_VertexA, a_VertexB, a_IntersectionFactorAB));
            base.m_DecalsMesh.Normals.Add(Vector3.Lerp(base.m_DecalsMesh.Normals[a_IndexA], base.m_DecalsMesh.Normals[a_IndexB], a_IntersectionFactorAB));
            if (base.m_DecalsMesh.Decals.UseVertexColors)
            {
                base.m_DecalsMesh.TargetVertexColors.Add(Color.Lerp(base.m_DecalsMesh.TargetVertexColors[a_IndexA], base.m_DecalsMesh.TargetVertexColors[a_IndexB], a_IntersectionFactorAB));
                base.m_DecalsMesh.VertexColors.Add(Color.Lerp(base.m_DecalsMesh.VertexColors[a_IndexA], base.m_DecalsMesh.VertexColors[a_IndexB], a_IntersectionFactorAB));
            }
            base.m_ActiveProjector.DecalsMeshUpperVertexIndex++;
            if (a_IsVertexAInside)
            {
                edge.newVertex2Index = count;
                base.m_RelativeVertexLocations[a_IndexB - a_RelativeVertexLocationsOffset] = RelativeVertexLocation.Outside;
            }
            else
            {
                edge.newVertex1Index = count;
                base.m_RelativeVertexLocations[a_IndexA - a_RelativeVertexLocationsOffset] = RelativeVertexLocation.Outside;
            }
            base.m_CutEdges.AddEdge(edge);
            return count;
        }

        private int CutEdgeOptimizedVerticesNormalsTangents(int a_RelativeVertexLocationsOffset, int a_IndexA, int a_IndexB, Vector3 a_VertexA, Vector3 a_VertexB, bool a_IsVertexAInside, float a_IntersectionFactorAB)
        {
            CutEdge edge = new CutEdge(a_IndexA, a_IndexB);
            int count = base.m_DecalsMesh.Vertices.Count;
            base.m_DecalsMesh.Vertices.Add(Vector3.Lerp(a_VertexA, a_VertexB, a_IntersectionFactorAB));
            base.m_DecalsMesh.Normals.Add(Vector3.Lerp(base.m_DecalsMesh.Normals[a_IndexA], base.m_DecalsMesh.Normals[a_IndexB], a_IntersectionFactorAB));
            base.m_DecalsMesh.Tangents.Add(Vector4.Lerp(base.m_DecalsMesh.Tangents[a_IndexA], base.m_DecalsMesh.Tangents[a_IndexB], a_IntersectionFactorAB));
            if (base.m_DecalsMesh.Decals.UseVertexColors)
            {
                base.m_DecalsMesh.TargetVertexColors.Add(Color.Lerp(base.m_DecalsMesh.TargetVertexColors[a_IndexA], base.m_DecalsMesh.TargetVertexColors[a_IndexB], a_IntersectionFactorAB));
                base.m_DecalsMesh.VertexColors.Add(Color.Lerp(base.m_DecalsMesh.VertexColors[a_IndexA], base.m_DecalsMesh.VertexColors[a_IndexB], a_IntersectionFactorAB));
            }
            base.m_ActiveProjector.DecalsMeshUpperVertexIndex++;
            if (a_IsVertexAInside)
            {
                edge.newVertex2Index = count;
                base.m_RelativeVertexLocations[a_IndexB - a_RelativeVertexLocationsOffset] = RelativeVertexLocation.Outside;
            }
            else
            {
                edge.newVertex1Index = count;
                base.m_RelativeVertexLocations[a_IndexA - a_RelativeVertexLocationsOffset] = RelativeVertexLocation.Outside;
            }
            base.m_CutEdges.AddEdge(edge);
            return count;
        }

        private int CutEdgeOptimizedVerticesNormalsTangentsUV2s(int a_RelativeVertexLocationsOffset, int a_IndexA, int a_IndexB, Vector3 a_VertexA, Vector3 a_VertexB, bool a_IsVertexAInside, float a_IntersectionFactorAB)
        {
            CutEdge edge = new CutEdge(a_IndexA, a_IndexB);
            int count = base.m_DecalsMesh.Vertices.Count;
            base.m_DecalsMesh.Vertices.Add(Vector3.Lerp(a_VertexA, a_VertexB, a_IntersectionFactorAB));
            base.m_DecalsMesh.Normals.Add(Vector3.Lerp(base.m_DecalsMesh.Normals[a_IndexA], base.m_DecalsMesh.Normals[a_IndexB], a_IntersectionFactorAB));
            base.m_DecalsMesh.Tangents.Add(Vector4.Lerp(base.m_DecalsMesh.Tangents[a_IndexA], base.m_DecalsMesh.Tangents[a_IndexB], a_IntersectionFactorAB));
            base.m_DecalsMesh.UV2s.Add(Vector2.Lerp(base.m_DecalsMesh.UV2s[a_IndexA], base.m_DecalsMesh.UV2s[a_IndexB], a_IntersectionFactorAB));
            if (base.m_DecalsMesh.Decals.UseVertexColors)
            {
                base.m_DecalsMesh.TargetVertexColors.Add(Color.Lerp(base.m_DecalsMesh.TargetVertexColors[a_IndexA], base.m_DecalsMesh.TargetVertexColors[a_IndexB], a_IntersectionFactorAB));
                base.m_DecalsMesh.VertexColors.Add(Color.Lerp(base.m_DecalsMesh.VertexColors[a_IndexA], base.m_DecalsMesh.VertexColors[a_IndexB], a_IntersectionFactorAB));
            }
            base.m_ActiveProjector.DecalsMeshUpperVertexIndex++;
            if (a_IsVertexAInside)
            {
                edge.newVertex2Index = count;
                base.m_RelativeVertexLocations[a_IndexB - a_RelativeVertexLocationsOffset] = RelativeVertexLocation.Outside;
            }
            else
            {
                edge.newVertex1Index = count;
                base.m_RelativeVertexLocations[a_IndexA - a_RelativeVertexLocationsOffset] = RelativeVertexLocation.Outside;
            }
            base.m_CutEdges.AddEdge(edge);
            return count;
        }

        private int CutEdgeOptimizedVerticesNormalsTangentsUVs(int a_RelativeVertexLocationsOffset, int a_IndexA, int a_IndexB, Vector3 a_VertexA, Vector3 a_VertexB, bool a_IsVertexAInside, float a_IntersectionFactorAB)
        {
            CutEdge edge = new CutEdge(a_IndexA, a_IndexB);
            int count = base.m_DecalsMesh.Vertices.Count;
            base.m_DecalsMesh.Vertices.Add(Vector3.Lerp(a_VertexA, a_VertexB, a_IntersectionFactorAB));
            base.m_DecalsMesh.Normals.Add(Vector3.Lerp(base.m_DecalsMesh.Normals[a_IndexA], base.m_DecalsMesh.Normals[a_IndexB], a_IntersectionFactorAB));
            base.m_DecalsMesh.Tangents.Add(Vector4.Lerp(base.m_DecalsMesh.Tangents[a_IndexA], base.m_DecalsMesh.Tangents[a_IndexB], a_IntersectionFactorAB));
            base.m_DecalsMesh.UVs.Add(Vector2.Lerp(base.m_DecalsMesh.UVs[a_IndexA], base.m_DecalsMesh.UVs[a_IndexB], a_IntersectionFactorAB));
            if (base.m_DecalsMesh.Decals.UseVertexColors)
            {
                base.m_DecalsMesh.TargetVertexColors.Add(Color.Lerp(base.m_DecalsMesh.TargetVertexColors[a_IndexA], base.m_DecalsMesh.TargetVertexColors[a_IndexB], a_IntersectionFactorAB));
                base.m_DecalsMesh.VertexColors.Add(Color.Lerp(base.m_DecalsMesh.VertexColors[a_IndexA], base.m_DecalsMesh.VertexColors[a_IndexB], a_IntersectionFactorAB));
            }
            base.m_ActiveProjector.DecalsMeshUpperVertexIndex++;
            if (a_IsVertexAInside)
            {
                edge.newVertex2Index = count;
                base.m_RelativeVertexLocations[a_IndexB - a_RelativeVertexLocationsOffset] = RelativeVertexLocation.Outside;
            }
            else
            {
                edge.newVertex1Index = count;
                base.m_RelativeVertexLocations[a_IndexA - a_RelativeVertexLocationsOffset] = RelativeVertexLocation.Outside;
            }
            base.m_CutEdges.AddEdge(edge);
            return count;
        }

        private int CutEdgeOptimizedVerticesNormalsTangentsUVsUV2s(int a_RelativeVertexLocationsOffset, int a_IndexA, int a_IndexB, Vector3 a_VertexA, Vector3 a_VertexB, bool a_IsVertexAInside, float a_IntersectionFactorAB)
        {
            CutEdge edge = new CutEdge(a_IndexA, a_IndexB);
            int count = base.m_DecalsMesh.Vertices.Count;
            base.m_DecalsMesh.Vertices.Add(Vector3.Lerp(a_VertexA, a_VertexB, a_IntersectionFactorAB));
            base.m_DecalsMesh.Normals.Add(Vector3.Lerp(base.m_DecalsMesh.Normals[a_IndexA], base.m_DecalsMesh.Normals[a_IndexB], a_IntersectionFactorAB));
            base.m_DecalsMesh.Tangents.Add(Vector4.Lerp(base.m_DecalsMesh.Tangents[a_IndexA], base.m_DecalsMesh.Tangents[a_IndexB], a_IntersectionFactorAB));
            base.m_DecalsMesh.UVs.Add(Vector2.Lerp(base.m_DecalsMesh.UVs[a_IndexA], base.m_DecalsMesh.UVs[a_IndexB], a_IntersectionFactorAB));
            base.m_DecalsMesh.UV2s.Add(Vector2.Lerp(base.m_DecalsMesh.UV2s[a_IndexA], base.m_DecalsMesh.UV2s[a_IndexB], a_IntersectionFactorAB));
            if (base.m_DecalsMesh.Decals.UseVertexColors)
            {
                base.m_DecalsMesh.TargetVertexColors.Add(Color.Lerp(base.m_DecalsMesh.TargetVertexColors[a_IndexA], base.m_DecalsMesh.TargetVertexColors[a_IndexB], a_IntersectionFactorAB));
                base.m_DecalsMesh.VertexColors.Add(Color.Lerp(base.m_DecalsMesh.VertexColors[a_IndexA], base.m_DecalsMesh.VertexColors[a_IndexB], a_IntersectionFactorAB));
            }
            base.m_ActiveProjector.DecalsMeshUpperVertexIndex++;
            if (a_IsVertexAInside)
            {
                edge.newVertex2Index = count;
                base.m_RelativeVertexLocations[a_IndexB - a_RelativeVertexLocationsOffset] = RelativeVertexLocation.Outside;
            }
            else
            {
                edge.newVertex1Index = count;
                base.m_RelativeVertexLocations[a_IndexA - a_RelativeVertexLocationsOffset] = RelativeVertexLocation.Outside;
            }
            base.m_CutEdges.AddEdge(edge);
            return count;
        }

        private int CutEdgeOptimizedVerticesNormalsUV2s(int a_RelativeVertexLocationsOffset, int a_IndexA, int a_IndexB, Vector3 a_VertexA, Vector3 a_VertexB, bool a_IsVertexAInside, float a_IntersectionFactorAB)
        {
            CutEdge edge = new CutEdge(a_IndexA, a_IndexB);
            int count = base.m_DecalsMesh.Vertices.Count;
            base.m_DecalsMesh.Vertices.Add(Vector3.Lerp(a_VertexA, a_VertexB, a_IntersectionFactorAB));
            base.m_DecalsMesh.Normals.Add(Vector3.Lerp(base.m_DecalsMesh.Normals[a_IndexA], base.m_DecalsMesh.Normals[a_IndexB], a_IntersectionFactorAB));
            base.m_DecalsMesh.UV2s.Add(Vector2.Lerp(base.m_DecalsMesh.UV2s[a_IndexA], base.m_DecalsMesh.UV2s[a_IndexB], a_IntersectionFactorAB));
            if (base.m_DecalsMesh.Decals.UseVertexColors)
            {
                base.m_DecalsMesh.TargetVertexColors.Add(Color.Lerp(base.m_DecalsMesh.TargetVertexColors[a_IndexA], base.m_DecalsMesh.TargetVertexColors[a_IndexB], a_IntersectionFactorAB));
                base.m_DecalsMesh.VertexColors.Add(Color.Lerp(base.m_DecalsMesh.VertexColors[a_IndexA], base.m_DecalsMesh.VertexColors[a_IndexB], a_IntersectionFactorAB));
            }
            base.m_ActiveProjector.DecalsMeshUpperVertexIndex++;
            if (a_IsVertexAInside)
            {
                edge.newVertex2Index = count;
                base.m_RelativeVertexLocations[a_IndexB - a_RelativeVertexLocationsOffset] = RelativeVertexLocation.Outside;
            }
            else
            {
                edge.newVertex1Index = count;
                base.m_RelativeVertexLocations[a_IndexA - a_RelativeVertexLocationsOffset] = RelativeVertexLocation.Outside;
            }
            base.m_CutEdges.AddEdge(edge);
            return count;
        }

        private int CutEdgeOptimizedVerticesNormalsUVs(int a_RelativeVertexLocationsOffset, int a_IndexA, int a_IndexB, Vector3 a_VertexA, Vector3 a_VertexB, bool a_IsVertexAInside, float a_IntersectionFactorAB)
        {
            CutEdge edge = new CutEdge(a_IndexA, a_IndexB);
            int count = base.m_DecalsMesh.Vertices.Count;
            base.m_DecalsMesh.Vertices.Add(Vector3.Lerp(a_VertexA, a_VertexB, a_IntersectionFactorAB));
            base.m_DecalsMesh.Normals.Add(Vector3.Lerp(base.m_DecalsMesh.Normals[a_IndexA], base.m_DecalsMesh.Normals[a_IndexB], a_IntersectionFactorAB));
            base.m_DecalsMesh.UVs.Add(Vector2.Lerp(base.m_DecalsMesh.UVs[a_IndexA], base.m_DecalsMesh.UVs[a_IndexB], a_IntersectionFactorAB));
            if (base.m_DecalsMesh.Decals.UseVertexColors)
            {
                base.m_DecalsMesh.TargetVertexColors.Add(Color.Lerp(base.m_DecalsMesh.TargetVertexColors[a_IndexA], base.m_DecalsMesh.TargetVertexColors[a_IndexB], a_IntersectionFactorAB));
                base.m_DecalsMesh.VertexColors.Add(Color.Lerp(base.m_DecalsMesh.VertexColors[a_IndexA], base.m_DecalsMesh.VertexColors[a_IndexB], a_IntersectionFactorAB));
            }
            base.m_ActiveProjector.DecalsMeshUpperVertexIndex++;
            if (a_IsVertexAInside)
            {
                edge.newVertex2Index = count;
                base.m_RelativeVertexLocations[a_IndexB - a_RelativeVertexLocationsOffset] = RelativeVertexLocation.Outside;
            }
            else
            {
                edge.newVertex1Index = count;
                base.m_RelativeVertexLocations[a_IndexA - a_RelativeVertexLocationsOffset] = RelativeVertexLocation.Outside;
            }
            base.m_CutEdges.AddEdge(edge);
            return count;
        }

        private int CutEdgeOptimizedVerticesNormalsUVsUV2s(int a_RelativeVertexLocationsOffset, int a_IndexA, int a_IndexB, Vector3 a_VertexA, Vector3 a_VertexB, bool a_IsVertexAInside, float a_IntersectionFactorAB)
        {
            CutEdge edge = new CutEdge(a_IndexA, a_IndexB);
            int count = base.m_DecalsMesh.Vertices.Count;
            base.m_DecalsMesh.Vertices.Add(Vector3.Lerp(a_VertexA, a_VertexB, a_IntersectionFactorAB));
            base.m_DecalsMesh.Normals.Add(Vector3.Lerp(base.m_DecalsMesh.Normals[a_IndexA], base.m_DecalsMesh.Normals[a_IndexB], a_IntersectionFactorAB));
            base.m_DecalsMesh.UVs.Add(Vector2.Lerp(base.m_DecalsMesh.UVs[a_IndexA], base.m_DecalsMesh.UVs[a_IndexB], a_IntersectionFactorAB));
            base.m_DecalsMesh.UV2s.Add(Vector2.Lerp(base.m_DecalsMesh.UV2s[a_IndexA], base.m_DecalsMesh.UV2s[a_IndexB], a_IntersectionFactorAB));
            if (base.m_DecalsMesh.Decals.UseVertexColors)
            {
                base.m_DecalsMesh.TargetVertexColors.Add(Color.Lerp(base.m_DecalsMesh.TargetVertexColors[a_IndexA], base.m_DecalsMesh.TargetVertexColors[a_IndexB], a_IntersectionFactorAB));
                base.m_DecalsMesh.VertexColors.Add(Color.Lerp(base.m_DecalsMesh.VertexColors[a_IndexA], base.m_DecalsMesh.VertexColors[a_IndexB], a_IntersectionFactorAB));
            }
            base.m_ActiveProjector.DecalsMeshUpperVertexIndex++;
            if (a_IsVertexAInside)
            {
                edge.newVertex2Index = count;
                base.m_RelativeVertexLocations[a_IndexB - a_RelativeVertexLocationsOffset] = RelativeVertexLocation.Outside;
            }
            else
            {
                edge.newVertex1Index = count;
                base.m_RelativeVertexLocations[a_IndexA - a_RelativeVertexLocationsOffset] = RelativeVertexLocation.Outside;
            }
            base.m_CutEdges.AddEdge(edge);
            return count;
        }

        private int CutEdgeUnoptimized(int a_RelativeVertexLocationsOffset, int a_IndexA, int a_IndexB, Vector3 a_VertexA, Vector3 a_VertexB, bool a_IsVertexAInside, float a_IntersectionFactorAB)
        {
            CutEdge edge = new CutEdge(a_IndexA, a_IndexB);
            Vector3 a = base.m_DecalsMesh.Normals[a_IndexA];
            int count = base.m_DecalsMesh.Vertices.Count;
            base.m_DecalsMesh.Vertices.Add(Vector3.Lerp(a_VertexA, a_VertexB, a_IntersectionFactorAB));
            base.m_DecalsMesh.Normals.Add(Vector3.Lerp(a, base.m_DecalsMesh.Normals[a_IndexB], a_IntersectionFactorAB));
            base.m_ActiveProjector.DecalsMeshUpperVertexIndex++;
            if (base.m_DecalsMesh.Decals.CurrentTangentsMode == TangentsMode.Target)
            {
                base.m_DecalsMesh.Tangents.Add(Vector4.Lerp(base.m_DecalsMesh.Tangents[a_IndexA], base.m_DecalsMesh.Tangents[a_IndexB], a_IntersectionFactorAB));
            }
            if (base.m_DecalsMesh.Decals.UseVertexColors)
            {
                base.m_DecalsMesh.TargetVertexColors.Add(Color.Lerp(base.m_DecalsMesh.TargetVertexColors[a_IndexA], base.m_DecalsMesh.TargetVertexColors[a_IndexB], a_IntersectionFactorAB));
                base.m_DecalsMesh.VertexColors.Add(Color.Lerp(base.m_DecalsMesh.VertexColors[a_IndexA], base.m_DecalsMesh.VertexColors[a_IndexB], a_IntersectionFactorAB));
            }
            if ((base.m_DecalsMesh.Decals.CurrentUVMode == UVMode.TargetUV) || (base.m_DecalsMesh.Decals.CurrentUVMode == UVMode.TargetUV2))
            {
                base.m_DecalsMesh.UVs.Add(Vector2.Lerp(base.m_DecalsMesh.UVs[a_IndexA], base.m_DecalsMesh.UVs[a_IndexB], a_IntersectionFactorAB));
            }
            if ((base.m_DecalsMesh.Decals.CurrentUV2Mode == UV2Mode.TargetUV) || (base.m_DecalsMesh.Decals.CurrentUV2Mode == UV2Mode.TargetUV2))
            {
                base.m_DecalsMesh.UV2s.Add(Vector2.Lerp(base.m_DecalsMesh.UV2s[a_IndexA], base.m_DecalsMesh.UV2s[a_IndexB], a_IntersectionFactorAB));
            }
            if (a_IsVertexAInside)
            {
                edge.newVertex2Index = count;
                base.m_RelativeVertexLocations[a_IndexB - a_RelativeVertexLocationsOffset] = RelativeVertexLocation.Outside;
            }
            else
            {
                edge.newVertex1Index = count;
                base.m_RelativeVertexLocations[a_IndexA - a_RelativeVertexLocationsOffset] = RelativeVertexLocation.Outside;
            }
            base.m_CutEdges.AddEdge(edge);
            return count;
        }

        internal override void InitializeDelegates()
        {
            base.m_GetCutEdgeDelegate = new CutEdgeDelegate(this.CutEdge);
            bool flag = base.m_DecalsMesh.Decals.CurrentTangentsMode == TangentsMode.Target;
            bool flag2 = (base.m_DecalsMesh.Decals.CurrentUVMode == UVMode.TargetUV) || (base.m_DecalsMesh.Decals.CurrentUVMode == UVMode.TargetUV2);
            bool flag3 = (base.m_DecalsMesh.Decals.CurrentUV2Mode == UV2Mode.TargetUV) || (base.m_DecalsMesh.Decals.CurrentUV2Mode == UV2Mode.TargetUV2);
            if (!flag && (!flag2 && !flag3))
            {
                base.m_CreateCutEdgeDelegate = new CutEdgeDelegate(this.CutEdgeOptimizedVerticesNormals);
            }
            else if (!flag && (!flag2 && flag3))
            {
                base.m_CreateCutEdgeDelegate = new CutEdgeDelegate(this.CutEdgeOptimizedVerticesNormalsUV2s);
            }
            else if (!flag && (flag2 && !flag3))
            {
                base.m_CreateCutEdgeDelegate = new CutEdgeDelegate(this.CutEdgeOptimizedVerticesNormalsUVs);
            }
            else if (!flag && (flag2 && flag3))
            {
                base.m_CreateCutEdgeDelegate = new CutEdgeDelegate(this.CutEdgeOptimizedVerticesNormalsUVsUV2s);
            }
            else if (flag && (!flag2 && !flag3))
            {
                base.m_CreateCutEdgeDelegate = new CutEdgeDelegate(this.CutEdgeOptimizedVerticesNormalsTangents);
            }
            else if (flag && (!flag2 && flag3))
            {
                base.m_CreateCutEdgeDelegate = new CutEdgeDelegate(this.CutEdgeOptimizedVerticesNormalsTangentsUV2s);
            }
            else if (flag && (flag2 && !flag3))
            {
                base.m_CreateCutEdgeDelegate = new CutEdgeDelegate(this.CutEdgeOptimizedVerticesNormalsTangentsUVs);
            }
            else if (flag && (flag2 && flag3))
            {
                base.m_CreateCutEdgeDelegate = new CutEdgeDelegate(this.CutEdgeOptimizedVerticesNormalsTangentsUVsUV2s);
            }
        }
    }
}

