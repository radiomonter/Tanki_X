using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class P3D_Result
{
    private static List<P3D_Result> pool = new List<P3D_Result>();
    public Vector3 Weights;
    public P3D_Triangle Triangle;
    public float Distance01;

    public static P3D_Result Despawn(P3D_Result result)
    {
        pool.Add(result);
        return null;
    }

    public Vector2 GetUV(P3D_CoordType coord)
    {
        if (coord == P3D_CoordType.UV1)
        {
            return this.UV1;
        }
        if (coord == P3D_CoordType.UV2)
        {
            return this.UV2;
        }
        return new Vector2();
    }

    public static P3D_Result Spawn()
    {
        if (pool.Count <= 0)
        {
            return new P3D_Result();
        }
        int index = pool.Count - 1;
        P3D_Result result = pool[index];
        pool.RemoveAt(index);
        return result;
    }

    public Vector2 UV1 =>
        ((this.Triangle.Coord1A * this.Weights.x) + (this.Triangle.Coord1B * this.Weights.y)) + (this.Triangle.Coord1C * this.Weights.z);

    public Vector2 UV2 =>
        ((this.Triangle.Coord2A * this.Weights.x) + (this.Triangle.Coord2B * this.Weights.y)) + (this.Triangle.Coord2C * this.Weights.z);

    public Vector2 Point =>
        ((this.Triangle.PointA * this.Weights.x) + (this.Triangle.PointB * this.Weights.y)) + (this.Triangle.PointC * this.Weights.z);
}

