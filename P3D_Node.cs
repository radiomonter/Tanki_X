using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class P3D_Node
{
    private static List<P3D_Node> pool = new List<P3D_Node>();
    public Bounds Bound;
    public bool Split;
    public int PositiveIndex;
    public int NegativeIndex;
    public int TriangleIndex;
    public int TriangleCount;

    public void CalculateBound(List<P3D_Triangle> triangles)
    {
        if ((triangles.Count > 0) && (this.TriangleCount > 0))
        {
            Vector3 min = triangles[this.TriangleIndex].Min;
            Vector3 max = triangles[this.TriangleIndex].Max;
            int num = (this.TriangleIndex + this.TriangleCount) - 1;
            while (true)
            {
                if (num <= this.TriangleIndex)
                {
                    this.Bound.SetMinMax(min, max);
                    break;
                }
                P3D_Triangle triangle = triangles[num];
                min = Vector3.Min(min, triangle.Min);
                max = Vector3.Max(max, triangle.Max);
                num--;
            }
        }
    }

    public static P3D_Node Despawn(P3D_Node node)
    {
        pool.Add(node);
        Bounds bounds = new Bounds();
        node.Bound = bounds;
        node.Split = false;
        node.PositiveIndex = 0;
        node.NegativeIndex = 0;
        node.TriangleIndex = 0;
        node.TriangleCount = 0;
        return null;
    }

    public static P3D_Node Spawn()
    {
        if (pool.Count <= 0)
        {
            return new P3D_Node();
        }
        int index = pool.Count - 1;
        P3D_Node node = pool[index];
        pool.RemoveAt(index);
        return node;
    }
}

