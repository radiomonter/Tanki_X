using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class P3D_Tree
{
    [SerializeField]
    private Mesh mesh;
    [SerializeField]
    private int vertexCount;
    [SerializeField]
    private int subMeshIndex;
    [SerializeField]
    private List<P3D_Node> nodes = new List<P3D_Node>();
    [SerializeField]
    private List<P3D_Triangle> triangles = new List<P3D_Triangle>();
    private static List<P3D_Triangle> potentials = new List<P3D_Triangle>();
    private static List<P3D_Result> results = new List<P3D_Result>();
    private static P3D_Tree tempInstance;

    private void AddToPotentials(P3D_Node node)
    {
        for (int i = node.TriangleIndex; i < (node.TriangleIndex + node.TriangleCount); i++)
        {
            potentials.Add(this.triangles[i]);
        }
    }

    private void AddToResults(P3D_Triangle triangle, Vector3 weights, float distance01)
    {
        P3D_Result item = P3D_Result.Spawn();
        item.Triangle = triangle;
        item.Weights = weights;
        item.Distance01 = distance01;
        results.Add(item);
    }

    private void BeginSearchBetween(Vector3 startPoint, Vector3 endPoint)
    {
        Vector3 direction = endPoint - startPoint;
        Ray ray = new Ray(startPoint, direction);
        this.SearchBetween(this.nodes[0], ray, direction.magnitude);
    }

    private void BeginSearchDistance(Vector3 point, float maxDistanceSqr)
    {
        this.SearchDistance(this.nodes[0], point, maxDistanceSqr);
    }

    public void Clear()
    {
        this.mesh = null;
        this.vertexCount = 0;
        this.subMeshIndex = 0;
        for (int i = this.triangles.Count - 1; i >= 0; i--)
        {
            P3D_Triangle.Despawn(this.triangles[i]);
        }
        this.triangles.Clear();
        for (int j = this.nodes.Count - 1; j >= 0; j--)
        {
            P3D_Node.Despawn(this.nodes[j]);
        }
        this.nodes.Clear();
    }

    public void ClearResults()
    {
        for (int i = results.Count - 1; i >= 0; i--)
        {
            P3D_Result.Despawn(results[i]);
        }
        results.Clear();
        potentials.Clear();
    }

    private void ConstructNodes()
    {
        P3D_Node item = P3D_Node.Spawn();
        this.nodes.Add(item);
        this.Pack(item, 0, this.triangles.Count);
    }

    private void ExtractTriangles()
    {
        if ((this.subMeshIndex >= 0) && (this.mesh.subMeshCount >= 0))
        {
            int submesh = Mathf.Min(this.subMeshIndex, this.mesh.subMeshCount - 1);
            int[] triangles = this.mesh.GetTriangles(submesh);
            Vector3[] vertices = this.mesh.vertices;
            Vector2[] uv = this.mesh.uv;
            Vector2[] vectorArray3 = this.mesh.uv2;
            if (triangles.Length > 0)
            {
                for (int i = (triangles.Length / 3) - 1; i >= 0; i--)
                {
                    P3D_Triangle item = P3D_Triangle.Spawn();
                    int index = triangles[i * 3];
                    int num5 = triangles[(i * 3) + 1];
                    int num6 = triangles[(i * 3) + 2];
                    item.PointA = vertices[index];
                    item.PointB = vertices[num5];
                    item.PointC = vertices[num6];
                    item.Coord1A = uv[index];
                    item.Coord1B = uv[num5];
                    item.Coord1C = uv[num6];
                    if (vectorArray3.Length > 0)
                    {
                        item.Coord2A = vectorArray3[index];
                        item.Coord2B = vectorArray3[num5];
                        item.Coord2C = vectorArray3[num6];
                    }
                    this.triangles.Add(item);
                }
            }
        }
    }

    public List<P3D_Result> FindBetweenAll(Vector3 startPoint, Vector3 endPoint)
    {
        this.ClearResults();
        if (this.IsReady)
        {
            this.BeginSearchBetween(startPoint, endPoint);
            for (int i = potentials.Count - 1; i >= 0; i--)
            {
                P3D_Triangle triangle = potentials[i];
                Vector3 weights = new Vector3();
                float num2 = 0f;
                if (P3D_Helper.IntersectBarycentric(startPoint, endPoint, triangle, out weights, out num2))
                {
                    this.AddToResults(triangle, weights, num2);
                }
            }
        }
        return results;
    }

    public P3D_Result FindBetweenNearest(Vector3 startPoint, Vector3 endPoint)
    {
        this.ClearResults();
        if (this.IsReady)
        {
            float positiveInfinity = float.PositiveInfinity;
            P3D_Triangle triangle = null;
            Vector3 vector2 = new Vector3();
            Vector3 weights = vector2;
            this.BeginSearchBetween(startPoint, endPoint);
            int num2 = potentials.Count - 1;
            while (true)
            {
                if (num2 < 0)
                {
                    if (triangle == null)
                    {
                        break;
                    }
                    return this.GetResult(triangle, weights, positiveInfinity);
                }
                P3D_Triangle triangle2 = potentials[num2];
                vector2 = new Vector3();
                Vector3 vector3 = vector2;
                float num3 = 0f;
                if (P3D_Helper.IntersectBarycentric(startPoint, endPoint, triangle2, out vector3, out num3) && (num3 < positiveInfinity))
                {
                    positiveInfinity = num3;
                    triangle = triangle2;
                    weights = vector3;
                }
                num2--;
            }
        }
        return null;
    }

    public P3D_Result FindNearest(Vector3 point, float maxDistance)
    {
        this.ClearResults();
        if (this.IsReady && (maxDistance > 0f))
        {
            float maxDistanceSqr = maxDistance * maxDistance;
            P3D_Triangle triangle = null;
            Vector3 vector2 = new Vector3();
            Vector3 weights = vector2;
            this.BeginSearchDistance(point, maxDistanceSqr);
            int num2 = potentials.Count - 1;
            while (true)
            {
                if (num2 < 0)
                {
                    if (triangle == null)
                    {
                        break;
                    }
                    return this.GetResult(triangle, weights, Mathf.Sqrt(maxDistanceSqr) / maxDistance);
                }
                P3D_Triangle triangle2 = potentials[num2];
                vector2 = new Vector3();
                Vector3 vector3 = vector2;
                float num3 = P3D_Helper.ClosestBarycentric(point, triangle2, out vector3);
                if (num3 <= maxDistanceSqr)
                {
                    maxDistanceSqr = num3;
                    triangle = triangle2;
                    weights = vector3;
                }
                num2--;
            }
        }
        return null;
    }

    public List<P3D_Result> FindPerpendicularAll(Vector3 point, float maxDistance)
    {
        this.ClearResults();
        if (this.IsReady && (maxDistance > 0f))
        {
            float maxDistanceSqr = maxDistance * maxDistance;
            this.BeginSearchDistance(point, maxDistanceSqr);
            for (int i = potentials.Count - 1; i >= 0; i--)
            {
                P3D_Triangle triangle = potentials[i];
                Vector3 weights = new Vector3();
                float distanceSqr = 0f;
                if (P3D_Helper.ClosestBarycentric(point, triangle, ref weights, ref distanceSqr) && (distanceSqr <= maxDistanceSqr))
                {
                    this.AddToResults(triangle, weights, Mathf.Sqrt(distanceSqr) / maxDistance);
                }
            }
        }
        return results;
    }

    public P3D_Result FindPerpendicularNearest(Vector3 point, float maxDistance)
    {
        this.ClearResults();
        if (this.IsReady && (maxDistance > 0f))
        {
            float maxDistanceSqr = maxDistance * maxDistance;
            P3D_Triangle triangle = null;
            Vector3 vector2 = new Vector3();
            Vector3 weights = vector2;
            this.BeginSearchDistance(point, maxDistanceSqr);
            int num2 = potentials.Count - 1;
            while (true)
            {
                if (num2 < 0)
                {
                    if (triangle == null)
                    {
                        break;
                    }
                    return this.GetResult(triangle, weights, Mathf.Sqrt(maxDistanceSqr) / maxDistance);
                }
                P3D_Triangle triangle2 = potentials[num2];
                vector2 = new Vector3();
                Vector3 vector3 = vector2;
                float distanceSqr = 0f;
                if (P3D_Helper.ClosestBarycentric(point, triangle2, ref vector3, ref distanceSqr) && (distanceSqr <= maxDistanceSqr))
                {
                    maxDistanceSqr = distanceSqr;
                    triangle = triangle2;
                    weights = vector3;
                }
                num2--;
            }
        }
        return null;
    }

    private P3D_Result GetResult(P3D_Triangle triangle, Vector3 weights, float distance01)
    {
        this.ClearResults();
        this.AddToResults(triangle, weights, distance01);
        return results[0];
    }

    private void Pack(P3D_Node node, int min, int max)
    {
        int num = max - min;
        node.TriangleIndex = min;
        node.TriangleCount = num;
        node.Split = num >= 5;
        node.CalculateBound(this.triangles);
        if (node.Split)
        {
            int num2 = (min + max) / 2;
            this.SortTriangles(min, max);
            node.PositiveIndex = this.nodes.Count;
            P3D_Node item = P3D_Node.Spawn();
            this.nodes.Add(item);
            this.Pack(item, min, num2);
            node.NegativeIndex = this.nodes.Count;
            P3D_Node node3 = P3D_Node.Spawn();
            this.nodes.Add(node3);
            this.Pack(node3, num2, max);
        }
    }

    private void SearchBetween(P3D_Node node, Ray ray, float maxDistance)
    {
        float distance = 0f;
        if (node.Bound.IntersectRay(ray, out distance) && (distance <= maxDistance))
        {
            if (!node.Split)
            {
                this.AddToPotentials(node);
            }
            else
            {
                if (node.PositiveIndex != 0)
                {
                    this.SearchBetween(this.nodes[node.PositiveIndex], ray, maxDistance);
                }
                if (node.NegativeIndex != 0)
                {
                    this.SearchBetween(this.nodes[node.NegativeIndex], ray, maxDistance);
                }
            }
        }
    }

    private void SearchDistance(P3D_Node node, Vector3 point, float maxDistanceSqr)
    {
        if (node.Bound.SqrDistance(point) < maxDistanceSqr)
        {
            if (!node.Split)
            {
                this.AddToPotentials(node);
            }
            else
            {
                if (node.PositiveIndex != 0)
                {
                    this.SearchDistance(this.nodes[node.PositiveIndex], point, maxDistanceSqr);
                }
                if (node.NegativeIndex != 0)
                {
                    this.SearchDistance(this.nodes[node.NegativeIndex], point, maxDistanceSqr);
                }
            }
        }
    }

    public void SetMesh(GameObject gameObject, int subMeshIndex = 0, bool forceUpdate = false)
    {
        Mesh bakedMesh = null;
        Mesh mesh = P3D_Helper.GetMesh(gameObject, ref bakedMesh);
        if (bakedMesh != null)
        {
            P3D_Helper.Destroy<Mesh>(bakedMesh);
            throw new Exception("P3D_Tree cannot manage baked meshes, call SetMesh with the Mesh directly to use animated meshes");
        }
        this.SetMesh(mesh, subMeshIndex, forceUpdate);
    }

    public void SetMesh(Mesh newMesh, int newSubMeshIndex = 0, bool forceUpdate = false)
    {
        if (newMesh == null)
        {
            this.Clear();
        }
        else if (forceUpdate || ((newMesh != this.mesh) || ((newSubMeshIndex != this.subMeshIndex) || (newMesh.vertexCount != this.vertexCount))))
        {
            this.Clear();
            this.mesh = newMesh;
            this.subMeshIndex = newSubMeshIndex;
            this.vertexCount = newMesh.vertexCount;
            this.ExtractTriangles();
            this.ConstructNodes();
        }
    }

    private void SortTriangle(P3D_Triangle triangle, ref int minIndex, ref int maxIndex, bool abovePivot)
    {
        if (abovePivot)
        {
            this.triangles[maxIndex - 1] = triangle;
            maxIndex--;
        }
        else
        {
            this.triangles[minIndex] = triangle;
            minIndex++;
        }
    }

    private void SortTriangles(int minIndex, int maxIndex)
    {
        potentials.Clear();
        Vector3 min = this.triangles[minIndex].Min;
        Vector3 max = this.triangles[minIndex].Max;
        Vector3 zero = Vector3.zero;
        for (int i = minIndex; i < maxIndex; i++)
        {
            P3D_Triangle item = this.triangles[i];
            min = Vector3.Min(min, item.Min);
            max = Vector3.Max(max, item.Max);
            zero += (item.PointA + item.PointB) + item.PointC;
            potentials.Add(item);
        }
        Vector3 vector4 = max - min;
        if ((vector4.x > vector4.y) && (vector4.x > vector4.z))
        {
            float num2 = P3D_Helper.Divide(zero.x, this.triangles.Count * 3f);
            for (int j = potentials.Count - 1; j >= 0; j--)
            {
                P3D_Triangle triangle = potentials[j];
                this.SortTriangle(triangle, ref minIndex, ref maxIndex, triangle.MidX >= num2);
            }
        }
        else if ((vector4.y > vector4.x) && (vector4.y > vector4.z))
        {
            float num4 = P3D_Helper.Divide(zero.y, this.triangles.Count * 3f);
            for (int j = potentials.Count - 1; j >= 0; j--)
            {
                P3D_Triangle triangle = potentials[j];
                this.SortTriangle(triangle, ref minIndex, ref maxIndex, triangle.MidY >= num4);
            }
        }
        else
        {
            float num6 = P3D_Helper.Divide(zero.z, this.triangles.Count * 3f);
            for (int j = potentials.Count - 1; j >= 0; j--)
            {
                P3D_Triangle triangle = potentials[j];
                this.SortTriangle(triangle, ref minIndex, ref maxIndex, triangle.MidZ >= num6);
            }
        }
    }

    public static P3D_Tree TempInstance
    {
        get
        {
            tempInstance ??= new P3D_Tree();
            return tempInstance;
        }
    }

    public bool IsReady =>
        this.nodes.Count > 0;
}

