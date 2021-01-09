using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class P3D_Triangle
{
    private static List<P3D_Triangle> pool = new List<P3D_Triangle>();
    public Vector3 PointA;
    public Vector3 PointB;
    public Vector3 PointC;
    public Vector2 Coord1A;
    public Vector2 Coord1B;
    public Vector2 Coord1C;
    public Vector2 Coord2A;
    public Vector2 Coord2B;
    public Vector2 Coord2C;

    public static P3D_Triangle Despawn(P3D_Triangle triangle)
    {
        pool.Add(triangle);
        return null;
    }

    public static P3D_Triangle Spawn()
    {
        if (pool.Count <= 0)
        {
            return new P3D_Triangle();
        }
        int index = pool.Count - 1;
        P3D_Triangle triangle = pool[index];
        pool.RemoveAt(index);
        return triangle;
    }

    public Vector3 Edge1 =>
        this.PointB - this.PointA;

    public Vector3 Edge2 =>
        this.PointC - this.PointA;

    public Vector3 Min =>
        Vector3.Min(this.PointA, Vector3.Min(this.PointB, this.PointC));

    public Vector3 Max =>
        Vector3.Max(this.PointA, Vector3.Max(this.PointB, this.PointC));

    public float MidX =>
        ((this.PointA.x + this.PointB.x) + this.PointC.x) / 3f;

    public float MidY =>
        ((this.PointA.y + this.PointB.y) + this.PointC.y) / 3f;

    public float MidZ =>
        ((this.PointA.z + this.PointB.z) + this.PointC.z) / 3f;
}

