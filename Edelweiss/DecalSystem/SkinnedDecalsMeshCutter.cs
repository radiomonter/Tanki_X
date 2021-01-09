namespace Edelweiss.DecalSystem
{
    using System;
    using UnityEngine;

    public class SkinnedDecalsMeshCutter : GenericDecalsMeshCutter<SkinnedDecals, SkinnedDecalProjectorBase, SkinnedDecalsMesh>
    {
        private BoneWeightElement[] m_BoneWeightElements = new BoneWeightElement[8];

        private int CutEdgeUnoptimized(int a_RelativeVertexLocationsOffset, int a_IndexA, int a_IndexB, Vector3 a_VertexA, Vector3 a_VertexB, bool a_IsVertexAInside, float a_IntersectionFactorAB)
        {
            CutEdge edge = new CutEdge(a_IndexA, a_IndexB);
            Vector3 a = base.m_DecalsMesh.OriginalVertices[a_IndexA];
            Vector3 vector3 = base.m_DecalsMesh.Normals[a_IndexA];
            BoneWeight weight = base.m_DecalsMesh.BoneWeights[a_IndexA];
            int count = base.m_DecalsMesh.Vertices.Count;
            base.m_DecalsMesh.OriginalVertices.Add(Vector3.Lerp(a, base.m_DecalsMesh.OriginalVertices[a_IndexB], a_IntersectionFactorAB));
            base.m_DecalsMesh.Vertices.Add(Vector3.Lerp(a_VertexA, a_VertexB, a_IntersectionFactorAB));
            base.m_DecalsMesh.Normals.Add(Vector3.Lerp(vector3, base.m_DecalsMesh.Normals[a_IndexB], a_IntersectionFactorAB));
            base.m_DecalsMesh.BoneWeights.Add(this.LerpBoneWeights(weight, base.m_DecalsMesh.BoneWeights[a_IndexB], a_IntersectionFactorAB));
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
                base.m_CreateCutEdgeDelegate = new CutEdgeDelegate(this.CutEdgeUnoptimized);
            }
            else if (!flag && (!flag2 && flag3))
            {
                base.m_CreateCutEdgeDelegate = new CutEdgeDelegate(this.CutEdgeUnoptimized);
            }
            else if (!flag && (flag2 && !flag3))
            {
                base.m_CreateCutEdgeDelegate = new CutEdgeDelegate(this.CutEdgeUnoptimized);
            }
            else if (!flag && (flag2 && flag3))
            {
                base.m_CreateCutEdgeDelegate = new CutEdgeDelegate(this.CutEdgeUnoptimized);
            }
            else if (flag && (!flag2 && !flag3))
            {
                base.m_CreateCutEdgeDelegate = new CutEdgeDelegate(this.CutEdgeUnoptimized);
            }
            else if (flag && (!flag2 && flag3))
            {
                base.m_CreateCutEdgeDelegate = new CutEdgeDelegate(this.CutEdgeUnoptimized);
            }
            else if (flag && (flag2 && !flag3))
            {
                base.m_CreateCutEdgeDelegate = new CutEdgeDelegate(this.CutEdgeUnoptimized);
            }
            else if (flag && (flag2 && flag3))
            {
                base.m_CreateCutEdgeDelegate = new CutEdgeDelegate(this.CutEdgeUnoptimized);
            }
        }

        private BoneWeight LerpBoneWeights(BoneWeight a_BoneWeight1, BoneWeight a_BoneWeight2, float a_Factor)
        {
            BoneWeightElement element = new BoneWeightElement();
            float num = 1f - a_Factor;
            element.index = a_BoneWeight1.boneIndex0;
            element.weight = a_BoneWeight1.weight0 * num;
            this.m_BoneWeightElements[0] = element;
            element.index = a_BoneWeight1.boneIndex1;
            element.weight = a_BoneWeight1.weight1 * num;
            this.m_BoneWeightElements[1] = element;
            element.index = a_BoneWeight1.boneIndex2;
            element.weight = a_BoneWeight1.weight2 * num;
            this.m_BoneWeightElements[2] = element;
            element.index = a_BoneWeight1.boneIndex3;
            element.weight = a_BoneWeight1.weight3 * num;
            this.m_BoneWeightElements[3] = element;
            float num2 = a_Factor;
            element.index = a_BoneWeight2.boneIndex0;
            element.weight = a_BoneWeight2.weight0 * num2;
            this.m_BoneWeightElements[4] = element;
            element.index = a_BoneWeight2.boneIndex1;
            element.weight = a_BoneWeight2.weight1 * num2;
            this.m_BoneWeightElements[5] = element;
            element.index = a_BoneWeight2.boneIndex2;
            element.weight = a_BoneWeight2.weight2 * num2;
            this.m_BoneWeightElements[6] = element;
            element.index = a_BoneWeight2.boneIndex3;
            element.weight = a_BoneWeight2.weight3 * num2;
            this.m_BoneWeightElements[7] = element;
            int index = 0;
            while (index < 4)
            {
                int num4 = this.m_BoneWeightElements[index].index;
                int num5 = 4;
                while (true)
                {
                    if (num5 >= 8)
                    {
                        index++;
                        break;
                    }
                    int num6 = this.m_BoneWeightElements[num5].index;
                    if (num4 == num6)
                    {
                        this.m_BoneWeightElements[index].weight += this.m_BoneWeightElements[num5].weight;
                        this.m_BoneWeightElements[num5].weight = 0f;
                        this.m_BoneWeightElements[num5].index = 0;
                    }
                    num5++;
                }
            }
            Array.Sort<BoneWeightElement>(this.m_BoneWeightElements);
            float num7 = 1f / (((this.m_BoneWeightElements[0].weight + this.m_BoneWeightElements[1].weight) + this.m_BoneWeightElements[2].weight) + this.m_BoneWeightElements[3].weight);
            return new BoneWeight { 
                boneIndex0 = this.m_BoneWeightElements[0].index,
                weight0 = this.m_BoneWeightElements[0].weight * num7,
                boneIndex1 = this.m_BoneWeightElements[1].index,
                weight1 = this.m_BoneWeightElements[1].weight * num7,
                boneIndex2 = this.m_BoneWeightElements[2].index,
                weight2 = this.m_BoneWeightElements[2].weight * num7,
                boneIndex3 = this.m_BoneWeightElements[3].index,
                weight3 = this.m_BoneWeightElements[3].weight * num7
            };
        }
    }
}

